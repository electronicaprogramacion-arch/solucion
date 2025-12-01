CREATE TABLE [dbo].[WorkOrder] (
    [WorkOrderId]     INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]      INT            NOT NULL CONSTRAINT [FK_WorkOrder_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerID]),
    [WorkOrderDate]   DATETIME2 (7)  NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [ControlNumber]   NVARCHAR (MAX) NULL,
    [TenantId]        INT            NOT NULL,
    [CalibrationType] INT            NOT NULL,
    [AddressId]       INT            NOT NULL CONSTRAINT [FK_WorkOrder_Address_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId]),
    [ContactId]       INT            NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [Invoice]         NVARCHAR (MAX) NULL,
    [CustomerInvoice] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([WorkOrderId] ASC)
);

