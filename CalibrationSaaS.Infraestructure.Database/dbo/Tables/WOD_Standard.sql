CREATE TABLE [dbo].[WOD_Standard] (
    [WorkOrderDetailID]  INT           NOT NULL,
    [PieceOfEquipmentID] VARCHAR (500) NOT NULL,
    [Option]             VARCHAR (50)  NULL,
    CONSTRAINT [PK_WOD_Standard] PRIMARY KEY CLUSTERED ([WorkOrderDetailID] ASC, [PieceOfEquipmentID] ASC),
    CONSTRAINT [FK_WOD_Standard_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]) ON DELETE CASCADE,
    CONSTRAINT [FK_WOD_Standard_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY ([WorkOrderDetailID]) REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_WOD_Standard_PieceOfEquipmentID]
    ON [dbo].[WOD_Standard]([PieceOfEquipmentID] ASC);

