CREATE TABLE [dbo].[Certificate] (
    [CertificateNumber]         NVARCHAR (50)  NOT NULL,
    [DueDate]                   DATETIME2 (7)  NOT NULL,
    [CalibrationDate]           DATETIME2 (7)  NOT NULL,
    [Company]                   NVARCHAR (MAX) NULL,
    [AffectDueDate]             BIT            NOT NULL,
    [Name]                      NVARCHAR (100) NULL,
    [Description]               NVARCHAR (100) NULL,
    [WorkOrderDetailId]         INT            NOT NULL CONSTRAINT [FK_Certificate_WorkOrderDetail_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    [Version]                   INT            NOT NULL,
    [WorkOrderDetailSerialized] NVARCHAR (MAX) NULL,
    [CertificateID]             INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Certificate] PRIMARY KEY CLUSTERED ([CertificateID] ASC)
);

