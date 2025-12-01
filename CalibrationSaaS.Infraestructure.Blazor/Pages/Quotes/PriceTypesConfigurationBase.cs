using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Shared.Component;
using Blazed.Controls;
using Blazed.Controls.Toast;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Quotes
{
    public class PriceTypesConfigurationBase : Base_Create<PriceType, IPriceTypeService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {
        [Inject] protected IConfiguration Configuration { get; set; } = null!;
        [Inject] protected new ILogger<PriceTypesConfigurationBase> Logger { get; set; } = null!;
        [Inject] protected new NavigationManager NavigationManager { get; set; } = null!;

        // Price Types Grid
        public ResponsiveTable<PriceType> GridPriceTypes { get; set; } = new ResponsiveTable<PriceType>();
        public List<PriceType> ListPriceTypes = new List<PriceType>();

        // Entity for GetPropertyName expressions (following the pattern from other search pages)
        protected new PriceType eq { get; set; } = new PriceType();

        // Price Type Prices Grid
        public ResponsiveTable<PriceTypePrice> GridPriceTypePrices { get; set; } = new ResponsiveTable<PriceTypePrice>();
        public List<PriceTypePrice> ListPriceTypePrices = new List<PriceTypePrice>();
        public EditContext PriceEditContext { get; set; } = null!;
        public PriceTypePrice PriceEntity { get; set; } = new PriceTypePrice();

        // Selected Price Type for master-detail view
        public PriceType? SelectedPriceType { get; set; }
        public PriceType Entity { get; set; } = new PriceType();

        // Entity Type options for dropdown
        public List<SelectOption> EntityTypeOptions { get; set; } = new List<SelectOption>();

        protected override async Task OnInitializedAsync()
        {
//            await JSRuntime.InvokeVoidAsync("console.log", "🚀 PriceTypes: OnInitializedAsync started");

            await base.OnInitializedAsync();

            PriceEditContext = new EditContext(PriceEntity);

            // Initialize entity type options
            EntityTypeOptions = new List<SelectOption>
            {
                new SelectOption { Value = (int)EntityType.PieceOfEquipment, Text = "Piece of Equipment" },
                new SelectOption { Value = (int)EntityType.EquipmentTemplate, Text = "Equipment Template" }
            };

//            await JSRuntime.InvokeVoidAsync("console.log", "📋 PriceTypes: About to call LoadInitialData");

            // Load initial data like QuotesSearch does
            await LoadInitialData();

//            await JSRuntime.InvokeVoidAsync("console.log", "✅ PriceTypes: OnInitializedAsync completed");
        }

        private async Task LoadInitialData()
        {
            try
            {
                //Logger.LogInformation("🔄 Starting to load initial price types data");
//                await JSRuntime.InvokeVoidAsync("console.log", "🔄 PriceTypes: Starting LoadInitialData");

                // Load price types data
                var pagination = new Pagination<PriceType>
                {
                    Page = 1,
                    Show = 100,
                    Filter = "",
                    Object = new Helpers.Controls.ValueObjects.FilterObject<PriceType>()
                };

                //Logger.LogInformation($"📋 Calling LoadData with pagination: Page={pagination.Page}, Show={pagination.Show}");
//                await JSRuntime.InvokeVoidAsync("console.log", $"📋 PriceTypes: Calling LoadData with Page={pagination.Page}, Show={pagination.Show}");

                var result = await LoadData(pagination);

                if (result != null)
                {
                    //Logger.LogInformation($"✅ LoadData returned result with Count={result.Count}, List.Count={result.List?.Count ?? 0}");
//                    await JSRuntime.InvokeVoidAsync("console.log", $"✅ PriceTypes: LoadData returned Count={result.Count}, List.Count={result.List?.Count ?? 0}");

                    ListPriceTypes = result.List?.OrderBy(x => x.SortOrder).ThenBy(x => x.Name).ToList() ?? new List<PriceType>();
                    TotalRows = result.Count;
                    paginasTotales = (int)Math.Ceiling((double)TotalRows / paginaSize);

                    //Logger.LogInformation($"📊 Set ListPriceTypes with {ListPriceTypes.Count} items, TotalRows={TotalRows}");
//                    await JSRuntime.InvokeVoidAsync("console.log", $"📊 PriceTypes: Set ListPriceTypes with {ListPriceTypes.Count} items, TotalRows={TotalRows}");

                    // Log first few items for debugging
                    if (ListPriceTypes.Any())
                    {
                        var firstItem = ListPriceTypes.First();
                        //Logger.LogInformation($"🔍 First price type: ID={firstItem.PriceTypeId}, Name='{firstItem.Name}', Description='{firstItem.Description}', RequiresTravel={firstItem.RequiresTravel}, IsActive={firstItem.IsActive}, SortOrder={firstItem.SortOrder}");
//                        await JSRuntime.InvokeVoidAsync("console.log", $"🔍 PriceTypes: First item - ID={firstItem.PriceTypeId}, Name='{firstItem.Name}', Description='{firstItem.Description}', RequiresTravel={firstItem.RequiresTravel}, IsActive={firstItem.IsActive}, SortOrder={firstItem.SortOrder}");

                        // Log all items to see their actual values
                        for (int i = 0; i < ListPriceTypes.Count; i++)
                        {
                            var item = ListPriceTypes[i];
//                            await JSRuntime.InvokeVoidAsync("console.log", $"🔍 PriceTypes[{i}]: ID={item.PriceTypeId}, Name='{item.Name}', Description='{item.Description}', RequiresTravel={item.RequiresTravel}, IsActive={item.IsActive}, SortOrder={item.SortOrder}");
                        }
                    }
                }
                else
                {
                    //Logger.LogWarning("❌ LoadData returned null result");
//                    await JSRuntime.InvokeVoidAsync("console.log", "❌ PriceTypes: LoadData returned null result");
                    ListPriceTypes = new List<PriceType>();
                    TotalRows = 0;
                    paginasTotales = 0;
                }

                // Force UI update
                StateHasChanged();
//                await JSRuntime.InvokeVoidAsync("console.log", "🔄 PriceTypes: StateHasChanged called");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "💥 Error loading initial price types data");
//                await JSRuntime.InvokeVoidAsync("console.log", $"💥 PriceTypes: Error loading data - {ex.Message}");
                await ShowToast("Error loading price types", Blazed.Controls.Toast.ToastLevel.Error);
            }
        }

        #region Price Types Methods

        public override async Task<ResultSet<PriceType>> LoadData(Pagination<PriceType> pag)
        {
            return await LoadPriceTypesData(pag);
        }

        public async Task<ResultSet<PriceType>> LoadPriceTypesData(Pagination<PriceType> pag)
        {
            try
            {
                //Logger.LogInformation($"🔍 LoadPriceTypesData called with Page={pag.Page}, Show={pag.Show}");
//                await JSRuntime.InvokeVoidAsync("console.log", $"🔍 PriceTypes: LoadPriceTypesData called with Page={pag.Page}, Show={pag.Show}");

                var result = await Client.GetPriceTypes(pag, new CallContext());

                //Logger.LogInformation($"📡 Client.GetPriceTypes returned: Count={result?.Count ?? 0}, List.Count={result?.List?.Count ?? 0}");
//                await JSRuntime.InvokeVoidAsync("console.log", $"📡 PriceTypes: Client.GetPriceTypes returned Count={result?.Count ?? 0}, List.Count={result?.List?.Count ?? 0}");

                if (result?.List != null && result.List.Any())
                {
                    var firstItem = result.List.First();
                    //Logger.LogInformation($"🎯 First price type from gRPC: ID={firstItem.PriceTypeId}, Name='{firstItem.Name}', Description='{firstItem.Description}', RequiresTravel={firstItem.RequiresTravel}, IsActive={firstItem.IsActive}");
//                    await JSRuntime.InvokeVoidAsync("console.log", $"🎯 PriceTypes: First item from gRPC - ID={firstItem.PriceTypeId}, Name='{firstItem.Name}', Description='{firstItem.Description}', RequiresTravel={firstItem.RequiresTravel}, IsActive={firstItem.IsActive}");
                }
                else
                {
                    //Logger.LogWarning("⚠️ No price types returned from gRPC service");
//                    await JSRuntime.InvokeVoidAsync("console.log", "⚠️ PriceTypes: No price types returned from gRPC service");
                }

                return result;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "💥 Error loading price types data from gRPC");
//                await JSRuntime.InvokeVoidAsync("console.log", $"💥 PriceTypes: Error loading data from gRPC - {ex.Message}");
                await ShowToast("Error loading price types", Blazed.Controls.Toast.ToastLevel.Error);
                return new ResultSet<PriceType> { List = new List<PriceType>(), Count = 0 };
            }
        }

        public async Task<bool> SubmittedPriceType(PriceType priceType)
        {
            try
            {
                // Validate unique name
                if (ListPriceTypes.Any(x => x.Name.Equals(priceType.Name, StringComparison.OrdinalIgnoreCase) && x.PriceTypeId != priceType.PriceTypeId))
                {
                    await ShowToast("A price type with this name already exists", Blazed.Controls.Toast.ToastLevel.Warning);
                    return false;
                }

                priceType.CreatedBy = "System"; // TODO: Get current user from context
                priceType.ModifiedBy = "System";
                priceType.CreatedDate = DateTime.UtcNow;
                priceType.ModifiedDate = DateTime.UtcNow;

                var result = await Client.CreatePriceType(priceType, new CallContext());
                
                if (result != null)
                {
                    await ShowToast("Price type created successfully", Blazed.Controls.Toast.ToastLevel.Success);
                    StateHasChanged();
                    return true;
                }
                
                await ShowToast("Error creating price type", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error creating price type: {Name}", priceType.Name);
                await ShowToast("Error creating price type", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
        }

        public async Task<bool> SubmittedUpdatePriceType(PriceType priceType)
        {
            try
            {
                // Validate unique name
                if (ListPriceTypes.Any(x => x.Name.Equals(priceType.Name, StringComparison.OrdinalIgnoreCase) && x.PriceTypeId != priceType.PriceTypeId))
                {
                    await ShowToast("A price type with this name already exists", Blazed.Controls.Toast.ToastLevel.Warning);
                    return false;
                }

                priceType.ModifiedBy = "System"; // TODO: Get current user from context
                priceType.ModifiedDate = DateTime.UtcNow;

                var result = await Client.UpdatePriceType(priceType, new CallContext());
                
                if (result != null)
                {
                    await ShowToast("Price type updated successfully", Blazed.Controls.Toast.ToastLevel.Success);
                    StateHasChanged();
                    
                    // Refresh detail grid if this is the selected price type
                    if (SelectedPriceType?.PriceTypeId == priceType.PriceTypeId)
                    {
                        SelectedPriceType = priceType;
                        await RefreshPriceTypePrices();
                    }
                    
                    return true;
                }
                
                await ShowToast("Error updating price type", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error updating price type: {Name}", priceType.Name);
                await ShowToast("Error updating price type", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
        }

        public async Task<bool> DeletePriceType(PriceType priceType)
        {
            try
            {
                var result = await Client.DeletePriceType(priceType, new CallContext());
                
                if (result != null)
                {
                    await ShowToast("Price type deleted successfully", Blazed.Controls.Toast.ToastLevel.Success);
                    StateHasChanged();
                    
                    // Clear detail grid if this was the selected price type
                    if (SelectedPriceType?.PriceTypeId == priceType.PriceTypeId)
                    {
                        SelectedPriceType = null;
                        ListPriceTypePrices.Clear();
                        StateHasChanged();
                    }
                    
                    return true;
                }
                
                await ShowToast("Error deleting price type", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error deleting price type: {Name}", priceType.Name);
                await ShowToast("Error deleting price type", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
        }

        public PriceType DefaultPriceType()
        {
            return new PriceType
            {
                Name = "",
                Description = "",
                IsActive = true,
                RequiresTravel = false,
                SortOrder = 0,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
        }

        public async Task OnPriceTypeSelected(PriceType priceType)
        {
            try
            {
                SelectedPriceType = priceType;
                await RefreshPriceTypePrices();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error selecting price type: {Name}", priceType.Name);
                await ShowToast("Error loading price type details", Blazed.Controls.Toast.ToastLevel.Error);
            }
        }

        #endregion

        #region Price Type Prices Methods

        public async Task<ResultSet<PriceTypePrice>> LoadPriceTypePricesData(Pagination<PriceTypePrice> pag)
        {
            try
            {
                if (SelectedPriceType == null)
                {
                    return new ResultSet<PriceTypePrice> { List = new List<PriceTypePrice>(), Count = 0 };
                }

                // Filter by selected price type using server-side filtering
                pag.Filter = SelectedPriceType.PriceTypeId.ToString();

                var result = await Client.GetPriceTypePrices(pag, new CallContext());

                if (result?.List != null)
                {
                    ListPriceTypePrices = result.List.ToList();
                    return result;
                }

                return new ResultSet<PriceTypePrice> { List = new List<PriceTypePrice>(), Count = 0 };
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading price type prices data for price type: {PriceTypeId}", SelectedPriceType?.PriceTypeId);
                await ShowToast("Error loading prices", Blazed.Controls.Toast.ToastLevel.Error);
                return new ResultSet<PriceTypePrice> { List = new List<PriceTypePrice>(), Count = 0 };
            }
        }

        public async Task<bool> SubmittedPriceTypePrice(PriceTypePrice priceTypePrice)
        {
            try
            {
                if (SelectedPriceType == null)
                {
                    await ShowToast("Please select a price type first", Blazed.Controls.Toast.ToastLevel.Warning);
                    return false;
                }

                priceTypePrice.PriceTypeId = SelectedPriceType.PriceTypeId;
                priceTypePrice.CreatedDate = DateTime.UtcNow;
                priceTypePrice.ModifiedDate = DateTime.UtcNow;

                var result = await Client.CreatePriceTypePrice(priceTypePrice, new CallContext());
                
                if (result != null)
                {
                    await ShowToast("Price created successfully", Blazed.Controls.Toast.ToastLevel.Success);
                    await RefreshPriceTypePrices();
                    return true;
                }
                
                await ShowToast("Error creating price", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error creating price type price");
                await ShowToast("Error creating price", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
        }

        public async Task<bool> SubmittedUpdatePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            try
            {
                priceTypePrice.ModifiedDate = DateTime.UtcNow;

                var result = await Client.UpdatePriceTypePrice(priceTypePrice, new CallContext());
                
                if (result != null)
                {
                    await ShowToast("Price updated successfully", Blazed.Controls.Toast.ToastLevel.Success);
                    await RefreshPriceTypePrices();
                    return true;
                }
                
                await ShowToast("Error updating price", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error updating price type price");
                await ShowToast("Error updating price", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
        }

        public async Task<bool> DeletePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            try
            {
                var result = await Client.DeletePriceTypePrice(priceTypePrice, new CallContext());
                
                if (result != null)
                {
                    await ShowToast("Price deleted successfully", Blazed.Controls.Toast.ToastLevel.Success);
                    await RefreshPriceTypePrices();
                    return true;
                }
                
                await ShowToast("Error deleting price", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error deleting price type price");
                await ShowToast("Error deleting price", Blazed.Controls.Toast.ToastLevel.Error);
                return false;
            }
        }

        public PriceTypePrice DefaultPriceTypePrice()
        {
            return new PriceTypePrice
            {
                PriceTypeId = SelectedPriceType?.PriceTypeId ?? 0,
                EntityType = EntityType.PieceOfEquipment,
                EntityId = 0,
                Price = 0,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
        }

        private async Task RefreshPriceTypePrices()
        {
            // Reload the price type prices data
            await LoadPriceTypePricesData(new Pagination<PriceTypePrice>());
            StateHasChanged();
        }

        public void ClearPricesList()
        {
            ListPriceTypePrices.Clear();
        }

        #endregion

        public class SelectOption
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        protected async Task ClearList()
        {
            try
            {
                ListPriceTypes.Clear();
                await LoadInitialData();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error clearing price types list");
                await ShowToast("Error refreshing price types list", Blazed.Controls.Toast.ToastLevel.Error);
            }
        }

        protected void ClearSelection()
        {
            SelectedPriceType = null;
            ListPriceTypePrices.Clear();
            StateHasChanged();
        }

        #region Navigation Methods

        public async Task ViewPriceType(int priceTypeId)
        {
            try
            {
                NavigationManager.NavigateTo($"/PriceTypeDetail/{priceTypeId}");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error navigating to price type detail for ID: {PriceTypeId}", priceTypeId);
                await ShowToast("Error opening price type details", Blazed.Controls.Toast.ToastLevel.Error);
            }
        }

        public async Task EditPriceType(int priceTypeId)
        {
            try
            {
                NavigationManager.NavigateTo($"/PriceTypeEdit/{priceTypeId}");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error navigating to price type edit for ID: {PriceTypeId}", priceTypeId);
                await ShowToast("Error opening price type editor", Blazed.Controls.Toast.ToastLevel.Error);
            }
        }

        public async Task CreateNewPriceType()
        {
            try
            {
                NavigationManager.NavigateTo("/PriceTypeCreate");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error navigating to price type creation");
                await ShowToast("Error opening price type creator", Blazed.Controls.Toast.ToastLevel.Error);
            }
        }

        #endregion
    }
}
