using System;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Application.Services
{
    /// <summary>
    /// Request object for searching audit logs with multiple filters
    /// </summary>
    [DataContract]
    public class AuditLogSearchRequest
    {
        /// <summary>
        /// Filter by entity type (e.g., "WorkOrder", "Customer")
        /// </summary>
        [DataMember(Order = 1)]
        public string? EntityType { get; set; }

        /// <summary>
        /// Filter by specific entity ID
        /// </summary>
        [DataMember(Order = 2)]
        public string? EntityId { get; set; }

        /// <summary>
        /// Filter by start date (inclusive)
        /// </summary>
        [DataMember(Order = 3)]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Filter by end date (inclusive)
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Filter by user name (partial match)
        /// </summary>
        [DataMember(Order = 5)]
        public string? UserName { get; set; }

        /// <summary>
        /// Filter by action type (e.g., "Create", "Update", "Delete")
        /// </summary>
        [DataMember(Order = 6)]
        public string? ActionType { get; set; }

        /// <summary>
        /// Page number for pagination (1-based)
        /// </summary>
        [DataMember(Order = 7)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page
        /// </summary>
        [DataMember(Order = 8)]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Column to sort by
        /// </summary>
        [DataMember(Order = 9)]
        public string SortColumn { get; set; } = "Timestamp";

        /// <summary>
        /// Whether to sort in descending order
        /// </summary>
        [DataMember(Order = 10)]
        public bool SortDescending { get; set; } = true;

        /// <summary>
        /// Filter by client IP address (partial match)
        /// </summary>
        [DataMember(Order = 11)]
        public string? ClientIpAddress { get; set; }
    }

    /// <summary>
    /// Request object for getting a specific audit log entry by ID
    /// </summary>
    [DataContract]
    public class AuditLogByIdRequest
    {
        /// <summary>
        /// The ID of the audit log entry to retrieve
        /// </summary>
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
