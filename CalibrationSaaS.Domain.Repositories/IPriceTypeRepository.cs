using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IPriceTypeRepository
    {
        // PriceType CRUD operations
        Task<PriceType> CreatePriceType(PriceType priceType);
        Task<ResultSet<PriceType>> GetPriceTypes(Pagination<PriceType> pagination);
        Task<PriceType> GetPriceTypeByID(int id);
        Task<PriceType> UpdatePriceType(PriceType priceType);
        Task<PriceType> DeletePriceType(PriceType priceType);
        Task<IEnumerable<PriceType>> GetActivePriceTypes();
        Task<bool> Save();

        // PriceTypePrice CRUD operations
        Task<PriceTypePrice> CreatePriceTypePrice(PriceTypePrice priceTypePrice);
        Task<ResultSet<PriceTypePrice>> GetPriceTypePrices(Pagination<PriceTypePrice> pagination);
        Task<PriceTypePrice> GetPriceTypePriceByID(int id);
        Task<PriceTypePrice> UpdatePriceTypePrice(PriceTypePrice priceTypePrice);
        Task<PriceTypePrice> DeletePriceTypePrice(PriceTypePrice priceTypePrice);
        Task<IEnumerable<PriceTypePrice>> GetPriceTypePricesByEntity(EntityType entityType, int entityId);

        // Pricing logic methods
        Task<decimal> GetPriceForEntity(EntityType entityType, int entityId, int priceTypeId);
        Task<IEnumerable<PriceTypePrice>> GetAllPricesForEntity(EntityType entityType, int entityId);
        Task<bool> RequiresTravelExpense(List<int> priceTypeIds);

        // Customer address methods for travel expense calculation
        Task<CustomerAddress> GetCustomerAddressById(int customerAddressId);
    }
}
