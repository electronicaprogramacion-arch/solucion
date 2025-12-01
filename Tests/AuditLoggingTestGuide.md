# Audit Logging System Test Guide

## Overview
This guide provides step-by-step instructions for testing the CalibrationSaaS audit logging system to ensure it's working correctly.

## Prerequisites

### ✅ **System Requirements**
- CalibrationSaaS database with AuditLogs table created
- All three services running:
  - Identity Provider
  - gRPC Service (CalibrationSaaS.Service)
  - Blazor Application (CalibrationSaaS.Infraestructure.Blazor)

### ✅ **Database Setup**
1. Ensure the AuditLogs table exists:
   ```sql
   -- Run this to check
   SELECT * FROM sysobjects WHERE name='AuditLogs' AND xtype='U'
   ```
2. If not exists, run: `src/CalibrationSaaS/Database/Scripts/CreateAuditLogTable.sql`

## Testing Steps

### 🧪 **Step 1: Create Test Data**

Run the test data creation script:
```sql
-- Execute this script in SQL Server Management Studio
-- File: src/CalibrationSaaS/Tests/CreateTestAuditEntries.sql
```

This script creates 8 sample audit entries covering:
- Customer creation and updates
- Work Order lifecycle
- Equipment management
- User management actions
- Delete operations

### 🔍 **Step 2: Verify Database Setup**

Run the verification script:
```sql
-- Execute this script to verify the system
-- File: src/CalibrationSaaS/Tests/VerifyAuditLogging.sql
```

Expected results:
- ✅ AuditLogs table exists with proper schema
- ✅ Indexes are created correctly
- ✅ Test data is present (8 entries)
- ✅ Query performance is acceptable

### 🖥️ **Step 3: Test the UI**

1. **Navigate to Audit Logs Page**
   - Open the Blazor application
   - Go to Settings → Audit Logs
   - URL: `https://localhost:5001/Settings/AuditLogs`

2. **Test Basic Functionality**
   - ✅ Page loads without errors
   - ✅ Grid displays audit log entries
   - ✅ Pagination works correctly
   - ✅ Sorting by columns works

3. **Test Search and Filtering**
   - **Entity Type Filter**: Select "Customer" → Should show customer-related entries
   - **Entity ID Filter**: Enter "TEST-CUST-001" → Should show specific customer entries
   - **Date Range Filter**: Set last 24 hours → Should show recent entries
   - **Clear Filters**: Should reset and show all entries

4. **Test Data Display**
   - ✅ Timestamp shows correctly
   - ✅ User names are displayed
   - ✅ Entity types and IDs are shown
   - ✅ Action types (Create, Update, Delete) are visible
   - ✅ JSON state data is properly formatted

### 🔄 **Step 4: Test Real-Time Audit Logging**

1. **Create a New Customer**
   - Navigate to Customer management
   - Create a new customer
   - Check Audit Logs → Should see new "Create" entry

2. **Update the Customer**
   - Edit the customer you just created
   - Change some fields and save
   - Check Audit Logs → Should see new "Update" entry

3. **Create a Work Order**
   - Navigate to Work Order management
   - Create a new work order
   - Check Audit Logs → Should see new "Create" entry

### 📊 **Step 5: Verify User Context**

Check that user context information is captured:
- ✅ **UserName**: Should show the logged-in user
- ✅ **UserId**: Should be populated
- ✅ **TenantId**: Should match the current tenant
- ✅ **ApplicationName**: Should be "CalibrationSaaS"

### 🚀 **Step 6: Performance Testing**

1. **Load Testing**
   - Create multiple audit entries (100+)
   - Test search and filtering performance
   - Verify pagination works with large datasets

2. **Query Performance**
   - Run the verification script
   - Check query duration (should be < 100ms for typical queries)

## Expected Results

### ✅ **Success Criteria**

1. **Database Integration**
   - AuditLogs table exists and is properly configured
   - Test data is created successfully
   - Indexes improve query performance

2. **Service Integration**
   - gRPC service returns audit log data
   - Search and filtering work correctly
   - Pagination and sorting function properly

3. **UI Integration**
   - Audit Logs page loads without errors
   - Data is displayed in a user-friendly format
   - Search and filter controls work as expected

4. **Real-Time Logging**
   - New operations create audit entries automatically
   - User context information is captured correctly
   - JSON state data is properly formatted

5. **User Context**
   - UserName, UserId, TenantId are populated
   - Authentication status is tracked
   - Application name is set correctly

## Troubleshooting

### ❌ **Common Issues**

1. **No Audit Entries Displayed**
   - Check if AuditLogs table exists
   - Verify gRPC service is running
   - Check database connection string
   - Run VerifyAuditLogging.sql for diagnostics

2. **Missing User Context**
   - Verify UserContextProvider is registered in DI
   - Check AuditConfiguration.SetServiceProvider() is called
   - Ensure user is properly authenticated

3. **Performance Issues**
   - Check if indexes are created
   - Verify query optimization
   - Consider data archiving for large datasets

4. **UI Not Loading**
   - Check browser console for errors
   - Verify Blazor application is running
   - Check network connectivity to gRPC service

### 🔧 **Diagnostic Queries**

```sql
-- Check recent audit entries
SELECT TOP 10 * FROM AuditLogs ORDER BY Timestamp DESC

-- Check user context population
SELECT 
    COUNT(*) as Total,
    COUNT(UserName) as HasUserName,
    COUNT(UserId) as HasUserId,
    COUNT(TenantId) as HasTenantId
FROM AuditLogs

-- Check entity type distribution
SELECT EntityType, COUNT(*) as Count
FROM AuditLogs
GROUP BY EntityType
ORDER BY Count DESC
```

## Test Data Cleanup

To remove test data after testing:
```sql
-- Remove test audit entries
DELETE FROM AuditLogs 
WHERE UserName LIKE 'test.%' OR EntityId LIKE 'TEST-%'
```

## Next Steps

After successful testing:
1. ✅ Document any issues found
2. ✅ Configure production settings
3. ✅ Set up monitoring and alerting
4. ✅ Train users on the audit log functionality
5. ✅ Establish data retention policies

## Support

For issues or questions:
- Check the AuditNetMigrationGuide.md for implementation details
- Review the AuditLogIntegrationTest.cs for code examples
- Consult the Audit.NET documentation for advanced configuration
