using CalibrationSaaS.CustomerPortal.Models.Equipment;
using LocalEquipmentDueDateDto = CalibrationSaaS.CustomerPortal.Models.Equipment.EquipmentDueDateDto;
using CalibrationSaaS.CustomerPortal.Models.ViewModels;

namespace CalibrationSaaS.CustomerPortal.Services.Equipment;

/// <summary>
/// Service interface for equipment due date management
/// </summary>
public interface IEquipmentDueDateService
{
    /// <summary>
    /// Search equipment due dates with filtering and pagination
    /// </summary>
    /// <param name="request">Search request parameters</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search response with equipment due dates</returns>
    Task<DueDateSearchResponse> SearchEquipmentDueDatesAsync(
        DueDateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get equipment due date by equipment ID
    /// </summary>
    /// <param name="equipmentId">Equipment ID</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Equipment due date information</returns>
    Task<LocalEquipmentDueDateDto?> GetEquipmentDueDateAsync(
        int equipmentId,
        int customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get overdue equipment
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of overdue equipment</returns>
    Task<List<LocalEquipmentDueDateDto>> GetOverdueEquipmentAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get equipment expiring soon
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="days">Number of days to look ahead (default: 30)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of equipment expiring soon</returns>
    Task<List<LocalEquipmentDueDateDto>> GetEquipmentExpiringSoonAsync(
        int customerId,
        int days = 30,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get equipment due in date range
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="fromDate">Start date</param>
    /// <param name="toDate">End date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of equipment due in date range</returns>
    Task<List<LocalEquipmentDueDateDto>> GetEquipmentDueInRangeAsync(
        int customerId, 
        DateTime fromDate, 
        DateTime toDate, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get due date summary statistics for dashboard
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Due date summary statistics</returns>
    Task<DueDateDashboardSummary> GetDueDateSummaryAsync(
        int customerId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get available filter options for due date search
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Available filter options</returns>
    Task<DueDateFilterOptions> GetFilterOptionsAsync(
        int customerId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculate due date status for equipment
    /// </summary>
    /// <param name="nextCalibrationDate">Next calibration due date</param>
    /// <param name="expiringSoonDays">Days to consider as expiring soon (default: 30)</param>
    /// <returns>Due date status</returns>
    DueDateStatus CalculateDueDateStatus(DateTime? nextCalibrationDate, int expiringSoonDays = 30);

    /// <summary>
    /// Calculate days until due or overdue
    /// </summary>
    /// <param name="nextCalibrationDate">Next calibration due date</param>
    /// <returns>Tuple of (DaysUntilDue, DaysOverdue)</returns>
    (int DaysUntilDue, int DaysOverdue) CalculateDueDateDays(DateTime? nextCalibrationDate);

    /// <summary>
    /// Export equipment due dates to Excel
    /// </summary>
    /// <param name="request">Search request parameters</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Excel file data</returns>
    Task<ExportResult> ExportToExcelAsync(
        DueDateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Export equipment due dates to PDF
    /// </summary>
    /// <param name="request">Search request parameters</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PDF file data</returns>
    Task<ExportResult> ExportToPdfAsync(
        DueDateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get equipment due date history
    /// </summary>
    /// <param name="equipmentId">Equipment ID</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of due date history entries</returns>
    Task<List<DueDateHistoryEntry>> GetDueDateHistoryAsync(
        int equipmentId, 
        int customerId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get due date trend data for charts
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="months">Number of months to include (default: 12)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Trend data for charts</returns>
    Task<List<DueDateTrendData>> GetDueDateTrendDataAsync(
        int customerId, 
        int months = 12, 
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Export result for due date reports
/// </summary>
public class ExportResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? FileData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long? FileSize { get; set; }
    public DateTime ExportDate { get; set; } = DateTime.UtcNow;
    public string? ExportId { get; set; }
    
    public static ExportResult CreateSuccess(byte[] fileData, string fileName, string contentType)
    {
        return new ExportResult
        {
            Success = true,
            FileData = fileData,
            FileName = fileName,
            ContentType = contentType,
            FileSize = fileData.Length,
            ExportId = Guid.NewGuid().ToString()
        };
    }
    
    public static ExportResult Failure(string errorMessage)
    {
        return new ExportResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}

/// <summary>
/// Due date history entry
/// </summary>
public class DueDateHistoryEntry
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty; // Calibration, Maintenance, Due Date Change
    public DateTime? PreviousDueDate { get; set; }
    public DateTime? NewDueDate { get; set; }
    public string? Notes { get; set; }
    public string? PerformedBy { get; set; }
    public int? WorkOrderId { get; set; }
    public string? WorkOrderNumber { get; set; }
    public int? CertificateId { get; set; }
    public string? CertificateNumber { get; set; }
    public DueDateStatus? PreviousStatus { get; set; }
    public DueDateStatus? NewStatus { get; set; }
    
    public string EventDescription
    {
        get
        {
            return EventType switch
            {
                "Calibration" => "Equipment calibrated",
                "Maintenance" => "Equipment maintained",
                "Due Date Change" => "Due date updated",
                "Work Order Created" => "Work order created",
                "Certificate Issued" => "Certificate issued",
                _ => EventType
            };
        }
    }
    
    public string EventIcon
    {
        get
        {
            return EventType switch
            {
                "Calibration" => "build",
                "Maintenance" => "handyman",
                "Due Date Change" => "event",
                "Work Order Created" => "assignment",
                "Certificate Issued" => "verified",
                _ => "info"
            };
        }
    }
}
