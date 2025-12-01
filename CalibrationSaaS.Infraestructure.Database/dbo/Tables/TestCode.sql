CREATE TABLE [dbo].[TestCode] (
    [TestCodeID]            INT            IDENTITY (1, 1) NOT NULL,
    [Code]                  NVARCHAR (MAX) NULL,
    [Description]           NVARCHAR (MAX) NULL,
    [RangeMIn]              FLOAT (53)     NULL,
    [RangeMax]              FLOAT (53)     NULL,
    [CalibrationTypeID]     INT            NULL,
    [CalibrationSubtTypeID] INT            NULL,
    [Accredited]            BIT            NULL,
    [UnitOfMeasureID]       INT            NULL,
    [ProcedureID]           INT            NULL,
    [EquipmentTypeID]       INT            NULL,
    CONSTRAINT [PK_TestCode] PRIMARY KEY CLUSTERED ([TestCodeID] ASC),
    CONSTRAINT [FK_TestCode_CalibrationType_CalibrationTypeID] FOREIGN KEY ([CalibrationTypeID]) REFERENCES [dbo].[CalibrationType] ([CalibrationTypeId]),
    CONSTRAINT [FK_TestCode_Procedure_ProcedureID] FOREIGN KEY ([ProcedureID]) REFERENCES [dbo].[Procedure] ([ProcedureID]),
    CONSTRAINT [FK_TestCode_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY ([UnitOfMeasureID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
);




GO
CREATE NONCLUSTERED INDEX [IX_TestCode_CalibrationTypeID]
    ON [dbo].[TestCode]([CalibrationTypeID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TestCode_ProcedureID]
    ON [dbo].[TestCode]([ProcedureID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TestCode_UnitOfMeasureID]
    ON [dbo].[TestCode]([UnitOfMeasureID] ASC);

