CREATE TABLE [dbo].[Micro] (
    [SequenceID]                                 INT NOT NULL,
    [CalibrationSubTypeId]                       INT NOT NULL,
    [WorkOrderDetailId]                          INT NOT NULL,
    [NumberOfSamples]                            INT NOT NULL,
    [TestPointID]                                INT NULL,
    [UnitOfMeasureId]                            INT NULL,
    [CalibrationUncertaintyValueUnitOfMeasureId] INT NULL,
    [UncertaintyID]                              INT NULL,
    CONSTRAINT [PK_Micro] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC),
    CONSTRAINT [FK_Micro_TestPoint_TestPointID] FOREIGN KEY ([TestPointID]) REFERENCES [dbo].[TestPoint] ([TestPointID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Micro_TestPointID]
    ON [dbo].[Micro]([TestPointID] ASC);

