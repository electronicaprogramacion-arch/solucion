CREATE TABLE [dbo].[TestPointGroup] (
    [TestPoitGroupID]                     INT            IDENTITY (1, 1) NOT NULL,
    [OutUnitOfMeasurementID]              INT            NOT NULL,
    [TypeID]                              NVARCHAR (MAX) NOT NULL,
    [Name]                                NVARCHAR (MAX) NOT NULL,
    [UnitOfMeasurementOutUnitOfMeasureID] INT            NULL CONSTRAINT [FK_TestPointGroup_UnitOfMeasure_UnitOfMeasurementOutUnitOfMeasureID] FOREIGN KEY([UnitOfMeasurementOutUnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    [Description]                         NVARCHAR (MAX) NULL,
    [EquipmentTemplateID]                 INT            NULL CONSTRAINT [FK_TestPointGroup_EquipmentTemplate_EquipmentTemplateID] FOREIGN KEY([EquipmentTemplateID])
REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID]),
    [WorkOrderDetailID]                   INT            NULL CONSTRAINT [FK_TestPointGroup_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    [PieceOfEquipmentID]                  VARCHAR (500)  NULL CONSTRAINT [FK_TestPointGroup_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    CONSTRAINT [PK_TestPointGroup] PRIMARY KEY CLUSTERED ([TestPoitGroupID] ASC)
);

