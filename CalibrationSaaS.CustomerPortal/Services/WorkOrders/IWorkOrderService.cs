using CalibrationSaaS.CustomerPortal.Models.WorkOrders;

namespace CalibrationSaaS.CustomerPortal.Services.WorkOrders;

public interface IWorkOrderService
{
    Task<WorkOrderSearchResult> SearchWorkOrdersAsync(WorkOrderSearchRequest request, int customerId);
    Task<WorkOrderDto?> GetWorkOrderAsync(int workOrderId, int customerId);
    Task<WorkOrderDto> CreateWorkOrderAsync(WorkOrderDto workOrder, int customerId);
    Task<WorkOrderDto> UpdateWorkOrderAsync(WorkOrderDto workOrder, int customerId);
    Task<bool> DeleteWorkOrderAsync(int workOrderId, int customerId);
    Task<WorkOrderStatistics> GetWorkOrderStatisticsAsync(int customerId);
    Task<byte[]> ExportWorkOrdersAsync(WorkOrderSearchRequest request, int customerId);
}
