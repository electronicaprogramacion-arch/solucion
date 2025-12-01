# CalibrationSaaS Audit Configuration Guide

## Overview

The CalibrationSaaS application includes comprehensive audit logging capabilities using Audit.NET. This guide explains how to configure, customize, and use the audit system.

## Architecture

The audit system consists of:
- **Audit.NET Framework**: Core auditing engine
- **AuditLog Entity**: Database table storing audit records
- **AuditConfiguration**: Configuration and setup
- **UserContextProvider**: User context tracking
- **Audit Log UI**: Search and view interface

## Configuration

### Basic Setup

Audit configuration is handled in `AuditConfiguration.cs`:

```csharp
// Configure Audit.NET global settings
Audit.Core.Configuration.Setup()
    .UseEntityFramework(ef => ef
        .AuditTypeMapper(t => typeof(AuditLog))
        .AuditEntityAction<AuditLog>((ev, entry, auditEntity) =>
        {
            MapAuditEvent(ev, entry, auditEntity);
        })
        .IgnoreMatchedProperties(true))
    .WithCreationPolicy(EventCreationPolicy.InsertOnEnd);
```

### Audited Entities

The following entities are currently audited:
- **WorkOrder**: Work order operations
- **Customer**: Customer management
- **PieceOfEquipment**: Equipment tracking
- **WorkOrderDetail**: Work order details
- **Quote**: Quote management
- **QuoteDetail**: Quote line items

### Custom Fields

The system automatically captures:
- **User Information**: UserName, UserId, TenantId
- **HTTP Context**: IP Address, Browser, Request URL
- **System Info**: Application name, timestamp, correlation ID
- **Performance**: Execution duration

## Database Schema

### AuditLogs Table

```sql
CREATE TABLE AuditLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL,
    UserName NVARCHAR(255),
    EntityType NVARCHAR(255) NOT NULL,
    EntityId NVARCHAR(255) NOT NULL,
    ActionType NVARCHAR(50) NOT NULL,
    PreviousState NVARCHAR(MAX),
    CurrentState NVARCHAR(MAX),
    ApplicationName NVARCHAR(255),
    TenantId INT,
    ExecutionDuration INT,
    ClientIpAddress NVARCHAR(45),
    CorrelationId NVARCHAR(255),
    AuditData NVARCHAR(MAX),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
```

### Indexes

```sql
-- Performance indexes
CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp);
CREATE INDEX IX_AuditLogs_EntityType ON AuditLogs(EntityType);
CREATE INDEX IX_AuditLogs_UserName ON AuditLogs(UserName);
CREATE INDEX IX_AuditLogs_TenantId ON AuditLogs(TenantId);
```

## Customization

### Adding New Audited Entities

1. **Update IsAuditableEntity method**:
```csharp
private static bool IsAuditableEntity(string entityTypeName)
{
    var auditableEntities = new[]
    {
        "WorkOrder",
        "Customer", 
        "PieceOfEquipment",
        "YourNewEntity" // Add here
    };
    return auditableEntities.Contains(entityTypeName);
}
```

2. **Update Entity Type Provider**:
```csharp
public List<EntityTypeOption> GetAuditedEntityTypes()
{
    return new List<EntityTypeOption>
    {
        new() { Value = "WorkOrder", Text = "Work Orders" },
        new() { Value = "YourNewEntity", Text = "Your Entity" }
    };
}
```

### Adding Custom Audit Fields

Modify the `AddCustomAuditFields` method:

```csharp
private static void AddCustomAuditFields(AuditScope scope)
{
    // Existing fields...
    
    // Add your custom fields
    scope.SetCustomField("CustomField1", "Value1");
    scope.SetCustomField("CustomField2", GetCustomValue());
}
```

### Disabling Audit for Specific Operations

```csharp
// Disable audit for a specific operation
using (var scope = AuditScope.Create("CustomOperation", () => { }))
{
    scope.Discard(); // This won't be audited
    // Your operation here
}
```

## User Interface

### Audit Log Search

The audit log interface (`/Log`) provides:
- **Date Range Filtering**: Search by timestamp
- **Entity Type Filtering**: Filter by entity type
- **User Filtering**: Search by username
- **Action Type Filtering**: Filter by Create/Update/Delete
- **Advanced Filters**: IP address, correlation ID, execution duration

### Audit Details Modal

Click "Details" to view:
- **Overview Tab**: Basic audit information
- **Previous State Tab**: Entity state before change
- **Current State Tab**: Entity state after change
- **Changes Tab**: Side-by-side comparison

### Export Features

- **Copy to Clipboard**: Copy audit data
- **Download JSON**: Export audit details
- **Excel Export**: Export search results

## Performance Considerations

### Selective Auditing

The system uses selective auditing for performance:
- Only critical entities are audited
- Non-essential entities are excluded
- Custom filtering reduces audit volume

### Database Optimization

- Proper indexing on search columns
- Partitioning for large datasets
- Regular cleanup of old audit records

### Configuration Options

```json
{
  "Audit": {
    "Enabled": true,
    "IncludeEntityObjects": true,
    "SelectiveAuditing": true,
    "MaxExecutionDuration": 5000
  }
}
```

## Troubleshooting

### Common Issues

1. **No Audit Records**: Check entity is in auditable list
2. **Performance Issues**: Review selective auditing configuration
3. **Missing User Context**: Verify UserContextProvider registration
4. **HTTP Context Missing**: Ensure IHttpContextAccessor is registered

### Debugging

Enable detailed logging:
```csharp
Audit.Core.Configuration.Setup()
    .WithCreationPolicy(EventCreationPolicy.InsertOnEnd)
    .WithAction(x => x.OnEventSaved(scope => 
    {
        Console.WriteLine($"Audit saved: {scope.EventType}");
    }));
```

## Security Considerations

- Audit logs contain sensitive data - secure appropriately
- Implement proper access controls for audit viewing
- Consider data retention policies
- Encrypt sensitive audit data if required

## Maintenance

### Regular Tasks

1. **Monitor Performance**: Check audit log search response times
2. **Clean Old Records**: Implement retention policy
3. **Review Audited Entities**: Update list as needed
4. **Validate Indexes**: Ensure optimal performance

### Backup Strategy

- Include AuditLogs table in backup procedures
- Consider separate backup schedule for audit data
- Test restore procedures regularly

## API Reference

### Key Classes

- `AuditConfiguration`: Main configuration class
- `AuditLogService`: Service for audit operations
- `UserContextProvider`: User context tracking
- `EntityTypeProvider`: Entity type management

### Key Methods

- `ConfigureAudit()`: Setup audit system
- `SearchAuditLogs()`: Search audit records
- `GetAuditedEntityTypes()`: Get entity types
- `AddCustomAuditFields()`: Add custom fields

For more information, see the source code documentation and inline comments.
