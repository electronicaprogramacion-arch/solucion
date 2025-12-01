using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Data.EntityFramework;
using Helpers.Controls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    /// <summary>
    /// Entity Framework implementation of IAuditLogRepository
    /// </summary>
    /// <typeparam name="TContext">DbContext type</typeparam>
    public class AuditLogRepositoryEF<TContext> : IAuditLogRepository, IDisposable 
        where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        private readonly IDbContextFactory<TContext> _dbFactory;
        private readonly ILogger<AuditLogRepositoryEF<TContext>> _logger;

        public AuditLogRepositoryEF(IDbContextFactory<TContext> dbFactory, ILogger<AuditLogRepositoryEF<TContext>> logger)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get audit logs with pagination
        /// </summary>
        public async Task<ResultSet<AuditLog>> GetAuditLogs(Pagination<AuditLog> pagination)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                var query = context.AuditLogs.AsQueryable();
                
                // Apply sorting
                query = ApplySorting(query, pagination.ColumnName, pagination.SortingAscending);
                
                // Get total count
                var totalCount = await query.CountAsync();
                
                // Apply pagination
                var auditLogs = await query
                    .Skip((pagination.Page - 1) * pagination.Show)
                    .Take(pagination.Show)
                    .ToListAsync();

                var pageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show);
                
                return new ResultSet<AuditLog>
                {
                    List = auditLogs,
                    Count = totalCount,
                    PageTotal = pageTotal,
                    CurrentPage = pagination.Page,
                    Message = "Success",
                    ClientPagination = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs");
                throw;
            }
        }

        /// <summary>
        /// Search audit logs with multiple filters
        /// </summary>
        public async Task<ResultSet<AuditLog>> SearchAuditLogs(string entityType, string entityId, DateTime? fromDate, DateTime? toDate, string userName, string actionType, int page, int pageSize, string sortColumn, bool sortDescending, string clientIpAddress = null)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                var query = context.AuditLogs.AsQueryable();
                
                // Apply filters
                if (!string.IsNullOrEmpty(entityType))
                {
                    query = query.Where(a => a.EntityType == entityType);
                }

                if (!string.IsNullOrEmpty(entityId))
                {
                    query = query.Where(a => a.EntityId == entityId);
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    query = query.Where(a => a.UserName.Contains(userName));
                }

                if (!string.IsNullOrEmpty(actionType))
                {
                    query = query.Where(a => a.ActionType == actionType);
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp <= toDate.Value);
                }

                // Apply extended audit filters
                if (!string.IsNullOrEmpty(clientIpAddress))
                {
                    query = query.Where(a => a.ClientIpAddress.Contains(clientIpAddress));
                }

                // Apply sorting
                query = ApplySorting(query, sortColumn, !sortDescending);
                
                // Get total count
                var totalCount = await query.CountAsync();
                
                // Apply pagination
                var auditLogs = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pageTotal = (int)Math.Ceiling((double)totalCount / pageSize);

                return new ResultSet<AuditLog>
                {
                    List = auditLogs,
                    Count = totalCount,
                    PageTotal = pageTotal,
                    CurrentPage = page,
                    Message = "Success",
                    ClientPagination = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching audit logs");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by entity type
        /// </summary>
        public async Task<ResultSet<AuditLog>> GetAuditLogsByEntityType(string entityType, Pagination<AuditLog> pagination)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                var query = context.AuditLogs
                    .Where(a => a.EntityType == entityType);
                
                query = ApplySorting(query, pagination.ColumnName, pagination.SortingAscending);
                
                var totalCount = await query.CountAsync();
                
                var auditLogs = await query
                    .Skip((pagination.Page - 1) * pagination.Show)
                    .Take(pagination.Show)
                    .ToListAsync();

                var pageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show);
                
                return new ResultSet<AuditLog>
                {
                    List = auditLogs,
                    Count = totalCount,
                    PageTotal = pageTotal,
                    CurrentPage = pagination.Page,
                    Message = "Success",
                    ClientPagination = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs by entity type");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by entity ID
        /// </summary>
        public async Task<ResultSet<AuditLog>> GetAuditLogsByEntityId(string entityId, Pagination<AuditLog> pagination)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                var query = context.AuditLogs
                    .Where(a => a.EntityId == entityId);
                
                query = ApplySorting(query, pagination.ColumnName, pagination.SortingAscending);
                
                var totalCount = await query.CountAsync();
                
                var auditLogs = await query
                    .Skip((pagination.Page - 1) * pagination.Show)
                    .Take(pagination.Show)
                    .ToListAsync();

                var pageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show);
                
                return new ResultSet<AuditLog>
                {
                    List = auditLogs,
                    Count = totalCount,
                    PageTotal = pageTotal,
                    CurrentPage = pagination.Page,
                    Message = "Success",
                    ClientPagination = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs by entity ID");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by date range
        /// </summary>
        public async Task<ResultSet<AuditLog>> GetAuditLogsByDateRange(DateTime? fromDate, DateTime? toDate, Pagination<AuditLog> pagination)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                var query = context.AuditLogs.AsQueryable();
                
                if (fromDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp >= fromDate.Value);
                }
                
                if (toDate.HasValue)
                {
                    query = query.Where(a => a.Timestamp <= toDate.Value);
                }
                
                query = ApplySorting(query, pagination.ColumnName, pagination.SortingAscending);
                
                var totalCount = await query.CountAsync();
                
                var auditLogs = await query
                    .Skip((pagination.Page - 1) * pagination.Show)
                    .Take(pagination.Show)
                    .ToListAsync();

                var pageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show);
                
                return new ResultSet<AuditLog>
                {
                    List = auditLogs,
                    Count = totalCount,
                    PageTotal = pageTotal,
                    CurrentPage = pagination.Page,
                    Message = "Success",
                    ClientPagination = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs by date range");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by user
        /// </summary>
        public async Task<ResultSet<AuditLog>> GetAuditLogsByUser(string userName, Pagination<AuditLog> pagination)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                var query = context.AuditLogs
                    .Where(a => a.UserName.Contains(userName));
                
                query = ApplySorting(query, pagination.ColumnName, pagination.SortingAscending);
                
                var totalCount = await query.CountAsync();
                
                var auditLogs = await query
                    .Skip((pagination.Page - 1) * pagination.Show)
                    .Take(pagination.Show)
                    .ToListAsync();

                var pageTotal = (int)Math.Ceiling((double)totalCount / pagination.Show);
                
                return new ResultSet<AuditLog>
                {
                    List = auditLogs,
                    Count = totalCount,
                    PageTotal = pageTotal,
                    CurrentPage = pagination.Page,
                    Message = "Success",
                    ClientPagination = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs by user");
                throw;
            }
        }

        /// <summary>
        /// Get audit log by ID
        /// </summary>
        public async Task<AuditLog> GetAuditLogById(Guid id)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                
                return await context.AuditLogs
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit log by ID");
                throw;
            }
        }

        /// <summary>
        /// Apply sorting to the query
        /// </summary>
        private static IQueryable<AuditLog> ApplySorting(IQueryable<AuditLog> query, string columnName, bool ascending)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return query.OrderByDescending(a => a.Timestamp);
            }

            return columnName.ToLower() switch
            {
                "timestamp" => ascending ? query.OrderBy(a => a.Timestamp) : query.OrderByDescending(a => a.Timestamp),
                "username" => ascending ? query.OrderBy(a => a.UserName) : query.OrderByDescending(a => a.UserName),
                "entitytype" => ascending ? query.OrderBy(a => a.EntityType) : query.OrderByDescending(a => a.EntityType),
                "entityid" => ascending ? query.OrderBy(a => a.EntityId) : query.OrderByDescending(a => a.EntityId),
                "actiontype" => ascending ? query.OrderBy(a => a.ActionType) : query.OrderByDescending(a => a.ActionType),
                _ => query.OrderByDescending(a => a.Timestamp)
            };
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}
