namespace CalibrationSaaS.CustomerPortal.Models.Certificates;

/// <summary>
/// Result of a certificate download operation
/// </summary>
public class CertificateDownloadResult
{
    /// <summary>
    /// Whether the download was successful
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if download failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Downloaded file data
    /// </summary>
    public byte[]? FileData { get; set; }
    
    /// <summary>
    /// File name for the download
    /// </summary>
    public string? FileName { get; set; }
    
    /// <summary>
    /// Content type of the file
    /// </summary>
    public string? ContentType { get; set; }
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSizeBytes { get; set; }
    
    /// <summary>
    /// Download timestamp
    /// </summary>
    public DateTime DownloadTimestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Certificate ID(s) that were downloaded
    /// </summary>
    public List<int> CertificateIds { get; set; } = new();
    
    /// <summary>
    /// Download type (single, bulk, etc.)
    /// </summary>
    public string? DownloadType { get; set; }
    
    /// <summary>
    /// Additional metadata about the download
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
    
    /// <summary>
    /// Create a successful download result
    /// </summary>
    public static CertificateDownloadResult CreateSuccess(
        byte[] fileData,
        string fileName,
        string contentType = "application/pdf",
        List<int>? certificateIds = null,
        string? downloadType = null)
    {
        return new CertificateDownloadResult
        {
            Success = true,
            FileData = fileData,
            FileName = fileName,
            ContentType = contentType,
            FileSizeBytes = fileData?.Length ?? 0,
            CertificateIds = certificateIds ?? new List<int>(),
            DownloadType = downloadType ?? "single"
        };
    }
    
    /// <summary>
    /// Create a failed download result
    /// </summary>
    public static CertificateDownloadResult Failure(string errorMessage)
    {
        return new CertificateDownloadResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
    
    /// <summary>
    /// Get file size in human-readable format
    /// </summary>
    public string FileSizeDisplay
    {
        get
        {
            if (FileSizeBytes < 1024) return $"{FileSizeBytes} B";
            if (FileSizeBytes < 1024 * 1024) return $"{FileSizeBytes / 1024:F1} KB";
            return $"{FileSizeBytes / (1024 * 1024):F1} MB";
        }
    }
    
    /// <summary>
    /// Whether the result contains file data
    /// </summary>
    public bool HasFileData => Success && FileData != null && FileData.Length > 0;
}
