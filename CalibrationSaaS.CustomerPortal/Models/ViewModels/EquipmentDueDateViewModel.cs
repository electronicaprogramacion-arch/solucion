using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.ViewModels;

/// <summary>
/// View model for equipment due date search functionality
/// </summary>
public class EquipmentDueDateSearchViewModel
{
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? EquipmentType { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Department { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? DueDateFrom { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? DueDateTo { get; set; }
    
    public string? Status { get; set; } // "All", "Overdue", "Due Soon", "Current"
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? SortBy { get; set; } = "NextDueDate";
    public bool SortDescending { get; set; } = false; // Ascending by default for due dates
}

/// <summary>
/// Equipment due date display model for grid
/// </summary>
public class EquipmentDueDateDisplayModel
{
    public int PieceOfEquipmentId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string EquipmentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Department { get; set; }
    public DateTime? LastCalibrationDate { get; set; }
    public DateTime? NextDueDate { get; set; }
    public string? CalibrationInterval { get; set; }
    public string? LastTechnician { get; set; }
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Days until due (negative if overdue)
    /// </summary>
    public int? DaysUntilDue => NextDueDate?.Subtract(DateTime.Today).Days;
    
    /// <summary>
    /// Status based on due date
    /// </summary>
    public string Status
    {
        get
        {
            if (!NextDueDate.HasValue) return "No Due Date";
            
            var days = DaysUntilDue;
            return days switch
            {
                < 0 => "Overdue",
                <= 30 => "Due Soon",
                _ => "Current"
            };
        }
    }
    
    /// <summary>
    /// Display-friendly status text
    /// </summary>
    public string StatusDisplay
    {
        get
        {
            if (!NextDueDate.HasValue) return "No Due Date Set";
            
            var days = DaysUntilDue;
            return days switch
            {
                < 0 => $"Overdue by {Math.Abs(days.Value)} days",
                0 => "Due Today",
                <= 7 => $"Due in {days} days",
                <= 30 => $"Due in {days} days",
                _ => "Current"
            };
        }
    }
    
    /// <summary>
    /// CSS class for status display
    /// </summary>
    public string StatusCssClass
    {
        get
        {
            if (!NextDueDate.HasValue) return "badge badge-secondary";
            
            var days = DaysUntilDue;
            return days switch
            {
                < 0 => "badge badge-danger",
                <= 7 => "badge badge-warning",
                <= 30 => "badge badge-info",
                _ => "badge badge-success"
            };
        }
    }
    
    /// <summary>
    /// Priority level for sorting
    /// </summary>
    public int Priority
    {
        get
        {
            if (!NextDueDate.HasValue) return 4;
            
            var days = DaysUntilDue;
            return days switch
            {
                < 0 => 1, // Overdue - highest priority
                <= 7 => 2, // Due this week
                <= 30 => 3, // Due this month
                _ => 4 // Current - lowest priority
            };
        }
    }
    
    /// <summary>
    /// Days since last calibration
    /// </summary>
    public int? DaysSinceLastCalibration => LastCalibrationDate?.Subtract(DateTime.Today).Days * -1;
    
    /// <summary>
    /// Formatted last calibration date
    /// </summary>
    public string LastCalibrationDisplay => LastCalibrationDate?.ToString("MMM dd, yyyy") ?? "Never";
    
    /// <summary>
    /// Formatted next due date
    /// </summary>
    public string NextDueDateDisplay => NextDueDate?.ToString("MMM dd, yyyy") ?? "Not Set";
}

/// <summary>
/// Equipment due date search result
/// </summary>
public class EquipmentDueDateSearchResult
{
    public List<EquipmentDueDateDisplayModel> Equipment { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
    public string? ErrorMessage { get; set; }
    public EquipmentDueDateSummary? Summary { get; set; }
}

/// <summary>
/// Summary statistics for equipment due dates
/// </summary>
public class EquipmentDueDateSummary
{
    public int TotalEquipment { get; set; }
    public int OverdueCount { get; set; }
    public int DueSoonCount { get; set; } // Due within 30 days
    public int CurrentCount { get; set; }
    public int NoDueDateCount { get; set; }
    
    /// <summary>
    /// Percentage of overdue equipment
    /// </summary>
    public double OverduePercentage => TotalEquipment > 0 ? (double)OverdueCount / TotalEquipment * 100 : 0;
    
    /// <summary>
    /// Percentage of equipment due soon
    /// </summary>
    public double DueSoonPercentage => TotalEquipment > 0 ? (double)DueSoonCount / TotalEquipment * 100 : 0;
    
    /// <summary>
    /// Percentage of current equipment
    /// </summary>
    public double CurrentPercentage => TotalEquipment > 0 ? (double)CurrentCount / TotalEquipment * 100 : 0;
}

/// <summary>
/// Equipment calibration history item
/// </summary>
public class CalibrationHistoryViewModel
{
    public int WorkOrderDetailId { get; set; }
    public int WorkOrderId { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public DateTime? CalibrationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string TechnicianName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsAccredited { get; set; }
    public string? Comments { get; set; }
    public bool CertificateAvailable { get; set; }
    
    /// <summary>
    /// Formatted calibration date
    /// </summary>
    public string CalibrationDateDisplay => CalibrationDate?.ToString("MMM dd, yyyy") ?? "N/A";
    
    /// <summary>
    /// Formatted due date
    /// </summary>
    public string DueDateDisplay => DueDate?.ToString("MMM dd, yyyy") ?? "N/A";
}
