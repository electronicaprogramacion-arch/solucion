using System;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Application.Services
{
    /// <summary>
    /// Data transfer object for audit log entries
    /// </summary>
    [DataContract]
    public class AuditLogEntry
    {
        /// <summary>
        /// Unique identifier for the audit log entry
        /// </summary>
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Timestamp when the audit event occurred
        /// </summary>
        [DataMember(Order = 2)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Name of the user who performed the action
        /// </summary>
        [DataMember(Order = 3)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Type of entity that was modified (e.g., "WorkOrder", "Customer")
        /// </summary>
        [DataMember(Order = 4)]
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier of the entity that was modified
        /// </summary>
        [DataMember(Order = 5)]
        public string EntityId { get; set; } = string.Empty;

        /// <summary>
        /// Type of action performed (e.g., "Create", "Update", "Delete")
        /// </summary>
        [DataMember(Order = 6)]
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// JSON representation of the entity state before the change
        /// </summary>
        [DataMember(Order = 7)]
        public string? PreviousState { get; set; }

        /// <summary>
        /// JSON representation of the entity state after the change
        /// </summary>
        [DataMember(Order = 8)]
        public string? CurrentState { get; set; }

        /// <summary>
        /// Additional context or metadata about the audit event
        /// </summary>
        [DataMember(Order = 9)]
        public string? AdditionalData { get; set; }

        /// <summary>
        /// Date when the audit log entry was created
        /// </summary>
        [DataMember(Order = 10)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Application name where the audit event occurred
        /// </summary>
        [DataMember(Order = 11)]
        public string? ApplicationName { get; set; }

        /// <summary>
        /// User ID who performed the action
        /// </summary>
        [DataMember(Order = 12)]
        public string? UserId { get; set; }

        /// <summary>
        /// Tenant ID for multi-tenant scenarios
        /// </summary>
        [DataMember(Order = 13)]
        public int? TenantId { get; set; }

        /// <summary>
        /// Tenant name for multi-tenant scenarios
        /// </summary>
        [DataMember(Order = 14)]
        public string? TenantName { get; set; }

        /// <summary>
        /// Execution duration in milliseconds
        /// </summary>
        [DataMember(Order = 15)]
        public int ExecutionDuration { get; set; }

        /// <summary>
        /// Client IP address where the action originated
        /// </summary>
        [DataMember(Order = 16)]
        public string? ClientIpAddress { get; set; }

        /// <summary>
        /// Correlation ID for tracking related operations
        /// </summary>
        [DataMember(Order = 17)]
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Browser information for web requests
        /// </summary>
        [DataMember(Order = 18)]
        public string? BrowserInfo { get; set; }

        /// <summary>
        /// HTTP method for web requests
        /// </summary>
        [DataMember(Order = 19)]
        public string? HttpMethod { get; set; }

        /// <summary>
        /// HTTP status code for web requests
        /// </summary>
        [DataMember(Order = 20)]
        public int? HttpStatusCode { get; set; }

        /// <summary>
        /// URL for web requests
        /// </summary>
        [DataMember(Order = 21)]
        public string? Url { get; set; }

        /// <summary>
        /// Exception details if an error occurred
        /// </summary>
        [DataMember(Order = 22)]
        public string? ExceptionDetails { get; set; }

        /// <summary>
        /// Additional comments about the audit event
        /// </summary>
        [DataMember(Order = 23)]
        public string? Comments { get; set; }

        /// <summary>
        /// Raw audit data from Audit.NET
        /// </summary>
        [DataMember(Order = 24)]
        public string? AuditData { get; set; }
    }
}
