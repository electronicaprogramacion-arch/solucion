using CalibrationSaaS.CustomerPortal.Models.Equipment;
using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.ViewModels;

/// <summary>
/// View model for equipment due date management
/// </summary>
public class DueDateViewModel
{
    public DueDateSearchRequest SearchRequest { get; set; } = new();
    public DueDateSearchResponse? SearchResponse { get; set; }
    public List<EquipmentDueDateDto> SelectedEquipment { get; set; } = new();
    
    // UI State
    public bool IsLoading { get; set; }
    public bool ShowAdvancedFilters { get; set; }
    public bool ShowSummaryStats { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    
    // Filter State
    public string QuickFilter { get; set; } = "all";
    public bool HasActiveFilters => !string.IsNullOrWhiteSpace(SearchRequest.SearchTerm) ||
                                   !string.IsNullOrWhiteSpace(SearchRequest.SerialNumber) ||
                                   !string.IsNullOrWhiteSpace(SearchRequest.EquipmentType) ||
                                   SearchRequest.DueStatus.HasValue ||
                                   SearchRequest.IsOverdue.HasValue ||
                                   SearchRequest.DueDateFrom.HasValue ||
                                   SearchRequest.DueDateTo.HasValue;
    
    // Selection State
    public bool HasSelectedEquipment => SelectedEquipment.Any();
    public int SelectedCount => SelectedEquipment.Count;
    public bool IsAllSelected { get; set; }
    
    // Export State
    public bool IsExporting { get; set; }
    public string ExportFormat { get; set; } = "excel";
    
    // Display Options
    public string ViewMode { get; set; } = "grid"; // grid, cards, calendar
    public int ItemsPerPage { get; set; } = 25;
    public bool ShowImages { get; set; } = true;
    public bool ShowStatusIcons { get; set; } = true;
    public bool ShowProgressBars { get; set; } = true;
    
    // Quick Filter Options
    public List<QuickFilterOption> QuickFilterOptions { get; set; } = new()
    {
        new("all", "All Equipment", "inventory", "var(--rz-primary)"),
        new("overdue", "Overdue", "error", "var(--rz-danger)"),
        new("expiring", "Expiring Soon", "warning", "var(--rz-warning)"),
        new("current", "Current", "check_circle", "var(--rz-success)"),
        new("this-week", "Due This Week", "today", "var(--rz-info)"),
        new("this-month", "Due This Month", "event", "var(--rz-secondary)"),
        new("no-due-date", "No Due Date", "help", "var(--rz-base-600)")
    };
    
    // Methods
    public void ClearFilters()
    {
        SearchRequest = new DueDateSearchRequest
        {
            Page = 1,
            PageSize = SearchRequest.PageSize,
            SortBy = SearchRequest.SortBy,
            SortDirection = SearchRequest.SortDirection
        };
        QuickFilter = "all";
        ErrorMessage = null;
        SuccessMessage = null;
    }
    
    public void ApplyQuickFilter(string filter)
    {
        QuickFilter = filter;
        SearchRequest.Page = 1; // Reset to first page
        
        // Clear existing filters
        SearchRequest.DueStatus = null;
        SearchRequest.IsOverdue = null;
        SearchRequest.IsExpiringSoon = null;
        SearchRequest.DueDateFrom = null;
        SearchRequest.DueDateTo = null;
        
        var now = DateTime.Now;
        
        switch (filter)
        {
            case "overdue":
                SearchRequest.IsOverdue = true;
                break;
            case "expiring":
                SearchRequest.IsExpiringSoon = true;
                break;
            case "current":
                SearchRequest.DueStatus = DueDateStatus.Current;
                break;
            case "this-week":
                SearchRequest.DueDateFrom = now;
                SearchRequest.DueDateTo = now.AddDays(7);
                break;
            case "this-month":
                SearchRequest.DueDateFrom = now;
                SearchRequest.DueDateTo = now.AddDays(30);
                break;
            case "no-due-date":
                SearchRequest.DueStatus = DueDateStatus.Unknown;
                break;
        }
    }
    
    public void SelectEquipment(EquipmentDueDateDto equipment, bool selected)
    {
        if (selected && !SelectedEquipment.Any(e => e.EquipmentId == equipment.EquipmentId))
        {
            SelectedEquipment.Add(equipment);
        }
        else if (!selected)
        {
            SelectedEquipment.RemoveAll(e => e.EquipmentId == equipment.EquipmentId);
        }
        
        UpdateSelectAllState();
    }
    
    public void SelectAllEquipment(bool selected)
    {
        if (SearchResponse?.Equipment != null)
        {
            if (selected)
            {
                foreach (var equipment in SearchResponse.Equipment)
                {
                    if (!SelectedEquipment.Any(e => e.EquipmentId == equipment.EquipmentId))
                    {
                        SelectedEquipment.Add(equipment);
                    }
                }
            }
            else
            {
                foreach (var equipment in SearchResponse.Equipment)
                {
                    SelectedEquipment.RemoveAll(e => e.EquipmentId == equipment.EquipmentId);
                }
            }
        }
        
        UpdateSelectAllState();
    }
    
    public void ClearSelection()
    {
        SelectedEquipment.Clear();
        IsAllSelected = false;
    }
    
    public bool IsEquipmentSelected(int equipmentId)
    {
        return SelectedEquipment.Any(e => e.EquipmentId == equipmentId);
    }
    
    private void UpdateSelectAllState()
    {
        if (SearchResponse?.Equipment != null && SearchResponse.Equipment.Any())
        {
            IsAllSelected = SearchResponse.Equipment.All(e => SelectedEquipment.Any(s => s.EquipmentId == e.EquipmentId));
        }
        else
        {
            IsAllSelected = false;
        }
    }
    
    public void SetSortOrder(string sortBy)
    {
        if (SearchRequest.SortBy == sortBy)
        {
            SearchRequest.SortDirection = SearchRequest.SortDirection == "asc" ? "desc" : "asc";
        }
        else
        {
            SearchRequest.SortBy = sortBy;
            SearchRequest.SortDirection = "asc";
        }
        
        SearchRequest.Page = 1; // Reset to first page when sorting
    }
    
    public string GetSortIcon(string sortBy)
    {
        if (SearchRequest.SortBy != sortBy)
            return "unfold_more";
            
        return SearchRequest.SortDirection == "asc" ? "keyboard_arrow_up" : "keyboard_arrow_down";
    }
    
    public void SetPageSize(int pageSize)
    {
        SearchRequest.PageSize = pageSize;
        SearchRequest.Page = 1; // Reset to first page
    }
    
    public void GoToPage(int page)
    {
        if (SearchResponse != null && page >= 1 && page <= SearchResponse.TotalPages)
        {
            SearchRequest.Page = page;
        }
    }
    
    public void NextPage()
    {
        if (SearchResponse?.HasNextPage == true)
        {
            SearchRequest.Page++;
        }
    }
    
    public void PreviousPage()
    {
        if (SearchResponse?.HasPreviousPage == true)
        {
            SearchRequest.Page--;
        }
    }
}

/// <summary>
/// Quick filter option for due date search
/// </summary>
public class QuickFilterOption
{
    public string Value { get; set; }
    public string Label { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    
    public QuickFilterOption(string value, string label, string icon, string color)
    {
        Value = value;
        Label = label;
        Icon = icon;
        Color = color;
    }
}

/// <summary>
/// Equipment due date dashboard summary
/// </summary>
public class DueDateDashboardSummary
{
    public int TotalEquipment { get; set; }
    public int OverdueEquipment { get; set; }
    public int ExpiringSoonEquipment { get; set; }
    public int CurrentEquipment { get; set; }
    public int DueThisWeek { get; set; }
    public int DueThisMonth { get; set; }
    public int DueNextMonth { get; set; }
    
    public double ComplianceRate => TotalEquipment > 0 ? (double)CurrentEquipment / TotalEquipment * 100 : 0;
    public double OverdueRate => TotalEquipment > 0 ? (double)OverdueEquipment / TotalEquipment * 100 : 0;
    
    public List<EquipmentDueDateDto> UpcomingDueDates { get; set; } = new();
    public List<EquipmentDueDateDto> OverdueEquipmentList { get; set; } = new();
    public List<DueDateTrendData> TrendData { get; set; } = new();
    
    public DateTime? NextDueDate { get; set; }
    public string? NextDueEquipment { get; set; }
    public int DaysToNextDue { get; set; }
}

/// <summary>
/// Due date trend data for charts
/// </summary>
public class DueDateTrendData
{
    public DateTime Date { get; set; }
    public int DueCount { get; set; }
    public int OverdueCount { get; set; }
    public int CompletedCount { get; set; }
    public string Label => Date.ToString("MMM yyyy");
}
