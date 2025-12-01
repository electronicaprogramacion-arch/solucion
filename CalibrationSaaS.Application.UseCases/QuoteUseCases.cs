using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Domain.BusinessExceptions;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Helpers;

namespace CalibrationSaaS.Application.UseCases
{
    public class QuoteUseCases
    {
        private readonly IQuoteRepository quoteRepository;
        private readonly IWorkOrderRepository workOrderRepository;
        private readonly WorkOrderDetailUseCase workOrderDetailUseCase;
        private readonly IPriceTypeRepository priceTypeRepository;
        private readonly IPieceOfEquipmentRepository pieceOfEquipmentRepository;
        private readonly IConfiguration configuration;

        public QuoteUseCases(IQuoteRepository quoteRepository, IWorkOrderRepository workOrderRepository, WorkOrderDetailUseCase workOrderDetailUseCase, IConfiguration configuration, IPriceTypeRepository priceTypeRepository, IPieceOfEquipmentRepository pieceOfEquipmentRepository)
        {
            this.quoteRepository = quoteRepository;
            this.workOrderRepository = workOrderRepository;
            this.workOrderDetailUseCase = workOrderDetailUseCase;
            this.configuration = configuration;
            this.priceTypeRepository = priceTypeRepository;
            this.pieceOfEquipmentRepository = pieceOfEquipmentRepository;
        }

        #region Quote Methods

        public async Task<Quote> CreateQuote(Quote quote)
        {
            // Check if quote number already exists
            var existingQuote = await quoteRepository.GetQuoteByNumber(quote.QuoteNumber);
            if (existingQuote != null)
            {
                throw new ExistingRecordException<Quote>("Quote number already exists", null, existingQuote);
            }

            quote.CreatedDate = DateTime.UtcNow;
            quote.ModifiedDate = DateTime.UtcNow;
            quote.TotalCost = 0; // Will be calculated when items are added

            // Ensure CreatedBy and ModifiedBy are set if they are null or empty
            if (string.IsNullOrEmpty(quote.CreatedBy))
            {
                quote.CreatedBy = "System";
            }
            if (string.IsNullOrEmpty(quote.ModifiedBy))
            {
                quote.ModifiedBy = "System";
            }

            return await quoteRepository.CreateQuote(quote);
        }

        public async Task<ResultSet<Quote>> GetQuotes(Pagination<Quote> pagination)
        {
            return await quoteRepository.GetQuotes(pagination);
        }

        public async Task<Quote> GetQuoteByID(Quote quote)
        {
            return await quoteRepository.GetQuoteByID(quote);
        }

        public async Task<Quote> UpdateQuote(Quote quote)
        {
            quote.ModifiedDate = DateTime.UtcNow;
            return await quoteRepository.UpdateQuote(quote);
        }

        public async Task<Quote> DeleteQuote(Quote quote)
        {
            return await quoteRepository.DeleteQuote(quote);
        }

        public async Task<IEnumerable<Quote>> GetQuotesByCustomerID(Customer customer)
        {
            return await quoteRepository.GetQuotesByCustomerID(customer);
        }

        public async Task<Quote> GetQuoteByNumber(string quoteNumber)
        {
            return await quoteRepository.GetQuoteByNumber(quoteNumber);
        }

        #endregion

        #region QuoteItem Methods

        public async Task<QuoteItem> CreateQuoteItem(QuoteItem quoteItem)
        {
            quoteItem.CreatedDate = DateTime.UtcNow;
            quoteItem.ModifiedDate = DateTime.UtcNow;

            // Auto-populate price if not set and equipment is selected
            if (quoteItem.UnitPrice == 0 && !string.IsNullOrEmpty(quoteItem.PieceOfEquipmentID) && quoteItem.PriceTypeId.HasValue)
            {
                await AutoPopulateQuoteItemPrice(quoteItem);
            }

            var result = await quoteRepository.CreateQuoteItem(quoteItem);

            // Update quote total cost after adding item
            var quote = await quoteRepository.GetQuoteByID(new Quote { QuoteID = quoteItem.QuoteID });
            if (quote != null)
            {
                await UpdateQuoteTotalCost(quote);
            }

            return result;
        }

        public async Task<ResultSet<QuoteItem>> GetQuoteItems(Pagination<QuoteItem> pagination)
        {
            return await quoteRepository.GetQuoteItems(pagination);
        }

        public async Task<QuoteItem> GetQuoteItemByID(QuoteItem quoteItem)
        {
            return await quoteRepository.GetQuoteItemByID(quoteItem);
        }

        public async Task<QuoteItem> UpdateQuoteItem(QuoteItem quoteItem)
        {
            quoteItem.ModifiedDate = DateTime.UtcNow;
            var result = await quoteRepository.UpdateQuoteItem(quoteItem);

            // Update quote total cost after updating item
            var quote = await quoteRepository.GetQuoteByID(new Quote { QuoteID = quoteItem.QuoteID });
            if (quote != null)
            {
                await UpdateQuoteTotalCost(quote);
            }

            return result;
        }

        public async Task<QuoteItem> DeleteQuoteItem(QuoteItem quoteItem)
        {
            var result = await quoteRepository.DeleteQuoteItem(quoteItem);

            // Update quote total cost after deleting item
            var quote = await quoteRepository.GetQuoteByID(new Quote { QuoteID = quoteItem.QuoteID });
            if (quote != null)
            {
                await UpdateQuoteTotalCost(quote);
            }

            return result;
        }

        public async Task<IEnumerable<QuoteItem>> GetQuoteItemsByQuoteID(Quote quote)
        {
            return await quoteRepository.GetQuoteItemsByQuoteID(quote);
        }

        public async Task<IEnumerable<QuoteItem>> GetChildQuoteItems(QuoteItem parentQuoteItem)
        {
            return await quoteRepository.GetChildQuoteItems(parentQuoteItem);
        }

        #endregion

        #region Business Logic Methods

        public async Task<IEnumerable<PieceOfEquipment>> GetChildEquipmentForQuote(PieceOfEquipment parentEquipment)
        {
            return await quoteRepository.GetChildEquipmentForQuote(parentEquipment);
        }

        public async Task<decimal> CalculateQuoteTotalCost(Quote quote)
        {
            return await quoteRepository.CalculateQuoteTotalCost(quote);
        }

        public async Task<Quote> UpdateQuoteTotalCost(Quote quote)
        {
            return await quoteRepository.UpdateQuoteTotalCost(quote);
        }

        public async Task<string> GenerateQuoteNumber()
        {
            return await quoteRepository.GenerateQuoteNumber();
        }

        public async Task<Quote> ChangeQuoteStatus(Quote quote, string newStatus)
        {
            return await quoteRepository.ChangeQuoteStatus(quote, newStatus);
        }

        public async Task<string> GenerateQuoteReport(Quote quote)
        {
            return await quoteRepository.GenerateQuoteReport(quote);
        }

        public async Task<byte[]> GenerateQuoteReportPDF(Quote quote)
        {
            return await quoteRepository.GenerateQuoteReportPDF(quote);
        }

        public async Task<bool> DoesPriceTypeRequireTravel(int priceTypeId)
        {
            return await quoteRepository.DoesPriceTypeRequireTravel(priceTypeId);
        }

        public async Task<WorkOrder> AcceptAndScheduleWorkOrder(Quote quote)
        {
            // Get the full quote with items
            var fullQuote = await quoteRepository.GetQuoteByID(quote);
            if (fullQuote == null)
            {
                throw new ExistingException("Quote not found");
            }

            // Get quote items
            var quoteItems = await quoteRepository.GetQuoteItemsByQuoteID(fullQuote);

            // Change quote status to 'Scheduled'
            fullQuote.Status = "Scheduled";
            await quoteRepository.UpdateQuote(fullQuote);

            // Determine scheduled date from quote's estimated delivery or default to current date
            var scheduledDate = fullQuote.EstimatedDelivery ?? DateTime.UtcNow.AddDays(7); // Default to 1 week from now if no estimated delivery

            // Create a new Work Order
            var workOrder = new WorkOrder
            {
                CustomerId = fullQuote.CustomerID ?? 0,
                AddressId = fullQuote.CustomerAddressId ?? 0,
                WorkOrderDate = DateTime.UtcNow,
                ScheduledDate = scheduledDate, // Set scheduled date from quote's estimated delivery
                Description = $"Work Order created from Quote #{fullQuote.QuoteNumber}",
                Name = $"Quote {fullQuote.QuoteNumber}",
                CalibrationType = 1, // Default calibration type
                ContactId = 1, // Default contact
                TenantId = 1, // Default tenant
                StatusID = 1 // Set to "Open" status (1 appears to be the open/active status based on code patterns)
            };

            var createdWorkOrder = await workOrderRepository.InsertWokOrder(workOrder);

            // Create Work Order Details for quote items with equipment
            if (quoteItems != null && quoteItems.Any())
            {
                await CreateWorkOrderDetailsFromQuoteItems(createdWorkOrder, quoteItems);
            }

            return createdWorkOrder;
        }

        private async Task CreateWorkOrderDetailsFromQuoteItems(WorkOrder workOrder, IEnumerable<QuoteItem> quoteItems)
        {
            // Get default status for new Work Order Details
            var defaultStatus = await workOrderDetailUseCase.GetStatus();
            var initialStatus = defaultStatus?.FirstOrDefault(s => s.StatusId == 1); // Assuming 1 is the initial status

            foreach (var quoteItem in quoteItems)
            {
                // Only create Work Order Detail if the quote item has an associated piece of equipment
                if (!string.IsNullOrEmpty(quoteItem.PieceOfEquipmentID) && quoteItem.PieceOfEquipment != null)
                {
                    var workOrderDetail = new WorkOrderDetail
                    {
                        WorkOrderDetailID = NumericExtensions.GetUniqueID(), // Generate unique ID
                        WorkOrderID = workOrder.WorkOrderId,
                        PieceOfEquipmentId = quoteItem.PieceOfEquipmentID,
                        TenantId = workOrder.TenantId,
                        IsAccredited = false, // Default value
                        CurrentStatusID = initialStatus?.StatusId ?? 1,
                        TestPointNumber = 5, // Default value
                        ClassHB44 = 0, // Default value
                        IsComercial = false, // Default value
                        Multiplier = 1, // Default value
                        OfflineStatus = 0, // Default value

                        // Required fields that were missing
                        CalibrationIntervalID = 1, // Default calibration interval
                        HumidityUOMID = 1, // Default humidity unit of measure
                        TemperatureUOMID = 1, // Default temperature unit of measure
                        DecimalNumber = 2, // Default decimal places
                        Resolution = 0.01, // Default resolution
                        Humidity = 0.0, // Default humidity value
                        Temperature = 0.0 // Default temperature value
                    };

                    // Use SaveWod method to properly persist the Work Order Detail
                    // The SaveWod method handles the complete persistence logic
                    await workOrderDetailUseCase.SaveWod(workOrderDetail, "QuoteAcceptance", false);
                }
            }
        }

        /// <summary>
        /// Automatically populates the unit price for a quote item using hierarchical pricing
        /// </summary>
        private async Task AutoPopulateQuoteItemPrice(QuoteItem quoteItem)
        {
            try
            {
                // Get the piece of equipment
                var pieceOfEquipment = await pieceOfEquipmentRepository.GetPieceOfEquipmentByID(quoteItem.PieceOfEquipmentID);
                if (pieceOfEquipment == null)
                {
                    return; // Cannot determine price without equipment
                }

                // Use hierarchical pricing to get the price
                var hierarchicalPrice = await priceTypeRepository.GetPriceForEntity(
                    EntityType.PieceOfEquipment,
                    int.Parse(quoteItem.PieceOfEquipmentID),
                    quoteItem.PriceTypeId.Value);

                // If no piece-specific price, try equipment template
                if (hierarchicalPrice == 0 && pieceOfEquipment.EquipmentTemplateId > 0)
                {
                    hierarchicalPrice = await priceTypeRepository.GetPriceForEntity(
                        EntityType.EquipmentTemplate,
                        pieceOfEquipment.EquipmentTemplateId,
                        quoteItem.PriceTypeId.Value);
                }

                // Set the price if found
                if (hierarchicalPrice > 0)
                {
                    quoteItem.UnitPrice = hierarchicalPrice;
                    quoteItem.ItemDescription = $"Auto-priced using {(hierarchicalPrice > 0 ? "hierarchical pricing" : "manual entry required")}";
                }
            }
            catch (Exception)
            {
                // Log error but don't fail the quote item creation
                // Price will remain 0 and require manual entry
            }
        }

        public static decimal GetCustomerQuoteMultiplier(CalibrationSaaS.Domain.Aggregates.Entities.Customer customer)
        {
            try
            {
                // Return the customer's quote multiplier, defaulting to 1.0 if not set
                return customer.QuoteMultiplier ?? 1.0m;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer quote multiplier: {ex.Message}", ex);
            }
        }

        public static decimal ApplyCustomerMultiplier(decimal basePrice, CalibrationSaaS.Domain.Aggregates.Entities.Customer customer)
        {
            try
            {
                var multiplier = GetCustomerQuoteMultiplier(customer);
                return basePrice * multiplier;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error applying customer multiplier: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
