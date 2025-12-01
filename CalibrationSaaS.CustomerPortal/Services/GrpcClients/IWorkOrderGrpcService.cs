using Microsoft.Extensions.Logging;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using CalibrationSaaS.Application.Services;
using ProtoBuf.Grpc;
using System.Linq;

namespace CalibrationSaaS.CustomerPortal.Services.GrpcClients;

/// <summary>
/// gRPC service interface for work order operations
/// </summary>
public interface IWorkOrderGrpcService
{
    /// <summary>
    /// Get work orders for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="fromDate">Start date filter</param>
    /// <param name="toDate">End date filter</param>
    /// <returns>Paginated work orders</returns>
    Task<PagedWorkOrderResult> GetWorkOrdersAsync(
        int customerId,
        string tenantId,
        int pageNumber = 1,
        int pageSize = 50,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get work order by ID
    /// </summary>
    /// <param name="workOrderId">Work order ID</param>
    /// <param name="customerId">Customer ID for authorization</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>Work order details or null if not found</returns>
    Task<WorkOrderDto?> GetWorkOrderByIdAsync(int workOrderId, int customerId, string tenantId);

    /// <summary>
    /// Get work order details for a work order
    /// </summary>
    /// <param name="workOrderId">Work order ID</param>
    /// <param name="customerId">Customer ID for authorization</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>List of work order details</returns>
    Task<List<WorkOrderDetailDto>> GetWorkOrderDetailsAsync(int workOrderId, int customerId, string tenantId);

    /// <summary>
    /// Get a specific work order detail by ID
    /// </summary>
    /// <param name="workOrderDetailId">Work order detail ID</param>
    /// <param name="customerId">Customer ID for authorization</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Work order detail or null if not found</returns>
    Task<WorkOrderDetailDto?> GetWorkOrderDetailByIdAsync(int workOrderDetailId, int customerId, string tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get equipment due dates for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="daysAhead">Number of days ahead to look for due dates</param>
    /// <returns>List of equipment with due dates</returns>
    Task<List<EquipmentDueDateDto>> GetEquipmentDueDatesAsync(int customerId, string tenantId, int daysAhead = 90, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get calibration history for equipment
    /// </summary>
    /// <param name="equipmentId">Equipment ID</param>
    /// <param name="customerId">Customer ID for authorization</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated calibration history</returns>
    Task<PagedCalibrationHistoryResult> GetCalibrationHistoryAsync(
        int equipmentId, 
        int customerId, 
        string tenantId, 
        int pageNumber = 1, 
        int pageSize = 20);

    /// <summary>
    /// Search equipment for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated equipment search results</returns>
    Task<PagedEquipmentResult> SearchEquipmentAsync(
        int customerId,
        string tenantId,
        string searchTerm,
        int pageNumber = 1,
        int pageSize = 50);

    /// <summary>
    /// Generate certificate PDF for a specific certificate
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PDF data as byte array</returns>
    Task<byte[]?> GenerateCertificatePdfAsync(int certificateId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Work order DTO for gRPC communication
/// </summary>
public class WorkOrderDto
{
    public int WorkOrderId { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string TechnicianName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Get status display with CSS class
    /// </summary>
    public string StatusCssClass => Status?.ToLower() switch
    {
        "completed" => "badge bg-success",
        "in progress" => "badge bg-primary",
        "scheduled" => "badge bg-info",
        "cancelled" => "badge bg-danger",
        _ => "badge bg-secondary"
    };

    /// <summary>
    /// Get formatted work order number for display
    /// </summary>
    public string DisplayNumber => string.IsNullOrEmpty(WorkOrderNumber) ? $"WO-{WorkOrderId}" : WorkOrderNumber;
}

/// <summary>
/// Work order detail DTO for gRPC communication
/// </summary>
public class WorkOrderDetailDto
{
    public int WorkOrderDetailId { get; set; }
    public int WorkOrderId { get; set; }
    public int EquipmentId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string EquipmentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CalibrationDate { get; set; }
    public DateTime? NextDueDate { get; set; }
    public string CalibrationStatus { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public bool CertificateAvailable { get; set; }
    public bool IsCompleted { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal ServiceAmount { get; set; }

    /// <summary>
    /// Get equipment display name
    /// </summary>
    public string EquipmentDisplay => $"{Manufacturer} {Model} ({SerialNumber})";

    /// <summary>
    /// Get calibration status CSS class
    /// </summary>
    public string StatusCssClass => CalibrationStatus?.ToLower() switch
    {
        "pass" => "badge bg-success",
        "fail" => "badge bg-danger",
        "limited" => "badge bg-warning",
        "in progress" => "badge bg-primary",
        _ => "badge bg-secondary"
    };

    /// <summary>
    /// Check if equipment is overdue
    /// </summary>
    public bool IsOverdue => NextDueDate.HasValue && NextDueDate.Value < DateTime.Today;

    /// <summary>
    /// Check if equipment is due soon (within 30 days)
    /// </summary>
    public bool IsDueSoon => NextDueDate.HasValue && 
                            NextDueDate.Value >= DateTime.Today && 
                            NextDueDate.Value <= DateTime.Today.AddDays(30);
}

/// <summary>
/// Equipment due date DTO for gRPC communication
/// </summary>
public class EquipmentDueDateDto
{
    public int EquipmentId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string EquipmentType { get; set; } = string.Empty;
    public DateTime? LastCalibrationDate { get; set; }
    public DateTime? NextDueDate { get; set; }
    public int CalibrationIntervalMonths { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int CustomerId { get; set; }

    /// <summary>
    /// Get equipment display name
    /// </summary>
    public string EquipmentDisplay => $"{Manufacturer} {Model}";

    /// <summary>
    /// Get days until due
    /// </summary>
    public int DaysUntilDue => NextDueDate.HasValue ? (NextDueDate.Value - DateTime.Today).Days : 0;

    /// <summary>
    /// Get due status
    /// </summary>
    public string DueStatus
    {
        get
        {
            if (!NextDueDate.HasValue) return "Unknown";
            
            var daysUntilDue = DaysUntilDue;
            return daysUntilDue switch
            {
                < 0 => "Overdue",
                <= 30 => "Due Soon",
                <= 90 => "Due Later",
                _ => "Current"
            };
        }
    }

    /// <summary>
    /// Get due status CSS class
    /// </summary>
    public string DueStatusCssClass => DueStatus switch
    {
        "Overdue" => "badge bg-danger",
        "Due Soon" => "badge bg-warning",
        "Due Later" => "badge bg-info",
        "Current" => "badge bg-success",
        _ => "badge bg-secondary"
    };

    /// <summary>
    /// Get next due date display
    /// </summary>
    public string NextDueDateDisplay => NextDueDate?.ToString("MMM dd, yyyy") ?? "Not Set";
}

/// <summary>
/// Calibration history DTO for gRPC communication
/// </summary>
public class CalibrationHistoryDto
{
    public int WorkOrderDetailId { get; set; }
    public int WorkOrderId { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public DateTime CalibrationDate { get; set; }
    public string CalibrationStatus { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public string TechnicianName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool CertificateAvailable { get; set; }

    /// <summary>
    /// Get calibration date display
    /// </summary>
    public string CalibrationDateDisplay => CalibrationDate.ToString("MMM dd, yyyy");

    /// <summary>
    /// Get status CSS class
    /// </summary>
    public string StatusCssClass => CalibrationStatus?.ToLower() switch
    {
        "pass" => "badge bg-success",
        "fail" => "badge bg-danger",
        "limited" => "badge bg-warning",
        _ => "badge bg-secondary"
    };
}

/// <summary>
/// Equipment DTO for gRPC communication
/// </summary>
public class EquipmentDto
{
    public int EquipmentId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string EquipmentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? LastCalibrationDate { get; set; }
    public DateTime? NextDueDate { get; set; }
    public int CalibrationIntervalMonths { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// Get equipment display name
    /// </summary>
    public string EquipmentDisplay => $"{Manufacturer} {Model} ({SerialNumber})";
}

/// <summary>
/// Paginated work order result
/// </summary>
public class PagedWorkOrderResult
{
    public List<WorkOrderDto> WorkOrders { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// Paginated calibration history result
/// </summary>
public class PagedCalibrationHistoryResult
{
    public List<CalibrationHistoryDto> History { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// Paginated equipment result
/// </summary>
public class PagedEquipmentResult
{
    public List<EquipmentDto> Equipment { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// gRPC service implementation for work order operations
/// </summary>
public class WorkOrderGrpcService : IWorkOrderGrpcService
{
    private readonly ILogger<WorkOrderGrpcService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITenantGrpcChannelFactory _channelFactory;

    public WorkOrderGrpcService(
        ILogger<WorkOrderGrpcService> logger,
        IConfiguration configuration,
        ITenantGrpcChannelFactory channelFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _channelFactory = channelFactory;
    }

    public async Task<PagedWorkOrderResult> GetWorkOrdersAsync(
        int customerId,
        string tenantId,
        int pageNumber = 1,
        int pageSize = 50,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("🔍 === GRPC SERVICE CALL: GetWorkOrdersAsync ===");
            _logger.LogInformation("🆔 CustomerId: {CustomerId}", customerId);
            _logger.LogInformation("🏢 TenantId: {TenantId}", tenantId);

            // Get tenant-specific gRPC channel and create service
            var grpcChannel = await _channelFactory.GetChannelAsync(tenantId);
            var workOrderService = grpcChannel.CreateGrpcService<IWorkOrderService<CallContext>>();

            // Call actual gRPC service
            var grpcResult = await workOrderService.GetWorkOrders(new CallContext());

            if (grpcResult?.WorkOrders != null)
            {
                // Map string tenant identifier to integer tenant ID
                var tenantIdInt = MapTenantIdentifierToId(tenantId);

                // Filter work orders by customer ID and tenant
                var filteredWorkOrders = grpcResult.WorkOrders
                    .Where(wo => wo.CustomerId == customerId && wo.TenantId == tenantIdInt)
                    .Select(wo => new WorkOrderDto
                    {
                        WorkOrderId = wo.WorkOrderId,
                        WorkOrderNumber = wo.ControlNumber ?? $"WO-{wo.WorkOrderId}",
                        CustomerId = wo.CustomerId,
                        CustomerName = wo.Customer?.Name ?? "Unknown",
                        CreatedDate = wo.WorkOrderDate,
                        ScheduledDate = wo.ScheduledDate,
                        CompletedDate = wo.CalibrationDate,
                        Status = wo.StatusID?.ToString() ?? "Unknown",
                        Description = wo.Description ?? "",
                        TenantId = wo.TenantId.ToString(),
                        TechnicianName = "",
                        TotalAmount = 0,
                        Notes = ""
                    })
                    .ToList();

                // Apply pagination
                var totalCount = filteredWorkOrders.Count;
                var pagedWorkOrders = filteredWorkOrders
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation("✅ Found {Count} work orders for customer {CustomerId}", totalCount, customerId);

                return new PagedWorkOrderResult
                {
                    WorkOrders = pagedWorkOrders,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }

            // Fallback to empty result
            return new PagedWorkOrderResult
            {
                WorkOrders = new List<WorkOrderDto>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting work orders for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);
            return new PagedWorkOrderResult { PageNumber = pageNumber, PageSize = pageSize };
        }
    }

    public async Task<WorkOrderDto?> GetWorkOrderByIdAsync(int workOrderId, int customerId, string tenantId)
    {
        try
        {
            // TODO: Implement actual gRPC call to CalibrationSaaS service
            await Task.Delay(100); // Simulate network call

            // Mock data
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting work order {WorkOrderId} for customer {CustomerId} in tenant {TenantId}",
                workOrderId, customerId, tenantId);
            return null;
        }
    }

    public async Task<List<WorkOrderDetailDto>> GetWorkOrderDetailsAsync(int workOrderId, int customerId, string tenantId)
    {
        try
        {
            _logger.LogInformation("🔍 === GRPC SERVICE CALL: GetWorkOrderDetailsAsync ===");
            _logger.LogInformation("🆔 WorkOrderId: {WorkOrderId}", workOrderId);
            _logger.LogInformation("🆔 CustomerId: {CustomerId}", customerId);
            _logger.LogInformation("🏢 TenantId: {TenantId}", tenantId);

            // Get tenant-specific gRPC channel and create service
            var grpcChannel = await _channelFactory.GetChannelAsync(tenantId);
            var workOrderDetailService = grpcChannel.CreateGrpcService<IWorkOrderDetailServices<CallContext>>();

            // Create WorkOrder DTO for the gRPC call
            var workOrderDto = new CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder
            {
                WorkOrderId = workOrderId
            };

            // Call actual gRPC service to get work order details
            var grpcResult = await workOrderDetailService.GetWorkOrderDetailXWorkOrder(workOrderDto, new CallContext());

            if (grpcResult?.WorkOrderDetails != null)
            {
                _logger.LogInformation("✅ Found {Count} work order details from gRPC service", grpcResult.WorkOrderDetails.Count);

                var workOrderDetails = grpcResult.WorkOrderDetails
                    .Select(wod => new WorkOrderDetailDto
                    {
                        WorkOrderDetailId = wod.WorkOrderDetailID,
                        WorkOrderId = wod.WorkOrderID,
                        EquipmentId = ParseEquipmentId(wod.PieceOfEquipmentId),
                        SerialNumber = wod.PieceOfEquipment?.SerialNumber ?? "N/A",
                        Manufacturer = wod.PieceOfEquipment?.EquipmentTemplate?.Manufacturer1?.Name ?? wod.PieceOfEquipment?.EquipmentTemplate?.Manufacturer ?? "Unknown",
                        Model = wod.PieceOfEquipment?.EquipmentTemplate?.Model ?? "Unknown",
                        Description = wod.PieceOfEquipment?.Description ?? "Unknown",
                        CalibrationStatus = wod.CurrentStatus?.Name ?? "Unknown",
                        Notes = wod.TechnicianComment ?? string.Empty,
                        NextDueDate = wod.PieceOfEquipment?.DueDate, // Fixed: use DueDate property
                        IsCompleted = wod.HasBeenCompleted || (wod.CurrentStatus?.IsLast == true)
                    })
                    .ToList();

                return workOrderDetails ?? new List<WorkOrderDetailDto>(); // Fixed nullability issue
            }

            _logger.LogWarning("📋 No work order details returned from gRPC service for WorkOrder {WorkOrderId}", workOrderId);
            return new List<WorkOrderDetailDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting work order details for {WorkOrderId}, customer {CustomerId} in tenant {TenantId}",
                workOrderId, customerId, tenantId);
            return new List<WorkOrderDetailDto>();
        }
    }

    public async Task<WorkOrderDetailDto?> GetWorkOrderDetailByIdAsync(int workOrderDetailId, int customerId, string tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Implement actual gRPC call to CalibrationSaaS service
            await Task.Delay(100); // Simulate network call

            // For now, return null since this is mock implementation
            // In real implementation, this would call the gRPC service to get the specific work order detail
            _logger.LogWarning("GetWorkOrderDetailByIdAsync not yet implemented - returning null for detail {WorkOrderDetailId}", workOrderDetailId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting work order detail {WorkOrderDetailId} for customer {CustomerId} in tenant {TenantId}",
                workOrderDetailId, customerId, tenantId);
            return null;
        }
    }

    public async Task<List<EquipmentDueDateDto>> GetEquipmentDueDatesAsync(int customerId, string tenantId, int daysAhead = 90, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("🔍 === GRPC SERVICE CALL: GetEquipmentDueDatesAsync ===");
            _logger.LogInformation("🆔 CustomerId: {CustomerId}", customerId);
            _logger.LogInformation("🏢 TenantId: {TenantId}", tenantId);
            _logger.LogInformation("📅 DaysAhead: {DaysAhead}", daysAhead);

            // Get tenant-specific gRPC channel and create service
            var grpcChannel = await _channelFactory.GetChannelAsync(tenantId);
            var equipmentRecallService = grpcChannel.CreateGrpcService<IEquipmentRecallService<CallContext>>();

            try
            {
                // Create filter for equipment recalls
                var filter = new Domain.Aggregates.Entities.EquipmentRecallFilter
                {
                    CustomerID = customerId,
                    FinalDueDate = DateTime.UtcNow.AddDays(daysAhead),
                    IncludeOverdue = true,
                    PageSize = 1000 // Get all equipment
                };

                // Call actual gRPC service to get equipment recalls
                var grpcResult = await equipmentRecallService.GetEquipmentRecalls(filter, new CallContext());
                _logger.LogInformation("✅ Successfully retrieved {Count} equipment recalls from gRPC service", grpcResult?.EquipmentRecalls?.Count ?? 0);

                if (grpcResult?.EquipmentRecalls != null)
                {
                    // Log equipment recall details for debugging
                    _logger.LogInformation("🔍 Analyzing {Count} equipment recalls from gRPC service", grpcResult.EquipmentRecalls.Count);
                    foreach (var recall in grpcResult.EquipmentRecalls.Take(3)) // Log first 3 recalls
                    {
                        _logger.LogInformation("📋 EquipmentRecall: ID={EquipmentId}, CustomerId={CustomerId}, SerialNumber={SerialNumber}, DueDate={DueDate}",
                            recall.PieceOfEquipmentID, recall.CustomerID, recall.SerialNumber, recall.DueDate);
                    }

                    var equipmentDueDates = grpcResult.EquipmentRecalls
                        .Select(recall => new EquipmentDueDateDto
                        {
                            EquipmentId = ParseEquipmentId(recall.PieceOfEquipmentID),
                            SerialNumber = recall.SerialNumber ?? "N/A",
                            Manufacturer = "Unknown", // Not available in EquipmentRecall
                            Model = "Unknown", // Not available in EquipmentRecall
                            EquipmentType = recall.Description ?? "Unknown",
                            LastCalibrationDate = recall.CalibrationDate,
                            NextDueDate = recall.DueDate,
                            CalibrationIntervalMonths = 12, // Default to 12 months
                            Location = "Unknown", // Not available in EquipmentRecall
                            Status = GetDueStatus(recall.DueDate, daysAhead),
                            CustomerName = recall.CustomerName ?? "Unknown",
                            CustomerId = recall.CustomerID
                        })
                        .OrderBy(e => e.NextDueDate)
                        .ToList();

                    _logger.LogInformation("✅ Found {Count} equipment due dates for customer {CustomerId}", equipmentDueDates.Count, customerId);
                    return equipmentDueDates;
                }
            }
            catch (Exception grpcEx)
            {
                _logger.LogWarning(grpcEx, "Failed to get equipment due dates from CalibrationSaaS gRPC service, falling back to mock data");
            }

            // Fallback to mock data for demo purposes
            _logger.LogInformation("📋 Using mock equipment due dates for customer {CustomerId}", customerId);
            return new List<EquipmentDueDateDto>
            {
                new EquipmentDueDateDto
                {
                    EquipmentId = 1,
                    SerialNumber = "SN001",
                    Manufacturer = "Fluke",
                    Model = "87V",
                    EquipmentType = "Digital Multimeter",
                    LastCalibrationDate = DateTime.UtcNow.AddDays(-300),
                    NextDueDate = DateTime.UtcNow.AddDays(30),
                    CalibrationIntervalMonths = 12,
                    Location = "Lab A",
                    Status = "Due Soon"
                },
                new EquipmentDueDateDto
                {
                    EquipmentId = 2,
                    SerialNumber = "SN002",
                    Manufacturer = "Ashcroft",
                    Model = "1009",
                    EquipmentType = "Pressure Gauge",
                    LastCalibrationDate = DateTime.UtcNow.AddDays(-200),
                    NextDueDate = DateTime.UtcNow.AddDays(15),
                    CalibrationIntervalMonths = 12,
                    Location = "Lab B",
                    Status = "Due Soon"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipment due dates for customer {CustomerId} in tenant {TenantId}", customerId, tenantId);
            return new List<EquipmentDueDateDto>();
        }
    }

    private string GetDueStatus(DateTime? nextDueDate, int daysAhead)
    {
        if (!nextDueDate.HasValue)
            return "Unknown";

        var daysUntilDue = (nextDueDate.Value - DateTime.UtcNow).Days;

        if (daysUntilDue < 0)
            return "Overdue";
        else if (daysUntilDue <= 30)
            return "Due Soon";
        else if (daysUntilDue <= daysAhead)
            return "Current";
        else
            return "Current";
    }

    public async Task<PagedCalibrationHistoryResult> GetCalibrationHistoryAsync(
        int equipmentId,
        int customerId,
        string tenantId,
        int pageNumber = 1,
        int pageSize = 20)
    {
        try
        {
            // TODO: Implement actual gRPC call to CalibrationSaaS service
            await Task.Delay(100); // Simulate network call

            // Mock data
            return new PagedCalibrationHistoryResult
            {
                History = new List<CalibrationHistoryDto>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting calibration history for equipment {EquipmentId}, customer {CustomerId} in tenant {TenantId}",
                equipmentId, customerId, tenantId);
            return new PagedCalibrationHistoryResult { PageNumber = pageNumber, PageSize = pageSize };
        }
    }

    public async Task<PagedEquipmentResult> SearchEquipmentAsync(
        int customerId,
        string tenantId,
        string searchTerm,
        int pageNumber = 1,
        int pageSize = 50)
    {
        try
        {
            // TODO: Implement actual gRPC call to CalibrationSaaS service
            await Task.Delay(100); // Simulate network call

            // Mock data
            return new PagedEquipmentResult
            {
                Equipment = new List<EquipmentDto>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching equipment for customer {CustomerId} in tenant {TenantId} with term '{SearchTerm}'",
                customerId, tenantId, searchTerm);
            return new PagedEquipmentResult { PageNumber = pageNumber, PageSize = pageSize };
        }
    }

    public async Task<byte[]?> GenerateCertificatePdfAsync(int certificateId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("🎬 DEMO MODE: Generating certificate PDF for certificate {CertificateId}", certificateId);

            // Demo: Return the specific PDF file for demo purposes
            // Use relative path from the application's content root
            var contentRoot = Directory.GetCurrentDirectory();
            var pdfPath = Path.Combine(contentRoot, "Calibration_Certificate_Kavoku_YU6754.pdf");

            _logger.LogInformation("🔍 Looking for demo PDF at: {PdfPath}", pdfPath);

            if (File.Exists(pdfPath))
            {
                _logger.LogInformation("📄 Demo PDF file found at: {PdfPath}", pdfPath);
                var pdfBytes = await File.ReadAllBytesAsync(pdfPath, cancellationToken);
                _logger.LogInformation("✅ Demo PDF loaded successfully, size: {Size} bytes", pdfBytes.Length);
                return pdfBytes;
            }
            else
            {
                _logger.LogWarning("⚠️ Demo PDF file not found at: {PdfPath}", pdfPath);

                // Try alternative paths
                var alternativePaths = new[]
                {
                    Path.Combine(contentRoot, "wwwroot", "Calibration_Certificate_Kavoku_YU6754.pdf"),
                    Path.Combine(contentRoot, "..", "..", "..", "Calibration_Certificate_Kavoku_YU6754.pdf"),
                    "/Users/javier/repos/CalibrationSaaS/src/CalibrationSaaS/CalibrationSaaS.CustomerPortal/Calibration_Certificate_Kavoku_YU6754.pdf"
                };

                foreach (var altPath in alternativePaths)
                {
                    _logger.LogInformation("🔍 Trying alternative path: {AltPath}", altPath);
                    if (File.Exists(altPath))
                    {
                        _logger.LogInformation("📄 Demo PDF found at alternative path: {AltPath}", altPath);
                        var pdfBytes = await File.ReadAllBytesAsync(altPath, cancellationToken);
                        _logger.LogInformation("✅ Demo PDF loaded successfully, size: {Size} bytes", pdfBytes.Length);
                        return pdfBytes;
                    }
                }

                _logger.LogError("❌ Demo PDF file not found in any of the expected locations");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error loading demo certificate PDF for certificate {CertificateId}", certificateId);
            return null;
        }
    }

    /// <summary>
    /// Maps string tenant identifier to integer tenant ID for database operations
    /// </summary>
    /// <param name="tenantIdentifier">String tenant identifier (e.g., "thermotemp")</param>
    /// <returns>Integer tenant ID for database operations</returns>
    private int MapTenantIdentifierToId(string tenantIdentifier)
    {
        // Map known tenant identifiers to their integer IDs
        // This should ideally come from a configuration or database lookup
        return tenantIdentifier?.ToLowerInvariant() switch
        {
            "thermotemp" => 1, // ThermoTemp tenant maps to ID 1
            "demo" => 2,       // Demo tenant maps to ID 2
            "test" => 3,       // Test tenant maps to ID 3
            _ => 1             // Default to tenant ID 1 for unknown tenants
        };
    }

    /// <summary>
    /// Parse PieceOfEquipmentID string to integer
    /// Handles various formats like "AAM001-3E10", "12345", etc.
    /// </summary>
    private static int ParseEquipmentId(string pieceOfEquipmentId)
    {
        if (string.IsNullOrEmpty(pieceOfEquipmentId))
            return 0;

        // Try direct integer parsing first
        if (int.TryParse(pieceOfEquipmentId, out var directId))
            return directId;

        // For alphanumeric IDs like "AAM001-3E10", try to extract numeric parts
        // This is a fallback - in a real system, you might want to use the string ID directly
        var numericPart = new string(pieceOfEquipmentId.Where(char.IsDigit).ToArray());
        if (int.TryParse(numericPart, out var extractedId))
            return extractedId;

        // If all else fails, return a hash code to ensure uniqueness
        return Math.Abs(pieceOfEquipmentId.GetHashCode());
    }
}
