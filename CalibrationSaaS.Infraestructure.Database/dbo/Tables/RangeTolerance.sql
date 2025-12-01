CREATE TABLE [dbo].[RangeTolerance] (
    [RangeToleranceID]    INT           IDENTITY (1, 1) NOT NULL,
    [MinValue]            FLOAT (53)    NOT NULL,
    [MaxValue]            FLOAT (53)    NOT NULL,
    [Percent]             FLOAT (53)    NOT NULL,
    [Resolution]          FLOAT (53)    NOT NULL,
    [EquipmentTemplateID] INT           NULL CONSTRAINT [FK_RangeTolerance_EquipmentTemplate_EquipmentTemplateID] FOREIGN KEY([EquipmentTemplateID])
REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID]),
    [ToleranceTypeID]     INT           NOT NULL,
    [PieceOfEquipmentID]  VARCHAR (500) NULL CONSTRAINT [FK_RangeTolerance_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    [WorkOrderDetailID]   INT           NULL CONSTRAINT [FK_RangeTolerance_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    CONSTRAINT [PK_RangeTolerance] PRIMARY KEY CLUSTERED ([RangeToleranceID] ASC)
);

