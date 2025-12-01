using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Certificates;

/// <summary>
/// Request model for certificate search operations
/// </summary>
public class CertificateSearchRequest
{
    /// <summary>
    /// Customer ID for filtering certificates
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenant filtering
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Certificate number search (partial match)
    /// </summary>
    public string? CertificateNumber { get; set; }
    
    /// <summary>
    /// Work order number search (partial match)
    /// </summary>
    public string? WorkOrderNumber { get; set; }
    
    /// <summary>
    /// Equipment serial number search (partial match)
    /// </summary>
    public string? EquipmentSerialNumber { get; set; }
    
    /// <summary>
    /// Equipment asset number search (partial match)
    /// </summary>
    public string? EquipmentAssetNumber { get; set; }
    
    /// <summary>
    /// Equipment description search (partial match)
    /// </summary>
    public string? EquipmentDescription { get; set; }
    
    /// <summary>
    /// Equipment manufacturer filter
    /// </summary>
    public string? Manufacturer { get; set; }
    
    /// <summary>
    /// Equipment model filter
    /// </summary>
    public string? Model { get; set; }
    
    /// <summary>
    /// Equipment type filter
    /// </summary>
    public string? EquipmentType { get; set; }
    
    /// <summary>
    /// Equipment type group filter
    /// </summary>
    public string? EquipmentTypeGroup { get; set; }
    
    /// <summary>
    /// Certificate status filter
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Issue date range start
    /// </summary>
    public DateTime? IssueDateFrom { get; set; }
    
    /// <summary>
    /// Issue date range end
    /// </summary>
    public DateTime? IssueDateTo { get; set; }
    
    /// <summary>
    /// Calibration date range start
    /// </summary>
    public DateTime? CalibrationDateFrom { get; set; }
    
    /// <summary>
    /// Calibration date range end
    /// </summary>
    public DateTime? CalibrationDateTo { get; set; }
    
    /// <summary>
    /// Next calibration date range start
    /// </summary>
    public DateTime? NextCalibrationDateFrom { get; set; }
    
    /// <summary>
    /// Next calibration date range end
    /// </summary>
    public DateTime? NextCalibrationDateTo { get; set; }
    
    /// <summary>
    /// Filter for certificates expiring soon (days)
    /// </summary>
    public int? ExpiringSoonDays { get; set; }
    
    /// <summary>
    /// Filter for overdue certificates
    /// </summary>
    public bool? IsOverdue { get; set; }
    
    /// <summary>
    /// Filter for compliant certificates only
    /// </summary>
    public bool? IsCompliant { get; set; }
    
    /// <summary>
    /// Technician name filter
    /// </summary>
    public string? TechnicianName { get; set; }
    
    /// <summary>
    /// Laboratory name filter
    /// </summary>
    public string? LaboratoryName { get; set; }
    
    /// <summary>
    /// Accreditation number filter
    /// </summary>
    public string? AccreditationNumber { get; set; }
    
    /// <summary>
    /// Equipment location filter
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// Equipment department filter
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// General date range start (for bulk operations)
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// General date range end (for bulk operations)
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Page number for pagination (1-based)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Page size for pagination
    /// </summary>
    [Range(1, 1000, ErrorMessage = "Page size must be between 1 and 1000")]
    public int PageSize { get; set; } = 25;
    
    /// <summary>
    /// Sort field
    /// </summary>
    public string? SortBy { get; set; } = "IssueDate";
    
    /// <summary>
    /// Sort direction (asc/desc)
    /// </summary>
    public string? SortDirection { get; set; } = "desc";
    
    /// <summary>
    /// Include calibration results in response
    /// </summary>
    public bool IncludeCalibrationResults { get; set; } = false;
    
    /// <summary>
    /// Include standards used in response
    /// </summary>
    public bool IncludeStandardsUsed { get; set; } = false;
    
    /// <summary>
    /// Include download statistics
    /// </summary>
    public bool IncludeDownloadStats { get; set; } = false;
    
    /// <summary>
    /// Free text search across multiple fields
    /// </summary>
    public string? SearchText { get; set; }
    
    /// <summary>
    /// Calculate skip value for pagination
    /// </summary>
    public int Skip => (Page - 1) * PageSize;
    
    /// <summary>
    /// Validate date ranges
    /// </summary>
    public bool IsValid(out List<string> errors)
    {
        errors = new List<string>();
        
        if (IssueDateFrom.HasValue && IssueDateTo.HasValue && IssueDateFrom > IssueDateTo)
        {
            errors.Add("Issue date 'from' cannot be greater than 'to'");
        }
        
        if (CalibrationDateFrom.HasValue && CalibrationDateTo.HasValue && CalibrationDateFrom > CalibrationDateTo)
        {
            errors.Add("Calibration date 'from' cannot be greater than 'to'");
        }
        
        if (NextCalibrationDateFrom.HasValue && NextCalibrationDateTo.HasValue && NextCalibrationDateFrom > NextCalibrationDateTo)
        {
            errors.Add("Next calibration date 'from' cannot be greater than 'to'");
        }

        if (DateFrom.HasValue && DateTo.HasValue && DateFrom > DateTo)
        {
            errors.Add("Date 'from' cannot be greater than 'to'");
        }

        if (ExpiringSoonDays.HasValue && ExpiringSoonDays <= 0)
        {
            errors.Add("Expiring soon days must be greater than 0");
        }
        
        return errors.Count == 0;
    }
}

/// <summary>
/// Certificate status enumeration
/// </summary>
public static class CertificateStatus
{
    public const string Valid = "Valid";
    public const string Expired = "Expired";
    public const string Pending = "Pending";
    public const string Cancelled = "Cancelled";
    public const string Draft = "Draft";
    public const string Superseded = "Superseded";
    
    public static readonly List<string> AllStatuses = new()
    {
        Valid, Expired, Pending, Cancelled, Draft, Superseded
    };
}

/// <summary>
/// Certificate sort options
/// </summary>
public static class CertificateSortOptions
{
    public const string IssueDate = "IssueDate";
    public const string CalibrationDate = "CalibrationDate";
    public const string NextCalibrationDate = "NextCalibrationDate";
    public const string CertificateNumber = "CertificateNumber";
    public const string WorkOrderNumber = "WorkOrderNumber";
    public const string EquipmentSerialNumber = "EquipmentSerialNumber";
    public const string EquipmentDescription = "EquipmentDescription";
    public const string CustomerName = "CustomerName";
    public const string Status = "Status";
    public const string TechnicianName = "TechnicianName";
    
    public static readonly List<string> AllSortOptions = new()
    {
        IssueDate, CalibrationDate, NextCalibrationDate, CertificateNumber,
        WorkOrderNumber, EquipmentSerialNumber, EquipmentDescription,
        CustomerName, Status, TechnicianName
    };
}
