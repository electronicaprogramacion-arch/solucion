using CalibrationSaaS.CustomerPortal.Data;
using CalibrationSaaS.CustomerPortal.Services.Authentication;
using CalibrationSaaS.CustomerPortal.Models.MultiTenancy;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using LocalTenantInfo = CalibrationSaaS.CustomerPortal.Models.MultiTenancy.TenantInfo;

namespace CalibrationSaaS.CustomerPortal.Services.Background;

/// <summary>
/// Background service for cleanup tasks
/// </summary>
public class CleanupBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CleanupBackgroundService> _logger;
    private readonly CleanupOptions _options;

    public CleanupBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<CleanupBackgroundService> logger,
        IOptions<CleanupOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleanup background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PerformCleanupTasksAsync();
                await Task.Delay(TimeSpan.FromMinutes(_options.IntervalMinutes), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during cleanup tasks");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Wait 5 minutes before retrying
            }
        }

        _logger.LogInformation("Cleanup background service stopped");
    }

    private async Task PerformCleanupTasksAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        try
        {
            _logger.LogDebug("Starting cleanup tasks for all tenants");

            // Try to get tenants from configuration directly
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var tenants = GetTenantsFromConfiguration(configuration);

            if (!tenants.Any())
            {
                _logger.LogWarning("No tenants configured, performing cleanup on default connection");
                await PerformCleanupForTenantAsync(scope, null);
                return;
            }

            _logger.LogInformation("Found {TenantCount} tenants configured", tenants.Count);

            // Perform cleanup for each tenant
            foreach (var tenant in tenants)
            {
                try
                {
                    _logger.LogDebug("Starting cleanup for tenant: {TenantName} ({TenantId})", tenant.Name, tenant.Identifier);
                    await PerformCleanupForTenantAsync(scope, tenant);
                    _logger.LogDebug("Completed cleanup for tenant: {TenantName}", tenant.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cleanup for tenant: {TenantName} ({TenantId})", tenant.Name, tenant.Identifier);
                    // Continue with other tenants even if one fails
                }
            }

            _logger.LogDebug("Cleanup tasks completed for all tenants");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup tasks");
            throw;
        }
    }

    private async Task PerformCleanupForTenantAsync(IServiceScope scope, LocalTenantInfo? tenant)
    {
        // Create tenant-specific database context
        var dbContextOptions = new DbContextOptionsBuilder<CustomerPortalDbContext>();

        var connectionString = GetTenantConnectionString(tenant);
        dbContextOptions.UseSqlServer(connectionString);

        using var dbContext = new CustomerPortalDbContext(dbContextOptions.Options);

        try
        {
            // Cleanup expired sessions directly
            await CleanupExpiredSessionsAsync(dbContext);

            // Cleanup expired 2FA codes directly
            await CleanupExpired2FACodesAsync(dbContext);

            // Cleanup old audit logs (keep last 90 days)
            await CleanupOldAuditLogsAsync(dbContext);

            // Cleanup orphaned data
            await CleanupOrphanedDataAsync(dbContext);
        }
        catch (Exception ex)
        {
            var tenantName = tenant?.Name ?? "Default";
            _logger.LogError(ex, "Error during cleanup tasks for tenant: {TenantName}", tenantName);
            throw;
        }
    }

    private string GetTenantConnectionString(LocalTenantInfo? tenant)
    {
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection") ??
                                     throw new InvalidOperationException("DefaultConnection not found");

        if (tenant == null)
            return defaultConnectionString;

        if (!string.IsNullOrEmpty(tenant.ConnectionString))
        {
            return tenant.ConnectionString;
        }

        // If no specific connection string, use default with tenant-specific database
        if (!string.IsNullOrEmpty(tenant.Identifier))
        {
            return defaultConnectionString.Replace("{TenantId}", tenant.Identifier);
        }

        return defaultConnectionString;
    }

    private List<LocalTenantInfo> GetTenantsFromConfiguration(IConfiguration configuration)
    {
        var tenants = new List<LocalTenantInfo>();

        try
        {
            // Get tenants from Finbuckle configuration section
            var tenantSection = configuration.GetSection("Finbuckle:MultiTenant:Stores:ConfigurationStore");

            foreach (var tenantConfig in tenantSection.GetChildren())
            {
                var tenant = new LocalTenantInfo
                {
                    Id = tenantConfig["Id"],
                    Identifier = tenantConfig["Identifier"] ?? tenantConfig.Key,
                    Name = tenantConfig["Name"],
                    ConnectionString = tenantConfig["ConnectionString"],
                    CompanyName = tenantConfig["CompanyName"] ?? "",
                    IsActive = tenantConfig.GetValue<bool>("IsActive", true),
                    TimeZone = tenantConfig["TimeZone"] ?? "UTC",
                    Culture = tenantConfig["Culture"] ?? "en-US",
                    ContactEmail = tenantConfig["ContactEmail"],
                    MaxUsers = tenantConfig.GetValue<int>("MaxUsers", 100),
                    MaxStorageMB = tenantConfig.GetValue<long>("MaxStorageMB", 1000),
                    EnableAuditLogging = tenantConfig.GetValue<bool>("EnableAuditLogging", true),
                    EnableEmailNotifications = tenantConfig.GetValue<bool>("EnableEmailNotifications", true)
                };

                if (!string.IsNullOrEmpty(tenant.Identifier))
                {
                    tenants.Add(tenant);
                    _logger.LogDebug("Found tenant: {TenantName} ({TenantId}) with connection: {HasConnection}",
                        tenant.Name, tenant.Identifier, !string.IsNullOrEmpty(tenant.ConnectionString));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading tenant configuration");
        }

        return tenants;
    }

    /// <summary>
    /// Cleanup expired and inactive sessions
    /// </summary>
    private async Task CleanupExpiredSessionsAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            var cutoffTime = DateTime.UtcNow;

            var expiredSessions = await dbContext.CustomerSessions
                .Where(s => s.ExpiresAt < cutoffTime || !s.IsActive)
                .ToListAsync();

            if (expiredSessions.Any())
            {
                dbContext.CustomerSessions.RemoveRange(expiredSessions);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
            }
            else
            {
                _logger.LogDebug("No expired sessions found for cleanup");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired sessions");
            throw;
        }
    }

    /// <summary>
    /// Cleanup expired and used 2FA codes
    /// </summary>
    private async Task CleanupExpired2FACodesAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            var cutoffTime = DateTime.UtcNow;

            var expiredCodes = await dbContext.TwoFactorCodes
                .Where(c => c.ExpiresAt < cutoffTime || c.IsUsed)
                .ToListAsync();

            if (expiredCodes.Any())
            {
                dbContext.TwoFactorCodes.RemoveRange(expiredCodes);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} expired 2FA codes", expiredCodes.Count);
            }
            else
            {
                _logger.LogDebug("No expired 2FA codes found for cleanup");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired 2FA codes");
            throw;
        }
    }

    private async Task CleanupOldAuditLogsAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_options.AuditLogRetentionDays);
            
            var oldLogs = await dbContext.AuthenticationAuditLogs
                .Where(log => log.Timestamp < cutoffDate)
                .ToListAsync();

            if (oldLogs.Any())
            {
                dbContext.AuthenticationAuditLogs.RemoveRange(oldLogs);
                await dbContext.SaveChangesAsync();
                
                _logger.LogInformation("Cleaned up {Count} old audit logs older than {CutoffDate}", 
                    oldLogs.Count, cutoffDate.ToString("yyyy-MM-dd"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old audit logs");
        }
    }

    private async Task CleanupOrphanedDataAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            await CleanupOrphaned2FACodesAsync(dbContext);
            await CleanupOldInactiveSessionsAsync(dbContext);
            await CleanupOldDataProtectionKeysAsync(dbContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up orphaned data");
            throw;
        }
    }

    /// <summary>
    /// Cleanup orphaned 2FA codes (used codes older than 1 hour)
    /// </summary>
    private async Task CleanupOrphaned2FACodesAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);

            var orphanedCodes = await dbContext.TwoFactorCodes
                .Where(code => code.CreatedAt < oneHourAgo && code.IsUsed)
                .ToListAsync();

            if (orphanedCodes.Any())
            {
                dbContext.TwoFactorCodes.RemoveRange(orphanedCodes);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} orphaned 2FA codes", orphanedCodes.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up orphaned 2FA codes");
        }
    }

    /// <summary>
    /// Cleanup old inactive sessions beyond retention period
    /// </summary>
    private async Task CleanupOldInactiveSessionsAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            var sessionCutoff = DateTime.UtcNow.AddDays(-_options.SessionRetentionDays);

            var oldSessions = await dbContext.CustomerSessions
                .Where(session => !session.IsActive &&
                       (session.EndedAt < sessionCutoff ||
                        (session.EndedAt == null && session.CreatedAt < sessionCutoff)))
                .ToListAsync();

            if (oldSessions.Any())
            {
                dbContext.CustomerSessions.RemoveRange(oldSessions);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} old inactive sessions", oldSessions.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old inactive sessions");
        }
    }

    /// <summary>
    /// Cleanup old data protection keys (keep only recent ones)
    /// </summary>
    private async Task CleanupOldDataProtectionKeysAsync(CustomerPortalDbContext dbContext)
    {
        try
        {
            // Keep only the most recent 10 data protection keys
            var keysToKeep = 10;

            var allKeys = await dbContext.DataProtectionKeys
                .OrderByDescending(k => k.Id)
                .ToListAsync();

            if (allKeys.Count > keysToKeep)
            {
                var keysToDelete = allKeys.Skip(keysToKeep).ToList();

                dbContext.DataProtectionKeys.RemoveRange(keysToDelete);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} old data protection keys", keysToDelete.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old data protection keys");
        }
    }
}

/// <summary>
/// Configuration options for cleanup service
/// </summary>
public class CleanupOptions
{
    public const string SectionName = "CleanupSettings";

    /// <summary>
    /// Cleanup interval in minutes
    /// </summary>
    public int IntervalMinutes { get; set; } = 60;

    /// <summary>
    /// Audit log retention period in days
    /// </summary>
    public int AuditLogRetentionDays { get; set; } = 90;

    /// <summary>
    /// Session retention period in days (for inactive sessions)
    /// </summary>
    public int SessionRetentionDays { get; set; } = 30;

    /// <summary>
    /// Enable detailed logging for cleanup operations
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = false;
}

/// <summary>
/// Security middleware for additional protection
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";

        // Add Content Security Policy
        var csp = "default-src 'self'; " +
                 "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net; " +
                 "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net; " +
                 "font-src 'self' https://fonts.gstatic.com; " +
                 "img-src 'self' data: https:; " +
                 "connect-src 'self'; " +
                 "frame-ancestors 'none';";
        
        context.Response.Headers["Content-Security-Policy"] = csp;

        await _next(context);
    }
}

/// <summary>
/// Rate limiting middleware for authentication endpoints
/// </summary>
public class AuthenticationRateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationRateLimitMiddleware> _logger;
    private readonly IMemoryCache _cache;
    private readonly AuthenticationRateLimitOptions _options;

    public AuthenticationRateLimitMiddleware(
        RequestDelegate next,
        ILogger<AuthenticationRateLimitMiddleware> logger,
        IMemoryCache cache,
        IOptions<AuthenticationRateLimitOptions> options)
    {
        _next = next;
        _logger = logger;
        _cache = cache;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAuthenticationEndpoint(context.Request.Path))
        {
            var clientId = GetClientIdentifier(context);
            var rateLimitKey = $"auth_rate_limit_{clientId}";

            var attempts = _cache.GetOrCreate(rateLimitKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.WindowMinutes);
                return new RateLimitInfo { Count = 0, FirstAttempt = DateTime.UtcNow };
            });

            attempts!.Count++;

            if (attempts.Count > _options.MaxAttempts)
            {
                _logger.LogWarning("Rate limit exceeded for client {ClientId} on authentication endpoint {Path}", 
                    clientId, context.Request.Path);

                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                return;
            }

            _cache.Set(rateLimitKey, attempts, TimeSpan.FromMinutes(_options.WindowMinutes));
        }

        await _next(context);
    }

    private bool IsAuthenticationEndpoint(PathString path)
    {
        var authPaths = new[] { "/api/auth/login", "/api/auth/verify", "/login", "/verify" };
        return authPaths.Any(authPath => path.StartsWithSegments(authPath, StringComparison.OrdinalIgnoreCase));
    }

    private string GetClientIdentifier(HttpContext context)
    {
        // Try to get IP address
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        // Check for forwarded IP
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
        {
            ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = ipAddress.Split(',')[0].Trim();
            }
        }

        return ipAddress ?? "unknown";
    }
}

/// <summary>
/// Rate limit tracking information
/// </summary>
public class RateLimitInfo
{
    public int Count { get; set; }
    public DateTime FirstAttempt { get; set; }
}

/// <summary>
/// Authentication rate limiting options
/// </summary>
public class AuthenticationRateLimitOptions
{
    public const string SectionName = "AuthenticationRateLimit";

    /// <summary>
    /// Maximum attempts per window
    /// </summary>
    public int MaxAttempts { get; set; } = 5;

    /// <summary>
    /// Time window in minutes
    /// </summary>
    public int WindowMinutes { get; set; } = 15;

    /// <summary>
    /// Enable rate limiting
    /// </summary>
    public bool Enabled { get; set; } = true;
}
