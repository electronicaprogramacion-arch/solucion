CREATE TABLE [dbo].[WOD_Weight] (
    [WorkOrderDetailID] INT          NOT NULL,
    [WeightSetID]       INT          NOT NULL,
    [Option]            VARCHAR (50) NULL,
    CONSTRAINT [PK_WOD_Weight] PRIMARY KEY CLUSTERED ([WorkOrderDetailID] ASC, [WeightSetID] ASC),
    CONSTRAINT [FK_WOD_Weight_WeightSet_WeightSetID] FOREIGN KEY ([WeightSetID]) REFERENCES [dbo].[WeightSet] ([WeightSetID]),
    CONSTRAINT [FK_WOD_Weight_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY ([WorkOrderDetailID]) REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
);



