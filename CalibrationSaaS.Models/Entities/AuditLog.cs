using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    /// <summary>
    /// Audit Log entity for storing Audit.NET audit events
    /// Compatible with existing AuditLogEntry model structure
    /// </summary>
    [Table("AuditLogs")]
    [DataContract]
    public class AuditLog : IGeneric
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public int AuditLogId { get; set; }

        [DataMember(Order = 2)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember(Order = 3)]
        public DateTime Timestamp { get; set; }

        [DataMember(Order = 4)]
        [MaxLength(255)]
        public string UserName { get; set; }

        [DataMember(Order = 5)]
        [MaxLength(255)]
        public string EntityType { get; set; }

        [DataMember(Order = 6)]
        [MaxLength(255)]
        public string EntityId { get; set; }

        [DataMember(Order = 7)]
        [MaxLength(100)]
        public string ActionType { get; set; }

        [DataMember(Order = 8)]
        public string PreviousState { get; set; }

        [DataMember(Order = 9)]
        public string CurrentState { get; set; }

        // Additional Audit.NET specific fields
        [DataMember(Order = 10)]
        [MaxLength(255)]
        public string ApplicationName { get; set; }

        [DataMember(Order = 11)]
        public string UserId { get; set; }

        [DataMember(Order = 12)]
        public int? TenantId { get; set; }

        [DataMember(Order = 13)]
        [MaxLength(255)]
        public string TenantName { get; set; }

        [DataMember(Order = 14)]
        public int ExecutionDuration { get; set; }

        [DataMember(Order = 15)]
        [MaxLength(255)]
        public string ClientIpAddress { get; set; }

        [DataMember(Order = 16)]
        [MaxLength(255)]
        public string CorrelationId { get; set; }

        [DataMember(Order = 17)]
        [MaxLength(500)]
        public string BrowserInfo { get; set; }

        [DataMember(Order = 18)]
        [MaxLength(10)]
        public string HttpMethod { get; set; }

        [DataMember(Order = 19)]
        public int? HttpStatusCode { get; set; }

        [DataMember(Order = 20)]
        [MaxLength(2000)]
        public string Url { get; set; }

        [DataMember(Order = 21)]
        public string ExceptionDetails { get; set; }

        [DataMember(Order = 22)]
        public string Comments { get; set; }

        [DataMember(Order = 23)]
        public string AuditData { get; set; }

        [DataMember(Order = 24)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
