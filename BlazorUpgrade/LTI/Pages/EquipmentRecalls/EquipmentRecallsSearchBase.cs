using Blazed.Controls;
using Blazed.Controls.Toast;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Shared.Component;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using Blazored.Modal;
using CustomerEntity = CalibrationSaaS.Domain.Aggregates.Entities.Customer;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.EquipmentRecalls
{
    public class EquipmentRecallsSearchBase : Base_Create<EquipmentRecall, IEquipmentRecallService<CallContext>, AppStateCompany>
    {
        public ResponsiveTable<EquipmentRecall> Grid { get; set; } = new ResponsiveTable<EquipmentRecall>();

        [Inject] public ILogger<EquipmentRecallsSearchBase> Logger { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public IModalService Modal { get; set; }
        [Inject] public ICustomerService<CallContext> CustomerService { get; set; }

        // Filter properties
        public EquipmentRecallFilter CurrentFilter { get; set; } = new EquipmentRecallFilter();
        public CustomerEntity SelectedCustomer { get; set; }
        public string SelectedCustomerName { get; set; } = "All Customers";
        public DateTime? InitialDueDate { get; set; }
        public DateTime? FinalDueDate { get; set; }
        public bool ShowCustomerSelector { get; set; } = false;

        // Grid data
        public List<EquipmentRecall> EquipmentRecalls { get; set; } = new List<EquipmentRecall>();
        public List<EquipmentRecall> SelectedEquipmentRecalls { get; set; } = new List<EquipmentRecall>();
        public int TotalRecalls { get; set; }
        public int OverdueRecalls { get; set; }
        public bool SelectAll { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadInitialData();

            // Test JavaScript function availability
            try
            {
                var result = await JSRuntime.InvokeAsync<string>("testJavaScriptFunction");
                Logger.LogInformation($"JavaScript test result: {result}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "JavaScript function test failed");
            }
        }

        public async Task LoadInitialData()
        {
            try
            {
                // Initialize with empty data - no automatic search
                EquipmentRecalls = new List<EquipmentRecall>();
                TotalRecalls = 0;
                OverdueRecalls = 0;

                // Initialize filter
                CurrentFilter = new EquipmentRecallFilter();

                Logger.LogInformation("Equipment Recalls page initialized with empty grid");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error initializing equipment recalls page");
                await ShowToast("Error initializing equipment recalls page", ToastLevel.Error);
            }
        }

        public async Task SearchEquipmentRecalls()
        {
            try
            {
                await ShowProgress();

                // Clear previous results first
                if (Grid != null)
                {
                    Grid.Clear();
                }
                EquipmentRecalls = new List<EquipmentRecall>();
                TotalRecalls = 0;
                OverdueRecalls = 0;

                // Force immediate UI update
                await InvokeAsync(StateHasChanged);
                await Task.Delay(50); // Small delay to ensure UI updates

                // Update filter with current values
                CurrentFilter.CustomerID = SelectedCustomer?.CustomerID;
                CurrentFilter.InitialDueDate = InitialDueDate;
                CurrentFilter.FinalDueDate = FinalDueDate;
                CurrentFilter.CustomerName = SelectedCustomer?.Name;
                CurrentFilter.PageSize = 500; // Set a reasonable page size
                CurrentFilter.PageNumber = 1; // Start with first page

                Logger.LogInformation($"Searching with filter - CustomerID: {CurrentFilter.CustomerID}, InitialDueDate: {CurrentFilter.InitialDueDate}, FinalDueDate: {CurrentFilter.FinalDueDate}, PageSize: {CurrentFilter.PageSize}");

                var result = await Client.GetEquipmentRecalls(CurrentFilter, new CallContext());

                Logger.LogInformation($"gRPC call completed - Success: {result?.Success}, Message: {result?.Message}");

                if (result.Success)
                {
                    EquipmentRecalls = result.EquipmentRecalls?.ToList() ?? new List<EquipmentRecall>();
                    TotalRecalls = result.TotalCount;
                    OverdueRecalls = result.OverdueCount;

                    Logger.LogInformation($"Search result - Records: {EquipmentRecalls.Count}, TotalCount from service: {result.TotalCount}, OverdueCount from service: {result.OverdueCount}");
                    Logger.LogInformation($"Updated properties - TotalRecalls: {TotalRecalls}, OverdueRecalls: {OverdueRecalls}");

                    // Update grid data source with multiple refresh attempts
                    if (Grid != null)
                    {
                        Grid.ItemsDataSource = EquipmentRecalls;
                        await InvokeAsync(StateHasChanged);
                        await Task.Delay(100); // Allow time for grid to process
                        await InvokeAsync(StateHasChanged); // Second refresh to ensure grid updates
                    }
                    else
                    {
                        StateHasChanged();
                    }
                }
                else
                {
                    await ShowToast(result.Message, ToastLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error searching equipment recalls");
                await ShowToast("Error searching equipment recalls", ToastLevel.Error);
            }
            finally
            {
                await CloseProgress();
            }
        }

        public void SelectEquipment(EquipmentRecall equipment, ChangeEventArgs args)
        {
            bool isSelected = (bool)args.Value;

            if (isSelected)
            {
                if (!SelectedEquipmentRecalls.Contains(equipment))
                {
                    SelectedEquipmentRecalls.Add(equipment);
                }
            }
            else
            {
                SelectedEquipmentRecalls.Remove(equipment);
                // If any item is deselected, uncheck Select All
                SelectAll = false;
            }

            // Update SelectAll state based on current selections
            SelectAll = EquipmentRecalls.Count > 0 && SelectedEquipmentRecalls.Count == EquipmentRecalls.Count;

            StateHasChanged();
        }

        public void ToggleSelectAll(ChangeEventArgs args)
        {
            var selectAll = (bool)args.Value;
            SelectAll = selectAll;

            if (selectAll)
            {
                // Select all equipment recalls
                SelectedEquipmentRecalls.Clear();
                SelectedEquipmentRecalls.AddRange(EquipmentRecalls);
            }
            else
            {
                // Deselect all equipment recalls
                SelectedEquipmentRecalls.Clear();
            }

            StateHasChanged();
        }

        public bool IsEquipmentSelected(EquipmentRecall equipment)
        {
            return SelectedEquipmentRecalls.Contains(equipment);
        }

        public async Task ClearFilters()
        {
            SelectedCustomer = null;
            SelectedCustomerName = "All Customers";
            InitialDueDate = null;
            FinalDueDate = null;
            CurrentFilter = new EquipmentRecallFilter();

            // Clear the grid and totals
            if (Grid != null)
            {
                Grid.Clear();
            }
            EquipmentRecalls = new List<EquipmentRecall>();
            TotalRecalls = 0;
            OverdueRecalls = 0;

            StateHasChanged();

            Logger.LogInformation("Filters cleared - grid reset to empty state");
        }

        public async Task ExportToExcel()
        {
            try
            {
                // Check if any equipment is selected
                if (!SelectedEquipmentRecalls.Any())
                {
                    await ShowToast("Please select at least one equipment recall to export", ToastLevel.Warning);
                    return;
                }

                await ShowProgress();

                Logger.LogInformation($"Starting Excel export for {SelectedEquipmentRecalls.Count} selected items");

                // Create a filter that includes only the selected equipment IDs
                var selectedIds = SelectedEquipmentRecalls.Select(x => x.PieceOfEquipmentID).ToList();
                var exportFilter = new EquipmentRecallFilter
                {
                    CustomerID = CurrentFilter.CustomerID,
                    InitialDueDate = CurrentFilter.InitialDueDate,
                    FinalDueDate = CurrentFilter.FinalDueDate,
                    SelectedEquipmentIdsString = string.Join(",", selectedIds) // Directly set the string for gRPC serialization
                };

                Logger.LogInformation($"Export filter - CustomerID: {exportFilter.CustomerID}, SelectedIds: {exportFilter.SelectedEquipmentIdsString}");

                var result = await Client.ExportEquipmentRecallsToExcel(exportFilter, new CallContext());

                Logger.LogInformation($"gRPC export result - Success: {result?.Success}, Message: {result?.Message}, Data length: {result?.Data?.Length ?? 0}");

                if (result.Success && result.Data != null && result.Data.Length > 0)
                {
                    var fileName = $"EquipmentRecalls_Selected_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    // Convert byte array to base64 string for JavaScript
                    var base64String = Convert.ToBase64String(result.Data);

                    Logger.LogInformation($"Attempting to download file: {fileName}, Data size: {result.Data.Length} bytes");

                    // Try multiple approaches to download the file
                    var downloadSuccess = false;

                    try
                    {
                        // First check if the JavaScript function exists
                        var functionExists = await JSRuntime.InvokeAsync<bool>("eval", "typeof downloadFileFromByteArray === 'function'");
                        Logger.LogInformation($"JavaScript function exists: {functionExists}");

                        if (functionExists)
                        {
                            // Call the download function directly
                            await JSRuntime.InvokeVoidAsync("downloadFileFromByteArray", fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64String);
                            Logger.LogInformation("JavaScript download function called successfully");
                            downloadSuccess = true;
                        }
                    }
                    catch (Exception jsEx)
                    {
                        Logger.LogWarning(jsEx, "Primary JavaScript download method failed, trying fallback");
                    }

                    // Fallback method: Create the function inline and call it
                    if (!downloadSuccess)
                    {
                        try
                        {
                            var jsCode = $@"
                                (function() {{
                                    try {{
                                        const byteCharacters = atob('{base64String}');
                                        const byteNumbers = new Array(byteCharacters.length);
                                        for (let i = 0; i < byteCharacters.length; i++) {{
                                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                                        }}
                                        const byteArray = new Uint8Array(byteNumbers);
                                        const blob = new Blob([byteArray], {{ type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }});

                                        const url = window.URL.createObjectURL(blob);
                                        const a = document.createElement('a');
                                        a.href = url;
                                        a.download = '{fileName}';
                                        document.body.appendChild(a);
                                        a.click();
                                        document.body.removeChild(a);
                                        window.URL.revokeObjectURL(url);
                                        return true;
                                    }} catch (error) {{
                                        console.error('Download error:', error);
                                        return false;
                                    }}
                                }})();
                            ";

                            var fallbackResult = await JSRuntime.InvokeAsync<bool>("eval", jsCode);
                            Logger.LogInformation($"Fallback download method result: {fallbackResult}");
                            downloadSuccess = fallbackResult;
                        }
                        catch (Exception fallbackEx)
                        {
                            Logger.LogError(fallbackEx, "Fallback JavaScript download method also failed");
                        }
                    }

                    if (!downloadSuccess)
                    {
                        Logger.LogError("All download methods failed");
                        await ShowToast("Failed to download the Excel file. Please check browser console for errors.", ToastLevel.Error);
                        return;
                    }
                    await ShowToast($"Excel file exported successfully with {SelectedEquipmentRecalls.Count} selected items", ToastLevel.Success);
                }
                else
                {
                    Logger.LogWarning($"Export failed - Success: {result?.Success}, Data length: {result?.Data?.Length ?? 0}, Message: {result?.Message}");
                    await ShowToast(result?.Message ?? "No data to export", ToastLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error exporting equipment recalls to Excel");
                await ShowToast($"Error exporting to Excel: {ex.Message}", ToastLevel.Error);
            }
            finally
            {
                await CloseProgress();
            }
        }

        public async Task OpenCustomerSelector()
        {
            try
            {
                var parameters = new ModalParameters();
                parameters.Add("SelectOnly", true);
                parameters.Add("IsModal", true);

                ModalOptions options = new ModalOptions();
                options.ContentScrollable = true;
                options.Class = "blazored-modal " + ModalSize.MediumWindow;

                var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.Pages.Customer.Customer_Search>("Select Customer", parameters, options);
                var result = await messageForm.Result;

                if (!result.Cancelled && result.Data != null)
                {
                    var customer = (CustomerEntity)result.Data;
                    await SelectCustomer(customer);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error opening customer selector");
                await ShowToast("Error opening customer selector", ToastLevel.Error);
            }
        }

        public async Task SelectCustomer(CustomerEntity customer)
        {
            try
            {
                if (customer != null)
                {
                    SelectedCustomer = customer;
                    SelectedCustomerName = customer.Name;
                    CloseCustomerSelector();
                    // Don't automatically search - let user click Search button
                    StateHasChanged();
                }
                else
                {
                    SelectedCustomer = null;
                    SelectedCustomerName = "All Customers";
                    CloseCustomerSelector();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error selecting customer");
                await ShowToast("Error selecting customer", ToastLevel.Error);
            }
        }

        public void CloseCustomerSelector()
        {
            ShowCustomerSelector = false;
            StateHasChanged();
        }

        public void ToggleCustomerSelector()
        {
            ShowCustomerSelector = !ShowCustomerSelector;
            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            // Don't automatically search - let users apply filters first
        }

        public string GetRowClass(EquipmentRecall recall)
        {
            if (recall.IsOverdue)
            {
                return "table-danger"; // Red for overdue
            }
            else if (recall.DaysUntilDue <= 30)
            {
                return "table-warning"; // Yellow for due soon
            }
            return "table-success"; // Green for not due yet
        }

        public async Task ShowToast(string message, ToastLevel level)
        {
            switch (level)
            {
                case ToastLevel.Success:
                    ToastService.ShowSuccess(message);
                    break;
                case ToastLevel.Warning:
                    ToastService.ShowWarning(message);
                    break;
                case ToastLevel.Error:
                    ToastService.ShowError(message);
                    break;
                default:
                    ToastService.ShowInfo(message);
                    break;
            }
        }
    }

    public enum ToastLevel
    {
        Success,
        Warning,
        Error,
        Info
    }
}
