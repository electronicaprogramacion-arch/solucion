CREATE TABLE [dbo].[BasicCalibrationResult] (
    [SequenceID]           INT            NOT NULL,
    [CalibrationSubTypeId] INT            NOT NULL,
    [WorkOrderDetailId]    INT            NOT NULL,
    [AsFound]              FLOAT (53)     NOT NULL,
    [AsLeft]               FLOAT (53)     NOT NULL,
    [WeightApplied]        FLOAT (53)     NOT NULL,
    [ReadingStandard]      FLOAT (53)     NOT NULL,
    [FinalReadingStandard] FLOAT (53)     NOT NULL,
    [TestResultID]         INT            NOT NULL,
    [Uncertainty]          FLOAT (53)     NULL,
    [Position]             INT            NOT NULL,
    [UnitOfMeasureID]      INT            NOT NULL,
    [InToleranceFound]     NVARCHAR (MAX) NULL,
    [InToleranceLeft]      NVARCHAR (MAX) NULL,
    [Discriminator]        NVARCHAR (MAX) NOT NULL,
    [Resolution]           FLOAT (53)     NOT NULL,
    [ComponentID]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BasicCalibrationResult] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC),
    CONSTRAINT [FK_BasicCalibrationResult_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY ([UnitOfMeasureID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
);



GO
