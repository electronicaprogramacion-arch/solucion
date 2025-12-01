using CalibrationSaaS.CustomerPortal.Data;
using CalibrationSaaS.CustomerPortal.Services.Authentication;
using CalibrationSaaS.CustomerPortal.Services.Background;
using CalibrationSaaS.CustomerPortal.Services.Certificates;
using CalibrationSaaS.CustomerPortal.Services.Downloads;
using CalibrationSaaS.CustomerPortal.Services.Email;
using CalibrationSaaS.CustomerPortal.Services.Equipment;
using CalibrationSaaS.CustomerPortal.Services.GrpcClients;
using CalibrationSaaS.CustomerPortal.Services.MultiTenancy;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using AspNetCoreRateLimit;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc.Client;
using LocalTenantInfo = CalibrationSaaS.CustomerPortal.Models.MultiTenancy.TenantInfo;
using FbTenantInfo = Finbuckle.MultiTenant.TenantInfo;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs explicitly
builder.WebHost.UseUrls("http://localhost:5002");

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/customerportal-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers(); // Add controller support for API endpoints

// Configure Multi-Tenancy with custom TenantInfo
builder.Services.AddMultiTenant<LocalTenantInfo>()
    .WithInMemoryStore()
    .WithDelegateStrategy(context =>
    {
        if (context is HttpContext httpContext)
        {
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var path = httpContext.Request.Path.Value;
            logger.LogInformation($"Delegate Strategy called for path: {path}");

            if (!string.IsNullOrEmpty(path))
            {
                var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length > 0)
                {
                    var potentialTenant = segments[0];
                    logger.LogInformation($"Potential tenant from path: {potentialTenant}");

                    // Check if it's a known tenant identifier (not a system path)
                    if (potentialTenant != "_blazor" &&
                        potentialTenant != "_framework" &&
                        potentialTenant != "_content" &&
                        potentialTenant != "css" &&
                        potentialTenant != "js" &&
                        potentialTenant != "img" &&
                        potentialTenant != "service-worker.js" &&
                        !potentialTenant.StartsWith("CalibrationSaaS"))
                    {
                        logger.LogInformation($"Delegate Strategy returning tenant: {potentialTenant}");

                        // Debug: Try to get the tenant store and check if tenant exists
                        try
                        {
                            var tenantStore = httpContext.RequestServices.GetRequiredService<IMultiTenantStore<LocalTenantInfo>>();
                            var tenantInfo = tenantStore.TryGetAsync(potentialTenant).Result;
                            if (tenantInfo != null)
                            {
                                logger.LogInformation($"Tenant found in store: {tenantInfo.Identifier} - {tenantInfo.Name}");
                            }
                            else
                            {
                                logger.LogWarning($"Tenant '{potentialTenant}' NOT found in store!");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error checking tenant store for '{potentialTenant}'");
                        }

                        return Task.FromResult<string?>(potentialTenant);
                    }
                    else
                    {
                        logger.LogInformation($"Delegate Strategy ignoring system path: {potentialTenant}");
                    }
                }
            }
        }
        return Task.FromResult<string?>(null);
    })
    .WithHeaderStrategy("X-Tenant-ID")
    .WithHostStrategy()
    .WithPerTenantAuthentication();

// Configure Entity Framework with Multi-Tenancy
builder.Services.AddDbContext<CustomerPortalDbContext>((serviceProvider, options) =>
{
    var tenantInfo = serviceProvider.GetService<LocalTenantInfo>();
    var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                 throw new InvalidOperationException("DefaultConnection not found");

    var connectionString = GetTenantConnectionString(tenantInfo, defaultConnectionString);
    options.UseSqlServer(connectionString);
});

// Configure Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Configure Multi-Tenancy Services
builder.Services.AddScoped<ITenantService, TenantService>();

// Add HTTP Context Accessor for authentication services
builder.Services.AddHttpContextAccessor();

// Configure Authentication Services
builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.SectionName));
builder.Services.Configure<TwoFactorOptions>(builder.Configuration.GetSection(TwoFactorOptions.SectionName));
builder.Services.AddScoped<ICustomerAuthenticationService, CustomerAuthenticationService>();
builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();
builder.Services.AddScoped<ISessionStorageService, SessionStorageService>();

// Configure Email Services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

// Configure Two-Factor Authentication Options
builder.Services.Configure<TwoFactorOptions>(builder.Configuration.GetSection("TwoFactor"));

// Configure gRPC Client Services
builder.Services.AddScoped<ICustomerPortalGrpcService, CustomerPortalGrpcService>();
builder.Services.AddScoped<IReportGrpcService, ReportGrpcService>();
builder.Services.AddScoped<IWorkOrderGrpcService, WorkOrderGrpcService>();

// Configure Certificate Services
builder.Services.AddScoped<ICertificateService, CertificateService>();

// Configure Equipment Services
builder.Services.AddScoped<IEquipmentDueDateService, EquipmentDueDateService>();

// Configure Dashboard Services
builder.Services.AddScoped<CalibrationSaaS.CustomerPortal.Services.Dashboard.IDashboardMetricsService, CalibrationSaaS.CustomerPortal.Services.Dashboard.DashboardMetricsService>();

// Configure Download Services
builder.Services.AddScoped<IDownloadService, DownloadService>();

// Configure gRPC Clients for CalibrationSaaS service
builder.Services.AddGrpc();

// Configure tenant-aware gRPC channel factory
builder.Services.AddSingleton<ITenantGrpcChannelFactory, TenantGrpcChannelFactory>();

// Configure backward compatibility GrpcChannel (for services that still need it)
builder.Services.AddScoped<GrpcChannel>(provider =>
{
    var channelFactory = provider.GetRequiredService<ITenantGrpcChannelFactory>();
    // Use GetAwaiter().GetResult() for synchronous registration
    return channelFactory.GetChannelAsync().GetAwaiter().GetResult();
});

// Configure Email Service
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SectionName));
builder.Services.AddScoped<IEmailService, EmailService>();

// Validate email configuration
var emailConfig = builder.Configuration.GetSection(EmailOptions.SectionName).Get<EmailOptions>();
if (emailConfig != null && !emailConfig.IsValid())
{
    var errors = emailConfig.GetValidationErrors();
    Console.WriteLine($"Email configuration validation failed: {string.Join(", ", errors)}");
}

// Configure Radzen Services
builder.Services.AddRadzenComponents();

// Configure Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configure Data Protection
builder.Services.AddDataProtection();

// Configure Authentication
builder.Services.AddCustomerAuthentication(options =>
{
    options.SessionTimeoutMinutes = builder.Configuration.GetValue<int>("AuthenticationSettings:SessionTimeoutMinutes", 30);
    options.RequireHttps = builder.Configuration.GetValue<bool>("SecuritySettings:RequireHttps", true);
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
});

// Configure Authorization
builder.Services.AddAuthorization();

// Configure Authentication State Provider - TEMPORARILY DISABLED FOR TESTING
// builder.Services.AddScoped<AuthenticationStateProvider, CustomerAuthenticationStateProvider>();

// Configure Security and Cleanup Services
builder.Services.Configure<CleanupOptions>(builder.Configuration.GetSection(CleanupOptions.SectionName));
builder.Services.Configure<AuthenticationRateLimitOptions>(builder.Configuration.GetSection(AuthenticationRateLimitOptions.SectionName));
builder.Services.AddHostedService<CleanupBackgroundService>();
builder.Services.AddMemoryCache(); // For rate limiting

// Configure CORS for gRPC
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGrpc", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    });
});

var app = builder.Build();

// Add tenant to in-memory store and debug
try
{
    var tenantStore = app.Services.GetRequiredService<IMultiTenantStore<LocalTenantInfo>>();
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("=== SETTING UP TENANT STORE ===");

    var tenant = new LocalTenantInfo
    {
        Id = "kavoku-tenant-001",
        Identifier = "kavoku",
        Name = "Kavoku Demo",
        ConnectionString = "Server=20.1.195.112;Initial Catalog=CalibrationSaaS_ThermoTemp_CP;Persist Security Info=False;User ID=DevUser;Password=Gr@nP@ramo2025!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=240;",
        CompanyName = "Kavoku",
        LogoUrl = "/logos/calibrationco/logo.svg",
        GrpcServiceUrl = "https://localhost:5333",
        IsActive = true,
        TimeZone = "UTC",
        Culture = "en-US",
        ContactEmail = "jpardo@kavoku.com",
        MaxUsers = 100,
        MaxStorageMB = 2000,
        EnableAuditLogging = true,
        EnableEmailNotifications = true
    };

    // Add the tenant to the store
    var addResult = await tenantStore.TryAddAsync(tenant);
    logger.LogInformation($"✅ Tenant setup complete: {tenant.Identifier} - {tenant.Name} (Added: {addResult})");

    logger.LogInformation("=== END TENANT STORE SETUP ===");
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error setting up tenant store");
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Use Security Headers
app.UseMiddleware<SecurityHeadersMiddleware>();

// Use Authentication Rate Limiting
app.UseMiddleware<AuthenticationRateLimitMiddleware>();

app.UseStaticFiles();

// Use Multi-Tenancy
app.UseMultiTenant();

// Debug: Add middleware to log route values
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    // Only log for tenant-related requests to reduce noise
    if (context.Request.Path.StartsWithSegments("/thermotemp") ||
        context.Request.Path.StartsWithSegments("/login"))
    {
        logger.LogInformation($"Request Path: {context.Request.Path}");

        // Log route values
        if (context.Request.RouteValues.Any())
        {
            foreach (var routeValue in context.Request.RouteValues)
            {
                logger.LogInformation($"Route Value: {routeValue.Key} = {routeValue.Value}");
            }
        }
        else
        {
            logger.LogInformation("No route values found");
        }

        // Log route data from HttpContext
        var routeData = context.GetRouteData();
        if (routeData?.Values?.Any() == true)
        {
            foreach (var routeValue in routeData.Values)
            {
                logger.LogInformation($"RouteData Value: {routeValue.Key} = {routeValue.Value}");
            }
        }
        else
        {
            logger.LogInformation("No route data found");
        }

        var tenantAccessor = context.RequestServices.GetRequiredService<IMultiTenantContextAccessor<LocalTenantInfo>>();
        var tenantContext = tenantAccessor.MultiTenantContext;
        logger.LogInformation($"Tenant Context after MultiTenant middleware: {tenantContext?.TenantInfo?.Identifier ?? "NULL"}");
    }

    await next();
});

// Use Rate Limiting
app.UseIpRateLimiting();

app.UseRouting();
app.UseCors("AllowGrpc");

// Use Session
app.UseSession();

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapControllers(); // Add controller routing for API endpoints
app.MapFallbackToPage("/_Host");

// Ensure database is created for each tenant - TEMPORARILY DISABLED FOR TESTING
/*
using (var scope = app.Services.CreateScope())
{
    var tenantService = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<LocalTenantInfo>>();
    var tenants = await tenantService.GetAllAsync();

    foreach (var tenant in tenants)
    {
        using var tenantScope = app.Services.CreateScope();
        tenantScope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor<LocalTenantInfo>>()
            .MultiTenantContext = new MultiTenantContext<LocalTenantInfo> { TenantInfo = tenant };

        var dbContext = tenantScope.ServiceProvider.GetRequiredService<CustomerPortalDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
*/

try
{
    Log.Information("Starting CalibrationSaaS Customer Portal");
    Log.Information("Application will listen on: http://localhost:5002");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Get tenant-specific database connection string
/// </summary>
/// <param name="tenantInfo">Tenant information</param>
/// <param name="defaultConnectionString">Default connection string template</param>
/// <returns>Tenant-specific connection string</returns>
static string GetTenantConnectionString(LocalTenantInfo? tenantInfo, string defaultConnectionString)
{
    if (tenantInfo == null)
        return defaultConnectionString;

    if (!string.IsNullOrEmpty(tenantInfo.ConnectionString))
    {
        return tenantInfo.ConnectionString;
    }

    // If no specific connection string, use default with tenant-specific database
    if (!string.IsNullOrEmpty(tenantInfo.Identifier))
    {
        return defaultConnectionString.Replace("{TenantId}", tenantInfo.Identifier);
    }

    return defaultConnectionString;
}
