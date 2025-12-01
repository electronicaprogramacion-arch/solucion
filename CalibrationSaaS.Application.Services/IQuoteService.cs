using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.QuoteService")]
    public interface IQuoteService<T>
    {
        // Quote related methods
        ValueTask<Quote> CreateQuote(Quote quote, T context);
        ValueTask<ResultSet<Quote>> GetQuotes(Pagination<Quote> pagination, T context);
        ValueTask<Quote> GetQuoteByID(Quote quote, T context);
        ValueTask<Quote> UpdateQuote(Quote quote, T context);
        ValueTask<Quote> DeleteQuote(Quote quote, T context);
        ValueTask<QuoteCollectionResult> GetQuotesByCustomerID(Customer customer, T context);
        ValueTask<Quote> GetQuoteByNumber(string quoteNumber, T context);

        // QuoteItem related methods
        ValueTask<QuoteItem> CreateQuoteItem(QuoteItem quoteItem, T context);
        ValueTask<ResultSet<QuoteItem>> GetQuoteItems(Pagination<QuoteItem> pagination, T context);
        ValueTask<QuoteItem> GetQuoteItemByID(QuoteItem quoteItem, T context);
        ValueTask<QuoteItem> UpdateQuoteItem(QuoteItem quoteItem, T context);
        ValueTask<QuoteItem> DeleteQuoteItem(QuoteItem quoteItem, T context);
        ValueTask<QuoteItemCollectionResult> GetQuoteItemsByQuoteID(Quote quote, T context);
        ValueTask<QuoteItemCollectionResult> GetChildQuoteItems(QuoteItem parentQuoteItem, T context);



        // Equipment Template pricing methods
        ValueTask<PriceResult> GetEquipmentTemplatePriceByServiceType(EquipmentTemplate equipmentTemplate, string serviceType, T context);

        // Piece of Equipment pricing methods
        ValueTask<PriceResult> GetPieceOfEquipmentPriceByServiceType(PieceOfEquipment pieceOfEquipment, string serviceType, T context);

        // Hierarchical pricing method (PoE → Equipment Template → Manual Entry)
        ValueTask<PriceResult> GetHierarchicalPrice(PieceOfEquipment pieceOfEquipment, string serviceType, T context);

        // New PriceType-based pricing methods
        ValueTask<PriceResult> GetHierarchicalPriceByPriceType(PieceOfEquipment pieceOfEquipment, int priceTypeId, T context);
        ValueTask<PriceResult> GetEquipmentTemplatePriceByPriceType(EquipmentTemplate equipmentTemplate, int priceTypeId, T context);
        ValueTask<PriceResult> GetPieceOfEquipmentPriceByPriceType(PieceOfEquipment pieceOfEquipment, int priceTypeId, T context);

        // Travel expense methods
        ValueTask<BoolResult> RequiresTravelExpense(Quote quote, T context);
        ValueTask<QuoteItem> CreateTravelExpenseQuoteItem(Quote quote, decimal travelExpense, T context);
        ValueTask<QuoteItem> RemoveTravelExpenseQuoteItem(Quote quote, T context);

        // Business logic methods
        ValueTask<PriceResult> CalculateQuoteTotalCost(Quote quote, T context);
        ValueTask<Quote> UpdateQuoteTotalCost(Quote quote, T context);
        ValueTask<PieceOfEquipmentCollectionResult> GetChildEquipmentForQuote(PieceOfEquipment parentEquipment, T context);
        ValueTask<StringResult> GenerateQuoteNumber(T context);
        ValueTask<Quote> ChangeQuoteStatus(Quote quote, string newStatus, T context);
        ValueTask<WorkOrder> AcceptAndScheduleWorkOrder(Quote quote, T context);
        ValueTask<PriceResult> GetCustomerQuoteMultiplier(Customer customer, T context);
        ValueTask<PriceResult> ApplyCustomerMultiplier(PriceResult basePrice, Customer customer, T context);

        // Report generation methods
        ValueTask<StringResult> GenerateQuoteReport(Quote quote, T context);
        ValueTask<ByteArrayResult> GenerateQuoteReportPDF(Quote quote, T context);
    }

    [DataContract]
    public class StringResult
    {
        [DataMember(Order = 1)]
        public string Value { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;
    }

    [DataContract]
    public class ByteArrayResult
    {
        [DataMember(Order = 1)]
        public byte[] Value { get; set; } = new byte[0];

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;
    }



    [DataContract]
    public class PieceOfEquipmentCollectionResult
    {
        [DataMember(Order = 1)]
        public List<PieceOfEquipment> PieceOfEquipments { get; set; } = new List<PieceOfEquipment>();
    }

    [DataContract]
    public class QuoteCollectionResult
    {
        [DataMember(Order = 1)]
        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }

    [DataContract]
    public class QuoteItemCollectionResult
    {
        [DataMember(Order = 1)]
        public List<QuoteItem> QuoteItems { get; set; } = new List<QuoteItem>();
    }
}
