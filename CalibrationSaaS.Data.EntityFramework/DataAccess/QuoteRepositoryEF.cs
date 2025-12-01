using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using Helpers;
using Helpers.Controls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class QuoteRepositoryEF<TContext> : IQuoteRepository, IDisposable where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        private readonly IDbContextFactory<TContext> DbFactory;

        public QuoteRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }

        #region Quote Methods

        public async Task<Quote> CreateQuote(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Check the database provider to determine ID handling strategy
            var providerName = context.Database.ProviderName;
            var isSqlite = providerName?.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) == true;

            if (isSqlite)
            {
                // For SQLite (offline mode), use GetUniqueID
                quote.QuoteID = NumericExtensions.GetUniqueID(quote.QuoteID);
            }
            else
            {
                // For SQL Server (online mode), ensure QuoteID is 0 so EF treats it as a new entity
                // This prevents conflicts with identity columns
                quote.QuoteID = 0;
            }

            // Don't override CreatedDate and ModifiedDate here - they should be set in the use case layer
            // Only set them if they are not already set
            if (quote.CreatedDate == default(DateTime))
            {
                quote.CreatedDate = DateTime.UtcNow;
            }
            if (quote.ModifiedDate == default(DateTime))
            {
                quote.ModifiedDate = DateTime.UtcNow;
            }

            // Ensure no navigation properties are attached to avoid tracking issues
            quote.Customer = null;
            quote.QuoteItems = new HashSet<QuoteItem>();

            // Explicitly set the entity state to Added to ensure EF treats it as a new entity
            var entry = context.Quote.Add(quote);
            entry.State = EntityState.Added;

            await context.SaveChangesAsync();

            // Reload the quote with Customer navigation property included
            var savedQuote = await context.Quote
                .Include(x => x.Customer)
                .Where(x => x.QuoteID == quote.QuoteID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return savedQuote ?? quote;
        }

        public async Task<ResultSet<Quote>> GetQuotes(Pagination<Quote> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Simple filter implementation - can be enhanced later
            Expression<Func<Quote, bool>> filterQuery = null;
            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                filterQuery = x => x.QuoteNumber.Contains(pagination.Filter) ||
                                  x.Customer.Name.Contains(pagination.Filter) ||
                                  x.Status.Contains(pagination.Filter);
            }

            var queryable = context.Quote.Include(x => x.Customer).AsQueryable();
            var simpleQuery = context.Quote;

            var result = await queryable.PaginationAndFilterQuery<Quote>(pagination, simpleQuery, filterQuery);
            return result;
        }

        public async Task<Quote> GetQuoteByID(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.Quote
                .Include(x => x.Customer)
                .ThenInclude(x => x.Aggregates)
                .ThenInclude(x => x.Addresses)
                .Include(x => x.Customer)
                .ThenInclude(x => x.Aggregates)
                .ThenInclude(x => x.Contacts)
                .Include(x => x.Customer)
                .ThenInclude(x => x.Aggregates)
                .ThenInclude(x => x.PhoneNumbers)
                .Include(x => x.Customer)
                .ThenInclude(x => x.Aggregates)
                .ThenInclude(x => x.EmailAddresses)
                .Include(x => x.QuoteItems)
                .ThenInclude(x => x.PieceOfEquipment)
                .ThenInclude(x => x.EquipmentTemplate)
                .ThenInclude(x => x.Manufacturer1)
                .Include(x => x.QuoteItems)
                .ThenInclude(x => x.PieceOfEquipment)
                .ThenInclude(x => x.EquipmentTemplate)
                .ThenInclude(x => x.EquipmentTypeObject)
                .Where(x => x.QuoteID == quote.QuoteID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Quote> UpdateQuote(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Fetch the existing quote to ensure we have all required fields
            var existingQuote = await context.Quote
                .Where(x => x.QuoteID == quote.QuoteID)
                .FirstOrDefaultAsync();

            if (existingQuote != null)
            {
                // Update only the fields that should be changed
                existingQuote.QuoteNumber = quote.QuoteNumber;
                existingQuote.CustomerID = quote.CustomerID;
                existingQuote.TotalCost = quote.TotalCost;
                existingQuote.Status = quote.Status;
                existingQuote.Priority = quote.Priority;
                existingQuote.ServiceType = quote.ServiceType;
                existingQuote.EstimatedDelivery = quote.EstimatedDelivery;
                existingQuote.Notes = quote.Notes;
                existingQuote.IsActive = quote.IsActive;

                // Set ModifiedDate and ModifiedBy
                existingQuote.ModifiedDate = quote.ModifiedDate != default(DateTime) ? quote.ModifiedDate : DateTime.UtcNow;
                if (!string.IsNullOrEmpty(quote.ModifiedBy))
                {
                    existingQuote.ModifiedBy = quote.ModifiedBy;
                }
                else if (string.IsNullOrEmpty(existingQuote.ModifiedBy))
                {
                    existingQuote.ModifiedBy = "System";
                }

                // Ensure CreatedBy and CreatedDate are never overwritten
                // These should remain as they were when the quote was first created

                await context.SaveChangesAsync();
                return existingQuote;
            }

            return quote;
        }

        public async Task<Quote> DeleteQuote(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            context.Entry(quote).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            
            return quote;
        }

        public async Task<IEnumerable<Quote>> GetQuotesByCustomerID(Customer customer)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var result = await context.Quote
                .Include(x => x.Customer)
                .Where(x => x.CustomerID == customer.CustomerID)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<Quote> GetQuoteByNumber(string quoteNumber)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.Quote
                .Include(x => x.Customer)
                .Include(x => x.QuoteItems)
                .ThenInclude(x => x.PieceOfEquipment)
                .Where(x => x.QuoteNumber == quoteNumber)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Quote> GetLastQuote()
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.Quote
                .Include(x => x.Customer)
                .OrderByDescending(x => x.QuoteID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region QuoteItem Methods

        public async Task<QuoteItem> CreateQuoteItem(QuoteItem quoteItem)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Don't set QuoteItemID for SQL Server contexts - let the identity column generate it
            // Only use GetUniqueID for SQLite contexts (offline mode)
            if (context.Database.ProviderName?.Contains("Sqlite") == true)
            {
                quoteItem.QuoteItemID = NumericExtensions.GetUniqueID(quoteItem.QuoteItemID);
            }
            else
            {
                // For SQL Server, ensure QuoteItemID is 0 so EF treats it as a new entity
                quoteItem.QuoteItemID = 0;
            }

            quoteItem.CreatedDate = DateTime.UtcNow;
            quoteItem.ModifiedDate = DateTime.UtcNow;

            // Clear navigation properties to prevent EF tracking issues
            quoteItem.Quote = null;
            quoteItem.PieceOfEquipment = null;
            quoteItem.ParentQuoteItem = null;
            quoteItem.ChildItems = new HashSet<QuoteItem>();

            // Explicitly set the entity state to Added to ensure EF treats it as a new entity
            var entry = context.QuoteItem.Add(quoteItem);
            entry.State = EntityState.Added;

            await context.SaveChangesAsync();

            return quoteItem;
        }

        public async Task<ResultSet<QuoteItem>> GetQuoteItems(Pagination<QuoteItem> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Simple filter implementation - can be enhanced later
            Expression<Func<QuoteItem, bool>> filterQuery = null;
            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                filterQuery = x => x.ItemDescription.Contains(pagination.Filter) ||
                                  x.Quote.QuoteNumber.Contains(pagination.Filter) ||
                                  x.PieceOfEquipment.SerialNumber.Contains(pagination.Filter);
            }

            var queryable = context.QuoteItem
                .Include(x => x.Quote)
                .Include(x => x.PieceOfEquipment)
                .AsQueryable();
            var simpleQuery = context.QuoteItem;

            var result = await queryable.PaginationAndFilterQuery<QuoteItem>(pagination, simpleQuery, filterQuery);
            return result;
        }

        public async Task<QuoteItem> GetQuoteItemByID(QuoteItem quoteItem)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.QuoteItem
                .Include(x => x.Quote)
                .Include(x => x.PieceOfEquipment)
                .Include(x => x.ChildItems)
                .Where(x => x.QuoteItemID == quoteItem.QuoteItemID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<QuoteItem> UpdateQuoteItem(QuoteItem quoteItem)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            quoteItem.ModifiedDate = DateTime.UtcNow;
            context.QuoteItem.Update(quoteItem);
            await context.SaveChangesAsync();

            return quoteItem;
        }

        public async Task<QuoteItem> DeleteQuoteItem(QuoteItem quoteItem)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            context.Entry(quoteItem).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return quoteItem;
        }

        public async Task<IEnumerable<QuoteItem>> GetQuoteItemsByQuoteID(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.QuoteItem
                .Include(x => x.PieceOfEquipment)
                .ThenInclude(x => x.EquipmentTemplate)
                .Where(x => x.QuoteID == quote.QuoteID)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<QuoteItem>> GetChildQuoteItems(QuoteItem parentQuoteItem)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.QuoteItem
                .Include(x => x.PieceOfEquipment)
                .Where(x => x.ParentQuoteItemID == parentQuoteItem.QuoteItemID)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        #endregion

        #region Business Logic Methods

        public async Task<decimal> CalculateQuoteTotalCost(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var quoteItems = await context.QuoteItem
                .Where(x => x.QuoteID == quote.QuoteID)
                .AsNoTracking()
                .ToListAsync();

            decimal totalCost = 0;
            foreach (var item in quoteItems)
            {
                totalCost += item.UnitPrice * item.Quantity;
            }

            return totalCost;
        }

        public async Task<Quote> UpdateQuoteTotalCost(Quote quote)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var totalCost = await CalculateQuoteTotalCost(quote);

            var existingQuote = await context.Quote
                .Where(x => x.QuoteID == quote.QuoteID)
                .FirstOrDefaultAsync();

            if (existingQuote != null)
            {
                // Only update the specific fields we want to change
                existingQuote.TotalCost = totalCost;
                existingQuote.ModifiedDate = DateTime.UtcNow;

                // Ensure ModifiedBy is set if it's null or empty
                if (string.IsNullOrEmpty(existingQuote.ModifiedBy))
                {
                    existingQuote.ModifiedBy = "System";
                }

                // Ensure CreatedBy is set if it's null or empty (should not happen, but safety check)
                if (string.IsNullOrEmpty(existingQuote.CreatedBy))
                {
                    existingQuote.CreatedBy = "System";
                }

                // Use Entry to only mark specific properties as modified
                context.Entry(existingQuote).Property(x => x.TotalCost).IsModified = true;
                context.Entry(existingQuote).Property(x => x.ModifiedDate).IsModified = true;
                context.Entry(existingQuote).Property(x => x.ModifiedBy).IsModified = true;

                // Only mark CreatedBy as modified if it was actually changed
                if (string.IsNullOrEmpty(existingQuote.CreatedBy))
                {
                    context.Entry(existingQuote).Property(x => x.CreatedBy).IsModified = true;
                }

                await context.SaveChangesAsync();

                return existingQuote;
            }

            return quote;
        }

        public async Task<IEnumerable<PieceOfEquipment>> GetChildEquipmentForQuote(PieceOfEquipment parentEquipment)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.PieceOfEquipment
                .Include(x => x.EquipmentTemplate)
                .Where(x => x.ParentID == parentEquipment.PieceOfEquipmentID.ToString())
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<string> GenerateQuoteNumber()
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var lastQuote = await context.Quote
                .OrderByDescending(x => x.QuoteID)
                .FirstOrDefaultAsync();

            var nextNumber = 1;
            if (lastQuote != null && !string.IsNullOrEmpty(lastQuote.QuoteNumber))
            {
                // Extract number from quote number (assuming format like "Q-001", "Q-002", etc.)
                var numberPart = lastQuote.QuoteNumber.Split('-').LastOrDefault();
                if (int.TryParse(numberPart, out var currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
            }

            return $"Q-{nextNumber:D3}";
        }

        public async Task<Quote> ChangeQuoteStatus(Quote quote, string newStatus)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var existingQuote = await context.Quote
                .Where(x => x.QuoteID == quote.QuoteID)
                .FirstOrDefaultAsync();

            if (existingQuote != null)
            {
                existingQuote.Status = newStatus;
                existingQuote.ModifiedDate = DateTime.UtcNow;
                existingQuote.ModifiedBy = "System"; // Set ModifiedBy when changing status

                context.Quote.Update(existingQuote);
                await context.SaveChangesAsync();

                return existingQuote;
            }

            return quote;
        }

        public async Task<string> GenerateQuoteReport(Quote quote)
        {
            // This would typically generate an HTML or other text-based report
            // For now, return a simple placeholder
            await Task.CompletedTask;
            return $"Quote Report for Quote #{quote.QuoteNumber} - Generated on {DateTime.UtcNow}";
        }

        public async Task<byte[]> GenerateQuoteReportPDF(Quote quote)
        {
            // This would typically use a PDF generation library
            // For now, return empty byte array as placeholder
            await Task.CompletedTask;
            return new byte[0];
        }

        public async Task<bool> DoesPriceTypeRequireTravel(int priceTypeId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var priceType = await context.PriceType
                .Where(pt => pt.PriceTypeId == priceTypeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return priceType?.RequiresTravel ?? false;
        }

        #endregion

        public void Dispose()
        {
            // Dispose resources if needed
        }
    }
}
