using Audit.Core;
using Audit.EntityFramework;
using Audit.SqlServer;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Configuration
{
    /// <summary>
    /// Configuration for Audit.NET integration with CalibrationSaaS
    /// </summary>
    public static class AuditConfiguration
    {
        /// <summary>
        /// Service provider for dependency injection in audit context
        /// </summary>
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// Set the service provider for audit configuration
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Configure Audit.NET for CalibrationSaaS
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration</param>
        public static void ConfigureAudit(this IServiceCollection services, IConfiguration configuration)
        {
            // Check if audit logging is enabled
            var auditEnabled = configuration.GetValue<bool>("AuditSettings:EnableAuditLogging", false);

            if (!auditEnabled)
            {
//                Console.WriteLine("AUDIT CONFIG: Audit logging is disabled in configuration");
                return; // Skip audit configuration entirely
            }

//            Console.WriteLine("AUDIT CONFIG: Audit logging is enabled");

            // Get connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure Audit.NET global settings
            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .AuditTypeMapper(t => typeof(AuditLog))
                    .AuditEntityAction<AuditLog>((ev, entry, auditEntity) =>
                    {
                        // CRITICAL FIX: Implement selective auditing to improve performance
                        if (!IsAuditableEntity(entry.EntityType?.Name))
                        {
                            return; // Skip non-auditable entities for performance
                        }

                        // VALIDATION: Prevent null/empty audit log entries
                        if (!IsValidAuditEntry(entry))
                        {
                            return; // Skip invalid entries
                        }

                        // Map Audit.NET event to our AuditLog entity
                        MapAuditEvent(ev, entry, auditEntity);
                    })
                    .IgnoreMatchedProperties(true))
                .WithCreationPolicy(EventCreationPolicy.InsertOnEnd) // AUDIT FIX: Use single-phase capture with Changes collection
                // NOTE: Using InsertOnEnd with entry.Changes for reliable original/new value capture
                .WithAction(x => x.OnScopeCreated(scope =>
                {    //TODO
                    // Add custom fields to audit scope
                    //AddCustomAuditFields(scope);
                }));

            // Note: We're using Entity Framework auditing, so no need for a separate SQL data provider
            // The audit data will be saved through the Entity Framework context
        }

        /// <summary>
        /// Configure which entities should be audited
        /// </summary>
        /// <param name="context">DbContext to configure</param>
        public static void ConfigureAuditedEntities(CalibrationSaaSDBContext context)
        {
            // Configure Entity Framework audit settings
            context.AuditDisabled = false;

            // Configure selective auditing for better performance
            context.IncludeEntityObjects = true; // Include entity objects for audit data

            // Note: Entity-specific configuration is done in the global setup
            // The entities to audit are configured in the UseEntityFramework() setup above.
        }

        /// <summary>
        /// Check if audit logging is enabled in configuration
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>True if audit logging is enabled</returns>
        public static bool IsAuditLoggingEnabled(IConfiguration configuration)
        {
            return configuration.GetValue<bool>("AuditSettings:EnableAuditLogging", false);
        }

        /// <summary>
        /// Determines if an entity type should be audited
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type</param>
        /// <returns>True if the entity should be audited</returns>
        private static bool IsAuditableEntity(string entityTypeName)
        {
            // Only audit the 5 specified critical entities
            if (string.IsNullOrWhiteSpace(entityTypeName))
                return false;

            var auditableEntities = new[]
            {
                "Manufacturer",      // Equipment manufacturers and vendors
                "EquipmentTemplate", // Equipment templates and configurations
                "PieceOfEquipment",  // Equipment and instruments managed in the system
                "WorkOrder",         // Calibration work orders and service requests
                "WorkOrderDetail"    // Individual line items and details within work orders
            };

            return auditableEntities.Contains(entityTypeName, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates if an audit entry contains required data
        /// </summary>
        /// <param name="entry">The audit entry to validate</param>
        /// <returns>True if the entry is valid for auditing</returns>
        private static bool IsValidAuditEntry(EventEntry entry)
        {
            // Check for null or empty entity type
            if (string.IsNullOrWhiteSpace(entry.EntityType?.Name))
                return false;

            // Check for null or empty entity ID
            var primaryKeyPair = entry.PrimaryKey?.FirstOrDefault();
            if (primaryKeyPair?.Value == null || string.IsNullOrWhiteSpace(primaryKeyPair.Value.ToString()))
                return false;

            // Check for valid action type
            if (string.IsNullOrWhiteSpace(entry.Action))
                return false;

            return true;
        }

        /// <summary>
        /// Gets the authenticated user name from audit event custom fields
        /// </summary>
        /// <param name="auditEvent">The audit event containing custom fields</param>
        /// <returns>The authenticated user name or fallback value</returns>
        private static string GetAuthenticatedUserName(AuditEvent auditEvent)
        {
            // DEBUG: Log all custom fields to understand what's available
//            Console.WriteLine($"AUDIT DEBUG: Available custom fields: {string.Join(", ", auditEvent.CustomFields.Keys)}");

            // First, try to get the authenticated user from custom fields
            if (auditEvent.CustomFields.ContainsKey("UserName"))
            {
                var customUserName = auditEvent.CustomFields["UserName"]?.ToString();
//                Console.WriteLine($"AUDIT DEBUG: Found UserName in custom fields: '{customUserName}'");
                if (!string.IsNullOrWhiteSpace(customUserName))
                {
                    return customUserName;
                }
            }

            // Check for authentication status
            if (auditEvent.CustomFields.ContainsKey("IsAuthenticated"))
            {
                var isAuth = auditEvent.CustomFields["IsAuthenticated"]?.ToString();
//                Console.WriteLine($"AUDIT DEBUG: IsAuthenticated: {isAuth}");
            }

            // Check for any user context errors
            if (auditEvent.CustomFields.ContainsKey("UserContextError"))
            {
                var error = auditEvent.CustomFields["UserContextError"]?.ToString();
//                Console.WriteLine($"AUDIT DEBUG: UserContextError: {error}");
            }

            // Fallback to environment user name, but only if no custom user was found
            var environmentUserName = auditEvent.Environment.UserName;
//            Console.WriteLine($"AUDIT DEBUG: Environment UserName: '{environmentUserName}'");
            if (!string.IsNullOrWhiteSpace(environmentUserName))
            {
                return environmentUserName;
            }

            // Final fallback
//            Console.WriteLine("AUDIT DEBUG: Using 'System' as final fallback");
            return "System";
        }

        /// <summary>
        /// Map Audit.NET event data to our AuditLog entity
        /// </summary>
        private static void MapAuditEvent(AuditEvent auditEvent, EventEntry entry, AuditLog auditEntity)
        {
            // Basic audit information
            auditEntity.Id = Guid.NewGuid();
            auditEntity.Timestamp = auditEvent.StartDate;

            // FIXED: Use authenticated user from custom fields instead of system user
            auditEntity.UserName = GetAuthenticatedUserName(auditEvent);

            auditEntity.EntityType = entry.EntityType?.Name ?? "Unknown";
            var primaryKeyPair = entry.PrimaryKey?.FirstOrDefault();
            auditEntity.EntityId = primaryKeyPair?.Value?.ToString() ?? "Unknown";
            auditEntity.ActionType = entry.Action; // Insert, Update, Delete

            // AUDIT DEBUG: Log audit entity creation
//            Console.WriteLine($"AUDIT DEBUG: Creating audit log for {auditEntity.EntityType} (ID: {auditEntity.EntityId}) - Action: {auditEntity.ActionType} - User: {auditEntity.UserName}");
            auditEntity.ApplicationName = auditEvent.Environment.MachineName;
            auditEntity.ExecutionDuration = (int)auditEvent.Duration;
            auditEntity.CreatedDate = DateTime.UtcNow;

            // Entity state information - FIXED: Properly capture before/after states with two-phase approach
            if (entry.Action == "Insert")
            {
                auditEntity.PreviousState = null;
                auditEntity.CurrentState = GetCurrentStateJson(entry);
            }
            else if (entry.Action == "Update")
            {
                // AUDIT FIX: Handle two-phase capture for updates
                HandleUpdateAuditCapture(auditEvent, entry, auditEntity);
            }
            else if (entry.Action == "Delete")
            {
                auditEntity.PreviousState = GetCurrentStateJson(entry);
                auditEntity.CurrentState = null;
            }

            // Additional audit data
            auditEntity.AuditData = auditEvent.ToJson();
            
            // Try to get user ID from custom fields or environment
            if (auditEvent.CustomFields.ContainsKey("UserId"))
            {
                auditEntity.UserId = auditEvent.CustomFields["UserId"]?.ToString();
            }

            // Try to get tenant information
            if (auditEvent.CustomFields.ContainsKey("TenantId"))
            {
                var tenantIdValue = auditEvent.CustomFields["TenantId"];
                if (tenantIdValue != null && int.TryParse(tenantIdValue.ToString(), out int tenantId))
                {
                    auditEntity.TenantId = tenantId;
                }
            }

            if (auditEvent.CustomFields.ContainsKey("TenantName"))
            {
                auditEntity.TenantName = auditEvent.CustomFields["TenantName"]?.ToString();
            }
        }

        /// <summary>
        /// Handle two-phase audit capture for update operations
        /// </summary>
        private static void HandleUpdateAuditCapture(AuditEvent auditEvent, EventEntry entry, AuditLog auditEntity)
        {
            // AUDIT FIX: Try to get original values from our DbContext storage
            var primaryKey = GetPrimaryKeyFromEntry(entry);
//            Console.WriteLine($"AUDIT DEBUG: Attempting to capture original values for {entry.EntityType?.Name} ID {primaryKey}");

            // Try to get original values from our storage first
            var originalValues = AuditOriginalValuesStorage
                .GetOriginalValues(entry.EntityType?.Name, primaryKey);

            if (originalValues != null && originalValues.Any())
            {
                // Use the original values captured by our DbContext
                auditEntity.PreviousState = System.Text.Json.JsonSerializer.Serialize(originalValues, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                // Use the current entity state for new values
                auditEntity.CurrentState = GetEntityStateJson(entry.Entity);

//                Console.WriteLine($"AUDIT DEBUG UPDATE (DBCONTEXT): Entity={entry.EntityType?.Name}, ID={primaryKey}");
//                Console.WriteLine($"AUDIT DEBUG PREVIOUS (DBCONTEXT): {auditEntity.PreviousState}");
//                Console.WriteLine($"AUDIT DEBUG CURRENT (DBCONTEXT): {auditEntity.CurrentState}");
                return;
            }

            // Try to get the EF context from the audit event
            var efEvent = auditEvent as Audit.EntityFramework.AuditEventEntityFramework;
            if (efEvent != null)
            {
//                Console.WriteLine($"AUDIT DEBUG: Found EF audit event");

                // Try to access the context through the event's properties
                // Note: The exact property name may vary depending on Audit.NET version
                // Let's try to find the context through reflection or available properties
                var contextProperty = efEvent.GetType().GetProperty("Context") ??
                                    efEvent.GetType().GetProperty("DbContext") ??
                                    efEvent.GetType().GetProperty("Database");

                if (contextProperty != null)
                {
                    var context = contextProperty.GetValue(efEvent) as Microsoft.EntityFrameworkCore.DbContext;
                    if (context != null)
                    {
//                        Console.WriteLine($"AUDIT DEBUG: Found EF context: {context.GetType().Name}");

                        // Try to find the entity in the change tracker
                        var entityEntry = context.ChangeTracker.Entries()
                            .FirstOrDefault(e => e.Entity.GetType().Name == entry.EntityType?.Name &&
                                                GetEntityPrimaryKey(e) == primaryKey?.ToString());

                        if (entityEntry != null)
                        {
//                            Console.WriteLine($"AUDIT DEBUG: Found entity entry with state: {entityEntry.State}");

                            // Capture original values from the entity entry
                            var originalValuesFromEF = new System.Collections.Generic.Dictionary<string, object>();
                            var currentValues = new System.Collections.Generic.Dictionary<string, object>();

                            foreach (var property in entityEntry.Properties)
                            {
                                if (property.IsModified)
                                {
                                    originalValuesFromEF[property.Metadata.Name] = property.OriginalValue;
                                    currentValues[property.Metadata.Name] = property.CurrentValue;

//                                    Console.WriteLine($"AUDIT DEBUG DIRECT: {property.Metadata.Name} = '{property.OriginalValue}' -> '{property.CurrentValue}'");
                                }
                            }

                            if (originalValuesFromEF.Any())
                            {
                                auditEntity.PreviousState = System.Text.Json.JsonSerializer.Serialize(originalValuesFromEF, new System.Text.Json.JsonSerializerOptions
                                {
                                    WriteIndented = true,
                                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                                });

                                auditEntity.CurrentState = System.Text.Json.JsonSerializer.Serialize(currentValues, new System.Text.Json.JsonSerializerOptions
                                {
                                    WriteIndented = true,
                                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                                });

//                                Console.WriteLine($"AUDIT DEBUG DIRECT SUCCESS: Captured {originalValuesFromEF.Count} changes");
//                                Console.WriteLine($"AUDIT DEBUG PREVIOUS (DIRECT): {auditEntity.PreviousState}");
//                                Console.WriteLine($"AUDIT DEBUG CURRENT (DIRECT): {auditEntity.CurrentState}");
                                return;
                            }
                        }
                    }
                }
                else
                {
//                    Console.WriteLine($"AUDIT DEBUG: Could not find context property in EF audit event");
                }
            }

            // Fallback to the Changes collection approach
            if (entry.Changes != null && entry.Changes.Any())
            {
                var originalValuesDict = entry.Changes.ToDictionary(c => c.ColumnName, c => c.OriginalValue);
                var newValues = entry.Changes.ToDictionary(c => c.ColumnName, c => c.NewValue);

                auditEntity.PreviousState = System.Text.Json.JsonSerializer.Serialize(originalValuesDict, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                auditEntity.CurrentState = System.Text.Json.JsonSerializer.Serialize(newValues, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

//                Console.WriteLine($"AUDIT DEBUG UPDATE (FALLBACK): Entity={entry.EntityType?.Name}, ID={primaryKey}");
//                Console.WriteLine($"AUDIT DEBUG CHANGES COUNT: {entry.Changes.Count()}");

                // Debug individual changes to show the timing issue
                foreach (var change in entry.Changes)
                {
//                    Console.WriteLine($"AUDIT DEBUG CHANGE: {change.ColumnName} = '{change.OriginalValue}' -> '{change.NewValue}'");
                }

//                Console.WriteLine($"AUDIT DEBUG PREVIOUS (FALLBACK): {auditEntity.PreviousState}");
//                Console.WriteLine($"AUDIT DEBUG CURRENT (FALLBACK): {auditEntity.CurrentState}");
            }
            else
            {
                // Last resort fallback
                auditEntity.PreviousState = null;
                auditEntity.CurrentState = GetEntityStateJson(entry.Entity);

//                Console.WriteLine($"AUDIT DEBUG FALLBACK: No changes available for {entry.EntityType?.Name}");
            }
        }

        /// <summary>
        /// Get primary key value from an entity entry
        /// </summary>
        private static string GetEntityPrimaryKey(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entityEntry)
        {
            try
            {
                var keyProperties = entityEntry.Metadata.FindPrimaryKey()?.Properties;
                if (keyProperties == null || !keyProperties.Any())
                    return null;

                if (keyProperties.Count == 1)
                {
                    return entityEntry.Property(keyProperties.First().Name).CurrentValue?.ToString();
                }

                var keyValues = keyProperties.Select(p => entityEntry.Property(p.Name).CurrentValue?.ToString() ?? "null");
                return string.Join("_", keyValues);
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"AUDIT ERROR: Failed to get entity primary key: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Extract primary key value from audit entry
        /// </summary>
        private static object GetPrimaryKeyFromEntry(EventEntry entry)
        {
            try
            {
                if (entry.PrimaryKey is System.Collections.Generic.Dictionary<string, object> pkDict)
                {
                    if (pkDict.Count == 1)
                    {
                        return pkDict.Values.First();
                    }
                    else
                    {
                        // Composite key - combine values
                        return string.Join("_", pkDict.Values.Select(v => v?.ToString() ?? "null"));
                    }
                }

                return entry.PrimaryKey;
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"AUDIT ERROR: Failed to extract primary key: {ex.Message}");
                return entry.PrimaryKey;
            }
        }

        /// <summary>
        /// Get previous state JSON for update operations using Entity Framework original values
        /// </summary>
        private static string GetPreviousStateJson(EventEntry entry)
        {
            // AUDIT FIX: Use Entity Framework's original values instead of entry.Changes
            // entry.Changes is unreliable because it may be captured after EF updates the tracking

            if (entry.Entity == null)
                return null;

            try
            {
                // Try to get original values from the audit event's custom fields
                if (entry.CustomFields != null && entry.CustomFields.ContainsKey("OriginalValues"))
                {
                    var originalValues = entry.CustomFields["OriginalValues"];
                    return System.Text.Json.JsonSerializer.Serialize(originalValues, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    });
                }

                // Fallback: Use entry.Changes if available (though this may be unreliable)
                if (entry.Changes != null && entry.Changes.Any())
                {
                    var previousState = entry.Changes.ToDictionary(
                        c => c.ColumnName,
                        c => c.OriginalValue
                    );

                    return System.Text.Json.JsonSerializer.Serialize(previousState, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    });
                }

                return null;
            }
            catch (Exception ex)
            {
//                Console.WriteLine($"AUDIT ERROR: Failed to get previous state: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get current state JSON for update operations
        /// </summary>
        private static string GetCurrentStateJson(EventEntry entry)
        {
            // For current state, we can safely use the entity object since it contains the new values
            return GetEntityStateJson(entry.Entity);
        }

        /// <summary>
        /// Get entity state JSON from the actual entity object
        /// </summary>
        private static string GetEntityStateJson(object entity)
        {
            if (entity == null)
                return null;

            try
            {
                return System.Text.Json.JsonSerializer.Serialize(entity, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                });
            }
            catch (Exception)
            {
                // Fallback to simple property extraction if serialization fails
                return GetEntityPropertiesJson(entity);
            }
        }

        /// <summary>
        /// Get entity properties as JSON (fallback method)
        /// </summary>
        private static string GetEntityPropertiesJson(object entity)
        {
            if (entity == null)
                return null;

            try
            {
                var properties = entity.GetType().GetProperties()
                    .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                    .ToDictionary(
                        p => p.Name,
                        p => {
                            try
                            {
                                return p.GetValue(entity);
                            }
                            catch
                            {
                                return null;
                            }
                        }
                    );

                return System.Text.Json.JsonSerializer.Serialize(properties, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }
            catch
            {
                return "{}";
            }
        }

        /// <summary>
        /// Add custom fields to audit scope
        /// </summary>
        private static void AddCustomAuditFields(AuditScope scope)
        {
            // Add application name
            scope.SetCustomField("ApplicationName", "CalibrationSaaS");

            // Add timestamp
            scope.SetCustomField("AuditTimestamp", DateTime.UtcNow);

            // Add correlation ID for tracking related operations
            scope.SetCustomField("CorrelationId", Guid.NewGuid().ToString());

            // Add HTTP context information if available
            try
            {
                if (_serviceProvider != null)
                {
                    var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
                    if (httpContextAccessor?.HttpContext != null)
                    {
                        var httpContext = httpContextAccessor.HttpContext;

                        // Capture HTTP request information
                        scope.SetCustomField("HttpMethod", httpContext.Request.Method);
                        scope.SetCustomField("RequestUrl", $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}");
                        scope.SetCustomField("ClientIpAddress", GetClientIpAddress(httpContext));
                        scope.SetCustomField("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                        scope.SetCustomField("Referer", httpContext.Request.Headers["Referer"].ToString());

                        // Capture browser information
                        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                        if (!string.IsNullOrEmpty(userAgent))
                        {
                            scope.SetCustomField("BrowserInfo", ParseBrowserInfo(userAgent));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the audit process
                scope.SetCustomField("HttpContextError", ex.Message);
            }

            // Add user context information if available
            try
            {
//                Console.WriteLine("AUDIT DEBUG: Attempting to get user context...");
                if (_serviceProvider != null)
                {
                    var userContextProvider = _serviceProvider.GetService<IUserContextProvider>();
//                    Console.WriteLine($"AUDIT DEBUG: UserContextProvider is {(userContextProvider != null ? "available" : "null")}");

                    if (userContextProvider != null)
                    {
                        // Get current user information
                        var userName = userContextProvider.GetCurrentUserName();
                        var userId = userContextProvider.GetUserId();
                        var tenantId = userContextProvider.GetTenantId();
                        var tenantName = userContextProvider.GetTenantName();
                        var techId = userContextProvider.GetTechId();
                        var isAuthenticated = userContextProvider.IsAuthenticated();

//                        Console.WriteLine($"AUDIT DEBUG: Retrieved user context - UserName: '{userName}', UserId: '{userId}', IsAuthenticated: {isAuthenticated}");

                        // Set custom fields for user context
                        if (!string.IsNullOrEmpty(userName))
                        {
                            scope.SetCustomField("UserName", userName);
//                            Console.WriteLine($"AUDIT DEBUG: Set UserName custom field to '{userName}'");
                        }
                        else
                        {
//                            Console.WriteLine("AUDIT DEBUG: UserName is null or empty");
                        }

                        if (!string.IsNullOrEmpty(userId))
                        {
                            scope.SetCustomField("UserId", userId);
                        }

                        if (tenantId.HasValue)
                        {
                            scope.SetCustomField("TenantId", tenantId.Value);
                        }

                        if (!string.IsNullOrEmpty(tenantName))
                        {
                            scope.SetCustomField("TenantName", tenantName);
                        }

                        if (techId.HasValue)
                        {
                            scope.SetCustomField("TechId", techId.Value);
                        }

                        // Add authentication status
                        scope.SetCustomField("IsAuthenticated", isAuthenticated);
                    }
                    else
                    {
//                        Console.WriteLine("AUDIT DEBUG: UserContextProvider service not found");
                    }
                }
                else
                {
//                    Console.WriteLine("AUDIT DEBUG: ServiceProvider is null");
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the audit process
//                Console.WriteLine($"AUDIT DEBUG: Error getting user context: {ex.Message}");
                scope.SetCustomField("UserContextError", ex.Message);
            }
        }

        /// <summary>
        /// Get client IP address from HTTP context
        /// </summary>
        private static string GetClientIpAddress(HttpContext httpContext)
        {
            try
            {
                // Check for forwarded IP first (for load balancers/proxies)
                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    return forwardedFor.Split(',')[0].Trim();
                }

                // Check for real IP header
                var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
                if (!string.IsNullOrEmpty(realIp))
                {
                    return realIp;
                }

                // Fall back to connection remote IP
                return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Parse browser information from User-Agent string
        /// </summary>
        private static string ParseBrowserInfo(string userAgent)
        {
            try
            {
                if (string.IsNullOrEmpty(userAgent))
                    return "Unknown";

                // Simple browser detection
                if (userAgent.Contains("Chrome") && !userAgent.Contains("Edg"))
                    return "Chrome";
                else if (userAgent.Contains("Firefox"))
                    return "Firefox";
                else if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
                    return "Safari";
                else if (userAgent.Contains("Edg"))
                    return "Edge";
                else if (userAgent.Contains("Opera") || userAgent.Contains("OPR"))
                    return "Opera";
                else if (userAgent.Contains("Trident") || userAgent.Contains("MSIE"))
                    return "Internet Explorer";
                else
                    return "Other";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
