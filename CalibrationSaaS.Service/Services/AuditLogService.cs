using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    /// <summary>
    /// gRPC service implementation for Audit Log operations
    /// </summary>
    public class AuditLogService : ServiceBase, IAuditLogService<CallContext>
    {
        private readonly AuditLogUseCases _auditLogUseCases;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService(
            AuditLogUseCases auditLogUseCases,
            ILogger<AuditLogService> logger)
        {
            _auditLogUseCases = auditLogUseCases ?? throw new ArgumentNullException(nameof(auditLogUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get audit logs with pagination and filtering
        /// </summary>
        public async ValueTask<ResultSet<AuditLogEntry>> GetAuditLogs(Pagination<AuditLogEntry> pagination, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("GetAuditLogs called with page {Page}, pageSize {PageSize}",
                    pagination.Page, pagination.Show);

                var result = await _auditLogUseCases.GetAuditLogs(pagination);
                
                _logger.LogInformation("GetAuditLogs returned {Count} records", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuditLogs");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by entity type
        /// </summary>
        public async ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByEntityType(string entityType, Pagination<AuditLogEntry> pagination, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("GetAuditLogsByEntityType called with entityType {EntityType}", entityType);

                var result = await _auditLogUseCases.GetAuditLogsByEntityType(entityType, pagination);
                
                _logger.LogInformation("GetAuditLogsByEntityType returned {Count} records", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuditLogsByEntityType");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by entity ID
        /// </summary>
        public async ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByEntityId(string entityId, Pagination<AuditLogEntry> pagination, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("GetAuditLogsByEntityId called with entityId {EntityId}", entityId);

                // Create search request for entity ID
                var searchRequest = new AuditLogSearchRequest
                {
                    EntityId = entityId,
                    Page = pagination.Page,
                    PageSize = pagination.Show,
                    SortColumn = pagination.ColumnName ?? "Timestamp",
                    SortDescending = !pagination.SortingAscending
                };

                var result = await _auditLogUseCases.SearchAuditLogs(searchRequest);
                
                _logger.LogInformation("GetAuditLogsByEntityId returned {Count} records", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuditLogsByEntityId");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by date range
        /// </summary>
        public async ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByDateRange(DateTime? fromDate, DateTime? toDate, Pagination<AuditLogEntry> pagination, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("GetAuditLogsByDateRange called with fromDate {FromDate}, toDate {ToDate}", fromDate, toDate);

                // Create search request for date range
                var searchRequest = new AuditLogSearchRequest
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Page = pagination.Page,
                    PageSize = pagination.Show,
                    SortColumn = pagination.ColumnName ?? "Timestamp",
                    SortDescending = !pagination.SortingAscending
                };

                var result = await _auditLogUseCases.SearchAuditLogs(searchRequest);
                
                _logger.LogInformation("GetAuditLogsByDateRange returned {Count} records", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuditLogsByDateRange");
                throw;
            }
        }

        /// <summary>
        /// Get audit logs by user
        /// </summary>
        public async ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByUser(string userName, Pagination<AuditLogEntry> pagination, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("GetAuditLogsByUser called with userName {UserName}", userName);

                // Create search request for user
                var searchRequest = new AuditLogSearchRequest
                {
                    UserName = userName,
                    Page = pagination.Page,
                    PageSize = pagination.Show,
                    SortColumn = pagination.ColumnName ?? "Timestamp",
                    SortDescending = !pagination.SortingAscending
                };

                var result = await _auditLogUseCases.SearchAuditLogs(searchRequest);
                
                _logger.LogInformation("GetAuditLogsByUser returned {Count} records", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuditLogsByUser");
                throw;
            }
        }

        /// <summary>
        /// Get a specific audit log entry by ID
        /// </summary>
        public async ValueTask<AuditLogEntry> GetAuditLogById(AuditLogByIdRequest request, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("GetAuditLogById called with id {Id}", request.Id);

                var result = await _auditLogUseCases.GetAuditLogById(request.Id);

                _logger.LogInformation("GetAuditLogById returned result: {Found}", result != null);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAuditLogById");
                throw;
            }
        }

        /// <summary>
        /// Search audit logs with multiple filters
        /// </summary>
        public async ValueTask<ResultSet<AuditLogEntry>> SearchAuditLogs(AuditLogSearchRequest searchRequest, CallContext context = default)
        {
            try
            {
                _logger.LogInformation("SearchAuditLogs called with filters: EntityType={EntityType}, EntityId={EntityId}, UserName={UserName}", 
                    searchRequest.EntityType, searchRequest.EntityId, searchRequest.UserName);

                var result = await _auditLogUseCases.SearchAuditLogs(searchRequest);
                
                _logger.LogInformation("SearchAuditLogs returned {Count} records", result.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchAuditLogs");
                throw;
            }
        }
    }
}
