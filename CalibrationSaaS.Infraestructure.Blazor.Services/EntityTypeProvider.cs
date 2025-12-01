using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    /// <summary>
    /// Provides information about entity types that are audited in the CalibrationSaaS system.
    /// This service maintains a centralized list of all entities that have audit logging enabled.
    /// </summary>
    public class EntityTypeProvider : IEntityTypeProvider
    {
        private readonly ILogger<EntityTypeProvider> _logger;
        private readonly List<AuditedEntityType> _auditedEntityTypes;

        public EntityTypeProvider(ILogger<EntityTypeProvider> logger)
        {
            _logger = logger;
            _auditedEntityTypes = InitializeAuditedEntityTypes();
        }

        /// <summary>
        /// Gets a list of all entity types that are configured for audit logging asynchronously.
        /// </summary>
        public async Task<IEnumerable<AuditedEntityType>> GetAuditedEntityTypesAsync()
        {
            _logger.LogInformation("AUDIT DEBUG: EntityTypeProvider.GetAuditedEntityTypesAsync() called. Total entities: {Count}", _auditedEntityTypes.Count);

            // FORCE CONSOLE OUTPUT - This should appear in browser console
            //Console.WriteLine($"AUDIT DEBUG CONSOLE: EntityTypeProvider.GetAuditedEntityTypesAsync() called. Total entities: {_auditedEntityTypes.Count}");

            // Log all configured entity types
            foreach (var entity in _auditedEntityTypes)
            {
                _logger.LogInformation("AUDIT DEBUG: Configured entity - Value: '{Value}', DisplayName: '{DisplayName}', IsActive: {IsActive}, SortOrder: {SortOrder}",
                    entity.Value, entity.DisplayName, entity.IsActive, entity.SortOrder);
                //Console.WriteLine($"AUDIT DEBUG CONSOLE: Configured entity - Value: '{entity.Value}', DisplayName: '{entity.DisplayName}', IsActive: {entity.IsActive}, SortOrder: {entity.SortOrder}");
            }

            // For now, return the static list. In the future, this could be enhanced to:
            // - Load from database configuration
            // - Load from configuration files
            // - Query the actual audit configuration
            await Task.CompletedTask; // Simulate async operation

            var activeEntities = _auditedEntityTypes.Where(e => e.IsActive).OrderBy(e => e.SortOrder).ToList();
            _logger.LogInformation("AUDIT DEBUG: Returning {Count} active audited entity types: {EntityTypes}",
                activeEntities.Count,
                string.Join(", ", activeEntities.Select(e => $"{e.Value}='{e.DisplayName}'")));

            //Console.WriteLine($"AUDIT DEBUG CONSOLE: Returning {activeEntities.Count} active audited entity types: {string.Join(", ", activeEntities.Select(e => $"{e.Value}='{e.DisplayName}'"))}");

            return activeEntities;
        }

        /// <summary>
        /// Gets a list of all entity types that are configured for audit logging synchronously.
        /// </summary>
        public IEnumerable<AuditedEntityType> GetAuditedEntityTypes()
        {
            _logger.LogDebug("Getting audited entity types synchronously");
            return _auditedEntityTypes.Where(e => e.IsActive).OrderBy(e => e.SortOrder);
        }

        /// <summary>
        /// Checks if a specific entity type is configured for audit logging.
        /// </summary>
        public bool IsEntityTypeAudited(string entityTypeName)
        {
            if (string.IsNullOrEmpty(entityTypeName))
                return false;

            return _auditedEntityTypes.Any(e => e.IsActive && 
                string.Equals(e.Value, entityTypeName, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the display name for a specific entity type.
        /// </summary>
        public string GetEntityTypeDisplayName(string entityTypeName)
        {
            if (string.IsNullOrEmpty(entityTypeName))
                return entityTypeName ?? string.Empty;

            var entityType = _auditedEntityTypes.FirstOrDefault(e => 
                string.Equals(e.Value, entityTypeName, System.StringComparison.OrdinalIgnoreCase));

            return entityType?.DisplayName ?? entityTypeName;
        }

        /// <summary>
        /// Initializes the list of audited entity types.
        /// This method defines only the 5 critical entity types that have audit logging enabled in CalibrationSaaS.
        /// </summary>
        private List<AuditedEntityType> InitializeAuditedEntityTypes()
        {
            return new List<AuditedEntityType>
            {
                new AuditedEntityType
                {
                    Value = "Manufacturer",
                    DisplayName = "Manufacturer",
                    Description = "Equipment manufacturers and vendors",
                    IsActive = true,
                    SortOrder = 1
                },
                new AuditedEntityType
                {
                    Value = "EquipmentTemplate",
                    DisplayName = "Equipment Template",
                    Description = "Equipment templates and configurations",
                    IsActive = true,
                    SortOrder = 2
                },
                new AuditedEntityType
                {
                    Value = "PieceOfEquipment",
                    DisplayName = "Piece Of Equipment",
                    Description = "Equipment and instruments managed in the system",
                    IsActive = true,
                    SortOrder = 3
                },
                new AuditedEntityType
                {
                    Value = "WorkOrder",
                    DisplayName = "Work Order",
                    Description = "Calibration work orders and service requests",
                    IsActive = true,
                    SortOrder = 4
                },
                new AuditedEntityType
                {
                    Value = "WorkOrderDetail",
                    DisplayName = "Work Order Detail",
                    Description = "Individual line items and details within work orders",
                    IsActive = true,
                    SortOrder = 5
                }
            };
        }
    }
}
