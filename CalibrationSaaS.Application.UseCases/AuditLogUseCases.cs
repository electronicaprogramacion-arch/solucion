using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Application.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    /// <summary>
    /// Use cases for Audit Log operations
    /// </summary>
    public class AuditLogUseCases
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<AuditLogUseCases> _logger;

        public AuditLogUseCases(IAuditLogRepository auditLogRepository, ILogger<AuditLogUseCases> logger)
        {
            _auditLogRepository = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get audit logs with pagination and filtering
        /// </summary>
        public async Task<ResultSet<AuditLogEntry>> GetAuditLogs(Pagination<AuditLogEntry> pagination)
        {
            try
            {
                _logger.LogInformation("Getting audit logs with pagination");
                
                // Create pagination for AuditLog entities
                var auditLogPagination = new Pagination<AuditLog>
                {
                    Page = pagination.Page,
                    Show = pagination.Show,
                    ColumnName = pagination.ColumnName,
                    SortingAscending = pagination.SortingAscending
                };

                var auditLogs = await _auditLogRepository.GetAuditLogs(auditLogPagination);
                
                // Convert AuditLog entities to AuditLogEntry DTOs
                var auditLogEntries = auditLogs.List.Select(ConvertToAuditLogEntry).ToList();
                
                return new ResultSet<AuditLogEntry>
                {
                    List = auditLogEntries,
                    Count = auditLogs.Count,
                    PageTotal = auditLogs.PageTotal,
                    CurrentPage = auditLogs.CurrentPage,
                    Message = auditLogs.Message,
                    ClientPagination = auditLogs.ClientPagination
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
        public async Task<ResultSet<AuditLogEntry>> SearchAuditLogs(AuditLogSearchRequest searchRequest)
        {
            try
            {
                _logger.LogInformation("Searching audit logs with filters: EntityType={EntityType}, EntityId={EntityId}, FromDate={FromDate}, ToDate={ToDate}", 
                    searchRequest.EntityType, searchRequest.EntityId, searchRequest.FromDate, searchRequest.ToDate);
                
                var auditLogs = await _auditLogRepository.SearchAuditLogs(
                    searchRequest.EntityType,
                    searchRequest.EntityId,
                    searchRequest.FromDate,
                    searchRequest.ToDate,
                    searchRequest.UserName,
                    searchRequest.ActionType,
                    searchRequest.Page,
                    searchRequest.PageSize,
                    searchRequest.SortColumn,
                    searchRequest.SortDescending,
                    searchRequest.ClientIpAddress);
                
                // Convert AuditLog entities to AuditLogEntry DTOs
                var auditLogEntries = auditLogs.List.Select(ConvertToAuditLogEntry).ToList();
                
                return new ResultSet<AuditLogEntry>
                {
                    List = auditLogEntries,
                    Count = auditLogs.Count,
                    PageTotal = auditLogs.PageTotal,
                    CurrentPage = auditLogs.CurrentPage,
                    Message = auditLogs.Message,
                    ClientPagination = auditLogs.ClientPagination
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
        public async Task<ResultSet<AuditLogEntry>> GetAuditLogsByEntityType(string entityType, Pagination<AuditLogEntry> pagination)
        {
            try
            {
                _logger.LogInformation("Getting audit logs by entity type: {EntityType}", entityType);
                
                // Create pagination for AuditLog entities
                var auditLogPagination = new Pagination<AuditLog>
                {
                    Page = pagination.Page,
                    Show = pagination.Show,
                    ColumnName = pagination.ColumnName,
                    SortingAscending = pagination.SortingAscending
                };

                var auditLogs = await _auditLogRepository.GetAuditLogsByEntityType(entityType, auditLogPagination);
                
                var auditLogEntries = auditLogs.List.Select(ConvertToAuditLogEntry).ToList();
                
                return new ResultSet<AuditLogEntry>
                {
                    List = auditLogEntries,
                    Count = auditLogs.Count,
                    PageTotal = auditLogs.PageTotal,
                    CurrentPage = auditLogs.CurrentPage,
                    Message = auditLogs.Message,
                    ClientPagination = auditLogs.ClientPagination
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs by entity type");
                throw;
            }
        }

        /// <summary>
        /// Get audit log by ID
        /// </summary>
        public async Task<AuditLogEntry> GetAuditLogById(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting audit log by ID: {Id}", id);
                
                var auditLog = await _auditLogRepository.GetAuditLogById(id);
                
                return auditLog != null ? ConvertToAuditLogEntry(auditLog) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit log by ID");
                throw;
            }
        }

        /// <summary>
        /// Convert AuditLog entity to AuditLogEntry DTO
        /// </summary>
        private static AuditLogEntry ConvertToAuditLogEntry(AuditLog auditLog)
        {
            return new AuditLogEntry
            {
                Id = auditLog.Id,
                Timestamp = auditLog.Timestamp,
                UserName = auditLog.UserName,
                EntityType = auditLog.EntityType,
                EntityId = auditLog.EntityId,
                ActionType = auditLog.ActionType,
                PreviousState = auditLog.PreviousState,
                CurrentState = auditLog.CurrentState,
                CreatedDate = auditLog.CreatedDate,

                // Extended audit fields
                ApplicationName = auditLog.ApplicationName,
                UserId = auditLog.UserId,
                TenantId = auditLog.TenantId,
                TenantName = auditLog.TenantName,
                ExecutionDuration = auditLog.ExecutionDuration,
                ClientIpAddress = auditLog.ClientIpAddress,
                CorrelationId = auditLog.CorrelationId,
                BrowserInfo = auditLog.BrowserInfo,
                HttpMethod = auditLog.HttpMethod,
                HttpStatusCode = auditLog.HttpStatusCode,
                Url = auditLog.Url,
                ExceptionDetails = auditLog.ExceptionDetails,
                Comments = auditLog.Comments,
                AuditData = auditLog.AuditData
            };
        }
    }
}
