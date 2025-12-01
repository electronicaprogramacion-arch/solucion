using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Quotes
{
    public class QuoteCreateBase : Base_Create<Quote, Func<dynamic, IQuoteService<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    {
        [Parameter] public int QuoteId { get; set; }

        [Inject] public ICustomerService<CallContext> CustomerService { get; set; }
        [Inject] public IModalService Modal { get; set; }
        [Inject] public ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2> DbFactory { get; set; }
        [Inject] public Func<dynamic, Application.Services.IQuoteService<CallContext>> quoteService { get; set; }
        [Inject] public Func<dynamic, IPieceOfEquipmentService<CallContext>> equipmentService { get; set; }
        // Travel expense functionality is handled by QuoteService, not a separate service
        [Inject] public IConfiguration Configuration { get; set; }

        public Quote CurrentQuote { get; set; } = new Quote();
        public List<QuoteItem> QuoteItems { get; set; } = new List<QuoteItem>();
        public string SelectedCustomerName { get; set; } = "";
        public Domain.Aggregates.Entities.Customer SelectedCustomer { get; set; }
        public string CustomerType { get; set; } = "";
        public bool IsEdit => QuoteId > 0;
        public string PageTitle => IsEdit ? "Edit Quote" : "Create Quote";
        public bool IsSaving { get; set; } = false;
        public bool IsGeneratingReport { get; set; } = false;
        public bool IsAcceptingAndScheduling { get; set; } = false;
        public bool ShowCustomerSelector { get; set; } = false;
        public bool IsInitializing { get; set; } = true;

        // Customer Address properties
        public List<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
        public int? SelectedCustomerAddressId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Console.WriteLine("QuoteCreateBase: Starting OnInitializedAsync");
                IsInitializing = true;

                Console.WriteLine($"QuoteCreateBase: CurrentQuote is null: {CurrentQuote == null}");
                // Ensure CurrentQuote is initialized before base initialization
                if (CurrentQuote == null)
                {
                    CurrentQuote = new Quote();
                    Console.WriteLine("QuoteCreateBase: Created new Quote instance");
                }

                Console.WriteLine($"QuoteCreateBase: eq is null: {eq == null}");
                // Synchronize the base class's eq property with our CurrentQuote
                eq = CurrentQuote;
                Console.WriteLine("QuoteCreateBase: Synchronized eq with CurrentQuote");

                Console.WriteLine("QuoteCreateBase: Calling base.OnInitializedAsync");
                await base.OnInitializedAsync();
                Console.WriteLine("QuoteCreateBase: base.OnInitializedAsync completed");

                Console.WriteLine($"QuoteCreateBase: Component is null: {Component == null}");
                // Ensure component permissions are set after base initialization
                if (Component != null && string.IsNullOrEmpty(Component.Group))
                {
                    Component.Group = "admin,tech.HasEdit,tech.HasView,tech.HasNew,job.HasView,job.HasEdit,job.HasSave,job.HasNew";
                    Console.WriteLine("QuoteCreateBase: Set Component.Group");
                }

                // Set the entity ID for the base class
                EntityID = QuoteId.ToString();
                Console.WriteLine($"QuoteCreateBase: Set EntityID to {EntityID}");

                Console.WriteLine($"QuoteCreateBase: IsEdit = {IsEdit}");
                if (IsEdit)
                {
                    Console.WriteLine("QuoteCreateBase: Loading existing quote");
                    await LoadQuote();
                }
                else
                {
                    Console.WriteLine("QuoteCreateBase: Initializing new quote");
                    await InitializeNewQuote();
                }

                Console.WriteLine($"QuoteCreateBase: After load/init - CurrentQuote is null: {CurrentQuote == null}");
                // Ensure eq is still synchronized after initialization
                eq = CurrentQuote;

                IsInitializing = false;
                Console.WriteLine("QuoteCreateBase: Set IsInitializing to false");

                // Force a state change to ensure the UI updates
                Console.WriteLine("QuoteCreateBase: Calling StateHasChanged");
                StateHasChanged();
                Console.WriteLine("QuoteCreateBase: OnInitializedAsync completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"QuoteCreateBase: Exception in OnInitializedAsync: {ex.Message}");
                Console.WriteLine($"QuoteCreateBase: Exception stack trace: {ex.StackTrace}");
                IsInitializing = false;
                Logger.LogError(ex, "Error initializing quote page");
                await ShowToast("Error loading quote data", ToastLevel.Error);
            }
        }

        private async Task LoadQuote()
        {
            try
            {
                Console.WriteLine($"LoadQuote: Starting to load quote with ID {QuoteId}");
                var quote = new Quote { QuoteID = QuoteId };
                // Use quoteService(DbFactory) instead of just quoteService
                CurrentQuote = await quoteService(DbFactory).GetQuoteByID(quote, new CallContext());
                Console.WriteLine($"LoadQuote: Retrieved quote from service, is null: {CurrentQuote == null}");

                if (CurrentQuote == null)
                {
                    Console.WriteLine("LoadQuote: Quote not found, showing error and navigating away");
                    await ShowToast("Quote not found", ToastLevel.Error);
                    NavigationManager.NavigateTo("/QuotesSearch");
                    return;
                }

                Console.WriteLine($"LoadQuote: Quote loaded successfully - ID: {CurrentQuote.QuoteID}, Number: {CurrentQuote.QuoteNumber}");

                // Load customer name and details and set customer type
                if (CurrentQuote.CustomerID.HasValue && CurrentQuote.CustomerID.Value > 0)
                {
                    Console.WriteLine($"LoadQuote: Quote has CustomerID: {CurrentQuote.CustomerID.Value}");
                    CustomerType = "Official";
                    if (CurrentQuote.Customer != null)
                    {
                        Console.WriteLine($"LoadQuote: Customer already loaded: {CurrentQuote.Customer.Name}");
                        SelectedCustomerName = CurrentQuote.Customer.Name;
                        SelectedCustomer = CurrentQuote.Customer;
                    }
                    else
                    {
                        Console.WriteLine("LoadQuote: Customer not loaded, loading customer details");
                        // Load customer details if not already loaded
                        await LoadCustomerDetails(CurrentQuote.CustomerID.Value);
                    }
                }
                else if (!string.IsNullOrEmpty(CurrentQuote.CustomerName))
                {
                    Console.WriteLine($"LoadQuote: Using text customer: {CurrentQuote.CustomerName}");
                    CustomerType = "Text";
                }

                // Load quote items
                Console.WriteLine("LoadQuote: Loading quote items");
                await LoadQuoteItems();

                // Synchronize with base class
                eq = CurrentQuote;
                Console.WriteLine("LoadQuote: Synchronized eq with CurrentQuote");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadQuote: Exception occurred: {ex.Message}");
                Console.WriteLine($"LoadQuote: Exception stack trace: {ex.StackTrace}");
                Logger.LogError(ex, "Error loading quote {QuoteId}", QuoteId);
                await ShowToast("Error loading quote", ToastLevel.Error);
            }
        }

        private async Task LoadQuoteItems()
        {
            try
            {
                var quote = new Quote { QuoteID = CurrentQuote.QuoteID };
                // Use quoteService(DbFactory) instead of just quoteService
                var itemsResult = await quoteService(DbFactory).GetQuoteItemsByQuoteID(quote, new CallContext());
                QuoteItems = itemsResult?.QuoteItems ?? new List<QuoteItem>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading quote items for quote {QuoteId}", CurrentQuote.QuoteID);
                await ShowToast("Error loading quote items", ToastLevel.Error);
            }
        }

        private async Task InitializeNewQuote()
        {
            try
            {
                Console.WriteLine("InitializeNewQuote: Starting to initialize new quote");
                // Generate a quote number locally instead of using the service
                string quoteNumber = $"Q-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}";
                Console.WriteLine($"InitializeNewQuote: Generated quote number: {quoteNumber}");

                CurrentQuote = new Quote
                {
                    QuoteID = 0, // Explicitly set to 0 to ensure it's treated as a new entity
                    QuoteNumber = quoteNumber,
                    Status = "Draft",
                    Priority = "Low",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedBy = "Current User", // TODO: Get from authentication context
                    ModifiedBy = "Current User", // TODO: Get from authentication context
                    TotalCost = 0,
                    CustomerID = 0, // Will be set when customer is selected
                    Customer = null // Ensure no customer entity is attached
                };

                Console.WriteLine($"InitializeNewQuote: Created new Quote object - ID: {CurrentQuote.QuoteID}, Number: {CurrentQuote.QuoteNumber}");

                // Synchronize with base class
                eq = CurrentQuote;
                Console.WriteLine("InitializeNewQuote: Synchronized eq with CurrentQuote");

                Logger.LogInformation("New quote initialized: QuoteNumber={QuoteNumber}", CurrentQuote.QuoteNumber);
                Console.WriteLine("InitializeNewQuote: Completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InitializeNewQuote: Exception occurred: {ex.Message}");
                Console.WriteLine($"InitializeNewQuote: Exception stack trace: {ex.StackTrace}");
                Logger.LogError(ex, "Error initializing new quote");
                await ShowToast("Error initializing quote", ToastLevel.Error);
            }
        }

        public async Task SaveQuote()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();

                if (CurrentQuote.CustomerID <= 0)
                {
                    await ShowToast("Please select a customer", ToastLevel.Error);
                    return;
                }

                // Ensure required fields are set
                CurrentQuote.ModifiedDate = DateTime.UtcNow;
                CurrentQuote.ModifiedBy = "Current User"; // TODO: Get from authentication context

                if (!IsEdit)
                {
                    CurrentQuote.CreatedDate = DateTime.UtcNow;
                    CurrentQuote.CreatedBy = "Current User"; // TODO: Get from authentication context
                }

                Logger.LogInformation("Saving quote: QuoteNumber={QuoteNumber}, CustomerID={CustomerID}, CreatedBy={CreatedBy}, ModifiedBy={ModifiedBy}",
                    CurrentQuote.QuoteNumber, CurrentQuote.CustomerID, CurrentQuote.CreatedBy, CurrentQuote.ModifiedBy);

                Quote result;
                if (IsEdit)
                {
                    result = await quoteService(DbFactory).UpdateQuote(CurrentQuote, new CallContext());
                    await ShowToast("Quote updated successfully", ToastLevel.Success);
                }
                else
                {
                    // Ensure QuoteID is 0 for new quotes to prevent identity column conflicts
                    CurrentQuote.QuoteID = 0;

                    // Clear navigation properties to prevent EF tracking issues
                    CurrentQuote.Customer = null;
                    CurrentQuote.QuoteItems = new HashSet<QuoteItem>();

                    Logger.LogInformation("Creating new quote with QuoteID={QuoteID}, QuoteNumber={QuoteNumber}",
                        CurrentQuote.QuoteID, CurrentQuote.QuoteNumber);

                    result = await quoteService(DbFactory).CreateQuote(CurrentQuote, new CallContext());
                    await ShowToast("Quote created successfully", ToastLevel.Success);

                    // Update the current component state to transition to edit mode
                    if (result != null)
                    {
                        // Update the QuoteId parameter to reflect the new quote ID
                        QuoteId = result.QuoteID;

                        // Update the current quote with the saved result
                        CurrentQuote = result;

                        // Set customer details from the returned quote
                        if (CurrentQuote.Customer != null)
                        {
                            SelectedCustomerName = CurrentQuote.Customer.Name;
                            SelectedCustomer = CurrentQuote.Customer;
                        }
                        else if (CurrentQuote.CustomerID > 0)
                        {
                            // Fallback: Load customer details if not included in the result
                            await LoadCustomerDetails(CurrentQuote.CustomerID.Value);
                        }

                        // Load quote items to display the quote details section
                        await LoadQuoteItems();

                        // Update the browser URL without navigation to reflect edit mode
                        NavigationManager.NavigateTo($"/QuoteEdit/{result.QuoteID}", false);

                        // Force a state refresh to show the quote details section
                        StateHasChanged();

                        Logger.LogInformation("Successfully transitioned to edit mode for quote {QuoteID}", result.QuoteID);
                    }

                    return;
                }

                if (result != null)
                {
                    CurrentQuote = result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving quote: {Message}", ex.Message);
                await ShowToast($"Error saving quote: {ex.Message}", ToastLevel.Error);
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/QuotesSearch");
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
                    var customer = (Domain.Aggregates.Entities.Customer)result.Data;
                    await SelectCustomer(customer);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error opening customer selector");
                await ShowToast("Error opening customer selector", ToastLevel.Error);
            }
        }

        public void CloseCustomerSelector()
        {
            ShowCustomerSelector = false;
            StateHasChanged();
        }

        private async Task LoadCustomerDetails(int customerId)
        {
            try
            {
                var customer = new Domain.Aggregates.Entities.Customer { CustomerID = customerId };
                SelectedCustomer = await CustomerService.GetCustomersByID(customer, new CallContext());
                if (SelectedCustomer != null)
                {
                    SelectedCustomerName = SelectedCustomer.Name;
                    Logger.LogInformation("Customer details loaded: CustomerID={CustomerID}, Name={Name}, QuoteMultiplier={QuoteMultiplier}",
                        SelectedCustomer.CustomerID, SelectedCustomer.Name, SelectedCustomer.QuoteMultiplier);

                    // Load customer addresses
                    await LoadCustomerAddresses(SelectedCustomer.CustomerID);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading customer details for CustomerID={CustomerID}", customerId);
                // Don't show error to user, just log it
            }
        }








        private async Task LoadCustomerAddresses(int customerId)
        {
            try
            {
                // TODO: Implement actual customer address loading
                // For now, create sample addresses
                CustomerAddresses = new List<CustomerAddress>
                {
                    new CustomerAddress
                    {
                        CustomerAddressId = 1,
                        CustomerId = customerId,
                        Address1 = "123 Main St",
                        City = "Anytown",
                        State = "CA",
                        ZipCode = "12345",
                        TravelExpense = 50.00m
                    },
                    new CustomerAddress
                    {
                        CustomerAddressId = 2,
                        CustomerId = customerId,
                        Address1 = "456 Oak Ave",
                        City = "Another City",
                        State = "CA",
                        ZipCode = "67890",
                        TravelExpense = 75.00m
                    }
                };

                Logger.LogInformation("Customer addresses loaded: Count={Count}", CustomerAddresses.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading customer addresses for CustomerID={CustomerID}", customerId);
                CustomerAddresses = new List<CustomerAddress>();
            }
        }

        public async Task OnCustomerAddressChanged(ChangeEventArgs e)
        {
            try
            {
                if (int.TryParse(e.Value?.ToString(), out var addressId))
                {
                    SelectedCustomerAddressId = addressId;
                    CurrentQuote.CustomerAddressId = addressId;

                    // Check if we need to add/remove travel expenses
                    await UpdateTravelExpenses();

                    Logger.LogInformation("Customer address changed to: {AddressId}", addressId);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling customer address change");
                await ShowToast("Error updating customer address", ToastLevel.Error);
            }
        }

        private async Task UpdateTravelExpenses()
        {
            try
            {
                if (!SelectedCustomerAddressId.HasValue || CurrentQuote?.QuoteID <= 0)
                {
                    Logger.LogInformation("No customer address selected or quote not saved yet, skipping travel expense update");
                    return;
                }

                // Check if any quote items require travel
                var requiresTravelResult = await quoteService(DbFactory).RequiresTravelExpense(CurrentQuote, new CallContext());

                if (requiresTravelResult.Value)
                {
                    // Get travel expense from customer address
                    var selectedAddress = CustomerAddresses?.FirstOrDefault(a => a.CustomerAddressId == SelectedCustomerAddressId.Value);
                    var travelExpense = selectedAddress?.TravelExpense ?? 0;

                    if (travelExpense > 0)
                    {
                        // Add or update travel expense quote item
                        await AddOrUpdateTravelExpenseItem(travelExpense);
                        Logger.LogInformation("Travel expense of {Amount} added for address: {AddressId}", travelExpense, SelectedCustomerAddressId);
                    }
                    else
                    {
                        // Remove travel expense item if no travel cost
                        await RemoveTravelExpenseItem();
                        Logger.LogInformation("No travel expense configured for address: {AddressId}", SelectedCustomerAddressId);
                    }
                }
                else
                {
                    // Remove travel expense item if no travel-requiring price types
                    await RemoveTravelExpenseItem();
                    Logger.LogInformation("No travel-requiring price types found, removing travel expense");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating travel expenses");
            }
        }

        public async Task SelectCustomer(Domain.Aggregates.Entities.Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    CurrentQuote.CustomerID = customer.CustomerID;
                    CurrentQuote.CustomerName = null; // Clear text-based customer name
                    // Don't attach the full customer entity to avoid EF tracking issues
                    // CurrentQuote.Customer = customer;
                    CurrentQuote.Customer = null;
                    SelectedCustomerName = customer.Name;
                    SelectedCustomer = customer;

                    Logger.LogInformation("Customer selected: CustomerID={CustomerID}, Name={Name}, QuoteMultiplier={QuoteMultiplier}",
                        customer.CustomerID, customer.Name, customer.QuoteMultiplier);

                    // Load customer addresses for official customers
                    await LoadCustomerAddresses(customer.CustomerID);

                    CloseCustomerSelector();
                    StateHasChanged();
                }
                else
                {
                    Logger.LogWarning("Attempted to select null customer");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error selecting customer");
                await ShowToast("Error selecting customer", ToastLevel.Error);
            }
        }

        public async Task OnCustomerTypeChanged(ChangeEventArgs e)
        {
            try
            {
                CustomerType = e.Value?.ToString() ?? "";

                // Clear existing customer data when switching types
                CurrentQuote.CustomerID = null;
                CurrentQuote.CustomerName = null;
                SelectedCustomerName = "";
                SelectedCustomer = null;

                Logger.LogInformation("Customer type changed to: {CustomerType}", CustomerType);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error changing customer type");
                await ShowToast("Error changing customer type", ToastLevel.Error);
            }
        }

        public async Task ClearCustomerSelection()
        {
            try
            {
                CurrentQuote.CustomerID = null;
                CurrentQuote.CustomerName = null;
                SelectedCustomerName = "";
                SelectedCustomer = null;
                CustomerType = "";

                Logger.LogInformation("Customer selection cleared");
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error clearing customer selection");
                await ShowToast("Error clearing customer selection", ToastLevel.Error);
            }
        }

        public async Task AddQuoteItem()
        {
            try
            {
                if (CurrentQuote.CustomerID <= 0)
                {
                    await ShowToast("Please select a customer before adding items", ToastLevel.Error);
                    return;
                }

                // Ensure we have customer information available
                if (SelectedCustomer == null && CurrentQuote.CustomerID > 0)
                {
                    await LoadCustomerDetails(CurrentQuote.CustomerID.Value);
                }

                var customerToPass = SelectedCustomer ?? CurrentQuote.Customer;
                Logger.LogInformation("AddQuoteItem: Customer info - SelectedCustomer: {SelectedCustomer} (ID: {SelectedCustomerID}), CurrentQuote.Customer: {CurrentQuoteCustomer} (ID: {CurrentQuoteCustomerID}), Final customer: {FinalCustomer} (ID: {FinalCustomerID})",
                    SelectedCustomer?.Name ?? "NULL", SelectedCustomer?.CustomerID ?? 0,
                    CurrentQuote.Customer?.Name ?? "NULL", CurrentQuote.Customer?.CustomerID ?? 0,
                    customerToPass?.Name ?? "NULL", customerToPass?.CustomerID ?? 0);

                var parameters = new ModalParameters();
                parameters.Add("QuoteID", CurrentQuote.QuoteID);
                // Use SelectedCustomer instead of CurrentQuote.Customer to ensure customer data is available
                parameters.Add("Customer", customerToPass);

                ModalOptions options = new ModalOptions();
                options.ContentScrollable = true;
                options.Class = "blazored-modal " + ModalSize.LargeWindow;

                var messageForm = Modal.Show<QuoteItemModal>("Add Quote Item", parameters, options);
                var result = await messageForm.Result;

                if (!result.Cancelled && result.Data != null)
                {
                    var quoteItem = (QuoteItem)result.Data;
                    await ShowToast("Quote item added successfully", ToastLevel.Success);

                    // Reload quote items and update total cost
                    await LoadQuoteItems();
                    await UpdateQuoteTotalCost();

                    // Update travel expenses when quote items change
                    await UpdateTravelExpenses();

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding quote item");
                await ShowToast("Error adding quote item", ToastLevel.Error);
            }
        }

        public async Task EditQuoteItem(QuoteItem item)
        {
            try
            {
                if (item == null) return;

                // Ensure we have customer information available
                if (SelectedCustomer == null && CurrentQuote.CustomerID > 0)
                {
                    await LoadCustomerDetails(CurrentQuote.CustomerID.Value);
                }

                var parameters = new ModalParameters();
                parameters.Add("QuoteItem", item);
                parameters.Add("QuoteID", CurrentQuote.QuoteID);
                // Use SelectedCustomer instead of CurrentQuote.Customer to ensure customer data is available
                parameters.Add("Customer", SelectedCustomer ?? CurrentQuote.Customer);

                ModalOptions options = new ModalOptions();
                options.ContentScrollable = true;
                options.Class = "blazored-modal " + ModalSize.LargeWindow;

                var messageForm = Modal.Show<QuoteItemModal>("Edit Quote Item", parameters, options);
                var result = await messageForm.Result;

                if (!result.Cancelled && result.Data != null)
                {
                    await ShowToast("Quote item updated successfully", ToastLevel.Success);

                    // Reload quote items and update total cost
                    await LoadQuoteItems();
                    await UpdateQuoteTotalCost();

                    // Update travel expenses when quote items change
                    await UpdateTravelExpenses();

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error editing quote item");
                await ShowToast("Error editing quote item", ToastLevel.Error);
            }
        }

        public async Task DeleteQuoteItem(QuoteItem item)
        {
            try
            {
                if (item == null) return;

                var result = await quoteService(DbFactory).DeleteQuoteItem(item, new CallContext());
                if (result != null)
                {
                    await ShowToast("Quote item deleted successfully", ToastLevel.Success);
                    await LoadQuoteItems();

                    // Update quote total cost
                    var quote = new Quote { QuoteID = CurrentQuote.QuoteID };
                    CurrentQuote = await quoteService(DbFactory).UpdateQuoteTotalCost(quote, new CallContext());

                    // Update travel expenses when quote items change
                    await UpdateTravelExpenses();

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting quote item {ItemId}", item?.QuoteItemID);
                await ShowToast("Error deleting quote item", ToastLevel.Error);
            }
        }

        private async Task UpdateQuoteTotalCost()
        {
            try
            {
                var quote = new Quote { QuoteID = CurrentQuote.QuoteID };
                CurrentQuote = await quoteService(DbFactory).UpdateQuoteTotalCost(quote, new CallContext());
                StateHasChanged(); // Force UI update to reflect new total cost
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating quote total cost");
                // Don't show error to user as this is a background operation
            }
        }

        private async Task AddOrUpdateTravelExpenseItem(decimal travelExpense)
        {
            try
            {
                // Look for existing travel expense item
                var existingTravelItem = QuoteItems?.FirstOrDefault(qi =>
                    qi.ItemDescription != null && qi.ItemDescription.Contains("Travel Expense"));

                if (existingTravelItem != null)
                {
                    // Update existing travel expense item
                    existingTravelItem.UnitPrice = travelExpense;
                    existingTravelItem.ModifiedDate = DateTime.UtcNow;
                    await quoteService(DbFactory).UpdateQuoteItem(existingTravelItem, new CallContext());
                }
                else
                {
                    // Create new travel expense item
                    var travelItem = new QuoteItem
                    {
                        QuoteID = CurrentQuote.QuoteID,
                        ItemDescription = "Travel Expense",
                        Quantity = 1,
                        UnitPrice = travelExpense,
                        PieceOfEquipmentID = null, // No equipment for travel expense
                        PriceTypeId = null, // Travel expense is not tied to a specific price type
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    await quoteService(DbFactory).CreateQuoteItem(travelItem, new CallContext());
                }

                // Reload quote items and update total cost
                await LoadQuoteItems();
                await UpdateQuoteTotalCost();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding/updating travel expense item");
            }
        }

        private async Task RemoveTravelExpenseItem()
        {
            try
            {
                var travelItem = QuoteItems?.FirstOrDefault(qi =>
                    qi.ItemDescription != null && qi.ItemDescription.Contains("Travel Expense"));

                if (travelItem != null)
                {
                    await quoteService(DbFactory).DeleteQuoteItem(travelItem, new CallContext());
                    await LoadQuoteItems();
                    await UpdateQuoteTotalCost();
                    Logger.LogInformation("Travel expense item removed from quote");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error removing travel expense item");
            }
        }

        public string GetQuoteReportUrl(int quoteId)
        {
            // Get the Reports API base URL from configuration
            var reportsBaseUrl = Configuration.GetSection("Reports")["Url"] ?? "http://localhost:44312";

            // Ensure no trailing slash
            reportsBaseUrl = reportsBaseUrl.TrimEnd('/');

            // Build the quote report URL with correct query parameter format
            return $"{reportsBaseUrl}/api/print/quote?id={quoteId}";
        }

        public async Task GenerateQuoteReport()
        {
            try
            {
                IsGeneratingReport = true;
                StateHasChanged();

                // Open the quote report in a new tab instead of downloading
                var reportUrl = GetQuoteReportUrl(CurrentQuote.QuoteID);
                await JSRuntime.InvokeVoidAsync("open", reportUrl, "_blank");

                await ShowToast("Quote report opened in new tab", ToastLevel.Success);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error opening quote report for QuoteID: {QuoteId}", CurrentQuote.QuoteID);
                await ShowToast("Error opening quote report", ToastLevel.Error);
            }
            finally
            {
                IsGeneratingReport = false;
                StateHasChanged();
            }
        }

        public async Task AcceptAndScheduleWorkOrder()
        {
            try
            {
                IsAcceptingAndScheduling = true;
                StateHasChanged();

                // Validate that the quote has items
                if (QuoteItems == null || !QuoteItems.Any())
                {
                    await ShowToast("Cannot create work order: Quote has no items", ToastLevel.Error);
                    return;
                }

                // Validate that the quote is in a valid status for acceptance
                if (CurrentQuote.Status == "Scheduled")
                {
                    await ShowToast("Quote is already scheduled", ToastLevel.Warning);
                    return;
                }

                // Call the backend service to accept and schedule
                var workOrder = await quoteService(DbFactory).AcceptAndScheduleWorkOrder(CurrentQuote, new CallContext());

                if (workOrder != null)
                {
                    await ShowToast($"Quote accepted and Work Order {workOrder.WorkOrderId} created successfully", ToastLevel.Success);

                    // Reload the quote to get the updated status
                    await LoadQuote();
                }
                else
                {
                    await ShowToast("Error creating work order", ToastLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error accepting quote and scheduling work order for QuoteID: {QuoteId}", CurrentQuote.QuoteID);
                await ShowToast("Error accepting quote and scheduling work order", ToastLevel.Error);
            }
            finally
            {
                IsAcceptingAndScheduling = false;
                StateHasChanged();
            }
        }
    }
}
