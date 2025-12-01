using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IQuoteRepository
    {
        #region Quote Methods
        Task<Quote> CreateQuote(Quote quote);
        Task<ResultSet<Quote>> GetQuotes(Pagination<Quote> pagination);
        Task<Quote> GetQuoteByID(Quote quote);
        Task<Quote> UpdateQuote(Quote quote);
        Task<Quote> DeleteQuote(Quote quote);
        Task<IEnumerable<Quote>> GetQuotesByCustomerID(Customer customer);
        Task<Quote> GetQuoteByNumber(string quoteNumber);
        Task<Quote> GetLastQuote();
        Task<bool> Save();
        #endregion

        #region QuoteItem Methods
        Task<QuoteItem> CreateQuoteItem(QuoteItem quoteItem);
        Task<ResultSet<QuoteItem>> GetQuoteItems(Pagination<QuoteItem> pagination);
        Task<QuoteItem> GetQuoteItemByID(QuoteItem quoteItem);
        Task<QuoteItem> UpdateQuoteItem(QuoteItem quoteItem);
        Task<QuoteItem> DeleteQuoteItem(QuoteItem quoteItem);
        Task<IEnumerable<QuoteItem>> GetQuoteItemsByQuoteID(Quote quote);
        Task<IEnumerable<QuoteItem>> GetChildQuoteItems(QuoteItem parentQuoteItem);
        #endregion



        #region Business Logic Methods
        Task<IEnumerable<PieceOfEquipment>> GetChildEquipmentForQuote(PieceOfEquipment parentEquipment);
        Task<decimal> CalculateQuoteTotalCost(Quote quote);
        Task<Quote> UpdateQuoteTotalCost(Quote quote);
        Task<string> GenerateQuoteNumber();
        Task<Quote> ChangeQuoteStatus(Quote quote, string newStatus);
        Task<string> GenerateQuoteReport(Quote quote);
        Task<byte[]> GenerateQuoteReportPDF(Quote quote);
        Task<bool> DoesPriceTypeRequireTravel(int priceTypeId);

        #endregion
    }
}
