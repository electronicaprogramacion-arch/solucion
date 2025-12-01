CREATE TABLE [dbo].[Rockwell] (
    [SequenceID]                                 INT        NOT NULL,
    [CalibrationSubTypeId]                       INT        NOT NULL,
    [WorkOrderDetailId]                          INT        NOT NULL,
    [Tolerance]                                  FLOAT (53) NOT NULL,
    [NumberOfSamples]                            INT        NOT NULL,
    [TestPointID]                                INT        NULL,
    [Uncertainty]                                FLOAT (53) NOT NULL,
    [UnitOfMeasureId]                            INT        NULL,
    [CalibrationUncertaintyValueUnitOfMeasureId] INT        NULL,
    [MinTolerance]                               FLOAT (53) NOT NULL,
    [MaxTolerance]                               FLOAT (53) NOT NULL,
    [UncertaintyID]                              INT        NULL,
    CONSTRAINT [PK_Rockwell] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC)
);










GO



GO
CREATE NONCLUSTERED INDEX [IX_Rockwell_TestPointID]
    ON [dbo].[Rockwell]([TestPointID] ASC);

