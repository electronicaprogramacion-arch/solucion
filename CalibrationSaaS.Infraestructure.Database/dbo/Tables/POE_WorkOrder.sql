CREATE TABLE [dbo].[POE_WorkOrder] (
    [PieceOfEquipmentID] VARCHAR (500) NOT NULL CONSTRAINT [FK_POE_WorkOrder_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    [WorkOrderID]        INT           NOT NULL CONSTRAINT [FK_POE_WorkOrder_WorkOrder_WorkOrderID] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrder] ([WorkOrderId])
);

