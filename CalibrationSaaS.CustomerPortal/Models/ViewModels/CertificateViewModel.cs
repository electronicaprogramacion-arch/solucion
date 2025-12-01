using CalibrationSaaS.CustomerPortal.Models.Certificates;
using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.ViewModels;

/// <summary>
/// View model for certificate display and interaction
/// </summary>
public class CertificateViewModel
{
    /// <summary>
    /// Certificate data
    /// </summary>
    public CertificateDto Certificate { get; set; } = new();
    
    /// <summary>
    /// Whether the certificate is selected for bulk operations
    /// </summary>
    public bool IsSelected { get; set; }
    
    /// <summary>
    /// Whether the certificate details are expanded
    /// </summary>
    public bool IsExpanded { get; set; }
    
    /// <summary>
    /// Whether the certificate is currently being downloaded
    /// </summary>
    public bool IsDownloading { get; set; }
    
    /// <summary>
    /// Download progress percentage (0-100)
    /// </summary>
    public int DownloadProgress { get; set; }
    
    /// <summary>
    /// Error message if download failed
    /// </summary>
    public string? DownloadError { get; set; }
    
    /// <summary>
    /// Whether the certificate can be downloaded
    /// </summary>
    public bool CanDownload => !string.IsNullOrEmpty(Certificate.PdfFileName) && 
                               Certificate.Status == CertificateStatus.Valid;
    
    /// <summary>
    /// Whether the certificate is overdue for recalibration
    /// </summary>
    public bool IsOverdue => Certificate.NextCalibrationDate.HasValue && 
                            Certificate.NextCalibrationDate.Value < DateTime.Now;
    
    /// <summary>
    /// Whether the certificate is expiring soon (within 30 days)
    /// </summary>
    public bool IsExpiringSoon => Certificate.NextCalibrationDate.HasValue && 
                                 Certificate.NextCalibrationDate.Value <= DateTime.Now.AddDays(30) &&
                                 Certificate.NextCalibrationDate.Value >= DateTime.Now;
    
    /// <summary>
    /// Days until next calibration (negative if overdue)
    /// </summary>
    public int? DaysUntilNextCalibration
    {
        get
        {
            if (!Certificate.NextCalibrationDate.HasValue) return null;
            return (int)(Certificate.NextCalibrationDate.Value - DateTime.Now).TotalDays;
        }
    }
    
    /// <summary>
    /// Status color for UI display
    /// </summary>
    public string StatusColor => Certificate.Status switch
    {
        CertificateStatus.Valid => IsOverdue ? "danger" : IsExpiringSoon ? "warning" : "success",
        CertificateStatus.Expired => "danger",
        CertificateStatus.Pending => "info",
        CertificateStatus.Cancelled => "secondary",
        CertificateStatus.Draft => "light",
        CertificateStatus.Superseded => "secondary",
        _ => "secondary"
    };
    
    /// <summary>
    /// Status icon for UI display
    /// </summary>
    public string StatusIcon => Certificate.Status switch
    {
        CertificateStatus.Valid => IsOverdue ? "warning" : IsExpiringSoon ? "schedule" : "check_circle",
        CertificateStatus.Expired => "error",
        CertificateStatus.Pending => "hourglass_empty",
        CertificateStatus.Cancelled => "cancel",
        CertificateStatus.Draft => "edit",
        CertificateStatus.Superseded => "history",
        _ => "help"
    };
    
    /// <summary>
    /// Formatted file size for display
    /// </summary>
    public string FormattedFileSize
    {
        get
        {
            if (!Certificate.PdfFileSize.HasValue) return "Unknown";
            
            var size = Certificate.PdfFileSize.Value;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = size;
            
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            
            return $"{len:0.##} {sizes[order]}";
        }
    }
    
    /// <summary>
    /// Compliance status display text
    /// </summary>
    public string ComplianceStatusText => Certificate.IsCompliant ? "Compliant" : "Non-Compliant";
    
    /// <summary>
    /// Compliance status color
    /// </summary>
    public string ComplianceStatusColor => Certificate.IsCompliant ? "success" : "danger";
    
    /// <summary>
    /// Summary of non-compliance reasons
    /// </summary>
    public string NonComplianceSummary => Certificate.NonComplianceReasons.Any() 
        ? string.Join(", ", Certificate.NonComplianceReasons.Take(3)) + 
          (Certificate.NonComplianceReasons.Count > 3 ? "..." : "")
        : string.Empty;
}

/// <summary>
/// View model for certificate search page
/// </summary>
public class CertificateSearchViewModel
{
    /// <summary>
    /// Search request parameters
    /// </summary>
    public CertificateSearchRequest SearchRequest { get; set; } = new();
    
    /// <summary>
    /// Search results
    /// </summary>
    public CertificateSearchResponse SearchResponse { get; set; } = new();
    
    /// <summary>
    /// List of certificate view models for display
    /// </summary>
    public List<CertificateViewModel> Certificates { get; set; } = new();
    
    /// <summary>
    /// Whether a search is currently in progress
    /// </summary>
    public bool IsSearching { get; set; }
    
    /// <summary>
    /// Search error message
    /// </summary>
    public string? SearchError { get; set; }
    
    /// <summary>
    /// Whether advanced filters are shown
    /// </summary>
    public bool ShowAdvancedFilters { get; set; }
    
    /// <summary>
    /// Whether bulk operations panel is shown
    /// </summary>
    public bool ShowBulkOperations { get; set; }
    
    /// <summary>
    /// Number of selected certificates
    /// </summary>
    public int SelectedCount => Certificates.Count(c => c.IsSelected);
    
    /// <summary>
    /// Whether all certificates are selected
    /// </summary>
    public bool AllSelected => Certificates.Any() && Certificates.All(c => c.IsSelected);
    
    /// <summary>
    /// Whether some certificates are selected
    /// </summary>
    public bool SomeSelected => Certificates.Any(c => c.IsSelected);
    
    /// <summary>
    /// Available equipment types for filtering
    /// </summary>
    public List<string> AvailableEquipmentTypes { get; set; } = new();
    
    /// <summary>
    /// Available manufacturers for filtering
    /// </summary>
    public List<string> AvailableManufacturers { get; set; } = new();
    
    /// <summary>
    /// Available technicians for filtering
    /// </summary>
    public List<string> AvailableTechnicians { get; set; } = new();
    
    /// <summary>
    /// Available laboratories for filtering
    /// </summary>
    public List<string> AvailableLaboratories { get; set; } = new();
    
    /// <summary>
    /// Available locations for filtering
    /// </summary>
    public List<string> AvailableLocations { get; set; } = new();
    
    /// <summary>
    /// Available departments for filtering
    /// </summary>
    public List<string> AvailableDepartments { get; set; } = new();
    
    /// <summary>
    /// Toggle selection of all certificates
    /// </summary>
    public void ToggleSelectAll()
    {
        var newState = !AllSelected;
        foreach (var certificate in Certificates)
        {
            certificate.IsSelected = newState;
        }
    }
    
    /// <summary>
    /// Clear all selections
    /// </summary>
    public void ClearSelection()
    {
        foreach (var certificate in Certificates)
        {
            certificate.IsSelected = false;
        }
    }
    
    /// <summary>
    /// Get selected certificate IDs
    /// </summary>
    public List<int> GetSelectedCertificateIds()
    {
        return Certificates.Where(c => c.IsSelected)
                          .Select(c => c.Certificate.Id)
                          .ToList();
    }
    
    /// <summary>
    /// Reset search filters
    /// </summary>
    public void ResetFilters()
    {
        SearchRequest = new CertificateSearchRequest
        {
            Page = 1,
            PageSize = SearchRequest.PageSize,
            SortBy = SearchRequest.SortBy,
            SortDirection = SearchRequest.SortDirection
        };
    }
    
    /// <summary>
    /// Apply quick filter presets
    /// </summary>
    public void ApplyQuickFilter(string filterType)
    {
        ResetFilters();
        
        switch (filterType.ToLower())
        {
            case "overdue":
                SearchRequest.IsOverdue = true;
                break;
            case "expiring":
                SearchRequest.ExpiringSoonDays = 30;
                break;
            case "recent":
                SearchRequest.IssueDateFrom = DateTime.Now.AddDays(-30);
                break;
            case "compliant":
                SearchRequest.IsCompliant = true;
                break;
            case "noncompliant":
                SearchRequest.IsCompliant = false;
                break;
        }
    }
}

/// <summary>
/// View model for certificate detail page
/// </summary>
public class CertificateDetailViewModel
{
    /// <summary>
    /// Certificate view model
    /// </summary>
    public CertificateViewModel Certificate { get; set; } = new();
    
    /// <summary>
    /// Whether calibration results are expanded
    /// </summary>
    public bool ShowCalibrationResults { get; set; } = true;
    
    /// <summary>
    /// Whether standards used are expanded
    /// </summary>
    public bool ShowStandardsUsed { get; set; } = true;
    
    /// <summary>
    /// Whether environmental conditions are expanded
    /// </summary>
    public bool ShowEnvironmentalConditions { get; set; } = true;
    
    /// <summary>
    /// Whether uncertainty information is expanded
    /// </summary>
    public bool ShowUncertaintyInfo { get; set; } = true;
    
    /// <summary>
    /// Whether compliance information is expanded
    /// </summary>
    public bool ShowComplianceInfo { get; set; } = true;
    
    /// <summary>
    /// Whether download history is expanded
    /// </summary>
    public bool ShowDownloadHistory { get; set; } = false;
    
    /// <summary>
    /// Error message for loading certificate details
    /// </summary>
    public string? LoadError { get; set; }
    
    /// <summary>
    /// Whether certificate details are loading
    /// </summary>
    public bool IsLoading { get; set; }
}
