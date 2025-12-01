using CalibrationSaaS.CustomerPortal.Services.Downloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Controllers;

/// <summary>
/// Controller for handling certificate downloads
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DownloadController : ControllerBase
{
    private readonly IDownloadService _downloadService;
    private readonly ILogger<DownloadController> _logger;

    public DownloadController(IDownloadService downloadService, ILogger<DownloadController> logger)
    {
        _downloadService = downloadService;
        _logger = logger;
    }

    /// <summary>
    /// Downloads a single certificate as PDF
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PDF file</returns>
    [HttpGet("certificate/{certificateId:int}")]
    public async Task<IActionResult> DownloadCertificate(int certificateId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("🎬 DEMO MODE: Downloading certificate {CertificateId}", certificateId);

            // Demo: Return the specific PDF file for demo purposes
            var contentRoot = Directory.GetCurrentDirectory();
            var pdfPath = Path.Combine(contentRoot, "Calibration_Certificate_Kavoku_YU6754.pdf");

            _logger.LogInformation("🔍 Looking for demo PDF at: {PdfPath}", pdfPath);

            if (System.IO.File.Exists(pdfPath))
            {
                _logger.LogInformation("📄 Demo PDF file found, serving for certificate {CertificateId}", certificateId);

                var pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfPath, cancellationToken);
                var fileName = $"Certificate_Kavoku_{certificateId}.pdf";

                _logger.LogInformation("✅ Demo PDF served successfully, size: {Size} bytes", pdfBytes.Length);

                return File(pdfBytes, "application/pdf", fileName);
            }
            else
            {
                // Try alternative paths
                var alternativePaths = new[]
                {
                    Path.Combine(contentRoot, "wwwroot", "Calibration_Certificate_Kavoku_YU6754.pdf"),
                    Path.Combine(contentRoot, "..", "..", "..", "Calibration_Certificate_Kavoku_YU6754.pdf"),
                    "/Users/javier/repos/CalibrationSaaS/src/CalibrationSaaS/CalibrationSaaS.CustomerPortal/Calibration_Certificate_Kavoku_YU6754.pdf"
                };

                foreach (var altPath in alternativePaths)
                {
                    _logger.LogInformation("🔍 Trying alternative path: {AltPath}", altPath);
                    if (System.IO.File.Exists(altPath))
                    {
                        _logger.LogInformation("📄 Demo PDF found at alternative path: {AltPath}", altPath);
                        var pdfBytes = await System.IO.File.ReadAllBytesAsync(altPath, cancellationToken);
                        var fileName = $"Certificate_Kavoku_{certificateId}.pdf";

                        _logger.LogInformation("✅ Demo PDF served successfully from alternative path, size: {Size} bytes", pdfBytes.Length);

                        return File(pdfBytes, "application/pdf", fileName);
                    }
                }

                _logger.LogError("❌ Demo PDF file not found in any of the expected locations");
                return NotFound(new { error = "Demo certificate file not found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error downloading demo certificate {CertificateId}", certificateId);
            return StatusCode(500, new { error = "An error occurred while downloading the certificate" });
        }
    }

    /// <summary>
    /// Downloads multiple certificates as ZIP archive
    /// </summary>
    /// <param name="request">Bulk download request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ZIP file</returns>
    [HttpPost("certificates/bulk")]
    public async Task<IActionResult> DownloadMultipleCertificates([FromBody] BulkDownloadRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get customer ID from authentication context
            var customerId = 1; // Placeholder

            var result = await _downloadService.DownloadMultipleCertificatesAsync(request.CertificateIds, customerId, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            if (result.FileData == null)
            {
                return NotFound(new { error = "No certificates found for download" });
            }

            return File(result.FileData, result.ContentType!, result.FileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading multiple certificates");
            return StatusCode(500, new { error = "An error occurred while downloading certificates" });
        }
    }

    /// <summary>
    /// Downloads all certificates for a customer
    /// </summary>
    /// <param name="dateFrom">Optional start date filter</param>
    /// <param name="dateTo">Optional end date filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ZIP file</returns>
    [HttpGet("certificates/all")]
    public async Task<IActionResult> DownloadAllCertificates(
        [FromQuery] DateTime? dateFrom = null, 
        [FromQuery] DateTime? dateTo = null, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Get customer ID from authentication context
            var customerId = 1; // Placeholder

            var result = await _downloadService.DownloadAllCertificatesAsync(customerId, dateFrom, dateTo, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            if (result.FileData == null)
            {
                return NotFound(new { error = "No certificates found for download" });
            }

            return File(result.FileData, result.ContentType!, result.FileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading all certificates");
            return StatusCode(500, new { error = "An error occurred while downloading all certificates" });
        }
    }

    /// <summary>
    /// Gets download statistics for the authenticated customer
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download statistics</returns>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetDownloadStatistics(CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Get customer ID from authentication context
            var customerId = 1; // Placeholder

            var statistics = await _downloadService.GetDownloadStatisticsAsync(customerId, cancellationToken);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting download statistics");
            return StatusCode(500, new { error = "An error occurred while retrieving download statistics" });
        }
    }

    /// <summary>
    /// Gets download history for the authenticated customer
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Download history</returns>
    [HttpGet("history")]
    public async Task<IActionResult> GetDownloadHistory(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 50, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            // TODO: Get customer ID from authentication context
            var customerId = 1; // Placeholder

            var history = await _downloadService.GetDownloadHistoryAsync(customerId, pageNumber, pageSize, cancellationToken);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting download history");
            return StatusCode(500, new { error = "An error occurred while retrieving download history" });
        }
    }

    /// <summary>
    /// Tracks a download event (for analytics)
    /// </summary>
    /// <param name="request">Download tracking request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response</returns>
    [HttpPost("track")]
    public async Task<IActionResult> TrackDownload([FromBody] DownloadTrackingRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get customer ID from authentication context
            var customerId = 1; // Placeholder

            var userAgent = Request.Headers.UserAgent.ToString();

            await _downloadService.TrackDownloadAsync(
                request.CertificateId, 
                customerId, 
                request.DownloadType, 
                request.FileName, 
                request.FileSize, 
                userAgent, 
                cancellationToken);

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking download");
            return StatusCode(500, new { error = "An error occurred while tracking the download" });
        }
    }

    /// <summary>
    /// Validates if a certificate can be downloaded
    /// </summary>
    /// <param name="certificateId">Certificate ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    [HttpGet("certificate/{certificateId:int}/validate")]
    public async Task<IActionResult> ValidateDownload(int certificateId, CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Get customer ID from authentication context
            var customerId = 1; // Placeholder

            var canDownload = await _downloadService.CanDownloadCertificateAsync(certificateId, customerId, cancellationToken);
            
            return Ok(new { canDownload, certificateId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating download for certificate {CertificateId}", certificateId);
            return StatusCode(500, new { error = "An error occurred while validating the download" });
        }
    }
}

/// <summary>
/// Request model for bulk certificate downloads
/// </summary>
public class BulkDownloadRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one certificate ID is required")]
    [MaxLength(100, ErrorMessage = "Cannot download more than 100 certificates at once")]
    public List<int> CertificateIds { get; set; } = new();
}

/// <summary>
/// Request model for download tracking
/// </summary>
public class DownloadTrackingRequest
{
    [Required]
    public int CertificateId { get; set; }

    [Required]
    [StringLength(50)]
    public string DownloadType { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [Range(1, long.MaxValue)]
    public long FileSize { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string? UserAgent { get; set; }
}
