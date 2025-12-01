CREATE TABLE [dbo].[GenericCalibration2] (
    [SequenceID]           INT            NOT NULL,
    [CalibrationSubTypeId] INT            NOT NULL,
    [ComponentID]          NVARCHAR (450) NOT NULL,
    [WorkOrderDetailId]    INT            NOT NULL,
    [NumberOfSamples]      INT            NOT NULL,
    [TestPointID]          INT            NULL,
    [UnitOfMeasureId]      INT            NULL,
    [Component]            VARCHAR (500)  NULL,
    CONSTRAINT [PK_GenericCalibration2] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [ComponentID] ASC),
    CONSTRAINT [FK_GenericCalibration2_TestPoint_TestPointID] FOREIGN KEY ([TestPointID]) REFERENCES [dbo].[TestPoint] ([TestPointID])
);




GO
CREATE NONCLUSTERED INDEX [IX_GenericCalibration2_TestPointID]
    ON [dbo].[GenericCalibration2]([TestPointID] ASC);

