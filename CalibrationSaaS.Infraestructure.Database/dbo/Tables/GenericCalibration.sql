CREATE TABLE [dbo].[GenericCalibration] (
    [SequenceID]           INT           NOT NULL,
    [CalibrationSubTypeId] INT           NOT NULL,
    [WorkOrderDetailId]    INT           NOT NULL,
    [NumberOfSamples]      INT           NOT NULL,
    [TestPointID]          INT           NULL,
    [UnitOfMeasureId]      INT           NULL,
    [ComponentID]          VARCHAR (500) NULL,
    CONSTRAINT [PK_GenericCalibration] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC),
    CONSTRAINT [FK_GenericCalibration_TestPoint_TestPointID] FOREIGN KEY ([TestPointID]) REFERENCES [dbo].[TestPoint] ([TestPointID])
);




GO
CREATE NONCLUSTERED INDEX [IX_GenericCalibration_TestPointID]
    ON [dbo].[GenericCalibration]([TestPointID] ASC);

