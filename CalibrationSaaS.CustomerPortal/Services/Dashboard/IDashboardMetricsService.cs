using CalibrationSaaS.CustomerPortal.Models.Dashboard;

namespace CalibrationSaaS.CustomerPortal.Services.Dashboard;

/// <summary>
/// Service for calculating dashboard metrics
/// </summary>
public interface IDashboardMetricsService
{
    /// <summary>
    /// Get dashboard metrics for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dashboard metrics</returns>
    Task<DashboardMetrics> GetDashboardMetricsAsync(int customerId, string tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get total certificates count for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total certificates count</returns>
    Task<int> GetTotalCertificatesAsync(int customerId, string tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get equipment expiring soon count for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="daysAhead">Number of days to look ahead (default: 30)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Equipment expiring soon count</returns>
    Task<int> GetEquipmentExpiringSoonAsync(int customerId, string tenantId, int daysAhead = 30, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get active service requests count for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Active service requests count</returns>
    Task<int> GetActiveServiceRequestsAsync(int customerId, string tenantId, CancellationToken cancellationToken = default);
}
