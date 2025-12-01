CREATE TABLE [dbo].[EquipmentCondition] (
    [EquipmentConditionId] INT            IDENTITY (1, 1) NOT NULL,
    [WorkOrderDetailId]    INT            NOT NULL CONSTRAINT [FK_EquipmentCondition_WorkOrderDetail_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    [IsAsFound]            BIT            NOT NULL,
    [Value]                BIT            NOT NULL,
    [Label]                NVARCHAR (MAX) NOT NULL,
    [Name]                 NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_EquipmentCondition] PRIMARY KEY CLUSTERED ([EquipmentConditionId] ASC)
);

