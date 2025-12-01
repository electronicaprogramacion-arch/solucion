-- =============================================
-- Address Search Performance Optimization Indexes
-- =============================================
-- This script creates indexes to optimize the Address_Search modal performance
-- when using AddressCustomerViewModel with JOIN queries

-- Check if indexes exist before creating them
-- =============================================

-- 1. Index on Address.AggregateID for JOIN performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Address') AND name = 'IX_Address_AggregateID_Performance')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Address_AggregateID_Performance] 
    ON [dbo].[Address] ([AggregateID])
    INCLUDE ([AddressId], [StreetAddress1], [City], [State], [ZipCode], [Country], [IsDelete])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    PRINT 'Created index IX_Address_AggregateID_Performance on Address table'
END
ELSE
BEGIN
    PRINT 'Index IX_Address_AggregateID_Performance already exists on Address table'
END

-- 2. Index on CustomerAggregates for JOIN performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('CustomerAggregates') AND name = 'IX_CustomerAggregates_AggregateID_CustomerID')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CustomerAggregates_AggregateID_CustomerID] 
    ON [dbo].[CustomerAggregates] ([AggregateID], [CustomerID])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    PRINT 'Created index IX_CustomerAggregates_AggregateID_CustomerID on CustomerAggregates table'
END
ELSE
BEGIN
    PRINT 'Index IX_CustomerAggregates_AggregateID_CustomerID already exists on CustomerAggregates table'
END

-- 3. Index on Customer.CustomerID for JOIN performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Customer') AND name = 'IX_Customer_CustomerID_Performance')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Customer_CustomerID_Performance] 
    ON [dbo].[Customer] ([CustomerID])
    INCLUDE ([Name], [CustomID], [Description], [IsDelete])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    PRINT 'Created index IX_Customer_CustomerID_Performance on Customer table'
END
ELSE
BEGIN
    PRINT 'Index IX_Customer_CustomerID_Performance already exists on Customer table'
END

-- 4. Composite index on Address for search filtering
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Address') AND name = 'IX_Address_Search_Filter')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Address_Search_Filter] 
    ON [dbo].[Address] ([IsDelete], [StreetAddress1], [City], [State], [ZipCode])
    INCLUDE ([AddressId], [AggregateID], [Country], [Description], [Name])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    PRINT 'Created index IX_Address_Search_Filter on Address table'
END
ELSE
BEGIN
    PRINT 'Index IX_Address_Search_Filter already exists on Address table'
END

-- 5. Composite index on Customer for search filtering
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Customer') AND name = 'IX_Customer_Search_Filter')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Customer_Search_Filter] 
    ON [dbo].[Customer] ([IsDelete], [Name], [CustomID])
    INCLUDE ([CustomerID], [Description])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    PRINT 'Created index IX_Customer_Search_Filter on Customer table'
END
ELSE
BEGIN
    PRINT 'Index IX_Customer_Search_Filter already exists on Customer table'
END

-- 6. Update statistics for better query optimization
UPDATE STATISTICS [dbo].[Address]
UPDATE STATISTICS [dbo].[Customer]
UPDATE STATISTICS [dbo].[CustomerAggregates]

PRINT 'Updated statistics for Address, Customer, and CustomerAggregates tables'

-- =============================================
-- Performance Monitoring Query
-- =============================================
-- Use this query to monitor the performance of the Address search
/*
SELECT 
    COUNT(*) as TotalRecords,
    COUNT(CASE WHEN a.IsDelete = 0 THEN 1 END) as ActiveAddresses,
    COUNT(CASE WHEN c.IsDelete = 0 THEN 1 END) as ActiveCustomers
FROM Address a
LEFT JOIN CustomerAggregates ca ON a.AggregateID = ca.AggregateID
LEFT JOIN Customer c ON ca.CustomerID = c.CustomerID
*/

-- =============================================
-- Index Usage Monitoring
-- =============================================
-- Use this query to check if the indexes are being used
/*
SELECT 
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE s.database_id = DB_ID()
    AND i.object_id IN (OBJECT_ID('Address'), OBJECT_ID('Customer'), OBJECT_ID('CustomerAggregates'))
    AND i.name LIKE 'IX_%Performance%' OR i.name LIKE 'IX_%Search%'
ORDER BY s.user_seeks + s.user_scans + s.user_lookups DESC
*/

PRINT 'Address Search Performance Optimization completed successfully!'
