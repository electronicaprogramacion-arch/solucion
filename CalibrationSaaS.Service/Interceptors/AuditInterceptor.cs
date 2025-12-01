using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Interceptors
{
    /// <summary>
    /// Entity Framework interceptor that captures original entity values before updates
    /// This solves the timing issue where Audit.NET captures changes after EF updates the change tracker
    /// </summary>
    public class AuditInterceptor : SaveChangesInterceptor
    {
        // Thread-safe storage for original values
        private static readonly ConcurrentDictionary<string, object> _originalValues = new();

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            CaptureOriginalValues(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            CaptureOriginalValues(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            // Clean up stored values after save is complete
            CleanupOriginalValues();
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            // Clean up stored values after save is complete
            CleanupOriginalValues();
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        /// <summary>
        /// Capture original values before Entity Framework processes changes
        /// </summary>
        private void CaptureOriginalValues(DbContext context)
        {
            if (context == null) return;

            var auditableEntities = new[]
            {
                "PieceOfEquipment",
                "Customer", 
                "WorkOrder",
                "Quote"
            };

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {
                    var entityTypeName = entry.Entity.GetType().Name;
                    
                    // Only capture for auditable entities
                    if (!auditableEntities.Contains(entityTypeName))
                        continue;

                    // Get the primary key value
                    var primaryKey = GetPrimaryKeyValue(entry);
                    if (primaryKey == null) continue;

                    var key = $"OriginalValues_{entityTypeName}_{primaryKey}";

                    // Capture original values from the database values (before changes)
                    var originalValues = new System.Collections.Generic.Dictionary<string, object>();
                    
                    foreach (var property in entry.Properties)
                    {
                        if (property.IsModified)
                        {
                            // Use OriginalValue which should contain the value from the database
                            originalValues[property.Metadata.Name] = property.OriginalValue;
                            
//                            Console.WriteLine($"INTERCEPTOR DEBUG: {entityTypeName}.{property.Metadata.Name} = '{property.OriginalValue}' -> '{property.CurrentValue}'");
                        }
                    }

                    if (originalValues.Any())
                    {
                        _originalValues[key] = originalValues;
//                        Console.WriteLine($"INTERCEPTOR DEBUG: Captured {originalValues.Count} original values for {entityTypeName} ID {primaryKey}");
                        
                        // Store in Audit.NET custom fields for later retrieval
                        StoreInAuditCustomFields(key, originalValues);
                    }
                }
            }
        }

        /// <summary>
        /// Get the primary key value from an entity entry
        /// </summary>
        private object GetPrimaryKeyValue(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            try
            {
                var keyProperties = entry.Metadata.FindPrimaryKey()?.Properties;
                if (keyProperties == null || !keyProperties.Any())
                    return null;

                // For single primary key
                if (keyProperties.Count == 1)
                {
                    return entry.Property(keyProperties.First().Name).CurrentValue;
                }

                // For composite primary key, create a combined key
                var keyValues = keyProperties.Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? "null");
                return string.Join("_", keyValues);
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"INTERCEPTOR ERROR: Failed to get primary key: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Store original values in Audit.NET custom fields
        /// This is a simplified approach - in a real implementation you might need to integrate more deeply with Audit.NET
        /// </summary>
        private void StoreInAuditCustomFields(string key, Dictionary<string, object> originalValues)
        {
            // For now, we'll rely on the static storage and retrieve it in the audit configuration
            // A more sophisticated approach would integrate directly with Audit.NET's scope
//            Console.WriteLine($"INTERCEPTOR DEBUG: Stored original values with key: {key}");
        }

        /// <summary>
        /// Clean up stored original values after save is complete
        /// </summary>
        private void CleanupOriginalValues()
        {
            _originalValues.Clear();
//            Console.WriteLine("INTERCEPTOR DEBUG: Cleaned up original values storage");
        }

        /// <summary>
        /// Get stored original values for a specific entity
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, object> GetOriginalValues(string entityTypeName, object primaryKey)
        {
            var key = $"OriginalValues_{entityTypeName}_{primaryKey}";
            
            if (_originalValues.TryGetValue(key, out var values) && values is System.Collections.Generic.Dictionary<string, object> originalValues)
            {
//                Console.WriteLine($"INTERCEPTOR DEBUG: Retrieved {originalValues.Count} original values for key: {key}");
                return originalValues;
            }

//            Console.WriteLine($"INTERCEPTOR DEBUG: No original values found for key: {key}");
            return null;
        }

        /// <summary>
        /// Store original values from DbContext (called by DbContext.SaveChanges)
        /// </summary>
        public static void StoreOriginalValues(string key, System.Collections.Generic.Dictionary<string, object> originalValues)
        {
            _originalValues[key] = originalValues;
//            Console.WriteLine($"INTERCEPTOR STORAGE: Stored values for key {key}");
        }
    }
}
