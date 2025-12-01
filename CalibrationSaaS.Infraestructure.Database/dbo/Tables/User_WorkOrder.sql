CREATE TABLE [dbo].[User_WorkOrder] (
    [UserID]      INT NOT NULL CONSTRAINT [FK_User_WorkOrder_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID]),
    [WorkOrderID] INT NOT NULL CONSTRAINT [FK_User_WorkOrder_WorkOrder_WorkOrderID] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrder] ([WorkOrderId]),
    CONSTRAINT [PK_User_WorkOrder] PRIMARY KEY CLUSTERED ([UserID] ASC, [WorkOrderID] ASC)
);

