namespace CalibrationSaaS.CustomerPortal.Models.WorkOrders;

public class WorkOrderDto
{
    public int Id { get; set; }
    public string WorkOrderNumber { get; set; } = "";
    public string Status { get; set; } = "";
    public string Priority { get; set; } = "";
    public DateTime? ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public EquipmentDto? Equipment { get; set; }
    public string? TechnicianName { get; set; }
    public string? Notes { get; set; }
    public string? WorkType { get; set; }
    public string CustomerName { get; set; } = "";
    public string Description { get; set; } = "";
}

public class EquipmentDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = "";
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public string Model { get; set; } = "";
}

public class WorkOrderSearchRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchText { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string SortBy { get; set; } = "ScheduledDate";
    public string SortDirection { get; set; } = "desc";
}

public class WorkOrderSearchResult
{
    public List<WorkOrderDto> WorkOrders { get; set; } = new();
    public int TotalCount { get; set; }
}

public class WorkOrderStatistics
{
    public int PendingCount { get; set; }
    public int ScheduledCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int CancelledCount { get; set; }
}
