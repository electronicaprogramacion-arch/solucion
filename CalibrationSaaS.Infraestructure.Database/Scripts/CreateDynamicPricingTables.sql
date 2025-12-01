-- Create dynamic pricing tables for PriceType and PriceTypePrice
-- This script creates the tables needed for the dynamic pricing system

-- Create PriceType table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PriceType' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[PriceType] (
        [PriceTypeId] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [RequiresTravel] BIT NOT NULL DEFAULT 0,
        [SortOrder] INT NOT NULL DEFAULT 0,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NULL,
        [ModifiedBy] NVARCHAR(100) NULL,
        [IncludesTravel] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [PK_PriceType] PRIMARY KEY CLUSTERED ([PriceTypeId] ASC)
    );
    
    PRINT 'Created PriceType table';
END
ELSE
BEGIN
    PRINT 'PriceType table already exists';
END

-- Create PriceTypePrice table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PriceTypePrice' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[PriceTypePrice] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [PriceTypeId] INT NOT NULL,
        [EntityType] INT NOT NULL, -- 1 = PieceOfEquipment, 2 = EquipmentTemplate
        [EntityId] INT NOT NULL,
        [Price] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_PriceTypePrice] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_PriceTypePrice_PriceType] FOREIGN KEY ([PriceTypeId]) 
            REFERENCES [dbo].[PriceType] ([PriceTypeId]) ON DELETE CASCADE
    );
    
    -- Create unique index to prevent duplicate price entries for the same entity and price type
    CREATE UNIQUE NONCLUSTERED INDEX [IX_PriceTypePrice_Unique] 
    ON [dbo].[PriceTypePrice] ([PriceTypeId], [EntityType], [EntityId]);
    
    -- Create index for performance
    CREATE NONCLUSTERED INDEX [IX_PriceTypePrice_PriceTypeId] 
    ON [dbo].[PriceTypePrice] ([PriceTypeId]);
    
    PRINT 'Created PriceTypePrice table with indexes';
END
ELSE
BEGIN
    PRINT 'PriceTypePrice table already exists';
END

-- Insert default price types based on the screenshot provided
IF NOT EXISTS (SELECT * FROM [dbo].[PriceType] WHERE [Name] = 'Laboratory')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [IsActive], [RequiresTravel], [SortOrder], [IncludesTravel])
    VALUES ('Laboratory', 'Standard laboratory calibration service - equipment brought to our facility', 1, 0, 1, 0);
    
    PRINT 'Inserted Laboratory price type';
END

IF NOT EXISTS (SELECT * FROM [dbo].[PriceType] WHERE [Name] = 'On Site')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [IsActive], [RequiresTravel], [SortOrder], [IncludesTravel])
    VALUES ('On Site', 'On-site calibration service performed at customer location', 1, 1, 2, 0);
    
    PRINT 'Inserted On Site price type';
END

IF NOT EXISTS (SELECT * FROM [dbo].[PriceType] WHERE [Name] = 'Outsourcing')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [IsActive], [RequiresTravel], [SortOrder], [IncludesTravel])
    VALUES ('Outsourcing', 'Calibration services outsourced to external providers', 1, 0, 3, 0);
    
    PRINT 'Inserted Outsourcing price type';
END

IF NOT EXISTS (SELECT * FROM [dbo].[PriceType] WHERE [Name] = 'Purchase')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [IsActive], [RequiresTravel], [SortOrder], [IncludesTravel])
    VALUES ('Purchase', 'Equipment purchase or replacement instead of calibration', 1, 0, 4, 0);
    
    PRINT 'Inserted Purchase price type';
END

PRINT 'Dynamic pricing tables creation completed successfully';
