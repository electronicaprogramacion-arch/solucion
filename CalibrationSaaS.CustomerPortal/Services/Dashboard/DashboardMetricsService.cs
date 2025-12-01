using CalibrationSaaS.CustomerPortal.Models.Dashboard;
using CalibrationSaaS.CustomerPortal.Services.GrpcClients;
using CalibrationSaaS.CustomerPortal.Services.Certificates;

namespace CalibrationSaaS.CustomerPortal.Services.Dashboard;

/// <summary>
/// Service for calculating dashboard metrics using gRPC services
/// </summary>
public class DashboardMetricsService : IDashboardMetricsService
{
    private readonly ILogger<DashboardMetricsService> _logger;
    private readonly IWorkOrderGrpcService _workOrderService;
    private readonly ICertificateService _certificateService;

    public DashboardMetricsService(
        ILogger<DashboardMetricsService> logger,
        IWorkOrderGrpcService workOrderService,
        ICertificateService certificateService)
    {
        _logger = logger;
        _workOrderService = workOrderService;
        _certificateService = certificateService;
    }

    public async Task<DashboardMetrics> GetDashboardMetricsAsync(int customerId, string tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Calculating dashboard metrics for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);

            // Execute all metric calculations in parallel for better performance
            var totalCertificatesTask = GetTotalCertificatesAsync(customerId, tenantId, cancellationToken);
            var equipmentExpiringSoonTask = GetEquipmentExpiringSoonAsync(customerId, tenantId, 30, cancellationToken);
            var activeServiceRequestsTask = GetActiveServiceRequestsAsync(customerId, tenantId, cancellationToken);

            // Wait for all tasks to complete
            await Task.WhenAll(totalCertificatesTask, equipmentExpiringSoonTask, activeServiceRequestsTask);

            // Get equipment due dates for additional metrics
            var equipmentDueDates = await _workOrderService.GetEquipmentDueDatesAsync(customerId, tenantId, 365, cancellationToken);

            var totalEquipment = equipmentDueDates?.Count ?? 0;
            var equipmentOverdue = equipmentDueDates?.Count(e => e.DueStatus == "Overdue") ?? 0;
            var equipmentCurrent = equipmentDueDates?.Count(e => e.DueStatus == "Current") ?? 0;

            // Calculate compliance rate
            var complianceRate = totalEquipment > 0 ? ((double)equipmentCurrent / totalEquipment) * 100 : 0.0;

            var metrics = new DashboardMetrics
            {
                CustomerId = customerId,
                TenantId = tenantId,
                TotalCertificates = await totalCertificatesTask,
                EquipmentExpiringSoon = await equipmentExpiringSoonTask,
                ActiveServiceRequests = await activeServiceRequestsTask,
                TotalEquipment = totalEquipment,
                EquipmentOverdue = equipmentOverdue,
                EquipmentCurrent = equipmentCurrent,
                ComplianceRate = complianceRate,
                CalculatedAt = DateTime.UtcNow
            };

            _logger.LogInformation("Dashboard metrics calculated: {Metrics}", System.Text.Json.JsonSerializer.Serialize(metrics));
            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating dashboard metrics for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);
            
            // Return empty metrics on error
            return new DashboardMetrics
            {
                CustomerId = customerId,
                TenantId = tenantId,
                CalculatedAt = DateTime.UtcNow
            };
        }
    }

    public async Task<int> GetTotalCertificatesAsync(int customerId, string tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Use certificate service to get total count of completed certificates
            var searchRequest = new Models.Certificates.CertificateSearchRequest
            {
                Page = 1,
                PageSize = 1, // We only need the count, not the actual data
                SortBy = "IssueDate",
                SortDirection = "desc"
            };

            var result = await _certificateService.SearchCertificatesAsync(searchRequest, customerId, cancellationToken);
            return result.TotalCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total certificates for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);
            return 0;
        }
    }

    public async Task<int> GetEquipmentExpiringSoonAsync(int customerId, string tenantId, int daysAhead = 30, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get equipment due dates and filter for those expiring within the specified days
            var equipmentDueDates = await _workOrderService.GetEquipmentDueDatesAsync(customerId, tenantId, daysAhead, cancellationToken);
            
            if (equipmentDueDates == null)
                return 0;

            var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);
            
            // Count equipment that is due within the next 'daysAhead' days
            return equipmentDueDates.Count(e => 
                e.NextDueDate.HasValue && 
                e.NextDueDate.Value <= cutoffDate && 
                e.NextDueDate.Value >= DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipment expiring soon for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);
            return 0;
        }
    }

    public async Task<int> GetActiveServiceRequestsAsync(int customerId, string tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get work orders and count those that are still active/open
            var workOrdersResult = await _workOrderService.GetWorkOrdersAsync(customerId, tenantId, 1, 1000, cancellationToken: cancellationToken);
            
            if (workOrdersResult?.WorkOrders == null)
                return 0;

            // Count work orders that are not completed (active service requests)
            return workOrdersResult.WorkOrders.Count(wo => 
                wo.Status != "Completed" && 
                wo.Status != "Cancelled" && 
                wo.Status != "Closed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active service requests for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);
            return 0;
        }
    }
}
