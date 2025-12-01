using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Certificates;

/// <summary>
/// Data Transfer Object for certificate information
/// </summary>
public class CertificateDto
{
    public int Id { get; set; }

    [Required]
    public string CertificateNumber { get; set; } = string.Empty;

    [Required]
    public string WorkOrderNumber { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime CalibrationDate { get; set; }

    public DateTime? NextCalibrationDate { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public string? Comments { get; set; }

    // Work Order References
    public int WorkOrderId { get; set; }
    public int WorkOrderDetailId { get; set; }

    // Additional Properties for gRPC compatibility
    public DateTime? NextDueDate { get; set; }
    public string CalibrationStatus { get; set; } = string.Empty;
    public string? TechnicalNotes { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    
    // Equipment Information
    public EquipmentDto Equipment { get; set; } = new();
    
    // Customer Information
    public CustomerDto Customer { get; set; } = new();
    
    // Calibration Results
    public List<CalibrationResultDto> CalibrationResults { get; set; } = new();
    
    // Certificate File Information
    public string? PdfFileName { get; set; }
    
    public long? PdfFileSize { get; set; }
    
    public DateTime? LastDownloadDate { get; set; }
    
    public int DownloadCount { get; set; }
    
    // Technician Information
    public string? TechnicianName { get; set; }
    
    public string? TechnicianSignature { get; set; }
    
    // Laboratory Information
    public string? LaboratoryName { get; set; }
    
    public string? LaboratoryAddress { get; set; }
    
    public string? AccreditationNumber { get; set; }
    
    // Environmental Conditions
    public decimal? Temperature { get; set; }

    public string? TemperatureRange { get; set; }

    public decimal? Humidity { get; set; }

    public string? HumidityRange { get; set; }

    public decimal? Pressure { get; set; }

    public string? PressureUnit { get; set; }

    public string? PressureRange { get; set; }

    public string? EnvironmentalNotes { get; set; }
    
    // Standards Used
    public List<StandardDto> StandardsUsed { get; set; } = new();
    
    // Uncertainty Information
    public decimal? MeasurementUncertainty { get; set; }

    public string? MeasurementUncertaintyUnit { get; set; }

    public decimal? ConfidenceLevel { get; set; }

    public decimal? CoverageFactor { get; set; }

    public string? UncertaintyMethod { get; set; }

    public string? UncertaintyStandard { get; set; }

    public int? DegreesOfFreedom { get; set; }

    public string? UncertaintyNotes { get; set; }

    public List<UncertaintyComponentDto> UncertaintyComponents { get; set; } = new();
    
    // Compliance Information
    public bool IsCompliant { get; set; }
    
    public string? ComplianceStandard { get; set; }
    
    public List<string> NonComplianceReasons { get; set; } = new();
}

/// <summary>
/// Equipment information for certificates
/// </summary>
public class EquipmentDto
{
    public int Id { get; set; }
    
    [Required]
    public string SerialNumber { get; set; } = string.Empty;
    
    [Required]
    public string AssetNumber { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? Manufacturer { get; set; }
    
    public string? Model { get; set; }
    
    public string? EquipmentType { get; set; }
    
    public string? EquipmentTypeGroup { get; set; }
    
    public decimal? Range { get; set; }
    
    public string? RangeUnit { get; set; }
    
    public decimal? Resolution { get; set; }
    
    public string? ResolutionUnit { get; set; }
    
    public decimal? Accuracy { get; set; }
    
    public string? AccuracyUnit { get; set; }
    
    public string? Location { get; set; }
    
    public string? Department { get; set; }
    
    public DateTime? LastCalibrationDate { get; set; }
    
    public DateTime? NextCalibrationDate { get; set; }
    
    public string? CalibrationInterval { get; set; }
    
    public string? Status { get; set; }
}

/// <summary>
/// Customer information for certificates
/// </summary>
public class CustomerDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? CompanyName { get; set; }
    
    public string? ContactPerson { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public AddressDto? Address { get; set; }
    
    public string? CustomerNumber { get; set; }
    
    public string? TaxId { get; set; }
}

/// <summary>
/// Address information
/// </summary>
public class AddressDto
{
    public string? Street { get; set; }
    
    public string? City { get; set; }
    
    public string? State { get; set; }
    
    public string? PostalCode { get; set; }
    
    public string? Country { get; set; }
    
    public string FormattedAddress => 
        string.Join(", ", new[] { Street, City, State, PostalCode, Country }
            .Where(s => !string.IsNullOrWhiteSpace(s)));
}

/// <summary>
/// Calibration result data
/// </summary>
public class CalibrationResultDto
{
    public int Id { get; set; }
    
    public string? TestPoint { get; set; }
    
    public decimal? NominalValue { get; set; }
    
    public decimal? AsFoundValue { get; set; }
    
    public decimal? AsLeftValue { get; set; }
    
    public decimal? Error { get; set; }
    
    public decimal? Uncertainty { get; set; }
    
    public string? Unit { get; set; }
    
    public bool? IsWithinTolerance { get; set; }

    public bool? IsInTolerance => IsWithinTolerance;

    public decimal? Tolerance { get; set; }

    public decimal? ToleranceMin { get; set; }

    public decimal? ToleranceMax { get; set; }
    
    public string? Comments { get; set; }
    
    public int? Sequence { get; set; }
}

/// <summary>
/// Standard equipment used in calibration
/// </summary>
public class StandardDto
{
    public int Id { get; set; }
    
    [Required]
    public string SerialNumber { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? Manufacturer { get; set; }
    
    public string? Model { get; set; }
    
    public string? CertificateNumber { get; set; }
    
    public DateTime? CalibrationDate { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public decimal? Uncertainty { get; set; }

    public string? UncertaintyUnit { get; set; }

    public decimal? Accuracy { get; set; }

    public string? AccuracyUnit { get; set; }
    
    public string? AccreditationBody { get; set; }
    
    public bool IsTraceableToNist { get; set; }
}

/// <summary>
/// Uncertainty component information
/// </summary>
public class UncertaintyComponentDto
{
    public int Id { get; set; }

    [Required]
    public string Source { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    public decimal Value { get; set; }

    public string? Unit { get; set; }

    public string? Distribution { get; set; }

    public decimal? Divisor { get; set; }

    public decimal StandardUncertainty { get; set; }

    public decimal? SensitivityCoefficient { get; set; }

    public string? Comments { get; set; }
}
