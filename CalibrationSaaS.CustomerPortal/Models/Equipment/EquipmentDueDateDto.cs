using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Equipment;

/// <summary>
/// Equipment due date information for customer portal
/// </summary>
public class EquipmentDueDateDto
{
    public int EquipmentId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? AssetTag { get; set; }
    public string? Location { get; set; }
    public string? Department { get; set; }
    
    // Equipment Type Information
    public string EquipmentType { get; set; } = string.Empty;
    public string EquipmentTypeGroup { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    
    // Calibration Information
    public DateTime? LastCalibrationDate { get; set; }
    public DateTime? NextCalibrationDate { get; set; }
    public int CalibrationIntervalMonths { get; set; }
    public string CalibrationStatus { get; set; } = string.Empty;
    
    // Due Date Status
    public DueDateStatus DueStatus { get; set; }
    public int DaysUntilDue { get; set; }
    public int DaysOverdue { get; set; }
    public bool IsOverdue => DaysOverdue > 0;
    public bool IsExpiringSoon => DaysUntilDue <= 30 && DaysUntilDue > 0;
    public bool IsCompliant => !IsOverdue;
    
    // Work Order Information
    public int? LastWorkOrderId { get; set; }
    public string? LastWorkOrderNumber { get; set; }
    public DateTime? LastWorkOrderDate { get; set; }
    public string? LastWorkOrderStatus { get; set; }
    
    // Certificate Information
    public int? LastCertificateId { get; set; }
    public string? LastCertificateNumber { get; set; }
    public DateTime? LastCertificateIssueDate { get; set; }
    
    // Customer Information
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    
    // Additional Properties
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    // Calculated Properties
    public string DueStatusText => DueStatus switch
    {
        DueDateStatus.Current => "Current",
        DueDateStatus.ExpiringSoon => "Expiring Soon",
        DueDateStatus.Overdue => "Overdue",
        DueDateStatus.Unknown => "Unknown",
        _ => "Unknown"
    };
    
    public string DueStatusColor => DueStatus switch
    {
        DueDateStatus.Current => "var(--rz-success)",
        DueDateStatus.ExpiringSoon => "var(--rz-warning)",
        DueDateStatus.Overdue => "var(--rz-danger)",
        DueDateStatus.Unknown => "var(--rz-secondary)",
        _ => "var(--rz-secondary)"
    };
    
    public string DueStatusIcon => DueStatus switch
    {
        DueDateStatus.Current => "check_circle",
        DueDateStatus.ExpiringSoon => "warning",
        DueDateStatus.Overdue => "error",
        DueDateStatus.Unknown => "help",
        _ => "help"
    };
    
    public string DueDateDisplayText
    {
        get
        {
            if (!NextCalibrationDate.HasValue)
                return "No due date set";
                
            if (IsOverdue)
                return $"Overdue by {DaysOverdue} day{(DaysOverdue == 1 ? "" : "s")}";
                
            if (IsExpiringSoon)
                return $"Due in {DaysUntilDue} day{(DaysUntilDue == 1 ? "" : "s")}";
                
            return $"Due {NextCalibrationDate.Value:MMM dd, yyyy}";
        }
    }
}

/// <summary>
/// Equipment due date status enumeration
/// </summary>
public enum DueDateStatus
{
    Unknown = 0,
    Current = 1,
    ExpiringSoon = 2,
    Overdue = 3
}

/// <summary>
/// Equipment due date search request
/// </summary>
public class DueDateSearchRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } = "asc";

    // Tenant Information
    public string? TenantId { get; set; }

    // Search Filters
    public string? SearchTerm { get; set; }
    public string? SerialNumber { get; set; }
    public string? AssetTag { get; set; }
    public string? EquipmentType { get; set; }
    public string? EquipmentTypeGroup { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Location { get; set; }
    public string? Department { get; set; }
    
    // Due Date Filters
    public DueDateStatus? DueStatus { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public bool? IsOverdue { get; set; }
    public bool? IsExpiringSoon { get; set; }
    public int? ExpiringSoonDays { get; set; } = 30;
    
    // Calibration Filters
    public DateTime? LastCalibrationFrom { get; set; }
    public DateTime? LastCalibrationTo { get; set; }
    public string? CalibrationStatus { get; set; }
    public int? CalibrationIntervalMonths { get; set; }
    
    // Additional Filters
    public bool? IsActive { get; set; } = true;
    public bool? HasCertificate { get; set; }
    public bool? HasWorkOrder { get; set; }
}

/// <summary>
/// Equipment due date search response
/// </summary>
public class DueDateSearchResponse
{
    public List<EquipmentDueDateDto> Equipment { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    
    // Summary Statistics
    public DueDateSummary Summary { get; set; } = new();
    
    // Applied Filters
    public Dictionary<string, object> AppliedFilters { get; set; } = new();
    
    // Available Filter Options
    public DueDateFilterOptions FilterOptions { get; set; } = new();
}

/// <summary>
/// Due date summary statistics
/// </summary>
public class DueDateSummary
{
    public int TotalEquipment { get; set; }
    public int CurrentEquipment { get; set; }
    public int ExpiringSoonEquipment { get; set; }
    public int OverdueEquipment { get; set; }
    public int UnknownStatusEquipment { get; set; }
    
    public double ComplianceRate => TotalEquipment > 0 ? (double)CurrentEquipment / TotalEquipment * 100 : 0;
    public double OverdueRate => TotalEquipment > 0 ? (double)OverdueEquipment / TotalEquipment * 100 : 0;
    public double ExpiringSoonRate => TotalEquipment > 0 ? (double)ExpiringSoonEquipment / TotalEquipment * 100 : 0;
    
    public DateTime? NextDueDate { get; set; }
    public int EquipmentDueThisWeek { get; set; }
    public int EquipmentDueThisMonth { get; set; }
    public int EquipmentDueNextMonth { get; set; }
}

/// <summary>
/// Available filter options for due date search
/// </summary>
public class DueDateFilterOptions
{
    public List<string> EquipmentTypes { get; set; } = new();
    public List<string> EquipmentTypeGroups { get; set; } = new();
    public List<string> Manufacturers { get; set; } = new();
    public List<string> Models { get; set; } = new();
    public List<string> Locations { get; set; } = new();
    public List<string> Departments { get; set; } = new();
    public List<string> CalibrationStatuses { get; set; } = new();
    public List<int> CalibrationIntervals { get; set; } = new();
    public DateTime? EarliestDueDate { get; set; }
    public DateTime? LatestDueDate { get; set; }
    public DateTime? EarliestCalibrationDate { get; set; }
    public DateTime? LatestCalibrationDate { get; set; }
}

/// <summary>
/// Equipment due date sort options
/// </summary>
public static class DueDateSortOptions
{
    public const string NextCalibrationDate = "NextCalibrationDate";
    public const string LastCalibrationDate = "LastCalibrationDate";
    public const string SerialNumber = "SerialNumber";
    public const string Description = "Description";
    public const string EquipmentType = "EquipmentType";
    public const string Manufacturer = "Manufacturer";
    public const string Model = "Model";
    public const string Location = "Location";
    public const string Department = "Department";
    public const string DueStatus = "DueStatus";
    public const string DaysUntilDue = "DaysUntilDue";
    public const string DaysOverdue = "DaysOverdue";
    public const string CalibrationStatus = "CalibrationStatus";
    public const string CustomerName = "CustomerName";
}
