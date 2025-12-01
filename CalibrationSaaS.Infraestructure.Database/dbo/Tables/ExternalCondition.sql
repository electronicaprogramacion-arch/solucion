CREATE TABLE [dbo].[ExternalCondition] (
    [ExternalConditionId] INT            IDENTITY (1, 1) NOT NULL,
    [WorkOrderDetailId]   INT            NOT NULL,
    [Temperature]         FLOAT (53)     NOT NULL,
    [Humidity]            FLOAT (53)     NOT NULL,
    [HRC]                 FLOAT (53)     NOT NULL,
    [HRA]                 FLOAT (53)     NOT NULL,
    [HRN]                 FLOAT (53)     NOT NULL,
    [Pass1]               BIT            NOT NULL,
    [Pass2]               BIT            NOT NULL,
    [Pass3]               BIT            NOT NULL,
    [Onesixteenth]        FLOAT (53)     NOT NULL,
    [AnEighth]            FLOAT (53)     NOT NULL,
    [Quarter]             FLOAT (53)     NOT NULL,
    [Medium]              FLOAT (53)     NOT NULL,
    [Name]                NVARCHAR (MAX) NULL,
    [IsAsFound]           BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [PassAnEighth]        BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [PassHRA]             BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [PassHRN]             BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [PassQuarter]         BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [Serial]              NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ExternalCondition] PRIMARY KEY CLUSTERED ([ExternalConditionId] ASC),
    CONSTRAINT [FK_ExternalCondition_ExternalCondition] FOREIGN KEY ([ExternalConditionId]) REFERENCES [dbo].[ExternalCondition] ([ExternalConditionId]),
    CONSTRAINT [FK_ExternalCondition_WorkOrderDetail] FOREIGN KEY ([WorkOrderDetailId]) REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ExternalCondition_WorkOrderDetailId]
    ON [dbo].[ExternalCondition]([WorkOrderDetailId] ASC);

