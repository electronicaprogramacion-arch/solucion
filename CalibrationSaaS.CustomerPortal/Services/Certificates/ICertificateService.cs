using CalibrationSaaS.CustomerPortal.Models.Certificates;
using LocalCertificateDto = CalibrationSaaS.CustomerPortal.Models.Certificates.CertificateDto;

namespace CalibrationSaaS.CustomerPortal.Services.Certificates;

/// <summary>
/// Service interface for certificate operations
/// </summary>
public interface ICertificateService
{
    /// <summary>
    /// Search certificates based on criteria
    /// </summary>
    /// <param name="request">Search criteria</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search results</returns>
    Task<CertificateSearchResponse> SearchCertificatesAsync(
        CertificateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get certificate by ID
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Certificate details</returns>
    Task<LocalCertificateDto?> GetCertificateByIdAsync(
        int certificateId,
        int customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get certificate by certificate number
    /// </summary>
    /// <param name="certificateNumber">Certificate number</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Certificate details</returns>
    Task<LocalCertificateDto?> GetCertificateByCertificateNumberAsync(
        string certificateNumber, 
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get certificates for specific equipment
    /// </summary>
    /// <param name="equipmentId">Equipment ID</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of certificates for the equipment</returns>
    Task<List<LocalCertificateDto>> GetCertificatesForEquipmentAsync(
        int equipmentId, 
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get recent certificates for dashboard
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="count">Number of certificates to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Recent certificates</returns>
    Task<List<LocalCertificateDto>> GetRecentCertificatesAsync(
        int customerId, 
        int count = 10, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get certificates expiring soon
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="days">Number of days to look ahead</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Certificates expiring soon</returns>
    Task<List<LocalCertificateDto>> GetCertificatesExpiringSoonAsync(
        int customerId, 
        int days = 30, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get overdue certificates
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Overdue certificates</returns>
    Task<List<LocalCertificateDto>> GetOverdueCertificatesAsync(
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get certificate statistics for dashboard
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Certificate statistics</returns>
    Task<CertificateStatistics> GetCertificateStatisticsAsync(
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get available filter options for search
    /// </summary>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Available filter options</returns>
    Task<CertificateFilterOptions> GetFilterOptionsAsync(
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Download certificate PDF
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Certificate PDF data</returns>
    Task<CertificateDownloadResult> DownloadCertificateAsync(
        int certificateId, 
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Download multiple certificates as ZIP
    /// </summary>
    /// <param name="certificateIds">List of certificate IDs</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ZIP file with certificates</returns>
    Task<CertificateDownloadResult> DownloadMultipleCertificatesAsync(
        List<int> certificateIds, 
        int customerId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Track certificate download
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="customerId">Customer ID for tenant filtering</param>
    /// <param name="downloadType">Type of download (single/bulk)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    Task<bool> TrackDownloadAsync(
        int certificateId, 
        int customerId, 
        string downloadType, 
        CancellationToken cancellationToken = default);
}


