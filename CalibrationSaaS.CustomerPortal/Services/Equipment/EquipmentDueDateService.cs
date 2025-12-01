using CalibrationSaaS.CustomerPortal.Models.Equipment;
using CalibrationSaaS.CustomerPortal.Models.ViewModels;
using CalibrationSaaS.CustomerPortal.Services.GrpcClients;
using Microsoft.Extensions.Logging;
using LocalEquipmentDueDateDto = CalibrationSaaS.CustomerPortal.Models.Equipment.EquipmentDueDateDto;

namespace CalibrationSaaS.CustomerPortal.Services.Equipment;

/// <summary>
/// Service implementation for equipment due date management
/// </summary>
public class EquipmentDueDateService : IEquipmentDueDateService
{
    private readonly ILogger<EquipmentDueDateService> _logger;
    private readonly IWorkOrderGrpcService _workOrderGrpcService;

    public EquipmentDueDateService(
        ILogger<EquipmentDueDateService> logger,
        IWorkOrderGrpcService workOrderGrpcService)
    {
        _logger = logger;
        _workOrderGrpcService = workOrderGrpcService;
    }

    public async Task<DueDateSearchResponse> SearchEquipmentDueDatesAsync(
        DueDateSearchRequest request,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching equipment due dates for customer {CustomerId} with request: {@Request}",
                customerId, request);

            // Get equipment due dates from gRPC service
            var equipmentDueDates = await _workOrderGrpcService.GetEquipmentDueDatesAsync(
                customerId,
                request.TenantId ?? "default",
                365, // Look ahead 365 days to get all equipment
                cancellationToken);

            if (equipmentDueDates == null)
            {
                _logger.LogWarning("No equipment due dates returned from gRPC service for customer {CustomerId}", customerId);
                equipmentDueDates = new List<Services.GrpcClients.EquipmentDueDateDto>();
            }

            // Convert gRPC DTOs to local DTOs
            var localEquipment = equipmentDueDates.Select(ConvertToLocalDto).ToList();

            // Apply filters
            var filteredEquipment = ApplyFilters(localEquipment, request);

            // Apply sorting
            var sortedEquipment = ApplySorting(filteredEquipment, request);

            // Apply pagination
            var totalCount = sortedEquipment.Count;
            var pagedEquipment = sortedEquipment
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Calculate summary
            var summary = CalculateSummary(filteredEquipment);
            
            // Get filter options
            var filterOptions = await GetFilterOptionsAsync(customerId, cancellationToken);
            
            // Build applied filters
            var appliedFilters = BuildAppliedFilters(request);

            var response = new DueDateSearchResponse
            {
                Equipment = pagedEquipment,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                Summary = summary,
                FilterOptions = filterOptions,
                AppliedFilters = appliedFilters
            };

            _logger.LogInformation("Equipment due date search completed for customer {CustomerId}: {TotalCount} results", 
                customerId, totalCount);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching equipment due dates for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<LocalEquipmentDueDateDto?> GetEquipmentDueDateAsync(
        int equipmentId, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting equipment due date for equipment {EquipmentId}, customer {CustomerId}", 
                equipmentId, customerId);

            // TODO: Replace with actual gRPC call
            var mockEquipment = await GetMockEquipmentDueDates(customerId);
            var equipment = mockEquipment.FirstOrDefault(e => e.EquipmentId == equipmentId);

            if (equipment != null)
            {
                _logger.LogInformation("Equipment due date found for equipment {EquipmentId}", equipmentId);
            }
            else
            {
                _logger.LogWarning("Equipment due date not found for equipment {EquipmentId}", equipmentId);
            }

            return equipment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipment due date for equipment {EquipmentId}", equipmentId);
            throw;
        }
    }

    public async Task<List<LocalEquipmentDueDateDto>> GetOverdueEquipmentAsync(
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting overdue equipment for customer {CustomerId}", customerId);

            var request = new DueDateSearchRequest
            {
                IsOverdue = true,
                PageSize = int.MaxValue,
                SortBy = DueDateSortOptions.DaysOverdue,
                SortDirection = "desc"
            };

            var response = await SearchEquipmentDueDatesAsync(request, customerId, cancellationToken);
            
            _logger.LogInformation("Found {Count} overdue equipment for customer {CustomerId}", 
                response.Equipment.Count, customerId);

            return response.Equipment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting overdue equipment for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<List<LocalEquipmentDueDateDto>> GetEquipmentExpiringSoonAsync(
        int customerId, 
        int days = 30, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting equipment expiring soon for customer {CustomerId} within {Days} days", 
                customerId, days);

            var request = new DueDateSearchRequest
            {
                IsExpiringSoon = true,
                ExpiringSoonDays = days,
                PageSize = int.MaxValue,
                SortBy = DueDateSortOptions.NextCalibrationDate,
                SortDirection = "asc"
            };

            var response = await SearchEquipmentDueDatesAsync(request, customerId, cancellationToken);
            
            _logger.LogInformation("Found {Count} equipment expiring soon for customer {CustomerId}", 
                response.Equipment.Count, customerId);

            return response.Equipment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipment expiring soon for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<List<LocalEquipmentDueDateDto>> GetEquipmentDueInRangeAsync(
        int customerId, 
        DateTime fromDate, 
        DateTime toDate, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting equipment due between {FromDate} and {ToDate} for customer {CustomerId}", 
                fromDate, toDate, customerId);

            var request = new DueDateSearchRequest
            {
                DueDateFrom = fromDate,
                DueDateTo = toDate,
                PageSize = int.MaxValue,
                SortBy = DueDateSortOptions.NextCalibrationDate,
                SortDirection = "asc"
            };

            var response = await SearchEquipmentDueDatesAsync(request, customerId, cancellationToken);
            
            _logger.LogInformation("Found {Count} equipment due in range for customer {CustomerId}", 
                response.Equipment.Count, customerId);

            return response.Equipment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipment due in range for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<DueDateDashboardSummary> GetDueDateSummaryAsync(
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting due date summary for customer {CustomerId}", customerId);

            // Get all equipment
            var allEquipmentRequest = new DueDateSearchRequest
            {
                PageSize = int.MaxValue
            };
            var allEquipmentResponse = await SearchEquipmentDueDatesAsync(allEquipmentRequest, customerId, cancellationToken);
            var allEquipment = allEquipmentResponse.Equipment;

            var now = DateTime.Now;
            var thisWeekEnd = now.AddDays(7);
            var thisMonthEnd = now.AddDays(30);
            var nextMonthEnd = now.AddDays(60);

            var summary = new DueDateDashboardSummary
            {
                TotalEquipment = allEquipment.Count,
                OverdueEquipment = allEquipment.Count(e => e.IsOverdue),
                ExpiringSoonEquipment = allEquipment.Count(e => e.IsExpiringSoon),
                CurrentEquipment = allEquipment.Count(e => e.DueStatus == DueDateStatus.Current),
                DueThisWeek = allEquipment.Count(e => e.NextCalibrationDate.HasValue && 
                    e.NextCalibrationDate.Value >= now && e.NextCalibrationDate.Value <= thisWeekEnd),
                DueThisMonth = allEquipment.Count(e => e.NextCalibrationDate.HasValue && 
                    e.NextCalibrationDate.Value >= now && e.NextCalibrationDate.Value <= thisMonthEnd),
                DueNextMonth = allEquipment.Count(e => e.NextCalibrationDate.HasValue && 
                    e.NextCalibrationDate.Value > thisMonthEnd && e.NextCalibrationDate.Value <= nextMonthEnd),
                UpcomingDueDates = allEquipment
                    .Where(e => e.NextCalibrationDate.HasValue && e.NextCalibrationDate.Value >= now)
                    .OrderBy(e => e.NextCalibrationDate)
                    .Take(10)
                    .ToList(),
                OverdueEquipmentList = allEquipment
                    .Where(e => e.IsOverdue)
                    .OrderByDescending(e => e.DaysOverdue)
                    .Take(10)
                    .ToList()
            };

            // Set next due date
            var nextDue = allEquipment
                .Where(e => e.NextCalibrationDate.HasValue && e.NextCalibrationDate.Value >= now)
                .OrderBy(e => e.NextCalibrationDate)
                .FirstOrDefault();

            if (nextDue != null)
            {
                summary.NextDueDate = nextDue.NextCalibrationDate;
                summary.NextDueEquipment = nextDue.Description;
                summary.DaysToNextDue = nextDue.DaysUntilDue;
            }

            // Get trend data
            summary.TrendData = await GetDueDateTrendDataAsync(customerId, 12, cancellationToken);

            _logger.LogInformation("Due date summary calculated for customer {CustomerId}: {TotalEquipment} total, {OverdueEquipment} overdue", 
                customerId, summary.TotalEquipment, summary.OverdueEquipment);

            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting due date summary for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<DueDateFilterOptions> GetFilterOptionsAsync(
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting filter options for customer {CustomerId}", customerId);

            // TODO: Replace with actual gRPC call to get filter options
            await Task.CompletedTask; // Satisfy async requirement
            var filterOptions = new DueDateFilterOptions
            {
                EquipmentTypes = new List<string> { "Scale", "Balance", "Gauge", "Meter", "Thermometer" },
                EquipmentTypeGroups = new List<string> { "Weighing", "Pressure", "Temperature", "Electrical" },
                Manufacturers = new List<string> { "Mettler Toledo", "Sartorius", "Fluke", "Keysight" },
                Models = new List<string> { "XS205", "ME204", "8845A", "34461A" },
                Locations = new List<string> { "Lab A", "Lab B", "Production", "QC" },
                Departments = new List<string> { "Quality", "Production", "R&D", "Maintenance" },
                CalibrationStatuses = new List<string> { "Current", "Overdue", "Expiring", "Unknown" },
                CalibrationIntervals = new List<int> { 3, 6, 12, 24, 36 },
                EarliestDueDate = DateTime.Now.AddYears(-2),
                LatestDueDate = DateTime.Now.AddYears(2),
                EarliestCalibrationDate = DateTime.Now.AddYears(-5),
                LatestCalibrationDate = DateTime.Now
            };

            return filterOptions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting filter options for customer {CustomerId}", customerId);
            throw;
        }
    }

    public DueDateStatus CalculateDueDateStatus(DateTime? nextCalibrationDate, int expiringSoonDays = 30)
    {
        if (!nextCalibrationDate.HasValue)
            return DueDateStatus.Unknown;

        var now = DateTime.Now.Date;
        var dueDate = nextCalibrationDate.Value.Date;

        if (dueDate < now)
            return DueDateStatus.Overdue;

        if (dueDate <= now.AddDays(expiringSoonDays))
            return DueDateStatus.ExpiringSoon;

        return DueDateStatus.Current;
    }

    public (int DaysUntilDue, int DaysOverdue) CalculateDueDateDays(DateTime? nextCalibrationDate)
    {
        if (!nextCalibrationDate.HasValue)
            return (0, 0);

        var now = DateTime.Now.Date;
        var dueDate = nextCalibrationDate.Value.Date;
        var daysDifference = (dueDate - now).Days;

        if (daysDifference >= 0)
            return (daysDifference, 0);
        else
            return (0, Math.Abs(daysDifference));
    }

    public async Task<ExportResult> ExportToExcelAsync(
        DueDateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Exporting equipment due dates to Excel for customer {CustomerId}", customerId);

            // Get all data for export (no pagination)
            var exportRequest = new DueDateSearchRequest
            {
                SearchTerm = request.SearchTerm,
                SerialNumber = request.SerialNumber,
                EquipmentType = request.EquipmentType,
                DueStatus = request.DueStatus,
                IsOverdue = request.IsOverdue,
                DueDateFrom = request.DueDateFrom,
                DueDateTo = request.DueDateTo,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                PageSize = int.MaxValue,
                Page = 1
            };

            var response = await SearchEquipmentDueDatesAsync(exportRequest, customerId, cancellationToken);

            // TODO: Implement actual Excel generation
            var fileName = $"Equipment_Due_Dates_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var mockExcelData = System.Text.Encoding.UTF8.GetBytes("Mock Excel Data");

            _logger.LogInformation("Excel export completed for customer {CustomerId}: {Count} records", 
                customerId, response.Equipment.Count);

            return ExportResult.CreateSuccess(mockExcelData, fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting equipment due dates to Excel for customer {CustomerId}", customerId);
            return ExportResult.Failure($"Export failed: {ex.Message}");
        }
    }

    public async Task<ExportResult> ExportToPdfAsync(
        DueDateSearchRequest request, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Exporting equipment due dates to PDF for customer {CustomerId}", customerId);

            // Get all data for export (no pagination)
            var exportRequest = new DueDateSearchRequest
            {
                SearchTerm = request.SearchTerm,
                SerialNumber = request.SerialNumber,
                EquipmentType = request.EquipmentType,
                DueStatus = request.DueStatus,
                IsOverdue = request.IsOverdue,
                DueDateFrom = request.DueDateFrom,
                DueDateTo = request.DueDateTo,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                PageSize = int.MaxValue,
                Page = 1
            };

            var response = await SearchEquipmentDueDatesAsync(exportRequest, customerId, cancellationToken);

            // TODO: Implement actual PDF generation
            var fileName = $"Equipment_Due_Dates_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var mockPdfData = System.Text.Encoding.UTF8.GetBytes("Mock PDF Data");

            _logger.LogInformation("PDF export completed for customer {CustomerId}: {Count} records", 
                customerId, response.Equipment.Count);

            return ExportResult.CreateSuccess(mockPdfData, fileName, "application/pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting equipment due dates to PDF for customer {CustomerId}", customerId);
            return ExportResult.Failure($"Export failed: {ex.Message}");
        }
    }

    public async Task<List<DueDateHistoryEntry>> GetDueDateHistoryAsync(
        int equipmentId, 
        int customerId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting due date history for equipment {EquipmentId}, customer {CustomerId}",
                equipmentId, customerId);

            // TODO: Replace with actual gRPC call
            await Task.CompletedTask; // Satisfy async requirement
            var mockHistory = new List<DueDateHistoryEntry>
            {
                new()
                {
                    Id = 1,
                    EquipmentId = equipmentId,
                    EventDate = DateTime.Now.AddDays(-30),
                    EventType = "Calibration",
                    PreviousDueDate = DateTime.Now.AddDays(-395),
                    NewDueDate = DateTime.Now.AddDays(335),
                    Notes = "Annual calibration completed",
                    PerformedBy = "Tech A",
                    WorkOrderId = 12345,
                    WorkOrderNumber = "WO-2024-001",
                    CertificateId = 67890,
                    CertificateNumber = "CERT-2024-001",
                    PreviousStatus = DueDateStatus.Overdue,
                    NewStatus = DueDateStatus.Current
                }
            };

            _logger.LogInformation("Found {Count} history entries for equipment {EquipmentId}", 
                mockHistory.Count, equipmentId);

            return mockHistory;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting due date history for equipment {EquipmentId}", equipmentId);
            throw;
        }
    }

    public async Task<List<DueDateTrendData>> GetDueDateTrendDataAsync(
        int customerId, 
        int months = 12, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting due date trend data for customer {CustomerId} for {Months} months",
                customerId, months);

            // TODO: Replace with actual gRPC call
            await Task.CompletedTask; // Satisfy async requirement
            var trendData = new List<DueDateTrendData>();
            var startDate = DateTime.Now.AddMonths(-months);

            for (int i = 0; i < months; i++)
            {
                var date = startDate.AddMonths(i);
                trendData.Add(new DueDateTrendData
                {
                    Date = date,
                    DueCount = Random.Shared.Next(5, 25),
                    OverdueCount = Random.Shared.Next(0, 10),
                    CompletedCount = Random.Shared.Next(10, 30)
                });
            }

            _logger.LogInformation("Generated {Count} trend data points for customer {CustomerId}", 
                trendData.Count, customerId);

            return trendData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting due date trend data for customer {CustomerId}", customerId);
            throw;
        }
    }

    // Private helper methods
    private async Task<List<LocalEquipmentDueDateDto>> GetMockEquipmentDueDates(int customerId)
    {
        // TODO: Replace with actual gRPC call to get equipment data
        await Task.Delay(10); // Simulate async call

        var mockData = new List<LocalEquipmentDueDateDto>();
        var random = new Random();

        for (int i = 1; i <= 50; i++)
        {
            var lastCalibration = DateTime.Now.AddDays(-random.Next(30, 400));
            var nextCalibration = lastCalibration.AddDays(365); // 1 year interval
            var (daysUntilDue, daysOverdue) = CalculateDueDateDays(nextCalibration);

            var equipment = new LocalEquipmentDueDateDto
            {
                EquipmentId = i,
                SerialNumber = $"SN{i:D6}",
                Description = $"Test Equipment {i}",
                AssetTag = $"AT{i:D4}",
                Location = $"Lab {(char)('A' + (i % 4))}",
                Department = new[] { "Quality", "Production", "R&D", "Maintenance" }[i % 4],
                EquipmentType = new[] { "Scale", "Balance", "Gauge", "Meter", "Thermometer" }[i % 5],
                EquipmentTypeGroup = new[] { "Weighing", "Pressure", "Temperature", "Electrical" }[i % 4],
                Manufacturer = new[] { "Mettler Toledo", "Sartorius", "Fluke", "Keysight" }[i % 4],
                Model = $"Model-{i % 10}",
                LastCalibrationDate = lastCalibration,
                NextCalibrationDate = nextCalibration,
                CalibrationIntervalMonths = 12,
                CalibrationStatus = "Current",
                DaysUntilDue = daysUntilDue,
                DaysOverdue = daysOverdue,
                LastWorkOrderId = 1000 + i,
                LastWorkOrderNumber = $"WO-2024-{i:D3}",
                LastWorkOrderDate = lastCalibration,
                LastWorkOrderStatus = "Completed",
                LastCertificateId = 2000 + i,
                LastCertificateNumber = $"CERT-2024-{i:D3}",
                LastCertificateIssueDate = lastCalibration.AddDays(1),
                CustomerId = customerId,
                CustomerName = "Test Customer",
                IsActive = true,
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedDate = lastCalibration
            };

            equipment.DueStatus = CalculateDueDateStatus(equipment.NextCalibrationDate);
            mockData.Add(equipment);
        }

        return mockData;
    }

    private List<LocalEquipmentDueDateDto> ApplyFilters(List<LocalEquipmentDueDateDto> equipment, DueDateSearchRequest request)
    {
        var filtered = equipment.AsQueryable();

        // Search term filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            filtered = filtered.Where(e =>
                e.SerialNumber.ToLower().Contains(searchTerm) ||
                e.Description.ToLower().Contains(searchTerm) ||
                (e.AssetTag != null && e.AssetTag.ToLower().Contains(searchTerm)) ||
                e.Manufacturer.ToLower().Contains(searchTerm) ||
                e.Model.ToLower().Contains(searchTerm));
        }

        // Specific field filters
        if (!string.IsNullOrWhiteSpace(request.SerialNumber))
            filtered = filtered.Where(e => e.SerialNumber.Contains(request.SerialNumber, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.AssetTag))
            filtered = filtered.Where(e => e.AssetTag != null && e.AssetTag.Contains(request.AssetTag, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.EquipmentType))
            filtered = filtered.Where(e => e.EquipmentType.Equals(request.EquipmentType, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.EquipmentTypeGroup))
            filtered = filtered.Where(e => e.EquipmentTypeGroup.Equals(request.EquipmentTypeGroup, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Manufacturer))
            filtered = filtered.Where(e => e.Manufacturer.Equals(request.Manufacturer, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Model))
            filtered = filtered.Where(e => e.Model.Equals(request.Model, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Location))
            filtered = filtered.Where(e => e.Location != null && e.Location.Equals(request.Location, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Department))
            filtered = filtered.Where(e => e.Department != null && e.Department.Equals(request.Department, StringComparison.OrdinalIgnoreCase));

        // Due date status filters
        if (request.DueStatus.HasValue)
            filtered = filtered.Where(e => e.DueStatus == request.DueStatus.Value);

        if (request.IsOverdue.HasValue)
            filtered = filtered.Where(e => e.IsOverdue == request.IsOverdue.Value);

        if (request.IsExpiringSoon.HasValue)
            filtered = filtered.Where(e => e.IsExpiringSoon == request.IsExpiringSoon.Value);

        // Date range filters
        if (request.DueDateFrom.HasValue)
            filtered = filtered.Where(e => e.NextCalibrationDate >= request.DueDateFrom.Value);

        if (request.DueDateTo.HasValue)
            filtered = filtered.Where(e => e.NextCalibrationDate <= request.DueDateTo.Value);

        if (request.LastCalibrationFrom.HasValue)
            filtered = filtered.Where(e => e.LastCalibrationDate >= request.LastCalibrationFrom.Value);

        if (request.LastCalibrationTo.HasValue)
            filtered = filtered.Where(e => e.LastCalibrationDate <= request.LastCalibrationTo.Value);

        // Other filters
        if (!string.IsNullOrWhiteSpace(request.CalibrationStatus))
            filtered = filtered.Where(e => e.CalibrationStatus.Equals(request.CalibrationStatus, StringComparison.OrdinalIgnoreCase));

        if (request.CalibrationIntervalMonths.HasValue)
            filtered = filtered.Where(e => e.CalibrationIntervalMonths == request.CalibrationIntervalMonths.Value);

        if (request.IsActive.HasValue)
            filtered = filtered.Where(e => e.IsActive == request.IsActive.Value);

        if (request.HasCertificate.HasValue)
            filtered = filtered.Where(e => (e.LastCertificateId.HasValue) == request.HasCertificate.Value);

        if (request.HasWorkOrder.HasValue)
            filtered = filtered.Where(e => (e.LastWorkOrderId.HasValue) == request.HasWorkOrder.Value);

        return filtered.ToList();
    }

    private List<LocalEquipmentDueDateDto> ApplySorting(List<LocalEquipmentDueDateDto> equipment, DueDateSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SortBy))
            return equipment.OrderBy(e => e.NextCalibrationDate).ToList();

        var isDescending = request.SortDirection?.ToLower() == "desc";

        return request.SortBy switch
        {
            DueDateSortOptions.NextCalibrationDate => isDescending
                ? equipment.OrderByDescending(e => e.NextCalibrationDate).ToList()
                : equipment.OrderBy(e => e.NextCalibrationDate).ToList(),
            DueDateSortOptions.LastCalibrationDate => isDescending
                ? equipment.OrderByDescending(e => e.LastCalibrationDate).ToList()
                : equipment.OrderBy(e => e.LastCalibrationDate).ToList(),
            DueDateSortOptions.SerialNumber => isDescending
                ? equipment.OrderByDescending(e => e.SerialNumber).ToList()
                : equipment.OrderBy(e => e.SerialNumber).ToList(),
            DueDateSortOptions.Description => isDescending
                ? equipment.OrderByDescending(e => e.Description).ToList()
                : equipment.OrderBy(e => e.Description).ToList(),
            DueDateSortOptions.EquipmentType => isDescending
                ? equipment.OrderByDescending(e => e.EquipmentType).ToList()
                : equipment.OrderBy(e => e.EquipmentType).ToList(),
            DueDateSortOptions.Manufacturer => isDescending
                ? equipment.OrderByDescending(e => e.Manufacturer).ToList()
                : equipment.OrderBy(e => e.Manufacturer).ToList(),
            DueDateSortOptions.Model => isDescending
                ? equipment.OrderByDescending(e => e.Model).ToList()
                : equipment.OrderBy(e => e.Model).ToList(),
            DueDateSortOptions.Location => isDescending
                ? equipment.OrderByDescending(e => e.Location).ToList()
                : equipment.OrderBy(e => e.Location).ToList(),
            DueDateSortOptions.Department => isDescending
                ? equipment.OrderByDescending(e => e.Department).ToList()
                : equipment.OrderBy(e => e.Department).ToList(),
            DueDateSortOptions.DueStatus => isDescending
                ? equipment.OrderByDescending(e => e.DueStatus).ToList()
                : equipment.OrderBy(e => e.DueStatus).ToList(),
            DueDateSortOptions.DaysUntilDue => isDescending
                ? equipment.OrderByDescending(e => e.DaysUntilDue).ToList()
                : equipment.OrderBy(e => e.DaysUntilDue).ToList(),
            DueDateSortOptions.DaysOverdue => isDescending
                ? equipment.OrderByDescending(e => e.DaysOverdue).ToList()
                : equipment.OrderBy(e => e.DaysOverdue).ToList(),
            DueDateSortOptions.CalibrationStatus => isDescending
                ? equipment.OrderByDescending(e => e.CalibrationStatus).ToList()
                : equipment.OrderBy(e => e.CalibrationStatus).ToList(),
            DueDateSortOptions.CustomerName => isDescending
                ? equipment.OrderByDescending(e => e.CustomerName).ToList()
                : equipment.OrderBy(e => e.CustomerName).ToList(),
            _ => equipment
        };
    }

    private DueDateSummary CalculateSummary(List<LocalEquipmentDueDateDto> equipment)
    {
        var now = DateTime.Now;
        var thisWeekEnd = now.AddDays(7);
        var thisMonthEnd = now.AddDays(30);
        var nextMonthEnd = now.AddDays(60);

        return new DueDateSummary
        {
            TotalEquipment = equipment.Count,
            CurrentEquipment = equipment.Count(e => e.DueStatus == DueDateStatus.Current),
            ExpiringSoonEquipment = equipment.Count(e => e.DueStatus == DueDateStatus.ExpiringSoon),
            OverdueEquipment = equipment.Count(e => e.DueStatus == DueDateStatus.Overdue),
            UnknownStatusEquipment = equipment.Count(e => e.DueStatus == DueDateStatus.Unknown),
            NextDueDate = equipment
                .Where(e => e.NextCalibrationDate.HasValue && e.NextCalibrationDate.Value >= now)
                .OrderBy(e => e.NextCalibrationDate)
                .FirstOrDefault()?.NextCalibrationDate,
            EquipmentDueThisWeek = equipment.Count(e => e.NextCalibrationDate.HasValue &&
                e.NextCalibrationDate.Value >= now && e.NextCalibrationDate.Value <= thisWeekEnd),
            EquipmentDueThisMonth = equipment.Count(e => e.NextCalibrationDate.HasValue &&
                e.NextCalibrationDate.Value >= now && e.NextCalibrationDate.Value <= thisMonthEnd),
            EquipmentDueNextMonth = equipment.Count(e => e.NextCalibrationDate.HasValue &&
                e.NextCalibrationDate.Value > thisMonthEnd && e.NextCalibrationDate.Value <= nextMonthEnd)
        };
    }

    private Dictionary<string, object> BuildAppliedFilters(DueDateSearchRequest request)
    {
        var filters = new Dictionary<string, object>();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            filters["Search"] = request.SearchTerm;

        if (!string.IsNullOrWhiteSpace(request.SerialNumber))
            filters["Serial Number"] = request.SerialNumber;

        if (!string.IsNullOrWhiteSpace(request.EquipmentType))
            filters["Equipment Type"] = request.EquipmentType;

        if (request.DueStatus.HasValue)
            filters["Due Status"] = request.DueStatus.Value.ToString();

        if (request.DueDateFrom.HasValue)
            filters["Due Date From"] = request.DueDateFrom.Value.ToString("yyyy-MM-dd");

        if (request.DueDateTo.HasValue)
            filters["Due Date To"] = request.DueDateTo.Value.ToString("yyyy-MM-dd");

        if (request.IsOverdue == true)
            filters["Overdue"] = "Yes";

        if (request.IsExpiringSoon == true)
            filters["Expiring Soon"] = "Yes";

        return filters;
    }

    /// <summary>
    /// Convert gRPC EquipmentDueDateDto to local EquipmentDueDateDto
    /// </summary>
    private LocalEquipmentDueDateDto ConvertToLocalDto(Services.GrpcClients.EquipmentDueDateDto grpcDto)
    {
        // Determine due status based on dates
        var dueStatus = DueDateStatus.Unknown;
        if (grpcDto.NextDueDate.HasValue)
        {
            var daysUntilDue = (grpcDto.NextDueDate.Value - DateTime.Now).Days;
            if (daysUntilDue < 0)
                dueStatus = DueDateStatus.Overdue;
            else if (daysUntilDue <= 30)
                dueStatus = DueDateStatus.ExpiringSoon;
            else
                dueStatus = DueDateStatus.Current;
        }

        return new LocalEquipmentDueDateDto
        {
            EquipmentId = grpcDto.EquipmentId,
            SerialNumber = grpcDto.SerialNumber ?? "N/A",
            Description = grpcDto.EquipmentType ?? "Unknown", // Use EquipmentType as Description (Equipment Template Name)
            Manufacturer = grpcDto.Manufacturer ?? "Unknown",
            Model = grpcDto.Model ?? "Unknown",
            EquipmentType = grpcDto.EquipmentType ?? "Unknown",
            Location = grpcDto.Location,
            LastCalibrationDate = grpcDto.LastCalibrationDate,
            NextCalibrationDate = grpcDto.NextDueDate,
            CalibrationIntervalMonths = grpcDto.CalibrationIntervalMonths,
            CalibrationStatus = grpcDto.Status ?? "Unknown",
            DueStatus = dueStatus,
            DaysUntilDue = grpcDto.NextDueDate.HasValue ? Math.Max(0, (grpcDto.NextDueDate.Value - DateTime.Today).Days) : 0,
            DaysOverdue = dueStatus == DueDateStatus.Overdue && grpcDto.NextDueDate.HasValue ?
                Math.Abs((grpcDto.NextDueDate.Value - DateTime.Today).Days) : 0,
            CustomerId = grpcDto.CustomerId,
            CustomerName = grpcDto.CustomerName ?? "Unknown",
            CreatedDate = DateTime.Now,
            IsActive = true
        };
    }
}
