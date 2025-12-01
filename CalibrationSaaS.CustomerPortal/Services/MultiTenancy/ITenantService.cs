using Finbuckle.MultiTenant;
using LocalTenantInfo = CalibrationSaaS.CustomerPortal.Models.MultiTenancy.TenantInfo;

namespace CalibrationSaaS.CustomerPortal.Services.MultiTenancy;

/// <summary>
/// Service interface for tenant management operations
/// </summary>
public interface ITenantService
{
    /// <summary>
    /// Get tenant by identifier
    /// </summary>
    /// <param name="identifier">Tenant identifier</param>
    /// <returns>Tenant information or null if not found</returns>
    Task<LocalTenantInfo?> GetTenantAsync(string identifier);

    /// <summary>
    /// Get tenant by ID
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Tenant information or null if not found</returns>
    Task<LocalTenantInfo?> GetTenantByIdAsync(string tenantId);

    /// <summary>
    /// Get all active tenants
    /// </summary>
    /// <returns>List of active tenants</returns>
    Task<List<LocalTenantInfo>> GetActiveTenantsAsync();

    /// <summary>
    /// Create a new tenant
    /// </summary>
    /// <param name="tenantInfo">Tenant information</param>
    /// <returns>Created tenant information</returns>
    Task<LocalTenantInfo> CreateTenantAsync(LocalTenantInfo tenantInfo);

    /// <summary>
    /// Update existing tenant
    /// </summary>
    /// <param name="tenantInfo">Updated tenant information</param>
    /// <returns>Updated tenant information</returns>
    Task<LocalTenantInfo> UpdateTenantAsync(LocalTenantInfo tenantInfo);

    /// <summary>
    /// Deactivate a tenant
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>True if deactivated successfully</returns>
    Task<bool> DeactivateTenantAsync(string tenantId);

    /// <summary>
    /// Activate a tenant
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>True if activated successfully</returns>
    Task<bool> ActivateTenantAsync(string tenantId);

    /// <summary>
    /// Delete a tenant (soft delete)
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>True if deleted successfully</returns>
    Task<bool> DeleteTenantAsync(string tenantId);

    /// <summary>
    /// Check if tenant identifier is available
    /// </summary>
    /// <param name="identifier">Identifier to check</param>
    /// <param name="excludeTenantId">Tenant ID to exclude from check (for updates)</param>
    /// <returns>True if identifier is available</returns>
    Task<bool> IsIdentifierAvailableAsync(string identifier, string? excludeTenantId = null);

    /// <summary>
    /// Get tenant by custom domain
    /// </summary>
    /// <param name="domain">Custom domain</param>
    /// <returns>Tenant information or null if not found</returns>
    Task<LocalTenantInfo?> GetTenantByDomainAsync(string domain);

    /// <summary>
    /// Get tenant connection string
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Connection string for the tenant</returns>
    Task<string?> GetTenantConnectionStringAsync(string tenantId);

    /// <summary>
    /// Update tenant connection string
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="connectionString">New connection string</param>
    /// <returns>True if updated successfully</returns>
    Task<bool> UpdateTenantConnectionStringAsync(string tenantId, string connectionString);

    /// <summary>
    /// Get tenant usage statistics
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Usage statistics</returns>
    Task<TenantUsageStats> GetTenantUsageStatsAsync(string tenantId);

    /// <summary>
    /// Check if tenant has exceeded usage limits
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Usage limit check result</returns>
    Task<TenantUsageLimitResult> CheckUsageLimitsAsync(string tenantId);

    /// <summary>
    /// Initialize tenant database
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>True if initialized successfully</returns>
    Task<bool> InitializeTenantDatabaseAsync(string tenantId);

    /// <summary>
    /// Backup tenant data
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>Backup result with file path or error</returns>
    Task<TenantBackupResult> BackupTenantDataAsync(string tenantId);

    /// <summary>
    /// Restore tenant data from backup
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="backupFilePath">Path to backup file</param>
    /// <returns>True if restored successfully</returns>
    Task<bool> RestoreTenantDataAsync(string tenantId, string backupFilePath);

    /// <summary>
    /// Get tenant audit logs
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="fromDate">Start date</param>
    /// <param name="toDate">End date</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated audit logs</returns>
    Task<PagedResult<TenantAuditLog>> GetTenantAuditLogsAsync(
        string tenantId, 
        DateTime fromDate, 
        DateTime toDate, 
        int pageNumber = 1, 
        int pageSize = 50);

    /// <summary>
    /// Log tenant activity
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="action">Action performed</param>
    /// <param name="details">Action details</param>
    /// <param name="userId">User who performed the action</param>
    /// <param name="ipAddress">IP address</param>
    /// <returns>True if logged successfully</returns>
    Task<bool> LogTenantActivityAsync(
        string tenantId, 
        string action, 
        string? details = null, 
        string? userId = null, 
        string? ipAddress = null);
}

/// <summary>
/// Tenant usage statistics
/// </summary>
public class TenantUsageStats
{
    public string TenantId { get; set; } = string.Empty;
    public int ActiveUsers { get; set; }
    public long StorageUsedMB { get; set; }
    public int ApiCallsToday { get; set; }
    public int ApiCallsThisMonth { get; set; }
    public DateTime LastActivity { get; set; }
    public Dictionary<string, object> CustomMetrics { get; set; } = new();
}

/// <summary>
/// Tenant usage limit check result
/// </summary>
public class TenantUsageLimitResult
{
    public bool IsWithinLimits { get; set; }
    public List<string> ExceededLimits { get; set; } = new();
    public Dictionary<string, object> CurrentUsage { get; set; } = new();
    public Dictionary<string, object> Limits { get; set; } = new();
}

/// <summary>
/// Tenant backup result
/// </summary>
public class TenantBackupResult
{
    public bool Success { get; set; }
    public string? BackupFilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime BackupDate { get; set; }
    public long BackupSizeMB { get; set; }
}

/// <summary>
/// Tenant audit log entry
/// </summary>
public class TenantAuditLog
{
    public int Id { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress { get; set; }
    public DateTime Timestamp { get; set; }
    public string? AdditionalData { get; set; }
}

/// <summary>
/// Paginated result wrapper
/// </summary>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
