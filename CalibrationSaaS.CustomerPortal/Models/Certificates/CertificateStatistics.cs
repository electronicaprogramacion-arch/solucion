namespace CalibrationSaaS.CustomerPortal.Models.Certificates;

/// <summary>
/// Certificate statistics for a customer
/// </summary>
public class CertificateStatistics
{
    /// <summary>
    /// Total number of certificates
    /// </summary>
    public int TotalCertificates { get; set; }
    
    /// <summary>
    /// Number of compliant certificates
    /// </summary>
    public int CompliantCertificates { get; set; }
    
    /// <summary>
    /// Number of non-compliant certificates
    /// </summary>
    public int NonCompliantCertificates { get; set; }
    
    /// <summary>
    /// Number of certificates expiring soon (within 30 days)
    /// </summary>
    public int ExpiringSoonCertificates { get; set; }
    
    /// <summary>
    /// Number of overdue certificates
    /// </summary>
    public int OverdueCertificates { get; set; }
    
    /// <summary>
    /// Number of certificates issued this month
    /// </summary>
    public int CertificatesThisMonth { get; set; }
    
    /// <summary>
    /// Number of certificates issued this year
    /// </summary>
    public int CertificatesThisYear { get; set; }
    
    /// <summary>
    /// Average calibration interval in days
    /// </summary>
    public double AverageCalibrationInterval { get; set; }
    
    /// <summary>
    /// Most common equipment type
    /// </summary>
    public string? MostCommonEquipmentType { get; set; }
    
    /// <summary>
    /// Most common manufacturer
    /// </summary>
    public string? MostCommonManufacturer { get; set; }

    /// <summary>
    /// Date of the most recent certificate
    /// </summary>
    public DateTime? LastCertificateDate { get; set; }
    
    /// <summary>
    /// Compliance percentage (0-100)
    /// </summary>
    public double CompliancePercentage => TotalCertificates > 0 
        ? (double)CompliantCertificates / TotalCertificates * 100 
        : 0;
    
    /// <summary>
    /// Overdue percentage (0-100)
    /// </summary>
    public double OverduePercentage => TotalCertificates > 0 
        ? (double)OverdueCertificates / TotalCertificates * 100 
        : 0;
    
    /// <summary>
    /// Expiring soon percentage (0-100)
    /// </summary>
    public double ExpiringSoonPercentage => TotalCertificates > 0 
        ? (double)ExpiringSoonCertificates / TotalCertificates * 100 
        : 0;
    
    /// <summary>
    /// Statistics by equipment type
    /// </summary>
    public Dictionary<string, int> CertificatesByEquipmentType { get; set; } = new();
    
    /// <summary>
    /// Statistics by manufacturer
    /// </summary>
    public Dictionary<string, int> CertificatesByManufacturer { get; set; } = new();
    
    /// <summary>
    /// Statistics by month (last 12 months)
    /// </summary>
    public Dictionary<string, int> CertificatesByMonth { get; set; } = new();
    
    /// <summary>
    /// Statistics by compliance status
    /// </summary>
    public Dictionary<string, int> CertificatesByStatus { get; set; } = new();
    
    /// <summary>
    /// Statistics by laboratory
    /// </summary>
    public Dictionary<string, int> CertificatesByLaboratory { get; set; } = new();
    
    /// <summary>
    /// Statistics by technician
    /// </summary>
    public Dictionary<string, int> CertificatesByTechnician { get; set; } = new();
    
    /// <summary>
    /// When the statistics were last updated
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Customer ID these statistics belong to
    /// </summary>
    public int CustomerId { get; set; }
    
    /// <summary>
    /// Date range for the statistics
    /// </summary>
    public DateRange StatisticsDateRange { get; set; } = new();
    
    /// <summary>
    /// Get a summary of key metrics
    /// </summary>
    public CertificateStatisticsSummary GetSummary()
    {
        return new CertificateStatisticsSummary
        {
            TotalCertificates = TotalCertificates,
            CompliancePercentage = CompliancePercentage,
            OverduePercentage = OverduePercentage,
            ExpiringSoonPercentage = ExpiringSoonPercentage,
            CertificatesThisMonth = CertificatesThisMonth,
            MostCommonEquipmentType = MostCommonEquipmentType,
            LastUpdated = LastUpdated
        };
    }
}

/// <summary>
/// Summary of key certificate statistics
/// </summary>
public class CertificateStatisticsSummary
{
    public int TotalCertificates { get; set; }
    public double CompliancePercentage { get; set; }
    public double OverduePercentage { get; set; }
    public double ExpiringSoonPercentage { get; set; }
    public int CertificatesThisMonth { get; set; }
    public string? MostCommonEquipmentType { get; set; }
    public DateTime LastUpdated { get; set; }
}
