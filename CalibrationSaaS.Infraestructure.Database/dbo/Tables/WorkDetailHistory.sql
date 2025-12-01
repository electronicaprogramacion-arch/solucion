CREATE TABLE [dbo].[WorkDetailHistory] (
    [WorkDetailHistoryID] INT            IDENTITY (1, 1) NOT NULL,
    [StatusId]            INT            NOT NULL,
    [Name]                NVARCHAR (MAX) NULL,
    [TechnicianID]        INT            NULL CONSTRAINT [FK_WorkDetailHistory_User_TechnicianID] FOREIGN KEY([TechnicianID])
REFERENCES [dbo].[User] ([UserID]),
    [TechnicianName]      NVARCHAR (MAX) NULL,
    [Date]                DATETIME2 (7)  NOT NULL,
    [Action]              NVARCHAR (MAX) NOT NULL,
    [IsEnable]            BIT            NOT NULL,
    [Description]         NVARCHAR (MAX) NULL,
    [Version]             INT            NOT NULL,
    [UserName]            NVARCHAR (MAX) NULL,
    [Data]                NVARCHAR (MAX) NULL,
    [WorkOrderDetailID]   INT            NOT NULL CONSTRAINT [FK_WorkDetailHistory_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    CONSTRAINT [PK_WorkDetailHistory] PRIMARY KEY CLUSTERED ([WorkDetailHistoryID] ASC)
);

