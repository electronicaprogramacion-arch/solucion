using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Data.EntityFramework;
using Helpers.Controls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Controls;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class PriceTypeRepositoryEF<TContext> : IPriceTypeRepository, IDisposable
        where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        private readonly IDbContextFactory<TContext> DbFactory;

        public PriceTypeRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }

        // PriceType CRUD operations
        public async Task<PriceType> CreatePriceType(PriceType priceType)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            priceType.CreatedDate = DateTime.UtcNow;
            priceType.ModifiedDate = DateTime.UtcNow;
            
            context.PriceType.Add(priceType);
            await context.SaveChangesAsync();
            return priceType;
        }

        public async Task<ResultSet<PriceType>> GetPriceTypes(Pagination<PriceType> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var query = context.PriceType.AsNoTracking().AsQueryable();

            // Apply filter if provided
            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                query = query.Where(pt => pt.Name.Contains(pagination.Filter));
            }

            // Apply pagination
            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(pt => pt.Name)
                .Skip((pagination.Page - 1) * pagination.Show)
                .Take(pagination.Show)
                .ToListAsync();

            return new ResultSet<PriceType>
            {
                List = data,
                Count = totalCount,
                CurrentPage = pagination.Page,
                PageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show)
            };
        }

        public async Task<PriceType> GetPriceTypeByID(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.PriceType.AsNoTracking().FirstOrDefaultAsync(pt => pt.PriceTypeId == id);
        }

        public async Task<PriceType> UpdatePriceType(PriceType priceType)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            priceType.ModifiedDate = DateTime.UtcNow;
            
            context.PriceType.Update(priceType);
            await context.SaveChangesAsync();
            return priceType;
        }

        public async Task<PriceType> DeletePriceType(PriceType priceType)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var entity = await context.PriceType.FindAsync(priceType.PriceTypeId);
            if (entity != null)
            {
                // Soft delete by setting IsActive to false
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            return priceType;
        }

        public async Task<IEnumerable<PriceType>> GetActivePriceTypes()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.PriceType
                .AsNoTracking()
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.Name)
                .ToListAsync();
        }

        // PriceTypePrice CRUD operations
        public async Task<PriceTypePrice> CreatePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            priceTypePrice.CreatedDate = DateTime.UtcNow;
            priceTypePrice.ModifiedDate = DateTime.UtcNow;
            
            context.PriceTypePrice.Add(priceTypePrice);
            await context.SaveChangesAsync();
            return priceTypePrice;
        }

        public async Task<ResultSet<PriceTypePrice>> GetPriceTypePrices(Pagination<PriceTypePrice> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var query = context.PriceTypePrice
                .Include(ptp => ptp.PriceType)
                .AsNoTracking()
                .AsQueryable();

            // Apply filter if provided (expecting PriceTypeId as filter)
            if (!string.IsNullOrEmpty(pagination.Filter) && int.TryParse(pagination.Filter, out int priceTypeId))
            {
                query = query.Where(ptp => ptp.PriceTypeId == priceTypeId);
            }

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(ptp => ptp.EntityType)
                .ThenBy(ptp => ptp.EntityId)
                .Skip((pagination.Page - 1) * pagination.Show)
                .Take(pagination.Show)
                .ToListAsync();

            return new ResultSet<PriceTypePrice>
            {
                List = data,
                Count = totalCount,
                CurrentPage = pagination.Page,
                PageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show)
            };
        }

        public async Task<PriceTypePrice> GetPriceTypePriceByID(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.PriceTypePrice
                .Include(ptp => ptp.PriceType)
                .AsNoTracking()
                .FirstOrDefaultAsync(ptp => ptp.Id == id);
        }

        public async Task<PriceTypePrice> UpdatePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            priceTypePrice.ModifiedDate = DateTime.UtcNow;
            
            context.PriceTypePrice.Update(priceTypePrice);
            await context.SaveChangesAsync();
            return priceTypePrice;
        }

        public async Task<PriceTypePrice> DeletePriceTypePrice(PriceTypePrice priceTypePrice)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var entity = await context.PriceTypePrice.FindAsync(priceTypePrice.Id);
            if (entity != null)
            {
                context.PriceTypePrice.Remove(entity);
                await context.SaveChangesAsync();
            }
            return priceTypePrice;
        }

        public async Task<IEnumerable<PriceTypePrice>> GetPriceTypePricesByEntity(EntityType entityType, int entityId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.PriceTypePrice
                .Include(ptp => ptp.PriceType)
                .AsNoTracking()
                .Where(ptp => ptp.EntityType == entityType && ptp.EntityId == entityId)
                .ToListAsync();
        }

        // Pricing logic methods
        public async Task<decimal> GetPriceForEntity(EntityType entityType, int entityId, int priceTypeId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var priceTypePrice = await context.PriceTypePrice
                .AsNoTracking()
                .FirstOrDefaultAsync(ptp => ptp.EntityType == entityType && 
                                          ptp.EntityId == entityId && 
                                          ptp.PriceTypeId == priceTypeId);
            
            return priceTypePrice?.Price ?? 0;
        }

        public async Task<IEnumerable<PriceTypePrice>> GetAllPricesForEntity(EntityType entityType, int entityId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.PriceTypePrice
                .Include(ptp => ptp.PriceType)
                .AsNoTracking()
                .Where(ptp => ptp.EntityType == entityType && ptp.EntityId == entityId)
                .ToListAsync();
        }

        public async Task<bool> RequiresTravelExpense(List<int> priceTypeIds)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.PriceType
                .AsNoTracking()
                .Where(pt => priceTypeIds.Contains(pt.PriceTypeId))
                .AnyAsync(pt => pt.IncludesTravel);
        }

        public async Task<CustomerAddress> GetCustomerAddressById(int customerAddressId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.CustomerAddress
                .AsNoTracking()
                .FirstOrDefaultAsync(ca => ca.CustomerAddressId == customerAddressId);
        }

        public async Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            try
            {
                bool res = (await context.SaveChangesAsync()) > 0;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            // DbFactory is managed by DI container
        }
    }
}
