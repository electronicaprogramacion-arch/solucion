using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Domain.Aggregates.Shared;
using Blazed.Controls.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Quotes
{
    public class PriceTypeDetailBase : Base_Create<PriceType, IPriceTypeService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {
        [Parameter] public int PriceTypeId { get; set; }
        
        [Inject] protected ILogger<PriceTypeDetailBase> Logger { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IToastService ToastService { get; set; }

        public PriceType CurrentPriceType { get; set; } = new PriceType();
        public bool IsLoading { get; set; } = true;
        public bool IsSaving { get; set; } = false;
        public bool IsNewPriceType => PriceTypeId == 0;
        public string PageTitle => IsNewPriceType ? "Create Price Type" : $"Edit Price Type - {CurrentPriceType?.Name}";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                Logger.LogInformation($"PriceTypeDetail initialized with PriceTypeId: {PriceTypeId}");

                if (IsNewPriceType)
                {
                    Logger.LogInformation("Initializing new price type");
                    await InitializeNewPriceType();
                }
                else
                {
                    Logger.LogInformation($"Loading existing price type with ID: {PriceTypeId}");
                    await LoadPriceType();
                }

                IsLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error initializing PriceTypeDetail page");
                ToastService.ShowError("Error loading price type data");
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task InitializeNewPriceType()
        {
            CurrentPriceType = new PriceType
            {
                Name = "",
                Description = "",
                IsActive = true,
                RequiresTravel = false,
                SortOrder = 0,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            
            Logger.LogInformation("New price type initialized");
        }

        private async Task LoadPriceType()
        {
            try
            {
                var priceType = new PriceType { PriceTypeId = PriceTypeId };
                CurrentPriceType = await Client.GetPriceTypeByID(priceType, new CallContext());

                if (CurrentPriceType == null)
                {
                    Logger.LogWarning($"Price type not found with ID: {PriceTypeId}");
                    ToastService.ShowError("Price type not found");
                    NavigationManager.NavigateTo("/Quotes/PriceTypesConfiguration");
                    return;
                }

                Logger.LogInformation($"Price type loaded successfully - ID: {CurrentPriceType.PriceTypeId}, Name: {CurrentPriceType.Name}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error loading price type with ID: {PriceTypeId}");
                ToastService.ShowError("Error loading price type");
                NavigationManager.NavigateTo("/Quotes/PriceTypesConfiguration");
            }
        }

        public async Task SavePriceType()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();

                Logger.LogInformation($"Saving price type - IsNew: {IsNewPriceType}, Name: {CurrentPriceType.Name}");

                if (IsNewPriceType)
                {
                    CurrentPriceType.CreatedBy = "System"; // TODO: Get current user
                    CurrentPriceType.ModifiedBy = "System";
                    CurrentPriceType.CreatedDate = DateTime.UtcNow;
                    CurrentPriceType.ModifiedDate = DateTime.UtcNow;

                    var result = await Client.CreatePriceType(CurrentPriceType, new CallContext());
                    
                    if (result != null)
                    {
                        Logger.LogInformation($"Price type created successfully with ID: {result.PriceTypeId}");
                        ToastService.ShowSuccess("Price type created successfully");
                        NavigationManager.NavigateTo("/Quotes/PriceTypesConfiguration");
                    }
                    else
                    {
                        ToastService.ShowError("Error creating price type");
                    }
                }
                else
                {
                    CurrentPriceType.ModifiedBy = "System"; // TODO: Get current user
                    CurrentPriceType.ModifiedDate = DateTime.UtcNow;

                    var result = await Client.UpdatePriceType(CurrentPriceType, new CallContext());
                    
                    if (result != null)
                    {
                        Logger.LogInformation($"Price type updated successfully - ID: {result.PriceTypeId}");
                        ToastService.ShowSuccess("Price type updated successfully");
                        NavigationManager.NavigateTo("/Quotes/PriceTypesConfiguration");
                    }
                    else
                    {
                        ToastService.ShowError("Error updating price type");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error saving price type - IsNew: {IsNewPriceType}");
                ToastService.ShowError("Error saving price type");
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        public async Task DeletePriceType()
        {
            try
            {
                if (IsNewPriceType) return;

                IsSaving = true;
                StateHasChanged();

                Logger.LogInformation($"Deleting price type - ID: {CurrentPriceType.PriceTypeId}, Name: {CurrentPriceType.Name}");

                var result = await Client.DeletePriceType(CurrentPriceType, new CallContext());
                
                if (result != null)
                {
                    Logger.LogInformation($"Price type deleted successfully - ID: {CurrentPriceType.PriceTypeId}");
                    ToastService.ShowSuccess("Price type deleted successfully");
                    NavigationManager.NavigateTo("/Quotes/PriceTypesConfiguration");
                }
                else
                {
                    ToastService.ShowError("Error deleting price type");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error deleting price type - ID: {CurrentPriceType.PriceTypeId}");
                ToastService.ShowError("Error deleting price type");
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        public void CancelEdit()
        {
            Logger.LogInformation("Price type edit cancelled");
            NavigationManager.NavigateTo("/Quotes/PriceTypesConfiguration");
        }
    }
}
