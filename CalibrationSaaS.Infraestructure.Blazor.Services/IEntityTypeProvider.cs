using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    /// <summary>
    /// Provides information about entity types that are audited in the CalibrationSaaS system.
    /// </summary>
    public interface IEntityTypeProvider
    {
        /// <summary>
        /// Gets a list of all entity types that are configured for audit logging.
        /// </summary>
        /// <returns>A list of audited entity types with their display names and values.</returns>
        Task<IEnumerable<AuditedEntityType>> GetAuditedEntityTypesAsync();

        /// <summary>
        /// Gets a list of all entity types that are configured for audit logging synchronously.
        /// </summary>
        /// <returns>A list of audited entity types with their display names and values.</returns>
        IEnumerable<AuditedEntityType> GetAuditedEntityTypes();

        /// <summary>
        /// Checks if a specific entity type is configured for audit logging.
        /// </summary>
        /// <param name="entityTypeName">The name of the entity type to check.</param>
        /// <returns>True if the entity type is audited, false otherwise.</returns>
        bool IsEntityTypeAudited(string entityTypeName);

        /// <summary>
        /// Gets the display name for a specific entity type.
        /// </summary>
        /// <param name="entityTypeName">The entity type name.</param>
        /// <returns>The display name for the entity type, or the original name if not found.</returns>
        string GetEntityTypeDisplayName(string entityTypeName);
    }

    /// <summary>
    /// Represents an entity type that is configured for audit logging.
    /// </summary>
    public class AuditedEntityType
    {
        /// <summary>
        /// The internal name/value of the entity type (e.g., "WorkOrder").
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// The user-friendly display name of the entity type (e.g., "Work Order").
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of what this entity type represents.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if this entity type is currently active for audit logging.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Sort order for displaying entity types in UI dropdowns.
        /// </summary>
        public int SortOrder { get; set; }
    }
}
