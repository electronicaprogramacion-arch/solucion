CREATE TABLE [dbo].[Status] (
    [StatusId]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [IsDefault]         BIT            NOT NULL,
    [IsEnable]          BIT            NOT NULL,
    [Possibilities]     NVARCHAR (MAX) NOT NULL,
    [WorkOrderDetailID] INT            NULL CONSTRAINT [FK_Status_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    [IsLast]            BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([StatusId] ASC)
);

