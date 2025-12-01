using CalibrationSaaS.CustomerPortal.Data;
using Microsoft.EntityFrameworkCore;
using Finbuckle.MultiTenant;
using LocalTenantInfo = CalibrationSaaS.CustomerPortal.Models.MultiTenancy.TenantInfo;

namespace CalibrationSaaS.CustomerPortal.Services.MultiTenancy;

/// <summary>
/// Service implementation for tenant management operations
/// </summary>
public class TenantService : ITenantService
{
    private readonly CustomerPortalDbContext _dbContext;
    private readonly IMultiTenantStore<LocalTenantInfo> _tenantStore;
    private readonly ILogger<TenantService> _logger;
    private readonly IConfiguration _configuration;

    public TenantService(
        CustomerPortalDbContext dbContext,
        IMultiTenantStore<LocalTenantInfo> tenantStore,
        ILogger<TenantService> logger,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _tenantStore = tenantStore;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<LocalTenantInfo?> GetTenantAsync(string identifier)
    {
        try
        {
            return await _tenantStore.TryGetAsync(identifier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant by identifier {Identifier}", identifier);
            return null;
        }
    }

    public async Task<LocalTenantInfo?> GetTenantByIdAsync(string tenantId)
    {
        try
        {
            var tenants = await _tenantStore.GetAllAsync();
            return tenants.FirstOrDefault(t => t.Id == tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant with ID {TenantId}", tenantId);
            return null;
        }
    }

    public async Task<List<LocalTenantInfo>> GetActiveTenantsAsync()
    {
        try
        {
            var tenants = await _tenantStore.GetAllAsync();
            return tenants.Where(t => t.IsActive).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active tenants");
            return new List<LocalTenantInfo>();
        }
    }

    public async Task<LocalTenantInfo> CreateTenantAsync(LocalTenantInfo tenantInfo)
    {
        try
        {
            // Validate tenant info
            var validationErrors = ValidateTenantInfo(tenantInfo);
            if (validationErrors.Any())
            {
                throw new ArgumentException($"Tenant validation failed: {string.Join(", ", validationErrors)}");
            }

            // Check if identifier is available
            if (!await IsIdentifierAvailableAsync(tenantInfo.Identifier!))
            {
                throw new ArgumentException($"Tenant identifier '{tenantInfo.Identifier}' is already in use");
            }

            // Set creation timestamp
            tenantInfo.CreatedAt = DateTime.UtcNow;
            tenantInfo.UpdatedAt = DateTime.UtcNow;

            // Add to store
            var success = await _tenantStore.TryAddAsync(tenantInfo);
            if (!success)
            {
                throw new InvalidOperationException("Failed to create tenant");
            }

            // Initialize tenant database
            await InitializeTenantDatabaseAsync(tenantInfo.Id!);

            _logger.LogInformation("Created new tenant {TenantId} with identifier {Identifier}", 
                tenantInfo.Id, tenantInfo.Identifier);

            return tenantInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant {Identifier}", tenantInfo.Identifier);
            throw;
        }
    }

    public async Task<LocalTenantInfo> UpdateTenantAsync(LocalTenantInfo tenantInfo)
    {
        try
        {
            // Validate tenant info
            var validationErrors = ValidateTenantInfo(tenantInfo);
            if (validationErrors.Any())
            {
                throw new ArgumentException($"Tenant validation failed: {string.Join(", ", validationErrors)}");
            }

            // Check if identifier is available (excluding current tenant)
            if (!await IsIdentifierAvailableAsync(tenantInfo.Identifier!, tenantInfo.Id))
            {
                throw new ArgumentException($"Tenant identifier '{tenantInfo.Identifier}' is already in use");
            }

            // Update timestamp
            tenantInfo.UpdatedAt = DateTime.UtcNow;

            // Update in store
            var success = await _tenantStore.TryUpdateAsync(tenantInfo);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update tenant");
            }

            _logger.LogInformation("Updated tenant {TenantId}", tenantInfo.Id);

            return tenantInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant {TenantId}", tenantInfo.Id);
            throw;
        }
    }

    public async Task<bool> DeactivateTenantAsync(string tenantId)
    {
        try
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }

            tenant.IsActive = false;
            tenant.UpdatedAt = DateTime.UtcNow;

            var success = await _tenantStore.TryUpdateAsync(tenant);
            if (success)
            {
                _logger.LogInformation("Deactivated tenant {TenantId}", tenantId);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating tenant {TenantId}", tenantId);
            return false;
        }
    }

    public async Task<bool> ActivateTenantAsync(string tenantId)
    {
        try
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }

            tenant.IsActive = true;
            tenant.UpdatedAt = DateTime.UtcNow;

            var success = await _tenantStore.TryUpdateAsync(tenant);
            if (success)
            {
                _logger.LogInformation("Activated tenant {TenantId}", tenantId);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating tenant {TenantId}", tenantId);
            return false;
        }
    }

    public async Task<bool> DeleteTenantAsync(string tenantId)
    {
        try
        {
            var success = await _tenantStore.TryRemoveAsync(tenantId);
            if (success)
            {
                _logger.LogInformation("Deleted tenant {TenantId}", tenantId);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tenant {TenantId}", tenantId);
            return false;
        }
    }

    public async Task<bool> IsIdentifierAvailableAsync(string identifier, string? excludeTenantId = null)
    {
        try
        {
            var tenants = await _tenantStore.GetAllAsync();
            return !tenants.Any(t => t.Identifier == identifier && t.Id != excludeTenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking identifier availability for {Identifier}", identifier);
            return false;
        }
    }

    public async Task<LocalTenantInfo?> GetTenantByDomainAsync(string domain)
    {
        try
        {
            var tenants = await _tenantStore.GetAllAsync();
            return tenants.FirstOrDefault(t => t.CustomDomain == domain);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant by domain {Domain}", domain);
            return null;
        }
    }

    public async Task<string?> GetTenantConnectionStringAsync(string tenantId)
    {
        try
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return null;
            }

            var defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");
            return GetTenantConnectionString(tenant, defaultConnectionString!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving connection string for tenant {TenantId}", tenantId);
            return null;
        }
    }

    public async Task<bool> UpdateTenantConnectionStringAsync(string tenantId, string connectionString)
    {
        try
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }

            tenant.ConnectionString = connectionString;
            tenant.UpdatedAt = DateTime.UtcNow;

            var success = await _tenantStore.TryUpdateAsync(tenant);
            if (success)
            {
                _logger.LogInformation("Updated connection string for tenant {TenantId}", tenantId);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating connection string for tenant {TenantId}", tenantId);
            return false;
        }
    }

    public async Task<TenantUsageStats> GetTenantUsageStatsAsync(string tenantId)
    {
        // TODO: Implement actual usage statistics collection
        // This would typically involve querying various tables to get usage metrics
        await Task.Delay(1); // Placeholder

        return new TenantUsageStats
        {
            TenantId = tenantId,
            ActiveUsers = 0,
            StorageUsedMB = 0,
            ApiCallsToday = 0,
            ApiCallsThisMonth = 0,
            LastActivity = DateTime.UtcNow
        };
    }

    public async Task<TenantUsageLimitResult> CheckUsageLimitsAsync(string tenantId)
    {
        try
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return new TenantUsageLimitResult { IsWithinLimits = false };
            }

            var stats = await GetTenantUsageStatsAsync(tenantId);
            var result = new TenantUsageLimitResult { IsWithinLimits = true };

            // Check user limit
            if (stats.ActiveUsers > tenant.MaxUsers)
            {
                result.IsWithinLimits = false;
                result.ExceededLimits.Add($"Active users ({stats.ActiveUsers}) exceeds limit ({tenant.MaxUsers})");
            }

            // Check storage limit
            if (stats.StorageUsedMB > tenant.MaxStorageMB)
            {
                result.IsWithinLimits = false;
                result.ExceededLimits.Add($"Storage usage ({stats.StorageUsedMB}MB) exceeds limit ({tenant.MaxStorageMB}MB)");
            }

            result.CurrentUsage["ActiveUsers"] = stats.ActiveUsers;
            result.CurrentUsage["StorageUsedMB"] = stats.StorageUsedMB;
            result.Limits["MaxUsers"] = tenant.MaxUsers;
            result.Limits["MaxStorageMB"] = tenant.MaxStorageMB;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking usage limits for tenant {TenantId}", tenantId);
            return new TenantUsageLimitResult { IsWithinLimits = false };
        }
    }

    public async Task<bool> InitializeTenantDatabaseAsync(string tenantId)
    {
        try
        {
            var tenant = await GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }

            // TODO: Implement tenant database initialization
            // This would typically involve creating the database schema for the tenant
            await Task.Delay(1); // Placeholder

            _logger.LogInformation("Initialized database for tenant {TenantId}", tenantId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing database for tenant {TenantId}", tenantId);
            return false;
        }
    }

    public async Task<TenantBackupResult> BackupTenantDataAsync(string tenantId)
    {
        // TODO: Implement tenant backup functionality
        await Task.Delay(1); // Placeholder

        return new TenantBackupResult
        {
            Success = false,
            ErrorMessage = "Backup functionality not yet implemented"
        };
    }

    public async Task<bool> RestoreTenantDataAsync(string tenantId, string backupFilePath)
    {
        // TODO: Implement tenant restore functionality
        await Task.Delay(1); // Placeholder
        return false;
    }

    public async Task<PagedResult<TenantAuditLog>> GetTenantAuditLogsAsync(
        string tenantId, 
        DateTime fromDate, 
        DateTime toDate, 
        int pageNumber = 1, 
        int pageSize = 50)
    {
        // TODO: Implement audit log retrieval
        await Task.Delay(1); // Placeholder

        return new PagedResult<TenantAuditLog>
        {
            Items = new List<TenantAuditLog>(),
            TotalCount = 0,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<bool> LogTenantActivityAsync(
        string tenantId, 
        string action, 
        string? details = null, 
        string? userId = null, 
        string? ipAddress = null)
    {
        try
        {
            // TODO: Implement audit logging
            await Task.Delay(1); // Placeholder

            _logger.LogInformation("Tenant activity logged: {TenantId} - {Action}", tenantId, action);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging tenant activity for {TenantId}", tenantId);
            return false;
        }
    }

    /// <summary>
    /// Validate tenant information
    /// </summary>
    /// <param name="tenantInfo">Tenant info to validate</param>
    /// <returns>List of validation errors</returns>
    private static List<string> ValidateTenantInfo(LocalTenantInfo tenantInfo)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(tenantInfo.Id))
            errors.Add("Tenant ID is required");

        if (string.IsNullOrWhiteSpace(tenantInfo.Identifier))
            errors.Add("Tenant Identifier is required");

        if (string.IsNullOrWhiteSpace(tenantInfo.Name))
            errors.Add("Tenant Name is required");

        return errors;
    }

    /// <summary>
    /// Get tenant-specific database connection string
    /// </summary>
    /// <param name="tenantInfo">Tenant information</param>
    /// <param name="defaultConnectionString">Default connection string template</param>
    /// <returns>Tenant-specific connection string</returns>
    private static string GetTenantConnectionString(LocalTenantInfo tenantInfo, string defaultConnectionString)
    {
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
}
