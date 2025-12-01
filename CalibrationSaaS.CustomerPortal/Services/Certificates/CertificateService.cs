using CalibrationSaaS.CustomerPortal.Models.Certificates;
using CalibrationSaaS.CustomerPortal.Services.GrpcClients;
using Microsoft.Extensions.Logging;
using LocalCertificateDto = CalibrationSaaS.CustomerPortal.Models.Certificates.CertificateDto;
using LocalEquipmentDto = CalibrationSaaS.CustomerPortal.Models.Certificates.EquipmentDto;
using LocalCustomerDto = CalibrationSaaS.CustomerPortal.Models.Certificates.CustomerDto;
using GrpcEquipmentDto = CalibrationSaaS.CustomerPortal.Services.GrpcClients.EquipmentDto;
using GrpcCustomerDto = CalibrationSaaS.CustomerPortal.Services.GrpcClients.CustomerDto;
using System.Diagnostics;

namespace CalibrationSaaS.CustomerPortal.Services.Certificates;

/// <summary>
/// Implementation of certificate service
/// </summary>
public class CertificateService : ICertificateService
{
    private readonly IReportGrpcService _reportGrpcService;
    private readonly IWorkOrderGrpcService _workOrderGrpcService;
    private readonly ILogger<CertificateService> _logger;

    public CertificateService(
        IReportGrpcService reportGrpcService,
        IWorkOrderGrpcService workOrderGrpcService,
        ILogger<CertificateService> logger)
    {
        _reportGrpcService = reportGrpcService;
        _workOrderGrpcService = workOrderGrpcService;
        _logger = logger;
    }

    public async Task<CertificateSearchResponse> SearchCertificatesAsync(
        CertificateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogInformation("Searching certificates for customer {CustomerId} with request {@Request}", 
                customerId, request);

            // Validate request
            if (!request.IsValid(out var errors))
            {
                _logger.LogWarning("Invalid search request: {Errors}", string.Join(", ", errors));
                return new CertificateSearchResponse
                {
                    Certificates = new List<LocalCertificateDto>(),
                    TotalCount = 0,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    ExecutionTimeMs = stopwatch.ElapsedMilliseconds
                };
            }

            // Get certificates from completed Work Order Details
            var certificates = await GetCertificatesFromWorkOrdersAsync(customerId, request, cancellationToken);

            // Apply additional filtering and sorting
            var filteredCertificates = ApplyFilters(certificates, request);
            var sortedCertificates = ApplySorting(filteredCertificates, request);
            
            // Apply pagination
            var totalCount = sortedCertificates.Count;
            var pagedCertificates = sortedCertificates
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToList();

            // Build applied filters summary
            var appliedFilters = BuildAppliedFilters(request);

            stopwatch.Stop();

            _logger.LogInformation("Certificate search completed in {ElapsedMs}ms. Found {TotalCount} certificates, returning {PageCount}", 
                stopwatch.ElapsedMilliseconds, totalCount, pagedCertificates.Count);

            return new CertificateSearchResponse
            {
                Certificates = pagedCertificates,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                AppliedFilters = appliedFilters
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching certificates for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<LocalCertificateDto?> GetCertificateByIdAsync(
        int certificateId,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting certificate {CertificateId} for customer {CustomerId}",
                certificateId, customerId);

            // Get certificate from work order details
            // The certificateId corresponds to a WorkOrderDetailId
            var workOrderDetail = await _workOrderGrpcService.GetWorkOrderDetailByIdAsync(
                certificateId, customerId, "default", cancellationToken);

            if (workOrderDetail == null)
            {
                _logger.LogWarning("Work order detail {WorkOrderDetailId} not found for customer {CustomerId}",
                    certificateId, customerId);
                return null;
            }

            // Get the parent work order to get completion information
            var workOrder = await _workOrderGrpcService.GetWorkOrderByIdAsync(
                workOrderDetail.WorkOrderId, customerId, "default");

            if (workOrder == null || workOrder.Status != "Completed" || workOrder.CompletedDate == null)
            {
                _logger.LogWarning("Work order {WorkOrderId} not completed for certificate {CertificateId}",
                    workOrderDetail.WorkOrderId, certificateId);
                return null;
            }

            // Convert work order detail to certificate
            var certificate = new LocalCertificateDto
            {
                Id = workOrderDetail.WorkOrderDetailId,
                CertificateNumber = $"CERT-{workOrder.WorkOrderNumber}-{workOrderDetail.WorkOrderDetailId}",
                WorkOrderNumber = workOrder.WorkOrderNumber,
                WorkOrderId = workOrder.WorkOrderId,
                WorkOrderDetailId = workOrderDetail.WorkOrderDetailId,
                IssueDate = workOrder.CompletedDate.Value,
                Status = "Issued",
                Equipment = new LocalEquipmentDto
                {
                    Id = workOrderDetail.EquipmentId,
                    Description = $"{workOrderDetail.Manufacturer} {workOrderDetail.Model}".Trim(),
                    SerialNumber = workOrderDetail.SerialNumber ?? "N/A",
                    Manufacturer = workOrderDetail.Manufacturer ?? "Unknown",
                    Model = workOrderDetail.Model ?? "Unknown"
                },
                Customer = new LocalCustomerDto
                {
                    Id = workOrder.CustomerId,
                    Name = workOrder.CustomerName
                },
                CalibrationDate = workOrder.CompletedDate.Value,
                NextDueDate = workOrderDetail.NextDueDate,
                CalibrationStatus = workOrderDetail.CalibrationStatus ?? "Pass",
                TechnicalNotes = workOrderDetail.Notes ?? string.Empty,
                CreatedDate = workOrder.CreatedDate,
                CreatedBy = "System"
            };

            return certificate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certificate {CertificateId} for customer {CustomerId}",
                certificateId, customerId);
            throw;
        }
    }

    public async Task<LocalCertificateDto?> GetCertificateByCertificateNumberAsync(
        string certificateNumber, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting certificate by number {CertificateNumber} for customer {CustomerId}", 
                certificateNumber, customerId);

            var certificate = await _reportGrpcService.GetCertificateByCertificateNumberAsync(
                certificateNumber, customerId, cancellationToken);

            return certificate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certificate by number {CertificateNumber} for customer {CustomerId}", 
                certificateNumber, customerId);
            throw;
        }
    }

    public async Task<List<LocalCertificateDto>> GetCertificatesForEquipmentAsync(
        int equipmentId, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting certificates for equipment {EquipmentId} for customer {CustomerId}", 
                equipmentId, customerId);

            var certificates = await _reportGrpcService.GetCertificatesForEquipmentAsync(
                equipmentId, customerId, cancellationToken);

            return certificates ?? new List<LocalCertificateDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certificates for equipment {EquipmentId} for customer {CustomerId}", 
                equipmentId, customerId);
            throw;
        }
    }

    public async Task<List<LocalCertificateDto>> GetRecentCertificatesAsync(
        int customerId, 
        int count = 10, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting {Count} recent certificates for customer {CustomerId}", 
                count, customerId);

            var request = new CertificateSearchRequest
            {
                Page = 1,
                PageSize = count,
                SortBy = CertificateSortOptions.IssueDate,
                SortDirection = "desc"
            };

            var response = await SearchCertificatesAsync(request, customerId, cancellationToken);
            return response.Certificates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recent certificates for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<List<LocalCertificateDto>> GetCertificatesExpiringSoonAsync(
        int customerId, 
        int days = 30, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting certificates expiring in {Days} days for customer {CustomerId}", 
                days, customerId);

            var request = new CertificateSearchRequest
            {
                ExpiringSoonDays = days,
                Page = 1,
                PageSize = 1000, // Get all expiring certificates
                SortBy = CertificateSortOptions.NextCalibrationDate,
                SortDirection = "asc"
            };

            var response = await SearchCertificatesAsync(request, customerId, cancellationToken);
            return response.Certificates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certificates expiring soon for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<List<LocalCertificateDto>> GetOverdueCertificatesAsync(
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting overdue certificates for customer {CustomerId}", customerId);

            var request = new CertificateSearchRequest
            {
                IsOverdue = true,
                Page = 1,
                PageSize = 1000, // Get all overdue certificates
                SortBy = CertificateSortOptions.NextCalibrationDate,
                SortDirection = "asc"
            };

            var response = await SearchCertificatesAsync(request, customerId, cancellationToken);
            return response.Certificates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting overdue certificates for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CertificateStatistics> GetCertificateStatisticsAsync(
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting certificate statistics for customer {CustomerId}", customerId);

            var statistics = await _reportGrpcService.GetCertificateStatisticsAsync(
                customerId, cancellationToken);

            return statistics ?? new CertificateStatistics();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certificate statistics for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CertificateFilterOptions> GetFilterOptionsAsync(
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting filter options for customer {CustomerId}", customerId);

            var filterOptions = await _reportGrpcService.GetCertificateFilterOptionsAsync(
                customerId, cancellationToken);

            return filterOptions ?? new CertificateFilterOptions();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting filter options for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CertificateDownloadResult> DownloadCertificateAsync(
        int certificateId, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Downloading certificate {CertificateId} for customer {CustomerId}", 
                certificateId, customerId);

            var result = await _reportGrpcService.DownloadCertificateAsync(
                certificateId, customerId, cancellationToken);

            if (result?.Success == true)
            {
                // Track the download
                await TrackDownloadAsync(certificateId, customerId, "single", cancellationToken);
            }

            return result ?? CertificateDownloadResult.Failure("Certificate not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading certificate {CertificateId} for customer {CustomerId}", 
                certificateId, customerId);
            return CertificateDownloadResult.Failure($"Download failed: {ex.Message}");
        }
    }

    public async Task<CertificateDownloadResult> DownloadMultipleCertificatesAsync(
        List<int> certificateIds, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Downloading {Count} certificates for customer {CustomerId}", 
                certificateIds.Count, customerId);

            var result = await _reportGrpcService.DownloadMultipleCertificatesAsync(
                certificateIds, customerId, cancellationToken);

            if (result?.Success == true)
            {
                // Track downloads for all certificates
                foreach (var certificateId in certificateIds)
                {
                    await TrackDownloadAsync(certificateId, customerId, "bulk", cancellationToken);
                }
            }

            return result ?? CertificateDownloadResult.Failure("Certificates not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading multiple certificates for customer {CustomerId}", customerId);
            return CertificateDownloadResult.Failure($"Bulk download failed: {ex.Message}");
        }
    }

    public async Task<bool> TrackDownloadAsync(
        int certificateId, 
        int customerId, 
        string downloadType, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Tracking download for certificate {CertificateId}, customer {CustomerId}, type {DownloadType}", 
                certificateId, customerId, downloadType);

            return await _reportGrpcService.TrackCertificateDownloadAsync(
                certificateId, customerId, downloadType, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking download for certificate {CertificateId}", certificateId);
            return false;
        }
    }

    private List<LocalCertificateDto> ApplyFilters(List<LocalCertificateDto> certificates, CertificateSearchRequest request)
    {
        var filtered = certificates.AsQueryable();

        // Apply text search
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var searchText = request.SearchText.ToLower();
            filtered = filtered.Where(c => 
                c.CertificateNumber.ToLower().Contains(searchText) ||
                c.WorkOrderNumber.ToLower().Contains(searchText) ||
                c.Equipment.SerialNumber.ToLower().Contains(searchText) ||
                c.Equipment.Description.ToLower().Contains(searchText) ||
                (c.Equipment.Manufacturer != null && c.Equipment.Manufacturer.ToLower().Contains(searchText)) ||
                (c.Equipment.Model != null && c.Equipment.Model.ToLower().Contains(searchText)));
        }

        // Apply overdue filter
        if (request.IsOverdue == true)
        {
            filtered = filtered.Where(c => c.NextCalibrationDate.HasValue && 
                                         c.NextCalibrationDate.Value < DateTime.Now);
        }

        // Apply expiring soon filter
        if (request.ExpiringSoonDays.HasValue)
        {
            var expiringDate = DateTime.Now.AddDays(request.ExpiringSoonDays.Value);
            filtered = filtered.Where(c => c.NextCalibrationDate.HasValue && 
                                         c.NextCalibrationDate.Value <= expiringDate &&
                                         c.NextCalibrationDate.Value >= DateTime.Now);
        }

        // Apply compliance filter
        if (request.IsCompliant.HasValue)
        {
            filtered = filtered.Where(c => c.IsCompliant == request.IsCompliant.Value);
        }

        return filtered.ToList();
    }

    private List<LocalCertificateDto> ApplySorting(List<LocalCertificateDto> certificates, CertificateSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SortBy))
            return certificates;

        var isDescending = request.SortDirection?.ToLower() == "desc";

        return request.SortBy switch
        {
            CertificateSortOptions.IssueDate => isDescending 
                ? certificates.OrderByDescending(c => c.IssueDate).ToList()
                : certificates.OrderBy(c => c.IssueDate).ToList(),
            CertificateSortOptions.CalibrationDate => isDescending 
                ? certificates.OrderByDescending(c => c.CalibrationDate).ToList()
                : certificates.OrderBy(c => c.CalibrationDate).ToList(),
            CertificateSortOptions.NextCalibrationDate => isDescending 
                ? certificates.OrderByDescending(c => c.NextCalibrationDate).ToList()
                : certificates.OrderBy(c => c.NextCalibrationDate).ToList(),
            CertificateSortOptions.CertificateNumber => isDescending 
                ? certificates.OrderByDescending(c => c.CertificateNumber).ToList()
                : certificates.OrderBy(c => c.CertificateNumber).ToList(),
            CertificateSortOptions.WorkOrderNumber => isDescending 
                ? certificates.OrderByDescending(c => c.WorkOrderNumber).ToList()
                : certificates.OrderBy(c => c.WorkOrderNumber).ToList(),
            CertificateSortOptions.EquipmentSerialNumber => isDescending 
                ? certificates.OrderByDescending(c => c.Equipment.SerialNumber).ToList()
                : certificates.OrderBy(c => c.Equipment.SerialNumber).ToList(),
            CertificateSortOptions.EquipmentDescription => isDescending 
                ? certificates.OrderByDescending(c => c.Equipment.Description).ToList()
                : certificates.OrderBy(c => c.Equipment.Description).ToList(),
            CertificateSortOptions.CustomerName => isDescending 
                ? certificates.OrderByDescending(c => c.Customer.Name).ToList()
                : certificates.OrderBy(c => c.Customer.Name).ToList(),
            CertificateSortOptions.Status => isDescending 
                ? certificates.OrderByDescending(c => c.Status).ToList()
                : certificates.OrderBy(c => c.Status).ToList(),
            CertificateSortOptions.TechnicianName => isDescending 
                ? certificates.OrderByDescending(c => c.TechnicianName).ToList()
                : certificates.OrderBy(c => c.TechnicianName).ToList(),
            _ => certificates
        };
    }

    private Dictionary<string, object> BuildAppliedFilters(CertificateSearchRequest request)
    {
        var filters = new Dictionary<string, object>();

        if (!string.IsNullOrWhiteSpace(request.CertificateNumber))
            filters["Certificate Number"] = request.CertificateNumber;
        
        if (!string.IsNullOrWhiteSpace(request.WorkOrderNumber))
            filters["Work Order Number"] = request.WorkOrderNumber;
        
        if (!string.IsNullOrWhiteSpace(request.Status))
            filters["Status"] = request.Status;
        
        if (request.IssueDateFrom.HasValue)
            filters["Issue Date From"] = request.IssueDateFrom.Value.ToString("yyyy-MM-dd");
        
        if (request.IssueDateTo.HasValue)
            filters["Issue Date To"] = request.IssueDateTo.Value.ToString("yyyy-MM-dd");
        
        if (request.IsOverdue == true)
            filters["Overdue"] = "Yes";
        
        if (request.ExpiringSoonDays.HasValue)
            filters["Expiring Soon"] = $"{request.ExpiringSoonDays} days";
        
        if (request.IsCompliant.HasValue)
            filters["Compliant"] = request.IsCompliant.Value ? "Yes" : "No";

        return filters;
    }

    /// <summary>
    /// Get certificates from completed Work Order Details
    /// </summary>
    private async Task<List<LocalCertificateDto>> GetCertificatesFromWorkOrdersAsync(
        int customerId,
        CertificateSearchRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting certificates from work orders for customer {CustomerId}", customerId);

            // Get work orders for the customer - use a large page size to get all work orders
            var workOrdersResult = await _workOrderGrpcService.GetWorkOrdersAsync(
                customerId,
                request.TenantId ?? "default", // Use tenant from request or default
                1,
                1000, // Large page size to get all work orders
                cancellationToken: cancellationToken);

            if (workOrdersResult?.WorkOrders == null)
            {
                _logger.LogWarning("No work orders found for customer {CustomerId}", customerId);
                return new List<LocalCertificateDto>();
            }

            var certificates = new List<LocalCertificateDto>();

            // Convert work orders to certificates - focus on completed work order details
            foreach (var workOrder in workOrdersResult.WorkOrders)
            {
                // Get work order details for this work order
                var workOrderDetails = await _workOrderGrpcService.GetWorkOrderDetailsAsync(
                    workOrder.WorkOrderId, customerId, request.TenantId ?? "default");

                // Create a certificate for each completed piece of equipment in the work order
                if (workOrderDetails != null)
                {
                    foreach (var detail in workOrderDetails.Where(d => d.IsCompleted))
                    {
                        // Use calibration date from detail if available, otherwise work order completion date
                        var calibrationDate = detail.CalibrationDate ?? workOrder.CompletedDate ?? DateTime.UtcNow;

                        var certificate = new LocalCertificateDto
                        {
                            Id = detail.WorkOrderDetailId,
                            CertificateNumber = $"CERT-{workOrder.WorkOrderNumber}-{detail.WorkOrderDetailId}",
                            WorkOrderNumber = workOrder.WorkOrderNumber,
                            WorkOrderId = workOrder.WorkOrderId,
                            WorkOrderDetailId = detail.WorkOrderDetailId,
                            IssueDate = calibrationDate,
                            Status = "Issued",
                            Equipment = new LocalEquipmentDto
                            {
                                Id = detail.EquipmentId,
                                Description = !string.IsNullOrEmpty(detail.Description) ? detail.Description : $"{detail.Manufacturer} {detail.Model}".Trim(),
                                SerialNumber = detail.SerialNumber ?? "N/A",
                                Manufacturer = detail.Manufacturer ?? "Unknown",
                                Model = detail.Model ?? "Unknown"
                            },
                            Customer = new LocalCustomerDto
                            {
                                Id = workOrder.CustomerId,
                                Name = workOrder.CustomerName
                            },
                            CalibrationDate = calibrationDate,
                            NextDueDate = detail.NextDueDate,
                            CalibrationStatus = detail.CalibrationStatus ?? "Pass",
                            TechnicalNotes = detail.Notes ?? string.Empty,
                            CreatedDate = workOrder.CreatedDate,
                            CreatedBy = "System" // Could be enhanced to get actual user
                        };

                        certificates.Add(certificate);
                    }
                }
            }

            _logger.LogInformation("Found {CertificateCount} certificates from {WorkOrderCount} work orders",
                certificates.Count, workOrdersResult.WorkOrders.Count);

            return certificates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certificates from work orders for customer {CustomerId}", customerId);
            return new List<LocalCertificateDto>();
        }
    }

}
