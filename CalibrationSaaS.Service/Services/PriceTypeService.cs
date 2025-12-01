using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Service.Services
{
    public class PriceTypeService : IPriceTypeService<CallContext>
    {
        private readonly ILogger<PriceTypeService> _logger;
        private readonly PriceTypeUseCases _priceTypeUseCases;

        public PriceTypeService(ILogger<PriceTypeService> logger, PriceTypeUseCases priceTypeUseCases)
        {
            _logger = logger;
            _priceTypeUseCases = priceTypeUseCases;
        }

        // PriceType CRUD operations
        public async ValueTask<PriceType> CreatePriceType(PriceType priceType, CallContext context)
        {
            try
            {
                _logger.LogInformation("Creating price type: {Name}", priceType.Name);
                var result = await _priceTypeUseCases.CreatePriceType(priceType);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating price type");
                throw;
            }
        }

        public async ValueTask<ResultSet<PriceType>> GetPriceTypes(Pagination<PriceType> pagination, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting price types with pagination");
                var result = await _priceTypeUseCases.GetPriceTypes(pagination);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting price types");
                return new ResultSet<PriceType>
                {
                    List = new List<PriceType>(),
                    Count = 0,
                    CurrentPage = 1,
                    PageTotal = 0
                };
            }
        }

        public async ValueTask<PriceType> GetPriceTypeByID(PriceType priceType, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting price type by ID: {PriceTypeId}", priceType.PriceTypeId);
                var result = await _priceTypeUseCases.GetPriceTypeByID(priceType.PriceTypeId);
                return result ?? priceType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting price type by ID");
                throw;
            }
        }

        public async ValueTask<PriceType> UpdatePriceType(PriceType priceType, CallContext context)
        {
            try
            {
                _logger.LogInformation("Updating price type: {PriceTypeId}", priceType.PriceTypeId);
                var result = await _priceTypeUseCases.UpdatePriceType(priceType);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating price type");
                throw;
            }
        }

        public async ValueTask<PriceType> DeletePriceType(PriceType priceType, CallContext context)
        {
            try
            {
                _logger.LogInformation("Deleting price type: {PriceTypeId}", priceType.PriceTypeId);
                var result = await _priceTypeUseCases.DeletePriceType(priceType);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting price type");
                throw;
            }
        }

        public async ValueTask<PriceTypeCollectionResult> GetActivePriceTypes(CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting active price types");
                var activePriceTypes = await _priceTypeUseCases.GetActivePriceTypes();
                var priceTypesList = activePriceTypes.ToList();

                return new PriceTypeCollectionResult
                {
                    PriceTypes = priceTypesList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active price types");
                return new PriceTypeCollectionResult
                {
                    PriceTypes = new List<PriceType>()
                };
            }
        }

        // PriceTypePrice CRUD operations
        public async ValueTask<PriceTypePrice> CreatePriceTypePrice(PriceTypePrice priceTypePrice, CallContext context)
        {
            try
            {
                _logger.LogInformation("Creating price type price for PriceTypeId: {PriceTypeId}, EntityType: {EntityType}, EntityId: {EntityId}",
                    priceTypePrice.PriceTypeId, priceTypePrice.EntityType, priceTypePrice.EntityId);
                var result = await _priceTypeUseCases.CreatePriceTypePrice(priceTypePrice);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating price type price");
                throw;
            }
        }

        public async ValueTask<ResultSet<PriceTypePrice>> GetPriceTypePrices(Pagination<PriceTypePrice> pagination, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting price type prices");
                var result = await _priceTypeUseCases.GetPriceTypePrices(pagination);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting price type prices");
                throw;
            }
        }

        public async ValueTask<PriceTypePrice> GetPriceTypePriceByID(PriceTypePrice priceTypePrice, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting price type price by ID: {Id}", priceTypePrice.Id);
                var result = await _priceTypeUseCases.GetPriceTypePriceByID(priceTypePrice.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting price type price by ID");
                throw;
            }
        }

        public async ValueTask<PriceTypePrice> UpdatePriceTypePrice(PriceTypePrice priceTypePrice, CallContext context)
        {
            try
            {
                _logger.LogInformation("Updating price type price ID: {Id}", priceTypePrice.Id);
                var result = await _priceTypeUseCases.UpdatePriceTypePrice(priceTypePrice);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating price type price");
                throw;
            }
        }

        public async ValueTask<PriceTypePrice> DeletePriceTypePrice(PriceTypePrice priceTypePrice, CallContext context)
        {
            try
            {
                _logger.LogInformation("Deleting price type price ID: {Id}", priceTypePrice.Id);
                var result = await _priceTypeUseCases.DeletePriceTypePrice(priceTypePrice);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting price type price");
                throw;
            }
        }

        public async ValueTask<PriceTypePriceCollectionResult> GetPriceTypePricesByEntity(EntityType entityType, int entityId, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting price type prices by entity: {EntityType} {EntityId}", entityType, entityId);
                var result = await _priceTypeUseCases.GetPriceTypePricesByEntity(entityType, entityId);
                return new PriceTypePriceCollectionResult { PriceTypePrices = result.ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting price type prices by entity");
                throw;
            }
        }

        // Pricing logic methods
        public async ValueTask<PriceResult> GetHierarchicalPrice(PieceOfEquipment pieceOfEquipment, int priceTypeId, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting hierarchical price for equipment {EquipmentId}, price type {PriceTypeId}",
                    pieceOfEquipment?.PieceOfEquipmentID, priceTypeId);
                var result = await _priceTypeUseCases.GetHierarchicalPrice(pieceOfEquipment, priceTypeId);
                return new PriceResult { Price = result, Description = "Price retrieved successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting hierarchical price");
                return new PriceResult { Price = 0, Description = "Error retrieving price - manual entry required" };
            }
        }

        public async ValueTask<PriceResult> GetPriceForEntity(EntityType entityType, int entityId, int priceTypeId, CallContext context)
        {
            try
            {
                _logger.LogInformation("Getting price for entity {EntityType} {EntityId}, price type {PriceTypeId}",
                    entityType, entityId, priceTypeId);
                var result = await _priceTypeUseCases.GetPriceForEntity(entityType, entityId, priceTypeId);
                return new PriceResult { Price = result, Description = "Price retrieved successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting price for entity");
                return new PriceResult { Price = 0, Description = "Error retrieving price - manual entry required" };
            }
        }

        public async ValueTask<PriceTypePriceCollectionResult> GetAllPricesForEntity(EntityType entityType, int entityId, CallContext context)
        {
            _logger.LogInformation("Getting all prices for entity");
            return new PriceTypePriceCollectionResult();
        }

        // Travel expense logic
        public async ValueTask<BoolResult> RequiresTravelExpense(List<int> priceTypeIds, CallContext context)
        {
            try
            {
                _logger.LogInformation("Checking travel expense requirement for price types: {PriceTypeIds}",
                    string.Join(", ", priceTypeIds));
                var result = await _priceTypeUseCases.RequiresTravelExpense(priceTypeIds);
                return new BoolResult { Value = result, Description = "Travel expense requirement checked successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking travel expense requirement");
                return new BoolResult { Value = false, Description = "Error checking travel expense requirement" };
            }
        }
    }
}
