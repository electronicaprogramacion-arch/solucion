using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.PriceTypeService")]
    public interface IPriceTypeService<T>
    {
        // PriceType CRUD operations
        ValueTask<PriceType> CreatePriceType(PriceType priceType, T context);
        ValueTask<ResultSet<PriceType>> GetPriceTypes(Pagination<PriceType> pagination, T context);
        ValueTask<PriceType> GetPriceTypeByID(PriceType priceType, T context);
        ValueTask<PriceType> UpdatePriceType(PriceType priceType, T context);
        ValueTask<PriceType> DeletePriceType(PriceType priceType, T context);
        ValueTask<PriceTypeCollectionResult> GetActivePriceTypes(T context);

        // PriceTypePrice CRUD operations
        ValueTask<PriceTypePrice> CreatePriceTypePrice(PriceTypePrice priceTypePrice, T context);
        ValueTask<ResultSet<PriceTypePrice>> GetPriceTypePrices(Pagination<PriceTypePrice> pagination, T context);
        ValueTask<PriceTypePrice> GetPriceTypePriceByID(PriceTypePrice priceTypePrice, T context);
        ValueTask<PriceTypePrice> UpdatePriceTypePrice(PriceTypePrice priceTypePrice, T context);
        ValueTask<PriceTypePrice> DeletePriceTypePrice(PriceTypePrice priceTypePrice, T context);
        ValueTask<PriceTypePriceCollectionResult> GetPriceTypePricesByEntity(EntityType entityType, int entityId, T context);

        // Pricing logic methods
        ValueTask<PriceResult> GetHierarchicalPrice(PieceOfEquipment pieceOfEquipment, int priceTypeId, T context);
        ValueTask<PriceResult> GetPriceForEntity(EntityType entityType, int entityId, int priceTypeId, T context);
        ValueTask<PriceTypePriceCollectionResult> GetAllPricesForEntity(EntityType entityType, int entityId, T context);

        // Travel expense logic
        ValueTask<BoolResult> RequiresTravelExpense(List<int> priceTypeIds, T context);
    }

    // Result classes for protobuf-net.Grpc compatibility
    [DataContract]
    public class PriceResult
    {
        [DataMember(Order = 1)]
        public decimal Price { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;
    }

    [DataContract]
    public class BoolResult
    {
        [DataMember(Order = 1)]
        public bool Value { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;
    }

    [DataContract]
    public class IntResult
    {
        [DataMember(Order = 1)]
        public int Value { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;
    }

    [DataContract]
    public class TravelExpenseCalculationResult
    {
        [DataMember(Order = 1)]
        public decimal Value { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; } = string.Empty;
    }

    [DataContract]
    public class PriceTypeCollectionResult
    {
        [DataMember(Order = 1)]
        public List<PriceType> PriceTypes { get; set; } = new List<PriceType>();
    }

    [DataContract]
    public class PriceTypePriceCollectionResult
    {
        [DataMember(Order = 1)]
        public List<PriceTypePrice> PriceTypePrices { get; set; } = new List<PriceTypePrice>();
    }
}
