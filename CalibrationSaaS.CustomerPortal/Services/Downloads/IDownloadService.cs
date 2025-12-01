using CalibrationSaaS.CustomerPortal.Models.Certificates;

namespace CalibrationSaaS.CustomerPortal.Services.Downloads;

/// <summary>
/// Service for handling certificate downloads and file operations
/// </summary>
public interface IDownloadService
{
    /// <summary>
    /// Downloads a single certificate as PDF
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="customerId">Customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download result with file data</returns>
    Task<DownloadResult> DownloadCertificateAsync(int certificateId, int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads multiple certificates as a ZIP archive
    /// </summary>
    /// <param name="certificateIds">List of certificate IDs</param>
    /// <param name="customerId">Customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download result with ZIP file data</returns>
    Task<DownloadResult> DownloadMultipleCertificatesAsync(List<int> certificateIds, int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads all certificates for a customer as ZIP archive
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="dateFrom">Optional start date filter</param>
    /// <param name="dateTo">Optional end date filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download result with ZIP file data</returns>
    Task<DownloadResult> DownloadAllCertificatesAsync(int customerId, DateTime? dateFrom = null, DateTime? dateTo = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets download statistics for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download statistics</returns>
    Task<DownloadStatistics> GetDownloadStatisticsAsync(int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tracks a download event
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="customerId">Customer ID</param>
    /// <param name="downloadType">Type of download</param>
    /// <param name="fileName">Downloaded file name</param>
    /// <param name="fileSize">File size in bytes</param>
    /// <param name="userAgent">User agent string</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task TrackDownloadAsync(int certificateId, int customerId, string downloadType, string fileName, long fileSize, string? userAgent = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a customer can download a certificate
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="customerId">Customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if download is allowed</returns>
    Task<bool> CanDownloadCertificateAsync(int certificateId, int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the download history for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download history</returns>
    Task<DownloadHistoryResponse> GetDownloadHistoryAsync(int customerId, int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of a download operation
/// </summary>
public class DownloadResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? FileData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long FileSize { get; set; }
    public DateTime DownloadDate { get; set; } = DateTime.UtcNow;

    public static DownloadResult CreateSuccess(byte[] fileData, string fileName, string contentType)
    {
        return new DownloadResult
        {
            Success = true,
            FileData = fileData,
            FileName = fileName,
            ContentType = contentType,
            FileSize = fileData.Length
        };
    }

    public static DownloadResult CreateError(string errorMessage)
    {
        return new DownloadResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}

/// <summary>
/// Download statistics for a customer
/// </summary>
public class DownloadStatistics
{
    public int TotalDownloads { get; set; }
    public int CertificatesDownloaded { get; set; }
    public long TotalBytesDownloaded { get; set; }
    public DateTime? LastDownloadDate { get; set; }
    public DateTime? FirstDownloadDate { get; set; }
    public Dictionary<string, int> DownloadsByType { get; set; } = new();
    public Dictionary<string, int> DownloadsByMonth { get; set; } = new();
    public List<TopDownloadedCertificate> TopDownloadedCertificates { get; set; } = new();
}

/// <summary>
/// Top downloaded certificate information
/// </summary>
public class TopDownloadedCertificate
{
    public int CertificateId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string EquipmentDescription { get; set; } = string.Empty;
    public int DownloadCount { get; set; }
    public DateTime LastDownloadDate { get; set; }
}

/// <summary>
/// Download history entry
/// </summary>
public class DownloadHistoryEntry
{
    public int Id { get; set; }
    public int CertificateId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string EquipmentDescription { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string DownloadType { get; set; } = string.Empty;
    public DateTime DownloadDate { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
}

/// <summary>
/// Download history response
/// </summary>
public class DownloadHistoryResponse
{
    public List<DownloadHistoryEntry> Downloads { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}

/// <summary>
/// Download types enumeration
/// </summary>
public static class DownloadTypes
{
    public const string Single = "single";
    public const string Bulk = "bulk";
    public const string All = "all";
    public const string Filtered = "filtered";
    public const string Scheduled = "scheduled";
}

/// <summary>
/// Content types for downloads
/// </summary>
public static class DownloadContentTypes
{
    public const string Pdf = "application/pdf";
    public const string Zip = "application/zip";
    public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string Csv = "text/csv";
}

/// <summary>
/// Download configuration options
/// </summary>
public class DownloadOptions
{
    public int MaxConcurrentDownloads { get; set; } = 5;
    public long MaxFileSizeBytes { get; set; } = 50 * 1024 * 1024; // 50MB
    public long MaxBulkDownloadSizeBytes { get; set; } = 500 * 1024 * 1024; // 500MB
    public int MaxCertificatesPerBulkDownload { get; set; } = 100;
    public TimeSpan DownloadTimeout { get; set; } = TimeSpan.FromMinutes(10);
    public bool EnableDownloadTracking { get; set; } = true;
    public bool EnableDownloadNotifications { get; set; } = true;
    public string DefaultDownloadPath { get; set; } = "/downloads";
    public bool CompressLargePdfs { get; set; } = true;
    public int CompressionThresholdBytes { get; set; } = 5 * 1024 * 1024; // 5MB
}
