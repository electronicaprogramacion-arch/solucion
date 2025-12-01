CREATE TABLE [dbo].[Force] (
    [SequenceID]                                 INT        NOT NULL,
    [CalibrationSubTypeId]                       INT        NOT NULL,
    [WorkOrderDetailId]                          INT        NOT NULL,
    [Tolerance]                                  FLOAT (53) NOT NULL,
    [NumberOfSamples]                            INT        NOT NULL,
    [TestPointID]                                INT        NOT NULL,
    [Uncertainty]                                FLOAT (53) NOT NULL,
    [UnitOfMeasureId]                            INT        NULL,
    [CalibrationUncertaintyValueUnitOfMeasureId] INT        NULL,
    [MinTolerance]                               FLOAT (53) NOT NULL,
    [MaxTolerance]                               FLOAT (53) NOT NULL,
    [ISO]                                        BIT        NOT NULL,
    [UncertaintyID]                              INT        NULL,
    CONSTRAINT [PK_Force] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC)
);

GO
ALTER TABLE [dbo].[Force]  ADD CONSTRAINT [FK_Force_ForceResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId] FOREIGN KEY([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
REFERENCES [dbo].[ForceResult] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]);