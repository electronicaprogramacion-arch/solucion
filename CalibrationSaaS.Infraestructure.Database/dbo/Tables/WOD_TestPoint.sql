CREATE TABLE [dbo].[WOD_TestPoint] (
    [WorkOrderDetailID]    INT NOT NULL CONSTRAINT [FK_WOD_TestPoint_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    [TestPointID]          INT NOT NULL CONSTRAINT [FK_WOD_TestPoint_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID]),
    [SecuenceID]           INT NOT NULL,
    [CalibrationSubTypeID] INT NOT NULL,
    CONSTRAINT [PK_WOD_TestPoint] PRIMARY KEY CLUSTERED ([WorkOrderDetailID] ASC, [TestPointID] ASC, [CalibrationSubTypeID] ASC, [SecuenceID] ASC)
);

