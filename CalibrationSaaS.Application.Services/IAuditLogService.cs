using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls.ValueObjects;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    /// <summary>
    /// Service contract for Audit Log operations
    /// </summary>
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.AuditLogService")]
    public interface IAuditLogService<T>
    {
        /// <summary>
        /// Get audit logs with pagination and filtering
        /// </summary>
        /// <param name="pagination">Pagination parameters with filters</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of audit log entries</returns>
        ValueTask<ResultSet<AuditLogEntry>> GetAuditLogs(Pagination<AuditLogEntry> pagination, T context);

        /// <summary>
        /// Get audit logs by entity type
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of audit log entries</returns>
        ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByEntityType(string entityType, Pagination<AuditLogEntry> pagination, T context);

        /// <summary>
        /// Get audit logs by entity ID
        /// </summary>
        /// <param name="entityId">Entity ID to filter by</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of audit log entries</returns>
        ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByEntityId(string entityId, Pagination<AuditLogEntry> pagination, T context);

        /// <summary>
        /// Get audit logs by date range
        /// </summary>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">End date</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of audit log entries</returns>
        ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByDateRange(DateTime? fromDate, DateTime? toDate, Pagination<AuditLogEntry> pagination, T context);

        /// <summary>
        /// Get audit logs by user
        /// </summary>
        /// <param name="userName">User name to filter by</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of audit log entries</returns>
        ValueTask<ResultSet<AuditLogEntry>> GetAuditLogsByUser(string userName, Pagination<AuditLogEntry> pagination, T context);

        /// <summary>
        /// Get a specific audit log entry by ID
        /// </summary>
        /// <param name="request">Request containing the audit log ID</param>
        /// <param name="context">Call context</param>
        /// <returns>Audit log entry</returns>
        ValueTask<AuditLogEntry> GetAuditLogById(AuditLogByIdRequest request, T context);

        /// <summary>
        /// Search audit logs with multiple filters
        /// </summary>
        /// <param name="searchRequest">Search request with filters</param>
        /// <param name="context">Call context</param>
        /// <returns>Paginated result set of audit log entries</returns>
        ValueTask<ResultSet<AuditLogEntry>> SearchAuditLogs(AuditLogSearchRequest searchRequest, T context);
    }
}
