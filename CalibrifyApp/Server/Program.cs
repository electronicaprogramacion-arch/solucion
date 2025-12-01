using Radzen;
using CalibrifyApp.Server.Components;

using Blazed.Controls.Route.Services;
using CalibrationSaaS.Domain.Aggregates.Shared;
using Microsoft.JSInterop;
using Fluxor;
using Microsoft.Extensions.Options;
using Blazored.Modal;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using CalibrationSaaS.Application.Services;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components;
using ProtoBuf.Grpc;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using CalibrifyApp.Server.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using CalibrationSaaS.Infraestructure.Blazor;
using Helpers.Controls;
using Microsoft.AspNetCore.Authorization;
using CalibrifyApp.Server.Middleware;
using Microsoft.AspNetCore.Components.Authorization;
using CalibrationSaaS.Domain.Aggregates.Security;
using Microsoft.AspNetCore.Identity;
using CalibrifyApp.Server.Data;
using CalibrifyApp.Server.Components.Account;


var builder = WebApplication.CreateBuilder(args);

#region Asp.net core Identity
// Add services to the container.
builder.Services.AddRazorComponents()
      .AddInteractiveServerComponents().AddHubOptions(options => {
          options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
          options.ClientTimeoutInterval = TimeSpan.FromSeconds(120);
          options.HandshakeTimeout = TimeSpan.FromSeconds(60);
          options.KeepAliveInterval = TimeSpan.FromSeconds(10);
          options.EnableDetailedErrors = true;
          options.MaximumParallelInvocationsPerClient = 10;
      })
      .AddInteractiveWebAssemblyComponents().AddAuthenticationStateSerialization();

// Authentication is configured by AddIdentity below
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasAccess", policy =>
    {
        // Use default roles
        string roles = "admin,tech,job";
        policy.Requirements.Add(new AccesRequirement(roles,null));
    });
});

// Register the authorization handler
builder.Services.AddScoped<IAuthorizationHandler, AccesHandler>();
#endregion

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();

// Add response compression for better performance
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    options.MimeTypes = Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream", "application/grpc-web", "application/grpc-web+proto" });
});

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("es-ES")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add Radzen's cookie theme service with the correct cookie name
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "CalibrifyTheme"; // Use the same cookie name as in the Blazor project
    options.Duration = TimeSpan.FromDays(365);
});

// Add ThemeService and CookieThemeService from the Blazor project
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<CalibrationSaaS.Infraestructure.Blazor.Services.CookieThemeService>();
builder.Services.AddHttpClient();



// Add router services
builder.Services.AddScoped<RouterSessionService>();

// Add AppSecurity
var appSecurity = new AppSecurity
{
    AssemblyName = builder.Configuration["VersionApp:Assembly"]
};
builder.Services.AddSingleton(appSecurity);
//builder.Services.AddSingleton<AppSecurity>();


// Configure proper authorization policies for ASP.NET Core Identity
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy =>
    {
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy("HasAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
    });

    // Set default policy to require authentication
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Fallback policy for when no policy is specified
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Use the Identity authentication scheme instead of NoAuth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});

// Register the AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, Microsoft.AspNetCore.Components.Server.ServerAuthenticationStateProvider>();

// Add server-side implementation of SignOutSessionStateManager for components that require it
builder.Services.AddScoped<Microsoft.AspNetCore.Components.WebAssembly.Authentication.SignOutSessionStateManager, CalibrifyApp.Server.Services.ServerSignOutSessionStateManager>();

// Register IdentityRedirectManager for Account pages
builder.Services.AddScoped<CalibrifyApp.Server.Components.Account.IdentityRedirectManager>();

// Register IdentityUserAccessor for Account pages
builder.Services.AddScoped<CalibrifyApp.Server.Components.Account.IdentityUserAccessor>();

// Register Data Synchronization Service
builder.Services.AddScoped<CalibrifyApp.Server.Services.IDataSynchronizationService, CalibrifyApp.Server.Services.DataSynchronizationService>();


// Add LazyAssemblyLoader for server-side
builder.Services.AddScoped(sp => new Microsoft.AspNetCore.Components.WebAssembly.Services.LazyAssemblyLoader(sp.GetRequiredService<IJSRuntime>()));

// Add Fluxor for state management with error handling
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(CalibrationSaaS.Infraestructure.Blazor._Imports).Assembly);
    options.UseRouting();
    options.UseReduxDevTools();
});

// Add component initialization service for better error handling and retry logic
builder.Services.AddScoped<CalibrifyApp.Server.Services.ComponentInitializationService>();

// Add component lifecycle optimizer to prevent double-loading issues
builder.Services.AddScoped<CalibrifyApp.Server.Services.ComponentLifecycleOptimizer>();

// Add background service for component cleanup
builder.Services.AddHostedService<CalibrifyApp.Server.Services.ComponentCleanupService>();

// Add TodosState for ResponsiveTableService
builder.Services.AddScoped<IState<CalibrifyApp.Server.Store.State.TodosState>>(provider =>
{
    var state = new CalibrifyApp.Server.Store.State.TodosState(false, null, null, null, null, null);
    return new CalibrifyApp.Server.Store.State.WrappedState<CalibrifyApp.Server.Store.State.TodosState>(state);
});

// Add MockStateFacade for components that require IBasicAuthFacade
builder.Services.AddScoped<CalibrifyApp.Server.Services.IBasicAuthFacade, CalibrifyApp.Server.Services.MockStateFacade>();

// Add SimpleStateFacadeImpl for components that require ISimpleStateFacade
builder.Services.AddScoped<CalibrifyApp.Server.Services.ISimpleStateFacade, CalibrifyApp.Server.Services.SimpleStateFacadeImpl>();

// Add MockStateFacade from CalibrationSaaS.Infraestructure.Blazor.Store for components that require IStateFacade
//builder.Services.AddScoped<Blazed.Controls.IStateFacade, Store.MockStateFacade>();

// Add custom circuit handler to handle JSDisconnectedException
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Server.Circuits.CircuitHandler, CalibrifyApp.Server.Services.CustomCircuitHandler>();

// Add DragDropService for ResponsiveTable components
builder.Services.AddScoped(typeof(Blazed.Controls.DragDropService<>));

// Add ResponsiveTableService
builder.Services.AddTransient<Blazed.Controls.IResponsiveTableService, CalibrifyApp.Server.Store.ResponsiveTableService>();

// Add Blazored.Modal
builder.Services.AddBlazoredModal();

// Add IModalHandler implementations for both interfaces
builder.Services.AddScoped<CalibrifyApp.Server.Services.IModalHandler, CalibrifyApp.Server.Services.ModalHandler>();
// Add the domain interface implementation to fix the "Cannot provide a value for property 'modalHandler'" error
builder.Services.AddScoped<CalibrationSaaS.Domain.Aggregates.Interfaces.IModalHandler>(sp =>
    new CalibrationSaaS.Infraestructure.Blazor.Shared.ModalHandler(sp.GetRequiredService<Blazored.Modal.Services.IModalService>()));

// Add BrowserService
builder.Services.AddScoped<Blazed.Controls.BrowserService, CalibrifyApp.Server.Services.BrowserServiceWrapper>();

// CookieThemeService already registered above

// Add MockAccessTokenProvider for components that require IAccessTokenProvider
builder.Services.AddScoped<Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider, CalibrifyApp.Server.Services.MockAccessTokenProvider>();


// Add AppStateCompany and AppState
builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>();
builder.Services.AddSingleton<CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>();


#region

string AddressEnnpoint = builder.Configuration["GrpcServices:Url"];
CalibrationSaaS.Infraestructure.Blazor.Services.ConnectionStatusService.grpcUrl = AddressEnnpoint;

// Register GrpcRetryPolicy as a singleton
builder.Services.AddSingleton<GrpcRetryPolicy>();

// Register GrpcChannelFactory as a singleton with both logger types
builder.Services.AddSingleton<GrpcChannelFactory>(sp => new GrpcChannelFactory(
    sp.GetRequiredService<ILogger<GrpcChannelFactory>>(),
    sp.GetRequiredService<ILogger<GrpcRetryPolicy>>()));

// Register OptimizedGrpcService as a scoped service
builder.Services.AddScoped(sp => new OptimizedGrpcService(
    sp.GetRequiredService<ILogger<OptimizedGrpcService>>(),
    sp.GetRequiredService<GrpcChannelFactory>(),
    sp,
    AddressEnnpoint));

// Register gRPC services using the factory
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IFileUpload>(AddressEnnpoint));
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IBasicsServices<CallContext>>(AddressEnnpoint));
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IPieceOfEquipmentService<CallContext>>(AddressEnnpoint));
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IWorkOrderDetailServices<CallContext>>(AddressEnnpoint));
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<ICustomerService<CallContext>>(AddressEnnpoint));
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IWorkOrderService<CallContext>>(AddressEnnpoint));
builder.Services.AddTransient(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IAssetsServices<CallContext>>(AddressEnnpoint));

//GrpcServiceFactory.SetToken("your-token-value");
builder.Services.AddScoped<Blazed.Controls.Toast.IToastService, Blazed.Controls.Toast.ToastService>();
// Add TodosState for ResponsiveTableService
builder.Services.AddScoped<IState<CalibrifyApp.Server.Store.State.TodosState>>(provider =>
{
    var state = new CalibrifyApp.Server.Store.State.TodosState(false, null, null, null, null, null);
    return new CalibrifyApp.Server.Store.State.WrappedState<CalibrifyApp.Server.Store.State.TodosState>(state);
});

// Add TodosState for KavokuComponentBase
builder.Services.AddScoped<IState<Helpers.Controls.TodosState>>(provider =>
{
    var state = new Helpers.Controls.TodosState(false, null, null, null, null, null);
    return new CalibrifyApp.Server.Store.State.WrappedState<Helpers.Controls.TodosState>(state);
});
builder.Services.AddFluentUIComponents();
builder.Services.AddScoped<ITooltipService, Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip.TooltipService>();

// Register SQLite services for offline mode
string path = $"Data Source=things.db;Foreign Keys=False";
builder.Services.AddDbContextFactory<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>(
    opts => opts.UseSqlite(path));
builder.Services.AddScoped<IBrowserCache, BrowserCache>();
builder.Services.AddScoped<ISqliteSwap, SqliteSwap>();
builder.Services.AddSingleton<IMigration>(new Migration(false));
builder.Services.AddScoped<ISqliteWasmDbContextFactory<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>, SqliteWasmDbContextFactory<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();
builder.Services.AddScoped<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.DatabaseService2<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();
builder.Services.AddScoped<CacheStorage.Utils.CacheStorageAccessor>();

// Register DbFactory as a dynamic object for components that require it
builder.Services.AddScoped<dynamic>(sp => sp.GetRequiredService<ISqliteWasmDbContextFactory<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>());

// Register factory methods for offline mode
if (!appSecurity.IsReport)
{
    builder.Services.AddTransient<Func<dynamic, IBasicsServices<CallContext>>>(GrpcServiceFactory.BasicsServiceFactorySqlite);

    // Add ISampleService2 for offline mode
    builder.Services.AddTransient<ISampleService2<CallContext>, CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.SampleService2Offline<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();
    builder.Services.AddTransient<Func<dynamic, ISampleService2<CallContext>>>(GrpcServiceFactory.SampleService2FactorySqlite);

    // Add IWorkOrderDetailServices for offline mode
    builder.Services.AddTransient<IWorkOrderDetailServices<CallContext>, CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.WODServiceOffline<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();
    builder.Services.AddTransient<Func<dynamic, IWorkOrderDetailServices<CallContext>>>(GrpcServiceFactory.WODServiceFactorySqlite);

    // Add IUOMService for offline mode
    builder.Services.AddTransient<IUOMService<CallContext>, CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.UOMServiceOffline<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();
    builder.Services.AddTransient<Func<dynamic, IUOMService<CallContext>>>(GrpcServiceFactory.UOMServiceFactorySqlite);

    // Add IAssetsServices for offline mode
    builder.Services.AddTransient<IAssetsServices<CallContext>, CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.AssetsServiceOffline<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>>();
    builder.Services.AddTransient<Func<dynamic, IAssetsServices<CallContext>>>(GrpcServiceFactory.AssetsServicesFactorySqlite);

    // Add ICustomerService factory
    builder.Services.AddTransient<Func<dynamic, ICustomerService<CallContext>>>(GrpcServiceFactory.CustomerServiceFactorySqlite);

    // Add IPieceOfEquipmentService factory
    builder.Services.AddTransient<Func<dynamic, IPieceOfEquipmentService<CallContext>>>(GrpcServiceFactory.PieceOfEquipmentServiceFactorySqlite);

    // Add IAddressServices factory
    builder.Services.AddTransient<IAddressServices>(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IAddressServices>(AddressEnnpoint));
    builder.Services.AddTransient<Func<dynamic, IAddressServices>>(GrpcServiceFactory.AddressServiceFactorySqlite);

    // Add Reports.Domain.ReportViewModels.Repeatability as a singleton
    builder.Services.AddSingleton<Reports.Domain.ReportViewModels.Repeatability>();

    // Add IReportService
    builder.Services.AddTransient<IReportService<CallContext>>(services => services.GetRequiredService<GrpcChannelFactory>().CreateService<IReportService<CallContext>>(AddressEnnpoint));
    builder.Services.AddTransient<Func<dynamic, IReportService<CallContext>>>(GrpcServiceFactory.ReportServiceFactorySqlite);

// Authentication is already configured by AddIdentity above
// Remove conflicting authentication services to use only ASP.NET Core Identity
}


#endregion

var app = builder.Build();


var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
};
forwardingOptions.KnownNetworks.Clear();
forwardingOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardingOptions);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use localization
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Enable response compression
app.UseResponseCompression();

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
// Add authentication and authorization middleware with no-op handlers
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<CalibrifyApp.Server.Components.App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   // Add the Blazor infrastructure assembly first to ensure its routes take precedence
   .AddAdditionalAssemblies(typeof(CalibrationSaaS.Infraestructure.Blazor._Imports).Assembly);

// Configure WebAssembly options
app.UseWebAssemblyDebugging();

// Add middleware to handle circuit disconnections gracefully
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next.Invoke();
});

// Use custom error handling middleware
app.UseErrorHandling();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();


public static class GrpcServiceFactory
{
    private static string _token;

    /// <summary>
    /// Creates a gRPC service client
    /// </summary>
    /// <typeparam name="T">The type of gRPC service to create</typeparam>
    /// <param name="AddressEndPoint">The endpoint address</param>
    /// <returns>A gRPC service client</returns>
    /// <remarks>
    /// This method is deprecated. Use GrpcChannelFactory.CreateService instead.
    /// </remarks>
    [Obsolete("This method is deprecated. Use GrpcChannelFactory.CreateService instead.")]
    public static T CreateService<T>(string AddressEndPoint) where T : class
    {
        Console.WriteLine("Warning: Using deprecated CreateService method. Use GrpcChannelFactory.CreateService instead.");
        return CreateChanel(AddressEndPoint).CreateGrpcService<T>();
    }

    /// <summary>
    /// Creates a gRPC channel
    /// </summary>
    /// <param name="AddressEndPoint">The endpoint address</param>
    /// <returns>A gRPC channel</returns>
    /// <remarks>
    /// This method is deprecated. Use GrpcChannelFactory instead.
    /// </remarks>
    [Obsolete("This method is deprecated. Use GrpcChannelFactory instead.")]
    private static GrpcChannel CreateChanel(string AddressEndPoint)
    {
        Console.WriteLine("Warning: Using deprecated CreateChanel method. Use GrpcChannelFactory instead.");

        var credentials = CallCredentials.FromInterceptor((context, metadata) =>
        {
            if (!string.IsNullOrEmpty(_token))
            {
                metadata.Add("Authorization", $"Bearer {_token}");
            }
            return Task.CompletedTask;
        });

        var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
        {
            Timeout = TimeSpan.FromSeconds(60) // Reduced from 180 seconds to 60 seconds
        };

        var channel = Grpc.Net.Client.GrpcChannel.ForAddress(AddressEndPoint, new GrpcChannelOptions
        {
            HttpClient = httpClient,
            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
        });

        return channel;
    }

    public static void SetToken(string token, IServiceProvider serviceProvider = null)
    {
        _token = token;

        // If service provider is available, update the token in the GrpcChannelFactory
        if (serviceProvider != null)
        {
            try
            {
                var factory = serviceProvider.GetRequiredService<GrpcChannelFactory>();
                factory.SetToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating token in GrpcChannelFactory: {ex.Message}");
            }
        }
    }

    public static Func<IServiceProvider, Func<dynamic, IBasicsServices<CallContext>>> BasicsServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IBasicsServices<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.BasicsServiceOffline<CalibrationSaaSDBContextOff2>(database);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, ISampleService2<CallContext>>> SampleService2FactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<ISampleService2<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.SampleService2Offline<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>(database);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, IWorkOrderDetailServices<CallContext>>> WODServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IWorkOrderDetailServices<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.WODServiceOffline<CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.CalibrationSaaSDBContextOff2>(database);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, IUOMService<CallContext>>> UOMServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IUOMService<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.UOMServiceOffline<CalibrationSaaSDBContextOff2>(database);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, IAssetsServices<CallContext>>> AssetsServicesFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IAssetsServices<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                return new CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite.AssetsServiceOffline<CalibrationSaaSDBContextOff2>(database);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, ICustomerService<CallContext>>> CustomerServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<ICustomerService<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                // If there's no offline implementation, just return the online service
                // This will fail gracefully when offline
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<ICustomerService<CallContext>>(ConnectionStatusService.grpcUrl);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, IPieceOfEquipmentService<CallContext>>> PieceOfEquipmentServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IPieceOfEquipmentService<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                // If there's no offline implementation, just return the online service
                // This will fail gracefully when offline
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IPieceOfEquipmentService<CallContext>>(ConnectionStatusService.grpcUrl);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, IAddressServices>> AddressServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IAddressServices>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                // If there's no offline implementation, just return the online service
                // This will fail gracefully when offline
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IAddressServices>(ConnectionStatusService.grpcUrl);
            }

            throw new ArgumentException("Could not determine location");
        };
    };

    public static Func<IServiceProvider, Func<dynamic, IReportService<CallContext>>> ReportServiceFactorySqlite =>
    service =>
    {
        return database =>
        {
            Console.WriteLine(ConnectionStatusService.GetCurrentStatus());
            if (ConnectionStatusService.GetCurrentStatus())//online
            {
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IReportService<CallContext>>(ConnectionStatusService.grpcUrl);
            }
            else
            {
                // If there's no offline implementation, just return the online service
                // This will fail gracefully when offline
                return service.GetRequiredService<GrpcChannelFactory>().CreateService<IReportService<CallContext>>(ConnectionStatusService.grpcUrl);
            }

            throw new ArgumentException("Could not determine location");
        };
    };
}