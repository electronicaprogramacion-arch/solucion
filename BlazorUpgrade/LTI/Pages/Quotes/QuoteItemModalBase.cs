using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics;
using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Quotes
{
    public class QuoteItemModalBase : ComponentBase
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter] public QuoteItem QuoteItem { get; set; }
        [Parameter] public int QuoteID { get; set; }
        [Parameter] public Domain.Aggregates.Entities.Customer Customer { get; set; }
        [Parameter] public string ServiceType { get; set; } = "Laboratory";

        [Inject] public IQuoteService<CallContext> QuoteService { get; set; }
        [Inject] public IPieceOfEquipmentService<CallContext> EquipmentService { get; set; }
        [Inject] public IPriceTypeService<CallContext> PriceTypeService { get; set; }
        [Inject] public ILogger<QuoteItemModalBase> Logger { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IModalService Modal { get; set; }

        public QuoteItem CurrentQuoteItem { get; set; } = new QuoteItem();
        public PieceOfEquipment SelectedEquipment { get; set; }
        public string SelectedEquipmentName { get; set; } = "";
        public bool IsEdit => QuoteItem != null && QuoteItem.QuoteItemID > 0;
        public string ModalTitle => IsEdit ? "Edit Quote Item" : "Add Quote Item";
        public string SaveButtonText => IsEdit ? "Update Item" : "Add Item";
        public bool IsSaving { get; set; } = false;
        public bool HasChildEquipment { get; set; } = false;
        public string PriceSource { get; set; } = "";
        public string EquipmentTypeDisplay { get; set; } = "";

        // PriceType-related properties
        public List<PriceType> ActivePriceTypes { get; set; } = new List<PriceType>();
        public int? SelectedPriceTypeId { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return CurrentQuoteItem.Quantity * CurrentQuoteItem.UnitPrice;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Load active price types first
                await LoadActivePriceTypes();

                if (IsEdit)
                {
                    CurrentQuoteItem = QuoteItem;
                    SelectedPriceTypeId = CurrentQuoteItem.PriceTypeId;
                    await LoadEquipmentInfo();
                    await LoadInitialPriceSource();
                }
                else
                {
                    CurrentQuoteItem = new QuoteItem
                    {
                        QuoteID = QuoteID,
                        Quantity = 1,
                        UnitPrice = 0,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    // Set default price type (Lab if available)
                    var defaultPriceType = ActivePriceTypes.FirstOrDefault(pt => pt.Name == "Lab")
                                         ?? ActivePriceTypes.FirstOrDefault();
                    if (defaultPriceType != null)
                    {
                        SelectedPriceTypeId = defaultPriceType.PriceTypeId;
                        CurrentQuoteItem.PriceTypeId = defaultPriceType.PriceTypeId;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error initializing quote item modal");
                ToastService.ShowError("Error initializing quote item");
            }
        }

        private async Task LoadActivePriceTypes()
        {
            try
            {
                var result = await PriceTypeService.GetActivePriceTypes(new CallContext());
                if (result != null && result.PriceTypes != null)
                {
                    ActivePriceTypes = result.PriceTypes.ToList();
                }
                else
                {
                    Logger.LogWarning("Failed to load active price types or result was null");
                    ActivePriceTypes = new List<PriceType>();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading active price types");
                ActivePriceTypes = new List<PriceType>();
            }
        }

        private async Task LoadInitialPriceSource()
        {
            try
            {
                Logger.LogInformation("Loading initial price source for existing quote item - ServiceType: {ServiceType}", ServiceType);

                if (SelectedEquipment != null)
                {
                    // Implement pricing logic entirely on client side to avoid gRPC issues
                    PriceResult priceResult = await CalculateClientSidePrice(SelectedEquipment, ServiceType);

                    if (priceResult != null)
                    {
                        // Set price source information without changing the actual price
                        PriceSource = !string.IsNullOrEmpty(priceResult.Description)
                            ? $"Price source: {priceResult.Description} - {ServiceType}: ${priceResult.Price:N2}"
                            : $"Price source: Equipment - {ServiceType}: ${priceResult.Price:N2}";

                        // Add customer multiplier info if applied
                        if (Customer?.QuoteMultiplier.HasValue == true && Customer.QuoteMultiplier.Value != 1.0m)
                        {
                            var expectedFinalPrice = priceResult.Price * Customer.QuoteMultiplier.Value;
                            PriceSource += $" (Customer multiplier {Customer.QuoteMultiplier.Value:N2} applied: ${expectedFinalPrice:N2})";
                        }

                        Logger.LogInformation("Initial price source calculated: {PriceSource}", PriceSource);
                    }
                    else
                    {
                        PriceSource = $"Manual Entry Required - No {ServiceType} Price Configured";
                    }
                }
                else
                {
                    PriceSource = "Manual Entry Required - No Equipment Selected";
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading initial price source for ServiceType {ServiceType}", ServiceType);
                PriceSource = $"Manual Entry Required - Error Loading {ServiceType} Price";
            }
        }

        private async Task LoadEquipmentInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(CurrentQuoteItem.PieceOfEquipmentID))
                {
                    var equipment = await EquipmentService.GetPieceOfEquipmentXId(
                        new PieceOfEquipment { PieceOfEquipmentID = CurrentQuoteItem.PieceOfEquipmentID },
                        new CallContext());
                    
                    if (equipment != null)
                    {
                        SelectedEquipment = equipment;
                        SelectedEquipmentName = $"{equipment.EquipmentTemplate?.Name} - {equipment.SerialNumber}";
                        await CheckForChildEquipment();
                        UpdateEquipmentTypeDisplay();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading equipment info");
            }
        }

        private void UpdateEquipmentTypeDisplay()
        {
            if (SelectedEquipment?.EquipmentTemplate?.EquipmentTypeObject != null)
            {
                var equipmentType = SelectedEquipment.EquipmentTemplate.EquipmentTypeObject;
                var equipmentTypeGroup = equipmentType.EquipmentTypeGroup;

                if (equipmentTypeGroup != null)
                {
                    EquipmentTypeDisplay = $"{equipmentTypeGroup.Name} - {equipmentType.Name}";
                }
                else
                {
                    EquipmentTypeDisplay = $"Unknown Group - {equipmentType.Name}";
                }

                CurrentQuoteItem.EquipmentTypeDisplay = EquipmentTypeDisplay;
            }
            else
            {
                EquipmentTypeDisplay = "Manual Entry Required";
                CurrentQuoteItem.EquipmentTypeDisplay = EquipmentTypeDisplay;
            }
        }

        public async Task OpenEquipmentSelector()
        {
            try
            {
                var parameters = new ModalParameters();
                parameters.Add("Customer", Customer);
                parameters.Add("SelectOnly", true);

                var modal = Modal.Show<PieceOfEquipment_Search>("Select Equipment", parameters);
                var result = await modal.Result;

                if (!result.Cancelled && result.Data is PieceOfEquipment equipment)
                {
                    // Reload equipment with full details to ensure EquipmentTemplate is loaded
                    var fullEquipment = await EquipmentService.GetPieceOfEquipmentXId(
                        new PieceOfEquipment { PieceOfEquipmentID = equipment.PieceOfEquipmentID },
                        new CallContext());

                    SelectedEquipment = fullEquipment ?? equipment;
                    CurrentQuoteItem.PieceOfEquipmentID = SelectedEquipment.PieceOfEquipmentID;
                    SelectedEquipmentName = $"{SelectedEquipment.EquipmentTemplate?.Name} - {SelectedEquipment.SerialNumber}";

                    // Reset price when equipment changes
                    CurrentQuoteItem.UnitPrice = 0;

                    // Mark as parent if it has children
                    await CheckForChildEquipment();
                    CurrentQuoteItem.IsParent = HasChildEquipment;

                    UpdateEquipmentTypeDisplay();
                    await LoadPriceForEquipment();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error opening equipment selector");
                ToastService.ShowError("Error opening equipment selector");
            }
        }

        public async Task ClearEquipmentSelection()
        {
            try
            {
                SelectedEquipment = null;
                CurrentQuoteItem.PieceOfEquipmentID = null;
                SelectedEquipmentName = "";
                HasChildEquipment = false;
                CurrentQuoteItem.IsParent = false;

                // Reset price when equipment is cleared
                CurrentQuoteItem.UnitPrice = 0;
                PriceSource = "Manual Entry Required - No Equipment Selected";
                EquipmentTypeDisplay = "Manual Entry Required";
                CurrentQuoteItem.EquipmentTypeDisplay = EquipmentTypeDisplay;

                StateHasChanged();

                Logger.LogInformation("Equipment selection cleared");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error clearing equipment selection");
            }
        }

        private async Task LoadPriceForEquipment()
        {
            try
            {
                if (SelectedEquipment == null)
                {
                    PriceSource = "Manual Entry Required - No Equipment Selected";
                    return;
                }

                Logger.LogInformation("LoadPriceForEquipment called with equipment: {EquipmentID}, ServiceType: {ServiceType}",
                    SelectedEquipment.PieceOfEquipmentID, ServiceType);

                Logger.LogInformation("Equipment details - HasTemplate: {HasTemplate}, Using dynamic pricing system",
                    SelectedEquipment.EquipmentTemplate != null);

                if (SelectedEquipment.EquipmentTemplate != null)
                {
                    // TODO: Equipment template pricing not yet implemented
                    Logger.LogInformation("Template details - Equipment template pricing not yet available");
                }

                // Use client-side pricing logic to avoid all gRPC pricing method issues
                Logger.LogInformation("Implementing hierarchical pricing using client-side logic...");
                var priceResult = await CalculateClientSidePrice(SelectedEquipment, ServiceType);

                if (priceResult != null && priceResult.Price > 0)
                {
                    var finalPrice = await ApplyCustomerMultiplier(priceResult.Price);
                    CurrentQuoteItem.UnitPrice = finalPrice;

                    // Set price source information
                    PriceSource = !string.IsNullOrEmpty(priceResult.Description)
                        ? $"Price source: {priceResult.Description} - {ServiceType}: ${priceResult.Price:N2}"
                        : $"Price source: Equipment - {ServiceType}: ${priceResult.Price:N2}";

                    // Add customer multiplier info if applied
                    if (Customer?.QuoteMultiplier.HasValue == true && Customer.QuoteMultiplier.Value != 1.0m)
                    {
                        PriceSource += $" (Customer multiplier {Customer.QuoteMultiplier.Value:N2} applied: ${finalPrice:N2})";
                    }

                    Logger.LogInformation("Pricing applied: {PriceSource}", PriceSource);
                }
                else
                {
                    // No price found, require manual entry
                    CurrentQuoteItem.UnitPrice = 0;
                    PriceSource = priceResult?.Description ?? $"Manual Entry Required - No {ServiceType} Price Configured";
                    Logger.LogInformation("No automatic pricing available, manual entry required. PriceResult: {PriceResult}", priceResult?.Description);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading price for equipment {EquipmentID}, ServiceType {ServiceType}", SelectedEquipment?.PieceOfEquipmentID, ServiceType);
                PriceSource = $"Manual Entry Required - Error Loading {ServiceType} Price";
                CurrentQuoteItem.UnitPrice = 0;
                StateHasChanged();
            }
        }

        private async Task<decimal> ApplyCustomerMultiplier(decimal basePrice)
        {
            try
            {
                if (Customer?.QuoteMultiplier.HasValue == true && Customer.QuoteMultiplier.Value != 1.0m)
                {
                    return basePrice * Customer.QuoteMultiplier.Value;
                }
                return basePrice;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error applying customer multiplier");
                return basePrice;
            }
        }

        private async Task CheckForChildEquipment()
        {
            try
            {
                if (SelectedEquipment != null)
                {
                    var childEquipmentResult = await QuoteService.GetChildEquipmentForQuote(SelectedEquipment, new CallContext());
                    HasChildEquipment = childEquipmentResult?.PieceOfEquipments?.Any() == true;
                    Logger.LogInformation("Child equipment check: {HasChildren}", HasChildEquipment);
                }
                else
                {
                    HasChildEquipment = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error checking for child equipment");
                HasChildEquipment = false;
            }
        }

        public async Task CalculateTotal()
        {
            // Force UI update for both unit price and total price
            await InvokeAsync(StateHasChanged);
        }

        public void OnValueChanged()
        {
            // Synchronous wrapper for @onchange events
            _ = CalculateTotal();
        }

        public async Task SaveQuoteItem()
        {
            try
            {
                IsSaving = true;

                // Basic validation
                if (CurrentQuoteItem.Quantity <= 0)
                {
                    ToastService.ShowError("Quantity must be greater than 0");
                    return;
                }

                if (CurrentQuoteItem.UnitPrice < 0)
                {
                    ToastService.ShowError("Unit price cannot be negative");
                    return;
                }

                // Equipment selection is now optional - no validation needed

                CurrentQuoteItem.ModifiedDate = DateTime.UtcNow;

                QuoteItem result;
                if (IsEdit)
                {
                    result = await QuoteService.UpdateQuoteItem(CurrentQuoteItem, new CallContext());
                    Logger.LogInformation("Quote item updated: {QuoteItemID}", result.QuoteItemID);
                }
                else
                {
                    result = await QuoteService.CreateQuoteItem(CurrentQuoteItem, new CallContext());
                    Logger.LogInformation("Quote item created: {QuoteItemID}", result.QuoteItemID);

                    // Create child items if this is a parent equipment
                    if (HasChildEquipment && SelectedEquipment != null)
                    {
                        await CreateChildQuoteItems(result);
                    }
                }

                ToastService.ShowSuccess(IsEdit ? "Quote item updated successfully" : "Quote item added successfully");

                // Notify parent component to update travel expenses if needed
                // This will be handled by the parent QuoteCreate component

                await BlazoredModal.CloseAsync(ModalResult.Ok(result));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving quote item");
                ToastService.ShowError("Error saving quote item");
            }
            finally
            {
                IsSaving = false;
            }
        }

        private async Task CreateChildQuoteItems(QuoteItem parentQuoteItem)
        {
            try
            {
                var childEquipmentResult = await QuoteService.GetChildEquipmentForQuote(SelectedEquipment, new CallContext());

                if (childEquipmentResult?.PieceOfEquipments != null)
                {
                    foreach (var child in childEquipmentResult.PieceOfEquipments)
                    {
                        var childQuoteItem = new QuoteItem
                        {
                            QuoteID = parentQuoteItem.QuoteID,
                            PieceOfEquipmentID = child.PieceOfEquipmentID,
                            Quantity = parentQuoteItem.Quantity, // Use same quantity as parent
                            UnitPrice = 0, // Child items might have different pricing
                            ParentQuoteItemID = parentQuoteItem.QuoteItemID,
                            IsParent = false,
                            SortOrder = 0,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        };

                        // Set equipment type display for child item
                        if (child.EquipmentTemplate?.EquipmentTypeObject != null)
                        {
                            var equipmentType = child.EquipmentTemplate.EquipmentTypeObject;
                            var equipmentTypeGroup = equipmentType.EquipmentTypeGroup;

                            if (equipmentTypeGroup != null)
                            {
                                childQuoteItem.EquipmentTypeDisplay = $"{equipmentTypeGroup.Name} - {equipmentType.Name}";
                            }
                            else
                            {
                                childQuoteItem.EquipmentTypeDisplay = $"Unknown Group - {equipmentType.Name}";
                            }
                        }
                        else
                        {
                            childQuoteItem.EquipmentTypeDisplay = "Manual Entry Required";
                        }

                        // Try to get pricing for child equipment using client-side logic
                        try
                        {
                            var childPriceResult = await CalculateClientSidePrice(child, ServiceType);
                            if (childPriceResult?.Price > 0)
                            {
                                childQuoteItem.UnitPrice = await ApplyCustomerMultiplier(childPriceResult.Price);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning(ex, "Could not get pricing for child equipment {ChildEquipmentID}", child.PieceOfEquipmentID);
                        }

                        await QuoteService.CreateQuoteItem(childQuoteItem, new CallContext());
                        Logger.LogInformation("Created child quote item for equipment: {ChildEquipmentID}", child.PieceOfEquipmentID);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating child quote items");
                ToastService.ShowWarning("Parent item created but some child items may not have been added");
            }
        }

        public async Task Cancel()
        {
            await BlazoredModal.CancelAsync();
        }

        /// <summary>
        /// Calculate price using client-side logic to avoid gRPC issues
        /// Implements hierarchical pricing: PoE → Equipment Template → Manual Entry
        /// </summary>
        private async Task<PriceResult> CalculateClientSidePrice(PieceOfEquipment equipment, string serviceType)
        {
            try
            {
                if (equipment == null)
                {
                    Logger.LogInformation("No equipment provided for pricing");
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Equipment Selected" };
                }

                Logger.LogInformation("Calculating client-side price for equipment {EquipmentID}, ServiceType: {ServiceType}",
                    equipment.PieceOfEquipmentID, serviceType);

                decimal price = 0;
                string priceSource = "";

                // Use dynamic pricing system based on selected price type
                if (SelectedPriceTypeId.HasValue)
                {
                    try
                    {
                        // Get hierarchical price using the dynamic pricing system
                        var priceResult = await PriceTypeService.GetHierarchicalPrice(equipment, SelectedPriceTypeId.Value, new CallContext());
                        price = priceResult.Price;

                        if (price > 0)
                        {
                            var priceType = ActivePriceTypes.FirstOrDefault(pt => pt.PriceTypeId == SelectedPriceTypeId.Value);
                            var priceTypeName = priceType?.Name ?? "Unknown";

                            // Determine the source of the price
                            var piecePriceResult = await PriceTypeService.GetPriceForEntity(EntityType.PieceOfEquipment,
                                int.Parse(equipment.PieceOfEquipmentID), SelectedPriceTypeId.Value, new CallContext());

                            if (piecePriceResult.Price > 0)
                            {
                                priceSource = $"Piece of Equipment - {priceTypeName}";
                                Logger.LogInformation("Found PoE {PriceType} price: {Price}", priceTypeName, price);
                            }
                            else
                            {
                                priceSource = $"Equipment Template - {priceTypeName}";
                                Logger.LogInformation("Found Equipment Template {PriceType} price: {Price}", priceTypeName, price);
                            }
                        }
                        else
                        {
                            priceSource = priceResult.Description ?? "Manual Entry Required - No Price Found";
                            Logger.LogInformation("No price found for selected price type {PriceTypeId}: {Description}", SelectedPriceTypeId.Value, priceSource);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error getting hierarchical price for equipment {EquipmentId} and price type {PriceTypeId}",
                            equipment.PieceOfEquipmentID, SelectedPriceTypeId.Value);
                    }
                }
                else
                {
                    Logger.LogInformation("No price type selected for pricing");
                }

                // If no price found, require manual entry
                if (price == 0)
                {
                    priceSource = $"Manual Entry Required - No {serviceType} Price Configured";
                    Logger.LogInformation("No automatic pricing found, manual entry required");
                }

                var result = new PriceResult { Price = price, Description = priceSource };
                Logger.LogInformation("Client-side pricing result: Price={Price}, Source={Source}", result.Price, result.Description);

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in client-side price calculation");
                return new PriceResult { Price = 0, Description = $"Manual Entry Required - Error Loading {serviceType} Price" };
            }
        }

        protected async Task OnPriceTypeChanged(ChangeEventArgs e)
        {
            try
            {
                if (int.TryParse(e.Value?.ToString(), out var priceTypeId))
                {
                    SelectedPriceTypeId = priceTypeId;
                    CurrentQuoteItem.PriceTypeId = priceTypeId;

                    // Update pricing when price type changes
                    if (SelectedEquipment != null)
                    {
                        await LoadPriceForEquipment();
                    }

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling price type change");
                ToastService.ShowError("Error updating price type");
            }
        }

        protected string GetSelectedPriceTypeName()
        {
            if (SelectedPriceTypeId.HasValue)
            {
                var priceType = ActivePriceTypes.FirstOrDefault(pt => pt.PriceTypeId == SelectedPriceTypeId.Value);
                return priceType?.Name ?? "Unknown";
            }
            return "Not selected";
        }

        protected bool GetSelectedPriceTypeIncludesTravel()
        {
            if (SelectedPriceTypeId.HasValue)
            {
                var priceType = ActivePriceTypes.FirstOrDefault(pt => pt.PriceTypeId == SelectedPriceTypeId.Value);
                return priceType?.IncludesTravel ?? false;
            }
            return false;
        }
    }
}
