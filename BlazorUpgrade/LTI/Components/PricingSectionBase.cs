using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace CalibrationSaaS.Infraestructure.Blazor.Components
{
    public class PricingSectionBase : ComponentBase
    {
        [Inject] protected IConfiguration Configuration { get; set; }
        [Inject] protected ILogger<PricingSectionBase> Logger { get; set; }
        [Inject] protected Func<dynamic, IPriceTypeService<CallContext>> PriceTypeServiceFactory { get; set; }

        [Parameter] public EntityType EntityType { get; set; }
        [Parameter] public int EntityId { get; set; }
        [Parameter] public int? TemplateId { get; set; }
        [Parameter] public bool Enabled { get; set; } = true;
        [Parameter] public EventCallback<List<PriceTypePrice>> OnPricesChanged { get; set; }

        protected List<PriceType> PriceTypes { get; set; } = new List<PriceType>();
        protected List<PriceTypePrice> CurrentPrices { get; set; } = new List<PriceTypePrice>();
        protected bool IsLoading { get; set; } = false;
        protected bool ShowPricingSection { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await CheckThermotempVisibility();
            
            if (ShowPricingSection)
            {
                await LoadPriceTypes();
                await LoadCurrentPrices();
            }
        }

        protected async Task CheckThermotempVisibility()
        {
            try
            {
                // Always show pricing section for all customers
                ShowPricingSection = true;

                //Logger.LogInformation("Pricing section visibility: {Visible} (Always enabled)", ShowPricingSection);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error checking pricing section visibility");
                ShowPricingSection = false;
            }
        }

        protected async Task LoadPriceTypes()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                var priceTypeService = PriceTypeServiceFactory(null);
                var callOptions = new CallOptions();
                
                var result = await priceTypeService.GetActivePriceTypes(new CallContext());
                
                if (result?.PriceTypes != null)
                {
                    PriceTypes = result.PriceTypes.ToList();
                    //Logger.LogInformation("Loaded {Count} price types", PriceTypes.Count);
                }
                else
                {
                    //Logger.LogWarning("No price types returned from service");
                    PriceTypes = new List<PriceType>();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading price types");
                PriceTypes = new List<PriceType>();
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async Task LoadCurrentPrices()
        {
            //TODO error in grpc call
            return;

            if (EntityId <= 0) return;

            try
            {
                var priceTypeService = PriceTypeServiceFactory(null);
                var callOptions = new CallOptions();
                
                var result = await priceTypeService.GetAllPricesForEntity(EntityType, EntityId, new CallContext());
                
                if (result?.PriceTypePrices != null)
                {
                    CurrentPrices = result.PriceTypePrices.ToList();
                    //Logger.LogInformation("Loaded {Count} current prices for {EntityType} {EntityId}", 
                //                        CurrentPrices.Count, EntityType, EntityId);
                }
                else
                {
                    CurrentPrices = new List<PriceTypePrice>();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading current prices for {EntityType} {EntityId}", EntityType, EntityId);
                CurrentPrices = new List<PriceTypePrice>();
            }
        }

        protected string GetPriceValue(int priceTypeId)
        {
            var price = CurrentPrices.FirstOrDefault(p => p.PriceTypeId == priceTypeId);
            return price?.Price.ToString("F2") ?? "";
        }

        protected async Task UpdatePrice(int priceTypeId, string value)
        {
            try
            {
                if (!decimal.TryParse(value, out decimal price))
                {
                    price = 0;
                }

                var existingPrice = CurrentPrices.FirstOrDefault(p => p.PriceTypeId == priceTypeId);
                
                if (existingPrice != null)
                {
                    existingPrice.Price = price;
                    existingPrice.ModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    var newPrice = new PriceTypePrice
                    {
                        PriceTypeId = priceTypeId,
                        EntityType = EntityType,
                        EntityId = EntityId,
                        Price = price,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };
                    CurrentPrices.Add(newPrice);
                }

                // Notify parent component of changes
                if (OnPricesChanged.HasDelegate)
                {
                    await OnPricesChanged.InvokeAsync(CurrentPrices);
                }

                //Logger.LogInformation("Updated price for {EntityType} {EntityId}, PriceType {PriceTypeId}: {Price}", 
                //                    EntityType, EntityId, priceTypeId, price);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error updating price for PriceType {PriceTypeId}", priceTypeId);
            }
        }

        public async Task SavePrices()
        {
            if (!CurrentPrices.Any()) return;

            try
            {
                var priceTypeService = PriceTypeServiceFactory(null);
                var callOptions = new CallOptions();

                foreach (var price in CurrentPrices)
                {
                    if (price.Id == 0) // New price
                    {
                        await priceTypeService.CreatePriceTypePrice(price, new CallContext());
                    }
                    else // Update existing price
                    {
                        await priceTypeService.UpdatePriceTypePrice(price, new CallContext());
                    }
                }

                //Logger.LogInformation("Saved {Count} prices for {EntityType} {EntityId}", 
                //                    CurrentPrices.Count, EntityType, EntityId);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error saving prices for {EntityType} {EntityId}", EntityType, EntityId);
                throw;
            }
        }

        public List<PriceTypePrice> GetCurrentPrices()
        {
            return CurrentPrices.ToList();
        }

        public async Task RefreshPrices()
        {
            await LoadCurrentPrices();
            StateHasChanged();
        }
    }
}
