CREATE TABLE [dbo].[RockwellResult] (
    [SequenceID]           INT            NOT NULL,
    [CalibrationSubTypeId] INT            NOT NULL,
    [WorkOrderDetailId]    INT            NOT NULL,
    [Nominal]              FLOAT (53)     NOT NULL,
    [ScaleRange]           NVARCHAR (MAX) NULL,
    [Standard]             FLOAT (53)     NOT NULL,
    [Average]              FLOAT (53)     NOT NULL,
    [Test1]                FLOAT (53)     NOT NULL,
    [Test2]                FLOAT (53)     NOT NULL,
    [Test3]                FLOAT (53)     NOT NULL,
    [Test4]                FLOAT (53)     NOT NULL,
    [Test5]                FLOAT (53)     NOT NULL,
    [Error]                FLOAT (53)     NOT NULL,
    [Repeateability]       FLOAT (53)     NOT NULL,
    [Uncertanty]           FLOAT (53)     NOT NULL,
    [Resolution]           FLOAT (53)     NOT NULL,
    [DecimalNumber]        INT            NOT NULL,
    [Position]             INT            NULL,
    [Max]                  FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [Min]                  FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    CONSTRAINT [PK_RockwellResult] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC),
    CONSTRAINT [FK_RockwellResult_Rockwell_SequenceID_CalibrationSubTypeId_WorkOrderDetailId] FOREIGN KEY ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]) REFERENCES [dbo].[Rockwell] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
);







