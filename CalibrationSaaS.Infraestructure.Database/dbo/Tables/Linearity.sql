CREATE TABLE [dbo].[Linearity] (
    [SequenceID]                                 INT            NOT NULL,
    [CalibrationSubTypeId]                       INT            NOT NULL,
    [WorkOrderDetailId]                          INT            NOT NULL CONSTRAINT [FK_Linearity_BalanceAndScaleCalibration_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[BalanceAndScaleCalibration] ([WorkOrderDetailId]),
    [Tolerance]                                  FLOAT (53)     NOT NULL,
    [NumberOfSamples]                            INT            NOT NULL,
    [TestPointID]                                INT            NOT NULL CONSTRAINT [FK_Linearity_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID]),
    [TotalNominal]                               FLOAT (53)     NOT NULL,
    [TotalActual]                                FLOAT (53)     NOT NULL,
    [SumUncertainty]                             FLOAT (53)     NOT NULL,
    [Quotient]                                   FLOAT (53)     NOT NULL,
    [Square]                                     FLOAT (53)     NOT NULL,
    [SumOfSquares]                               FLOAT (53)     NOT NULL,
    [TotalUncertainty]                           FLOAT (53)     NOT NULL,
    [ExpandedUncertainty]                        FLOAT (53)     NOT NULL,
    [CalibrationUncertaintyType]                 NVARCHAR (MAX) NULL,
    [CalibrationUncertaintyDistribution]         NVARCHAR (MAX) NULL,
    [CalibrationUncertaintyDivisor]              FLOAT (53)     NOT NULL,
    [UnitOfMeasureId]                            INT            NULL CONSTRAINT [FK_Linearity_UnitOfMeasure_UnitOfMeasureId] FOREIGN KEY([UnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    [CalibrationUncertaintyValueUnitOfMeasureId] INT            NULL CONSTRAINT [FK_Linearity_UnitOfMeasure_CalibrationUncertaintyValueUnitOfMeasureId] FOREIGN KEY([CalibrationUncertaintyValueUnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    [MinTolerance]                               FLOAT (53)     NOT NULL,
    [MaxTolerance]                               FLOAT (53)     NOT NULL,
    [MaxToleranceAsLeft]                         FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [MinToleranceAsLeft]                         FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    CONSTRAINT [PK_Linearity] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC)
);

GO
ALTER TABLE [dbo].[Linearity] ADD CONSTRAINT [FK_Linearity_BasicCalibrationResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId] FOREIGN KEY([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
REFERENCES [dbo].[BasicCalibrationResult] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]);

