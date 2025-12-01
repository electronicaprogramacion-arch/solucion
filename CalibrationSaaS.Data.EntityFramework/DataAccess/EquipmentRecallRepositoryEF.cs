using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using CalibrationSaaS.Data.EntityFramework;
using Helpers.Controls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class EquipmentRecallRepositoryEF<TContext> : IEquipmentRecallRepository, IDisposable 
        where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        private readonly IDbContextFactory<TContext> DbFactory;

        public EquipmentRecallRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }

        public async Task<IEnumerable<EquipmentRecall>> GetEquipmentRecalls(EquipmentRecallFilter filter)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var query = context.PieceOfEquipment
                .AsNoTracking()
                .Include(p => p.Customer)
                .Include(p => p.EquipmentTemplate)
                .AsQueryable();

            // Apply filters
            if (filter.CustomerID.HasValue)
            {
                query = query.Where(p => p.CustomerId == filter.CustomerID.Value);
            }

            if (filter.InitialDueDate.HasValue)
            {
                query = query.Where(p => p.DueDate >= filter.InitialDueDate.Value);
            }

            if (filter.FinalDueDate.HasValue)
            {
                query = query.Where(p => p.DueDate <= filter.FinalDueDate.Value);
            }

            if (!filter.IncludeOverdue)
            {
                query = query.Where(p => p.DueDate >= DateTime.Today);
            }

            // Filter by selected equipment IDs if provided (for Excel export)
            if (!string.IsNullOrEmpty(filter.SelectedEquipmentIdsString))
            {
                var selectedIds = filter.SelectedEquipmentIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(p => selectedIds.Contains(p.PieceOfEquipmentID));
            }

            var results = await query
                .OrderBy(p => p.DueDate)
                .Select(p => new EquipmentRecall
                {
                    PieceOfEquipmentID = p.PieceOfEquipmentID,
                    SerialNumber = p.SerialNumber,
                    CalibrationDate = p.CalibrationDate,
                    DueDate = p.DueDate,
                    CustomerName = p.Customer.Name,
                    CustomerID = p.CustomerId,
                    Description = p.EquipmentTemplate.Name ?? "",
                    // Excel-only columns remain empty for grid display
                    Technician = "",
                    ReportNumber = "",
                    WorkOrderNumber = "",
                    PONumber = "",
                    QuoteNumber = "",
                    PTNumber = ""
                })
                .ToListAsync();

            return results;
        }

        public async Task<ResultSet<EquipmentRecall>> GetEquipmentRecallsPaginated(Pagination<EquipmentRecall> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var query = context.PieceOfEquipment
                .AsNoTracking()
                .Include(p => p.Customer)
                .Include(p => p.EquipmentTemplate)
                .AsQueryable();

            // Apply filters from pagination entity if available
            if (pagination.Entity != null)
            {
                var filter = new EquipmentRecallFilter
                {
                    CustomerID = pagination.Entity.CustomerID > 0 ? pagination.Entity.CustomerID : null,
                    InitialDueDate = pagination.Entity.DueDate != DateTime.MinValue ? pagination.Entity.DueDate : null,
                    FinalDueDate = pagination.Entity.DueDate != DateTime.MinValue ? pagination.Entity.DueDate : null
                };

                if (filter.CustomerID.HasValue)
                {
                    query = query.Where(p => p.CustomerId == filter.CustomerID.Value);
                }

                if (filter.InitialDueDate.HasValue)
                {
                    query = query.Where(p => p.DueDate >= filter.InitialDueDate.Value);
                }

                if (filter.FinalDueDate.HasValue)
                {
                    query = query.Where(p => p.DueDate <= filter.FinalDueDate.Value);
                }
            }

            var totalCount = await query.CountAsync();

            var results = await query
                .OrderBy(p => p.DueDate)
                .Skip((pagination.Page - 1) * pagination.Show)
                .Take(pagination.Show)
                .Select(p => new EquipmentRecall
                {
                    PieceOfEquipmentID = p.PieceOfEquipmentID,
                    SerialNumber = p.SerialNumber,
                    CalibrationDate = p.CalibrationDate,
                    DueDate = p.DueDate,
                    CustomerName = p.Customer.Name,
                    CustomerID = p.CustomerId,
                    Description = p.EquipmentTemplate.Name ?? "",
                    // Excel-only columns remain empty for grid display
                    Technician = "",
                    ReportNumber = "",
                    WorkOrderNumber = "",
                    PONumber = "",
                    QuoteNumber = "",
                    PTNumber = ""
                })
                .ToListAsync();

            return new ResultSet<EquipmentRecall>
            {
                List = results,
                Count = totalCount,
                CurrentPage = pagination.Page,
                PageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show)
            };
        }

        public async Task<int> GetEquipmentRecallsCount(EquipmentRecallFilter filter)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var query = context.PieceOfEquipment.AsNoTracking().AsQueryable();

            // Apply filters
            if (filter.CustomerID.HasValue)
            {
                query = query.Where(p => p.CustomerId == filter.CustomerID.Value);
            }

            if (filter.InitialDueDate.HasValue)
            {
                query = query.Where(p => p.DueDate >= filter.InitialDueDate.Value);
            }

            if (filter.FinalDueDate.HasValue)
            {
                query = query.Where(p => p.DueDate <= filter.FinalDueDate.Value);
            }

            if (!filter.IncludeOverdue)
            {
                query = query.Where(p => p.DueDate >= DateTime.Today);
            }

            // Filter by selected equipment IDs if provided (for Excel export)
            if (!string.IsNullOrEmpty(filter.SelectedEquipmentIdsString))
            {
                var selectedIds = filter.SelectedEquipmentIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(p => selectedIds.Contains(p.PieceOfEquipmentID));
            }

            return await query.CountAsync();
        }

        public async Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return (await context.SaveChangesAsync()) > 0;
        }

        public void Dispose()
        {
            // DbFactory is injected and managed by DI container
        }
    }
}
