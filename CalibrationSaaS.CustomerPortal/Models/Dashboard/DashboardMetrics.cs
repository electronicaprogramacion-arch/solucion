namespace CalibrationSaaS.CustomerPortal.Models.Dashboard;

/// <summary>
/// Dashboard metrics for customer portal
/// </summary>
public class DashboardMetrics
{
    /// <summary>
    /// Total number of completed certificates for the customer
    /// </summary>
    public int TotalCertificates { get; set; }

    /// <summary>
    /// Number of pieces of equipment due for calibration in the next 30 days
    /// </summary>
    public int EquipmentExpiringSoon { get; set; }

    /// <summary>
    /// Number of active (open) service requests for the customer
    /// </summary>
    public int ActiveServiceRequests { get; set; }

    /// <summary>
    /// Total number of pieces of equipment for the customer
    /// </summary>
    public int TotalEquipment { get; set; }

    /// <summary>
    /// Number of overdue equipment
    /// </summary>
    public int EquipmentOverdue { get; set; }

    /// <summary>
    /// Number of current (up-to-date) equipment
    /// </summary>
    public int EquipmentCurrent { get; set; }

    /// <summary>
    /// Compliance rate percentage
    /// </summary>
    public double ComplianceRate { get; set; }

    /// <summary>
    /// Customer ID these metrics belong to
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Tenant ID these metrics belong to
    /// </summary>
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// When these metrics were calculated
    /// </summary>
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}
