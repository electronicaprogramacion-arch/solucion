-- Migration script to create ProcedureEquipment table
-- This script creates the missing ProcedureEquipment table with proper relationships

-- Create ProcedureEquipment table
CREATE TABLE [dbo].[ProcedureEquipment] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ProcedureID] INT NOT NULL,
    [PieceOfEquipmentID] VARCHAR(500) NOT NULL,
    [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(100) NULL,
    
    CONSTRAINT [PK_ProcedureEquipment] PRIMARY KEY CLUSTERED ([Id] ASC),
    
    -- Foreign key to Procedure table
    CONSTRAINT [FK_ProcedureEquipment_Procedure_ProcedureID] 
        FOREIGN KEY ([ProcedureID]) 
        REFERENCES [dbo].[Procedure] ([ProcedureID]) 
        ON DELETE CASCADE,
    
    -- Foreign key to PieceOfEquipment table
    CONSTRAINT [FK_ProcedureEquipment_PieceOfEquipment_PieceOfEquipmentID] 
        FOREIGN KEY ([PieceOfEquipmentID]) 
        REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]) 
        ON DELETE CASCADE
);

-- Create unique index to prevent duplicate associations
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProcedureEquipment_Unique] 
ON [dbo].[ProcedureEquipment] ([ProcedureID], [PieceOfEquipmentID]);

-- Create index for better query performance
CREATE NONCLUSTERED INDEX [IX_ProcedureEquipment_ProcedureID] 
ON [dbo].[ProcedureEquipment] ([ProcedureID]);

CREATE NONCLUSTERED INDEX [IX_ProcedureEquipment_PieceOfEquipmentID] 
ON [dbo].[ProcedureEquipment] ([PieceOfEquipmentID]);

PRINT 'ProcedureEquipment table created successfully with all constraints and indexes.';
