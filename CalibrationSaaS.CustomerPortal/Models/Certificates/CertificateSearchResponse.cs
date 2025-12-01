using CalibrationSaaS.CustomerPortal.Models.Certificates;

namespace CalibrationSaaS.CustomerPortal.Models.Certificates;

/// <summary>
/// Response model for certificate search operations
/// </summary>
public class CertificateSearchResponse
{
    /// <summary>
    /// List of certificates matching the search criteria
    /// </summary>
    public List<CertificateDto> Certificates { get; set; } = new();
    
    /// <summary>
    /// Total number of certificates matching the search criteria (before pagination)
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Current page number
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; } = 20;
    
    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    
    /// <summary>
    /// Whether there is a previous page
    /// </summary>
    public bool HasPreviousPage => Page > 1;
    
    /// <summary>
    /// Whether there is a next page
    /// </summary>
    public bool HasNextPage => Page < TotalPages;
    
    /// <summary>
    /// Search execution time in milliseconds
    /// </summary>
    public long ExecutionTimeMs { get; set; }
    
    /// <summary>
    /// Whether the search was successful
    /// </summary>
    public bool Success { get; set; } = true;
    
    /// <summary>
    /// Error message if search failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Applied filters summary
    /// </summary>
    public Dictionary<string, object> AppliedFilters { get; set; } = new();
    
    /// <summary>
    /// Search metadata
    /// </summary>
    public CertificateSearchMetadata Metadata { get; set; } = new();
}

/// <summary>
/// Metadata about the certificate search
/// </summary>
public class CertificateSearchMetadata
{
    /// <summary>
    /// Search query used
    /// </summary>
    public string? SearchQuery { get; set; }
    
    /// <summary>
    /// Filters applied
    /// </summary>
    public List<string> FiltersApplied { get; set; } = new();
    
    /// <summary>
    /// Sort criteria used
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Sort direction used
    /// </summary>
    public string? SortDirection { get; set; }
    
    /// <summary>
    /// Search timestamp
    /// </summary>
    public DateTime SearchTimestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Customer ID for the search
    /// </summary>
    public int? CustomerId { get; set; }
}
