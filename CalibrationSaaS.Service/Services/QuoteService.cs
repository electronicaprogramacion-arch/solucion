using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Application.UseCases;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class QuoteService : ServiceBase, IQuoteService<CallContext>
    {
        private readonly QuoteUseCases quoteLogic;
        private readonly PriceTypeUseCases priceTypeLogic;
        private readonly ILogger _logger;

        public QuoteService(QuoteUseCases quoteLogic, PriceTypeUseCases priceTypeLogic, ILogger<QuoteService> logger)
        {
            this.quoteLogic = quoteLogic;
            this.priceTypeLogic = priceTypeLogic;
            this._logger = logger;
        }

        #region Quote Methods

        public async ValueTask<Quote> CreateQuote(Quote quote, CallContext context)
        {
            try
            {
                // Validation will be handled by the use case layer

                // Generate quote number if not provided
                if (string.IsNullOrEmpty(quote.QuoteNumber))
                {
                    var quoteNumberResult = await GenerateQuoteNumber(context);
                    quote.QuoteNumber = quoteNumberResult.Value;
                }

                var result = await quoteLogic.CreateQuote(quote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quote");
                throw;
            }
        }

        public async ValueTask<ResultSet<Quote>> GetQuotes(Pagination<Quote> pagination, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuotes(pagination);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quotes");
                throw;
            }
        }

        public async ValueTask<Quote> GetQuoteByID(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuoteByID(quote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quote by ID");
                throw;
            }
        }

        public async ValueTask<Quote> UpdateQuote(Quote quote, CallContext context)
        {
            try
            {
                // Validation will be handled by the use case layer

                var result = await quoteLogic.UpdateQuote(quote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quote");
                throw;
            }
        }

        public async ValueTask<Quote> DeleteQuote(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.DeleteQuote(quote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quote");
                throw;
            }
        }

        public async ValueTask<QuoteCollectionResult> GetQuotesByCustomerID(Customer customer, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuotesByCustomerID(customer);
                return new QuoteCollectionResult { Quotes = result.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quotes by customer ID");
                throw;
            }
        }

        public async ValueTask<Quote> GetQuoteByNumber(string quoteNumber, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuoteByNumber(quoteNumber);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quote by number");
                throw;
            }
        }

        #endregion

        #region QuoteItem Methods

        public async ValueTask<QuoteItem> CreateQuoteItem(QuoteItem quoteItem, CallContext context)
        {
            try
            {
                // Validation will be handled by the use case layer

                var result = await quoteLogic.CreateQuoteItem(quoteItem);

                // Update quote total cost - fetch the existing quote first to avoid EF tracking issues
                var existingQuote = await quoteLogic.GetQuoteByID(new Quote { QuoteID = quoteItem.QuoteID });
                if (existingQuote != null)
                {
                    await UpdateQuoteTotalCost(existingQuote, context);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quote item");
                throw;
            }
        }

        public async ValueTask<ResultSet<QuoteItem>> GetQuoteItems(Pagination<QuoteItem> pagination, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuoteItems(pagination);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quote items");
                throw;
            }
        }

        public async ValueTask<QuoteItem> GetQuoteItemByID(QuoteItem quoteItem, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuoteItemByID(quoteItem);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quote item by ID");
                throw;
            }
        }

        public async ValueTask<QuoteItem> UpdateQuoteItem(QuoteItem quoteItem, CallContext context)
        {
            try
            {
                // Validation will be handled by the use case layer

                var result = await quoteLogic.UpdateQuoteItem(quoteItem);

                // Update quote total cost - fetch the existing quote first to avoid EF tracking issues
                var existingQuote = await quoteLogic.GetQuoteByID(new Quote { QuoteID = quoteItem.QuoteID });
                if (existingQuote != null)
                {
                    await UpdateQuoteTotalCost(existingQuote, context);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quote item");
                throw;
            }
        }

        public async ValueTask<QuoteItem> DeleteQuoteItem(QuoteItem quoteItem, CallContext context)
        {
            try
            {
                var result = await quoteLogic.DeleteQuoteItem(quoteItem);

                // Update quote total cost - fetch the existing quote first to avoid EF tracking issues
                var existingQuote = await quoteLogic.GetQuoteByID(new Quote { QuoteID = quoteItem.QuoteID });
                if (existingQuote != null)
                {
                    await UpdateQuoteTotalCost(existingQuote, context);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quote item");
                throw;
            }
        }

        public async ValueTask<QuoteItemCollectionResult> GetQuoteItemsByQuoteID(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetQuoteItemsByQuoteID(quote);
                return new QuoteItemCollectionResult { QuoteItems = result.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quote items by quote ID");
                throw;
            }
        }

        public async ValueTask<QuoteItemCollectionResult> GetChildQuoteItems(QuoteItem parentQuoteItem, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetChildQuoteItems(parentQuoteItem);
                return new QuoteItemCollectionResult { QuoteItems = result.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting child quote items");
                throw;
            }
        }

        #endregion

        #region Business Logic Methods

        public async ValueTask<PriceResult> CalculateQuoteTotalCost(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.CalculateQuoteTotalCost(quote);
                return new PriceResult { Price = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating quote total cost");
                throw;
            }
        }

        public async ValueTask<PriceResult> GetEquipmentTemplatePriceByServiceType(EquipmentTemplate equipmentTemplate, string serviceType, CallContext context)
        {
            try
            {
                // TODO: Implement equipment template pricing using dynamic pricing system
                // This will use the PriceTypePrice table to get prices for equipment templates
                decimal price = 0;

                _logger.LogInformation("Retrieved equipment template price: EquipmentTemplateID={EquipmentTemplateID}, ServiceType={ServiceType}, Price={Price}",
                    equipmentTemplate.EquipmentTemplateID, serviceType, price);

                return new PriceResult { Price = price };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting equipment template price by service type");
                throw;
            }
        }

        public async ValueTask<PriceResult> GetPieceOfEquipmentPriceByServiceType(PieceOfEquipment pieceOfEquipment, string serviceType, CallContext context)
        {
            try
            {
                // This method is deprecated - use the dynamic pricing system instead
                // For backward compatibility, return zero and require manual entry
                decimal price = 0;

                _logger.LogWarning("GetPieceOfEquipmentPriceByServiceType is deprecated. Use dynamic pricing system instead. PieceOfEquipmentID={PieceOfEquipmentID}, ServiceType={ServiceType}",
                    pieceOfEquipment.PieceOfEquipmentID, serviceType);

                return new PriceResult { Price = price, Description = "Manual Entry Required - Use Dynamic Pricing System" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting piece of equipment price by service type");
                throw;
            }
        }

        public async ValueTask<PriceResult> GetHierarchicalPrice(PieceOfEquipment pieceOfEquipment, string serviceType, CallContext context)
        {
            try
            {
                // If no equipment is provided, return zero price (manual entry required)
                if (pieceOfEquipment == null)
                {
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Equipment Selected" };
                }

                // This method is deprecated - use the dynamic pricing system with PriceType instead
                _logger.LogWarning("GetHierarchicalPrice with serviceType is deprecated. Use GetHierarchicalPriceByPriceType instead. PieceOfEquipmentID={PieceOfEquipmentID}, ServiceType={ServiceType}",
                    pieceOfEquipment.PieceOfEquipmentID, serviceType);

                decimal price = 0;
                string priceSource = $"Manual Entry Required - Use Dynamic Pricing System Instead of {serviceType}";

                _logger.LogInformation("Deprecated hierarchical price calculation: PieceOfEquipmentID={PieceOfEquipmentID}, ServiceType={ServiceType}, Price={Price}, Source={Source}",
                    pieceOfEquipment?.PieceOfEquipmentID, serviceType, price, priceSource);

                return new PriceResult { Price = price, Description = priceSource };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating hierarchical price for equipment {PieceOfEquipmentID}, ServiceType {ServiceType}: {Error}",
                    pieceOfEquipment?.PieceOfEquipmentID, serviceType, ex.Message);
                return new PriceResult { Price = 0, Description = $"Manual Entry Required - Error Loading {serviceType} Price" };
            }
        }

        // New PriceType-based pricing methods
        public async ValueTask<PriceResult> GetHierarchicalPriceByPriceType(PieceOfEquipment pieceOfEquipment, int priceTypeId, CallContext context)
        {
            try
            {
                decimal price = 0;
                string priceSource = "";

                if (pieceOfEquipment == null)
                {
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Equipment Selected" };
                }

                // First priority: PieceOfEquipment price
                var poePrice = await GetPieceOfEquipmentPriceByPriceType(pieceOfEquipment, priceTypeId, context);
                if (poePrice.Price > 0)
                {
                    return poePrice;
                }

                // Second priority: EquipmentTemplate price
                if (pieceOfEquipment.EquipmentTemplate != null)
                {
                    var templatePrice = await GetEquipmentTemplatePriceByPriceType(pieceOfEquipment.EquipmentTemplate, priceTypeId, context);
                    if (templatePrice.Price > 0)
                    {
                        return templatePrice;
                    }
                }

                // No price found, require manual entry
                return new PriceResult { Price = 0, Description = "Manual Entry Required - No Price Configured" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating hierarchical price by price type");
                return new PriceResult { Price = 0, Description = "Manual Entry Required - Error Loading Price" };
            }
        }

        public async ValueTask<PriceResult> GetEquipmentTemplatePriceByPriceType(EquipmentTemplate equipmentTemplate, int priceTypeId, CallContext context)
        {
            try
            {
                if (equipmentTemplate == null)
                {
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Equipment Template" };
                }

                // Use the dynamic pricing system to get the price for this equipment template
                var price = await priceTypeLogic.GetPriceForEntity(EntityType.EquipmentTemplate,
                    equipmentTemplate.EquipmentTemplateID, priceTypeId);

                if (price > 0)
                {
                    _logger.LogInformation("Found equipment template price: EquipmentTemplateID={EquipmentTemplateID}, PriceTypeId={PriceTypeId}, Price={Price}",
                        equipmentTemplate.EquipmentTemplateID, priceTypeId, price);
                    return new PriceResult { Price = price, Description = "Equipment Template Price" };
                }
                else
                {
                    _logger.LogInformation("No price found for equipment template: EquipmentTemplateID={EquipmentTemplateID}, PriceTypeId={PriceTypeId}",
                        equipmentTemplate.EquipmentTemplateID, priceTypeId);
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Template Price Configured" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting equipment template price by price type: EquipmentTemplateID={EquipmentTemplateID}, PriceTypeId={PriceTypeId}",
                    equipmentTemplate?.EquipmentTemplateID, priceTypeId);
                return new PriceResult { Price = 0, Description = "Manual Entry Required - Error Loading Price" };
            }
        }

        public async ValueTask<PriceResult> GetPieceOfEquipmentPriceByPriceType(PieceOfEquipment pieceOfEquipment, int priceTypeId, CallContext context)
        {
            try
            {
                if (pieceOfEquipment == null)
                {
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Equipment Selected" };
                }

                // Use the dynamic pricing system to get the price for this specific piece of equipment
                var price = await priceTypeLogic.GetPriceForEntity(EntityType.PieceOfEquipment,
                    int.Parse(pieceOfEquipment.PieceOfEquipmentID), priceTypeId);

                if (price > 0)
                {
                    _logger.LogInformation("Found piece of equipment price: PieceOfEquipmentID={PieceOfEquipmentID}, PriceTypeId={PriceTypeId}, Price={Price}",
                        pieceOfEquipment.PieceOfEquipmentID, priceTypeId, price);
                    return new PriceResult { Price = price, Description = "Piece of Equipment Price" };
                }
                else
                {
                    _logger.LogInformation("No price found for piece of equipment: PieceOfEquipmentID={PieceOfEquipmentID}, PriceTypeId={PriceTypeId}",
                        pieceOfEquipment.PieceOfEquipmentID, priceTypeId);
                    return new PriceResult { Price = 0, Description = "Manual Entry Required - No Price Configured" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting piece of equipment price by price type: PieceOfEquipmentID={PieceOfEquipmentID}, PriceTypeId={PriceTypeId}",
                    pieceOfEquipment?.PieceOfEquipmentID, priceTypeId);
                return new PriceResult { Price = 0, Description = "Manual Entry Required - Error Loading Price" };
            }
        }

        // Travel expense methods
        public async ValueTask<BoolResult> RequiresTravelExpense(Quote quote, CallContext context)
        {
            try
            {
                // Get all quote items for this quote
                var quoteItems = await quoteLogic.GetQuoteItemsByQuoteID(quote);

                if (quoteItems == null || !quoteItems.Any())
                {
                    return new BoolResult { Value = false, Description = "No quote items found" };
                }

                // Check if any quote item uses a price type that requires travel
                foreach (var item in quoteItems)
                {
                    if (item.PriceTypeId.HasValue)
                    {
                        // Use the quote logic to check if the price type requires travel
                        var requiresTravel = await quoteLogic.DoesPriceTypeRequireTravel(item.PriceTypeId.Value);

                        if (requiresTravel)
                        {
                            return new BoolResult
                            {
                                Value = true,
                                Description = $"Travel required for price type ID: {item.PriceTypeId.Value}"
                            };
                        }
                    }
                }

                return new BoolResult { Value = false, Description = "No travel-requiring price types found" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking travel expense requirement");
                return new BoolResult { Value = false, Description = "Error checking travel expense requirement" };
            }
        }

        public async ValueTask<QuoteItem> CreateTravelExpenseQuoteItem(Quote quote, decimal travelExpense, CallContext context)
        {
            try
            {
                var travelItem = new QuoteItem
                {
                    QuoteID = quote.QuoteID,
                    ItemDescription = "Travel Expenses",
                    Quantity = 1,
                    UnitPrice = travelExpense,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                var result = await quoteLogic.CreateQuoteItem(travelItem);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating travel expense quote item");
                throw;
            }
        }

        public async ValueTask<QuoteItem> RemoveTravelExpenseQuoteItem(Quote quote, CallContext context)
        {
            try
            {
                // Find and remove travel expense item
                // This would require querying quote items for travel expense entries
                // For now, return null as placeholder
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing travel expense quote item");
                throw;
            }
        }

        public async ValueTask<Quote> UpdateQuoteTotalCost(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.UpdateQuoteTotalCost(quote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quote total cost");
                throw;
            }
        }

        public async ValueTask<PieceOfEquipmentCollectionResult> GetChildEquipmentForQuote(PieceOfEquipment parentEquipment, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GetChildEquipmentForQuote(parentEquipment);
                return new PieceOfEquipmentCollectionResult { PieceOfEquipments = result.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting child equipment for quote");
                throw;
            }
        }

        public async ValueTask<StringResult> GenerateQuoteNumber(CallContext context)
        {
            try
            {
                var result = await quoteLogic.GenerateQuoteNumber();
                return new StringResult { Value = result, Description = "Quote number generated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating quote number");
                throw;
            }
        }

        public async ValueTask<Quote> ChangeQuoteStatus(Quote quote, string newStatus, CallContext context)
        {
            try
            {
                var result = await quoteLogic.ChangeQuoteStatus(quote, newStatus);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing quote status");
                throw;
            }
        }

        public async ValueTask<WorkOrder> AcceptAndScheduleWorkOrder(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.AcceptAndScheduleWorkOrder(quote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting quote and scheduling work order");
                throw;
            }
        }

        public async ValueTask<PriceResult> GetCustomerQuoteMultiplier(Customer customer, CallContext context)
        {
            try
            {
                var result = QuoteUseCases.GetCustomerQuoteMultiplier(customer);
                return new PriceResult { Price = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer quote multiplier for customer {CustomerID}", customer.CustomerID);
                throw;
            }
        }

        public async ValueTask<PriceResult> ApplyCustomerMultiplier(PriceResult basePrice, Customer customer, CallContext context)
        {
            try
            {
                var result = QuoteUseCases.ApplyCustomerMultiplier(basePrice.Price, customer);
                return new PriceResult { Price = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying customer multiplier for customer {CustomerID}", customer.CustomerID);
                throw;
            }
        }

        #endregion

        #region Report Generation Methods

        public async ValueTask<StringResult> GenerateQuoteReport(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GenerateQuoteReport(quote);
                return new StringResult { Value = result, Description = "Quote report generated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating quote report");
                throw;
            }
        }

        public async ValueTask<ByteArrayResult> GenerateQuoteReportPDF(Quote quote, CallContext context)
        {
            try
            {
                var result = await quoteLogic.GenerateQuoteReportPDF(quote);
                return new ByteArrayResult { Value = result, Description = "Quote PDF report generated successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating quote report PDF");
                throw;
            }
        }

        #endregion
    }
}
