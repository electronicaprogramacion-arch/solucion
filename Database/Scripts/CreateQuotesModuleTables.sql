-- Create Quotes Module Tables
-- This script creates the Quote, QuoteItem, PriceType, and PriceTypePrice tables

-- Create PriceType table first (referenced by QuoteItem)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PriceType' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[PriceType] (
        [PriceTypeId] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [SortOrder] INT NOT NULL DEFAULT 0,
        [RequiresTravel] BIT NOT NULL DEFAULT 0,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [ModifiedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        
        CONSTRAINT [PK_PriceType] PRIMARY KEY CLUSTERED ([PriceTypeId] ASC),
        CONSTRAINT [UQ_PriceType_Name] UNIQUE ([Name])
    );
    
    CREATE NONCLUSTERED INDEX [IX_PriceType_IsActive] ON [dbo].[PriceType] ([IsActive]);
    CREATE NONCLUSTERED INDEX [IX_PriceType_SortOrder] ON [dbo].[PriceType] ([SortOrder]);
    
    PRINT 'PriceType table created successfully.';
END
ELSE
BEGIN
    PRINT 'PriceType table already exists.';
END

-- Create Quote table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Quote' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Quote] (
        [QuoteID] INT IDENTITY(1,1) NOT NULL,
        [QuoteNumber] NVARCHAR(50) NOT NULL,
        [CustomerID] INT NULL,
        [CustomerAddressId] INT NULL,
        [CustomerName] NVARCHAR(200) NULL,
        [TotalCost] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'Draft',
        [Priority] NVARCHAR(50) NOT NULL DEFAULT 'Low',
        [EstimatedDelivery] DATETIME2(7) NULL,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [ModifiedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [Notes] NVARCHAR(MAX) NULL,
        [ServiceType] NVARCHAR(20) NOT NULL DEFAULT 'Laboratory',
        [IsActive] BIT NOT NULL DEFAULT 1,
        
        CONSTRAINT [PK_Quote] PRIMARY KEY CLUSTERED ([QuoteID] ASC),
        CONSTRAINT [UQ_Quote_QuoteNumber] UNIQUE ([QuoteNumber])
    );
    
    CREATE NONCLUSTERED INDEX [IX_Quote_CustomerID] ON [dbo].[Quote] ([CustomerID]);
    CREATE NONCLUSTERED INDEX [IX_Quote_Status] ON [dbo].[Quote] ([Status]);
    CREATE NONCLUSTERED INDEX [IX_Quote_CreatedDate] ON [dbo].[Quote] ([CreatedDate]);
    CREATE NONCLUSTERED INDEX [IX_Quote_IsActive] ON [dbo].[Quote] ([IsActive]);
    
    PRINT 'Quote table created successfully.';
END
ELSE
BEGIN
    PRINT 'Quote table already exists.';
END

-- Create QuoteItem table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='QuoteItem' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[QuoteItem] (
        [QuoteItemID] INT IDENTITY(1,1) NOT NULL,
        [QuoteID] INT NOT NULL,
        [PieceOfEquipmentID] NVARCHAR(50) NULL,
        [Quantity] INT NOT NULL DEFAULT 1,
        [UnitPrice] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [ItemDescription] NVARCHAR(500) NULL,
        [ParentQuoteItemID] INT NULL,
        [IsParent] BIT NOT NULL DEFAULT 0,
        [SortOrder] INT NOT NULL DEFAULT 0,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [EquipmentTypeDisplay] NVARCHAR(200) NULL,
        [PriceTypeId] INT NULL,
        
        CONSTRAINT [PK_QuoteItem] PRIMARY KEY CLUSTERED ([QuoteItemID] ASC),
        CONSTRAINT [FK_QuoteItem_Quote] FOREIGN KEY ([QuoteID]) REFERENCES [dbo].[Quote] ([QuoteID]) ON DELETE CASCADE,
        CONSTRAINT [FK_QuoteItem_ParentQuoteItem] FOREIGN KEY ([ParentQuoteItemID]) REFERENCES [dbo].[QuoteItem] ([QuoteItemID]),
        CONSTRAINT [FK_QuoteItem_PriceType] FOREIGN KEY ([PriceTypeId]) REFERENCES [dbo].[PriceType] ([PriceTypeId]),
        CONSTRAINT [CK_QuoteItem_Quantity] CHECK ([Quantity] > 0),
        CONSTRAINT [CK_QuoteItem_UnitPrice] CHECK ([UnitPrice] >= 0)
    );
    
    CREATE NONCLUSTERED INDEX [IX_QuoteItem_QuoteID] ON [dbo].[QuoteItem] ([QuoteID]);
    CREATE NONCLUSTERED INDEX [IX_QuoteItem_PriceTypeId] ON [dbo].[QuoteItem] ([PriceTypeId]);
    CREATE NONCLUSTERED INDEX [IX_QuoteItem_ParentQuoteItemID] ON [dbo].[QuoteItem] ([ParentQuoteItemID]);
    
    PRINT 'QuoteItem table created successfully.';
END
ELSE
BEGIN
    PRINT 'QuoteItem table already exists.';
END

-- Create PriceTypePrice table (for customer-specific pricing)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PriceTypePrice' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[PriceTypePrice] (
        [PriceTypePriceId] INT IDENTITY(1,1) NOT NULL,
        [PriceTypeId] INT NOT NULL,
        [CustomerID] INT NULL,
        [EquipmentTemplateID] INT NULL,
        [Price] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        [ModifiedBy] NVARCHAR(100) NOT NULL DEFAULT 'System',
        
        CONSTRAINT [PK_PriceTypePrice] PRIMARY KEY CLUSTERED ([PriceTypePriceId] ASC),
        CONSTRAINT [FK_PriceTypePrice_PriceType] FOREIGN KEY ([PriceTypeId]) REFERENCES [dbo].[PriceType] ([PriceTypeId]) ON DELETE CASCADE,
        CONSTRAINT [CK_PriceTypePrice_Price] CHECK ([Price] >= 0)
    );
    
    CREATE NONCLUSTERED INDEX [IX_PriceTypePrice_PriceTypeId] ON [dbo].[PriceTypePrice] ([PriceTypeId]);
    CREATE NONCLUSTERED INDEX [IX_PriceTypePrice_CustomerID] ON [dbo].[PriceTypePrice] ([CustomerID]);
    CREATE NONCLUSTERED INDEX [IX_PriceTypePrice_EquipmentTemplateID] ON [dbo].[PriceTypePrice] ([EquipmentTemplateID]);
    CREATE NONCLUSTERED INDEX [IX_PriceTypePrice_IsActive] ON [dbo].[PriceTypePrice] ([IsActive]);
    
    PRINT 'PriceTypePrice table created successfully.';
END
ELSE
BEGIN
    PRINT 'PriceTypePrice table already exists.';
END

-- Insert default price types
IF NOT EXISTS (SELECT 1 FROM [dbo].[PriceType] WHERE [Name] = 'On Site')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [RequiresTravel], [SortOrder])
    VALUES ('On Site', 'On-site calibration services', 1, 1);
    PRINT 'Default price type "On Site" inserted.';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[PriceType] WHERE [Name] = 'Laboratory')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [RequiresTravel], [SortOrder])
    VALUES ('Laboratory', 'Laboratory calibration services', 0, 2);
    PRINT 'Default price type "Laboratory" inserted.';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[PriceType] WHERE [Name] = 'Outsourcing')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [RequiresTravel], [SortOrder])
    VALUES ('Outsourcing', 'Outsourced calibration services', 0, 3);
    PRINT 'Default price type "Outsourcing" inserted.';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[PriceType] WHERE [Name] = 'Purchase')
BEGIN
    INSERT INTO [dbo].[PriceType] ([Name], [Description], [RequiresTravel], [SortOrder])
    VALUES ('Purchase', 'Equipment purchase', 0, 4);
    PRINT 'Default price type "Purchase" inserted.';
END

PRINT 'Quotes Module tables creation script completed successfully.';
