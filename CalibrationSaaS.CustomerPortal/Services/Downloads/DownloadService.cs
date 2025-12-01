using CalibrationSaaS.CustomerPortal.Data;
using CalibrationSaaS.CustomerPortal.Models.Certificates;
using CalibrationSaaS.CustomerPortal.Services.Certificates;
using CalibrationSaaS.CustomerPortal.Services.GrpcClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Text;

namespace CalibrationSaaS.CustomerPortal.Services.Downloads;

/// <summary>
/// Implementation of download service for certificate downloads
/// </summary>
public class DownloadService : IDownloadService
{
    private readonly ICertificateService _certificateService;
    private readonly IWorkOrderGrpcService _workOrderGrpcService;
    private readonly CustomerPortalDbContext _dbContext;
    private readonly ILogger<DownloadService> _logger;
    private readonly DownloadOptions _options;

    public DownloadService(
        ICertificateService certificateService,
        IWorkOrderGrpcService workOrderGrpcService,
        CustomerPortalDbContext dbContext,
        ILogger<DownloadService> logger,
        IOptions<DownloadOptions> options)
    {
        _certificateService = certificateService;
        _workOrderGrpcService = workOrderGrpcService;
        _dbContext = dbContext;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<DownloadResult> DownloadCertificateAsync(int certificateId, int customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting certificate download for Certificate ID: {CertificateId}, Customer ID: {CustomerId}", 
                certificateId, customerId);

            // Validate access
            if (!await CanDownloadCertificateAsync(certificateId, customerId, cancellationToken))
            {
                return DownloadResult.CreateError("You don't have permission to download this certificate.");
            }

            // Get certificate information
            var certificate = await _certificateService.GetCertificateByIdAsync(certificateId, customerId, cancellationToken);
            if (certificate == null)
            {
                return DownloadResult.CreateError("Certificate not found.");
            }

            // Generate PDF through gRPC service
            var pdfData = await _workOrderGrpcService.GenerateCertificatePdfAsync(certificateId, cancellationToken);
            if (pdfData == null || pdfData.Length == 0)
            {
                return DownloadResult.CreateError("Failed to generate certificate PDF.");
            }

            // Validate file size
            if (pdfData.Length > _options.MaxFileSizeBytes)
            {
                return DownloadResult.CreateError($"Certificate file size exceeds maximum allowed size of {FormatFileSize(_options.MaxFileSizeBytes)}.");
            }

            // Create filename
            var fileName = $"Certificate_{certificate.CertificateNumber}_{DateTime.Now:yyyyMMdd}.pdf";

            // Track download
            if (_options.EnableDownloadTracking)
            {
                await TrackDownloadAsync(certificateId, customerId, DownloadTypes.Single, fileName, pdfData.Length, cancellationToken: cancellationToken);
            }

            _logger.LogInformation("Certificate download completed successfully for Certificate ID: {CertificateId}", certificateId);

            return DownloadResult.CreateSuccess(pdfData, fileName, DownloadContentTypes.Pdf);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading certificate {CertificateId} for customer {CustomerId}", certificateId, customerId);
            return DownloadResult.CreateError("An error occurred while downloading the certificate. Please try again.");
        }
    }

    public async Task<DownloadResult> DownloadMultipleCertificatesAsync(List<int> certificateIds, int customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting bulk certificate download for {Count} certificates, Customer ID: {CustomerId}", 
                certificateIds.Count, customerId);

            if (certificateIds.Count > _options.MaxCertificatesPerBulkDownload)
            {
                return DownloadResult.CreateError($"Cannot download more than {_options.MaxCertificatesPerBulkDownload} certificates at once.");
            }

            // Validate access for all certificates
            var validCertificateIds = new List<int>();
            foreach (var certificateId in certificateIds)
            {
                if (await CanDownloadCertificateAsync(certificateId, customerId, cancellationToken))
                {
                    validCertificateIds.Add(certificateId);
                }
            }

            if (!validCertificateIds.Any())
            {
                return DownloadResult.CreateError("No certificates available for download.");
            }

            // Create ZIP archive
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var totalSize = 0L;

                foreach (var certificateId in validCertificateIds)
                {
                    try
                    {
                        var certificate = await _certificateService.GetCertificateByIdAsync(certificateId, customerId, cancellationToken);
                        if (certificate == null) continue;

                        var pdfData = await _workOrderGrpcService.GenerateCertificatePdfAsync(certificateId, cancellationToken);
                        if (pdfData == null || pdfData.Length == 0) continue;

                        totalSize += pdfData.Length;
                        if (totalSize > _options.MaxBulkDownloadSizeBytes)
                        {
                            _logger.LogWarning("Bulk download size limit exceeded, stopping at {Size}", FormatFileSize(totalSize));
                            break;
                        }

                        var fileName = $"Certificate_{certificate.CertificateNumber}.pdf";
                        var entry = archive.CreateEntry(fileName);
                        
                        using var entryStream = entry.Open();
                        await entryStream.WriteAsync(pdfData, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to add certificate {CertificateId} to ZIP archive", certificateId);
                    }
                }
            }

            var zipData = memoryStream.ToArray();
            if (zipData.Length == 0)
            {
                return DownloadResult.CreateError("No certificates could be added to the download archive.");
            }

            var zipFileName = $"Certificates_{DateTime.Now:yyyyMMdd_HHmmss}.zip";

            // Track bulk download
            if (_options.EnableDownloadTracking)
            {
                foreach (var certificateId in validCertificateIds)
                {
                    await TrackDownloadAsync(certificateId, customerId, DownloadTypes.Bulk, zipFileName, zipData.Length, cancellationToken: cancellationToken);
                }
            }

            _logger.LogInformation("Bulk certificate download completed successfully for {Count} certificates", validCertificateIds.Count);

            return DownloadResult.CreateSuccess(zipData, zipFileName, DownloadContentTypes.Zip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading multiple certificates for customer {CustomerId}", customerId);
            return DownloadResult.CreateError("An error occurred while downloading certificates. Please try again.");
        }
    }

    public async Task<DownloadResult> DownloadAllCertificatesAsync(int customerId, DateTime? dateFrom = null, DateTime? dateTo = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting download all certificates for Customer ID: {CustomerId}", customerId);

            // Get all certificates for customer
            var searchRequest = new CertificateSearchRequest
            {
                Page = 1,
                PageSize = _options.MaxCertificatesPerBulkDownload,
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            var searchResponse = await _certificateService.SearchCertificatesAsync(searchRequest, customerId, cancellationToken);
            var certificateIds = searchResponse.Certificates.Select(c => c.Id).ToList();

            if (!certificateIds.Any())
            {
                return DownloadResult.CreateError("No certificates found for the specified criteria.");
            }

            return await DownloadMultipleCertificatesAsync(certificateIds, customerId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading all certificates for customer {CustomerId}", customerId);
            return DownloadResult.CreateError("An error occurred while downloading all certificates. Please try again.");
        }
    }

    public async Task<DownloadStatistics> GetDownloadStatisticsAsync(int customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            // This would typically query a download tracking table
            // For now, return mock data
            await Task.CompletedTask; // Satisfy async requirement
            return new DownloadStatistics
            {
                TotalDownloads = 0,
                CertificatesDownloaded = 0,
                TotalBytesDownloaded = 0,
                LastDownloadDate = null,
                FirstDownloadDate = null,
                DownloadsByType = new Dictionary<string, int>(),
                DownloadsByMonth = new Dictionary<string, int>(),
                TopDownloadedCertificates = new List<TopDownloadedCertificate>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting download statistics for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task TrackDownloadAsync(int certificateId, int customerId, string downloadType, string fileName, long fileSize, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_options.EnableDownloadTracking) return;

            // This would typically insert into a download tracking table
            // For now, just log the download
            _logger.LogInformation("Download tracked: Certificate {CertificateId}, Customer {CustomerId}, Type {DownloadType}, File {FileName}, Size {FileSize}", 
                certificateId, customerId, downloadType, fileName, fileSize);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to track download for certificate {CertificateId}", certificateId);
        }
    }

    public async Task<bool> CanDownloadCertificateAsync(int certificateId, int customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if certificate exists and belongs to customer
            var certificate = await _certificateService.GetCertificateByIdAsync(certificateId, customerId, cancellationToken);
            return certificate != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking download permission for certificate {CertificateId}, customer {CustomerId}", certificateId, customerId);
            return false;
        }
    }

    public async Task<DownloadHistoryResponse> GetDownloadHistoryAsync(int customerId, int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            // This would typically query a download history table
            // For now, return empty response
            await Task.CompletedTask; // Satisfy async requirement
            return new DownloadHistoryResponse
            {
                Downloads = new List<DownloadHistoryEntry>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting download history for customer {CustomerId}", customerId);
            throw;
        }
    }

    private static string FormatFileSize(long bytes)
    {
        if (bytes == 0) return "0 Bytes";
        
        const int k = 1024;
        var sizes = new[] { "Bytes", "KB", "MB", "GB" };
        var i = (int)Math.Floor(Math.Log(bytes) / Math.Log(k));
        
        return $"{Math.Round(bytes / Math.Pow(k, i), 2)} {sizes[i]}";
    }
}
