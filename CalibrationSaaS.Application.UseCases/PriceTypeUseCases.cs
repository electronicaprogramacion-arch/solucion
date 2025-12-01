using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class PriceTypeUseCases
    {
        private readonly IPriceTypeRepository repository;

        public PriceTypeUseCases(IPriceTypeRepository repository)
        {
            this.repository = repository;
        }

        // PriceType CRUD operations
        public async Task<PriceType> CreatePriceType(PriceType priceType)
        {
            if (await ExistsPriceType(priceType))
            {
                throw new InvalidOperationException($"Price type with name '{priceType.Name}' already exists.");
            }

            var result = await repository.CreatePriceType(priceType);
            return result;
        }

        public async Task<ResultSet<PriceType>> GetPriceTypes(Pagination<PriceType> pagination)
        {
            return await repository.GetPriceTypes(pagination);
        }

        public async Task<PriceType> GetPriceTypeByID(int id)
        {
            return await repository.GetPriceTypeByID(id);
        }

        public async Task<PriceType> UpdatePriceType(PriceType priceType)
        {
            var result = await repository.UpdatePriceType(priceType);
            return result;
        }

        public async Task<PriceType> DeletePriceType(PriceType priceType)
        {
            var result = await repository.DeletePriceType(priceType);
            return result;
        }

        public async Task<IEnumerable<PriceType>> GetActivePriceTypes()
        {
            return await repository.GetActivePriceTypes();
        }

        private async Task<bool> ExistsPriceType(PriceType priceType)
        {
            var activePriceTypes = await repository.GetActivePriceTypes();
            return activePriceTypes.Any(pt => pt.Name.Equals(priceType.Name, StringComparison.OrdinalIgnoreCase) && pt.PriceTypeId != priceType.PriceTypeId);
        }

        // PriceTypePrice CRUD operations
        public async Task<PriceTypePrice> CreatePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            var result = await repository.CreatePriceTypePrice(priceTypePrice);
            return result;
        }

        public async Task<ResultSet<PriceTypePrice>> GetPriceTypePrices(Pagination<PriceTypePrice> pagination)
        {
            return await repository.GetPriceTypePrices(pagination);
        }

        public async Task<PriceTypePrice> GetPriceTypePriceByID(int id)
        {
            return await repository.GetPriceTypePriceByID(id);
        }

        public async Task<PriceTypePrice> UpdatePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            var result = await repository.UpdatePriceTypePrice(priceTypePrice);
            return result;
        }

        public async Task<PriceTypePrice> DeletePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            var result = await repository.DeletePriceTypePrice(priceTypePrice);
            return result;
        }

        public async Task<IEnumerable<PriceTypePrice>> GetPriceTypePricesByEntity(EntityType entityType, int entityId)
        {
            return await repository.GetPriceTypePricesByEntity(entityType, entityId);
        }

        // Pricing logic methods
        public async Task<decimal> GetPriceForEntity(EntityType entityType, int entityId, int priceTypeId)
        {
            return await repository.GetPriceForEntity(entityType, entityId, priceTypeId);
        }

        public async Task<IEnumerable<PriceTypePrice>> GetAllPricesForEntity(EntityType entityType, int entityId)
        {
            return await repository.GetAllPricesForEntity(entityType, entityId);
        }

        public async Task<bool> RequiresTravelExpense(List<int> priceTypeIds)
        {
            return await repository.RequiresTravelExpense(priceTypeIds);
        }

        // Hierarchical pricing logic
        public async Task<decimal> GetHierarchicalPrice(PieceOfEquipment pieceOfEquipment, int priceTypeId)
        {
            // First try to get price for the specific piece of equipment
            var piecePrice = await repository.GetPriceForEntity(EntityType.PieceOfEquipment, int.Parse(pieceOfEquipment.PieceOfEquipmentID), priceTypeId);
            if (piecePrice > 0)
            {
                return piecePrice;
            }

            // If no specific price, try to get price for the equipment template
            if (pieceOfEquipment.EquipmentTemplateId != null && pieceOfEquipment.EquipmentTemplateId > 0)
            {
                var templatePrice = await repository.GetPriceForEntity(EntityType.EquipmentTemplate, pieceOfEquipment.EquipmentTemplateId, priceTypeId);
                if (templatePrice > 0)
                {
                    return templatePrice;
                }
            }

            // If no price found, return 0 (manual entry required)
            return 0;
        }

        // Customer address methods for travel expense calculation
        public async Task<CustomerAddress> GetCustomerAddressById(int customerAddressId)
        {
            return await repository.GetCustomerAddressById(customerAddressId);
        }
    }
}
