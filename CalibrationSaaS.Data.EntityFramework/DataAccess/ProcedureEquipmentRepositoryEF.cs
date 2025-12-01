using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Data.EntityFramework;
using Helpers.Controls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class ProcedureEquipmentRepositoryEF<TContext> : IProcedureEquipmentRepository, IDisposable
        where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        private readonly IDbContextFactory<TContext> DbFactory;

        public ProcedureEquipmentRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }

        // ProcedureEquipment CRUD operations
        public async Task<ProcedureEquipment> CreateProcedureEquipment(ProcedureEquipment procedureEquipment)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            procedureEquipment.CreatedDate = DateTime.UtcNow;
            
            context.ProcedureEquipment.Add(procedureEquipment);
            await context.SaveChangesAsync();
            return procedureEquipment;
        }

        public async Task<ResultSet<ProcedureEquipment>> GetProcedureEquipments(Pagination<ProcedureEquipment> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var query = context.ProcedureEquipment
                .Include(pe => pe.Procedure)
                .Include(pe => pe.PieceOfEquipment)
                .AsNoTracking()
                .AsQueryable();

            // Apply filter if provided
            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                query = query.Where(pe => pe.Procedure.Name.Contains(pagination.Filter) ||
                                         pe.PieceOfEquipment.SerialNumber.Contains(pagination.Filter));
            }

            // Apply pagination
            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(pe => pe.Procedure.Name)
                .ThenBy(pe => pe.PieceOfEquipment.SerialNumber)
                .Skip((pagination.Page - 1) * pagination.Show)
                .Take(pagination.Show)
                .ToListAsync();

            return new ResultSet<ProcedureEquipment>
            {
                List = data,
                Count = totalCount,
                CurrentPage = pagination.Page,
                PageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show)
            };
        }

        public async Task<ProcedureEquipment> GetProcedureEquipmentByID(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.ProcedureEquipment
                .Include(pe => pe.Procedure)
                .Include(pe => pe.PieceOfEquipment)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Id == id);
        }

        public async Task<ProcedureEquipment> UpdateProcedureEquipment(ProcedureEquipment procedureEquipment)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            context.ProcedureEquipment.Update(procedureEquipment);
            await context.SaveChangesAsync();
            return procedureEquipment;
        }

        public async Task<ProcedureEquipment> DeleteProcedureEquipment(ProcedureEquipment procedureEquipment)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var entity = await context.ProcedureEquipment.FindAsync(procedureEquipment.Id);
            if (entity != null)
            {
                context.ProcedureEquipment.Remove(entity);
                await context.SaveChangesAsync();
            }
            return procedureEquipment;
        }

        // Association-specific methods
        public async Task<IEnumerable<ProcedureEquipment>> GetProceduresByEquipment(string pieceOfEquipmentId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.ProcedureEquipment
                .Include(pe => pe.Procedure)
                .Include(pe => pe.PieceOfEquipment)
                .AsNoTracking()
                .Where(pe => pe.PieceOfEquipmentID == pieceOfEquipmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProcedureEquipment>> GetEquipmentByProcedure(int procedureId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.ProcedureEquipment
                .Include(pe => pe.Procedure)
                .Include(pe => pe.PieceOfEquipment)
                .AsNoTracking()
                .Where(pe => pe.ProcedureID == procedureId)
                .ToListAsync();
        }

        public async Task<bool> IsProcedureAssociatedWithEquipment(int procedureId, string pieceOfEquipmentId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.ProcedureEquipment
                .AsNoTracking()
                .AnyAsync(pe => pe.ProcedureID == procedureId && pe.PieceOfEquipmentID == pieceOfEquipmentId);
        }

        public async Task<ProcedureEquipment> GetProcedureEquipmentAssociation(int procedureId, string pieceOfEquipmentId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.ProcedureEquipment
                .Include(pe => pe.Procedure)
                .Include(pe => pe.PieceOfEquipment)
                .AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.ProcedureID == procedureId && pe.PieceOfEquipmentID == pieceOfEquipmentId);
        }

        // Bulk operations
        public async Task<IEnumerable<ProcedureEquipment>> CreateMultipleProcedureEquipments(IEnumerable<ProcedureEquipment> procedureEquipments)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            foreach (var pe in procedureEquipments)
            {
                pe.CreatedDate = DateTime.UtcNow;
            }
            
            context.ProcedureEquipment.AddRange(procedureEquipments);
            await context.SaveChangesAsync();
            return procedureEquipments;
        }

        public async Task<bool> DeleteProcedureEquipmentsByEquipment(string pieceOfEquipmentId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var associations = await context.ProcedureEquipment
                .Where(pe => pe.PieceOfEquipmentID == pieceOfEquipmentId)
                .ToListAsync();
            
            if (associations.Any())
            {
                context.ProcedureEquipment.RemoveRange(associations);
                await context.SaveChangesAsync();
            }
            
            return true;
        }

        public async Task<bool> DeleteProcedureEquipmentsByProcedure(int procedureId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var associations = await context.ProcedureEquipment
                .Where(pe => pe.ProcedureID == procedureId)
                .ToListAsync();
            
            if (associations.Any())
            {
                context.ProcedureEquipment.RemoveRange(associations);
                await context.SaveChangesAsync();
            }
            
            return true;
        }

        // Business logic methods
        public async Task<IEnumerable<Procedure>> GetAvailableProceduresForEquipment(string pieceOfEquipmentId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var associatedProcedureIds = await context.ProcedureEquipment
                .Where(pe => pe.PieceOfEquipmentID == pieceOfEquipmentId)
                .Select(pe => pe.ProcedureID)
                .ToListAsync();
            
            return await context.Procedure
                .AsNoTracking()
                .Where(p => !associatedProcedureIds.Contains(p.ProcedureID))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<PieceOfEquipment>> GetAvailableEquipmentForProcedure(int procedureId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var associatedEquipmentIds = await context.ProcedureEquipment
                .Where(pe => pe.ProcedureID == procedureId)
                .Select(pe => pe.PieceOfEquipmentID)
                .ToListAsync();
            
            return await context.PieceOfEquipment
                .AsNoTracking()
                .Where(poe => !associatedEquipmentIds.Contains(poe.PieceOfEquipmentID))
                .OrderBy(poe => poe.SerialNumber)
                .ToListAsync();
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
            // DbContext is disposed automatically with 'await using'
        }
    }
}
