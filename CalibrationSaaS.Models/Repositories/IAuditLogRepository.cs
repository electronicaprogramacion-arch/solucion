using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls.ValueObjects;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Audit Log operations
    /// </summary>
    public interface IAuditLogRepository
    {
        /// <summary>
        /// Get audit logs with pagination
        /// </summary>
        /// <param name="pagination">Pagination parameters</param>
        /// <returns>Paginated result set of audit logs</returns>
        Task<ResultSet<AuditLog>> GetAuditLogs(Pagination<AuditLog> pagination);

        /// <summary>
        /// Search audit logs with multiple filters
        /// </summary>
        /// <param name="entityType">Entity type filter</param>
        /// <param name="entityId">Entity ID filter</param>
        /// <param name="fromDate">Start date filter</param>
        /// <param name="toDate">End date filter</param>
        /// <param name="userName">User name filter</param>
        /// <param name="actionType">Action type filter</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortColumn">Sort column</param>
        /// <param name="sortDescending">Sort direction</param>
        /// <param name="clientIpAddress">Client IP address filter</param>
        /// <returns>Paginated result set of audit logs</returns>
        Task<ResultSet<AuditLog>> SearchAuditLogs(string entityType, string entityId, DateTime? fromDate, DateTime? toDate, string userName, string actionType, int page, int pageSize, string sortColumn, bool sortDescending, string clientIpAddress = null);

        /// <summary>
        /// Get audit logs by entity type
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <returns>Paginated result set of audit logs</returns>
        Task<ResultSet<AuditLog>> GetAuditLogsByEntityType(string entityType, Pagination<AuditLog> pagination);

        /// <summary>
        /// Get audit logs by entity ID
        /// </summary>
        /// <param name="entityId">Entity ID to filter by</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <returns>Paginated result set of audit logs</returns>
        Task<ResultSet<AuditLog>> GetAuditLogsByEntityId(string entityId, Pagination<AuditLog> pagination);

        /// <summary>
        /// Get audit logs by date range
        /// </summary>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">End date</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <returns>Paginated result set of audit logs</returns>
        Task<ResultSet<AuditLog>> GetAuditLogsByDateRange(DateTime? fromDate, DateTime? toDate, Pagination<AuditLog> pagination);

        /// <summary>
        /// Get audit logs by user
        /// </summary>
        /// <param name="userName">User name to filter by</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <returns>Paginated result set of audit logs</returns>
        Task<ResultSet<AuditLog>> GetAuditLogsByUser(string userName, Pagination<AuditLog> pagination);

        /// <summary>
        /// Get a specific audit log by ID
        /// </summary>
        /// <param name="id">Audit log ID</param>
        /// <returns>Audit log entity</returns>
        Task<AuditLog> GetAuditLogById(Guid id);
    }
}
