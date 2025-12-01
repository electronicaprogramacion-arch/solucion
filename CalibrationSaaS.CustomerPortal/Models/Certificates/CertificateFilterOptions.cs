namespace CalibrationSaaS.CustomerPortal.Models.Certificates;

/// <summary>
/// Available filter options for certificate searches
/// </summary>
public class CertificateFilterOptions
{
    /// <summary>
    /// Available certificate statuses
    /// </summary>
    public List<string> AvailableStatuses { get; set; } = new();
    
    /// <summary>
    /// Available equipment types
    /// </summary>
    public List<string> AvailableEquipmentTypes { get; set; } = new();

    /// <summary>
    /// Available manufacturers
    /// </summary>
    public List<string> AvailableManufacturers { get; set; } = new();

    /// <summary>
    /// Available technicians
    /// </summary>
    public List<string> AvailableTechnicians { get; set; } = new();

    /// <summary>
    /// Available laboratories
    /// </summary>
    public List<string> AvailableLaboratories { get; set; } = new();

    // Alias properties for backward compatibility
    public List<string> EquipmentTypes => AvailableEquipmentTypes;
    public List<string> Manufacturers => AvailableManufacturers;
    public List<string> Technicians => AvailableTechnicians;
    public List<string> Laboratories => AvailableLaboratories;
    public List<string> Locations => AvailableLocations;
    public List<string> Departments => AvailableDepartments;
    
    /// <summary>
    /// Available compliance standards
    /// </summary>
    public List<string> AvailableComplianceStandards { get; set; } = new();
    
    /// <summary>
    /// Available calibration intervals
    /// </summary>
    public List<string> AvailableCalibrationIntervals { get; set; } = new();
    
    /// <summary>
    /// Available locations
    /// </summary>
    public List<string> AvailableLocations { get; set; } = new();
    
    /// <summary>
    /// Available departments
    /// </summary>
    public List<string> AvailableDepartments { get; set; } = new();
    
    /// <summary>
    /// Date range for certificates (earliest and latest dates)
    /// </summary>
    public DateRange CertificateDateRange { get; set; } = new();
    
    /// <summary>
    /// Date range for calibration dates
    /// </summary>
    public DateRange CalibrationDateRange { get; set; } = new();
    
    /// <summary>
    /// Date range for next calibration dates
    /// </summary>
    public DateRange NextCalibrationDateRange { get; set; } = new();
    
    /// <summary>
    /// Quick filter presets
    /// </summary>
    public List<QuickFilterOption> QuickFilters { get; set; } = new()
    {
        new QuickFilterOption { Name = "All Certificates", Value = "all", Description = "Show all certificates" },
        new QuickFilterOption { Name = "Overdue", Value = "overdue", Description = "Certificates past due date" },
        new QuickFilterOption { Name = "Expiring Soon", Value = "expiring", Description = "Certificates expiring within 30 days" },
        new QuickFilterOption { Name = "Recent", Value = "recent", Description = "Certificates issued in last 30 days" },
        new QuickFilterOption { Name = "Compliant", Value = "compliant", Description = "Compliant certificates only" },
        new QuickFilterOption { Name = "Non-Compliant", Value = "noncompliant", Description = "Non-compliant certificates only" }
    };
}

/// <summary>
/// Date range for filtering
/// </summary>
public class DateRange
{
    /// <summary>
    /// Earliest date available
    /// </summary>
    public DateTime? MinDate { get; set; }
    
    /// <summary>
    /// Latest date available
    /// </summary>
    public DateTime? MaxDate { get; set; }
    
    /// <summary>
    /// Whether the date range is valid
    /// </summary>
    public bool IsValid => MinDate.HasValue && MaxDate.HasValue && MinDate <= MaxDate;
}

/// <summary>
/// Quick filter option for common searches
/// </summary>
public class QuickFilterOption
{
    /// <summary>
    /// Display name for the filter
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Filter value/key
    /// </summary>
    public string Value { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of what the filter does
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Icon class for the filter (optional)
    /// </summary>
    public string? IconClass { get; set; }
    
    /// <summary>
    /// Badge color for the filter (optional)
    /// </summary>
    public string? BadgeColor { get; set; }
}
