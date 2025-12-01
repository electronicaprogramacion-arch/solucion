CREATE TABLE [dbo].[WorkOrderDetailByEquipment] (
    [WorkOrderDetailID]    INT            IDENTITY (1, 1) NOT NULL,
    [Company]              NVARCHAR (MAX) NULL,
    [Model]                NVARCHAR (MAX) NULL,
    [SerialNumber]         NVARCHAR (MAX) NULL,
    [EquipmentType]        NVARCHAR (MAX) NULL,
    [WorkOrderId]          INT            NOT NULL,
    [WorkOrderReceiveDate] DATETIME2 (7)  NULL,
    [Status]               NVARCHAR (MAX) NULL,
    [Name]                 NVARCHAR (MAX) NULL,
    [Description]          NVARCHAR (MAX) NULL,
    [EquipmentTypeID]      INT            NOT NULL,
    [StatusId]             INT            NOT NULL,
    [StatusDate]           DATETIME2 (7)  NULL,
    CONSTRAINT [PK_WorkOrderDetailByEquipment] PRIMARY KEY CLUSTERED ([WorkOrderDetailID] ASC)
);

