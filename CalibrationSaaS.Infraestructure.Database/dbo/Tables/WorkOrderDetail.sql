CREATE TABLE [dbo].[WorkOrderDetail] (
    [WorkOrderDetailID]        INT            IDENTITY (1, 1) NOT NULL,
    [WorkOderID]               INT            NOT NULL,
    [TenantId]                 INT            NOT NULL,
    [PieceOfEquipmentId]       VARCHAR (500)  NOT NULL,
    [IsAccredited]             BIT            NOT NULL,
    [IsService]                BIT            NOT NULL,
    [IsRepair]                 BIT            NOT NULL,
    [SelectedNewStatus]        SMALLINT       NOT NULL,
    [CertificateComment]       VARCHAR (500)  NULL,
    [Humidity]                 FLOAT (53)     NOT NULL,
    [Temperature]              FLOAT (53)     NOT NULL,
    [Description]              NVARCHAR (MAX) NULL,
    [TemperatureUOMID]         INT            NOT NULL,
    [StandarID]                INT            NOT NULL,
    [CalibrationIntervalID]    INT            NOT NULL,
    [CalibrationIntervalName]  INT            NOT NULL,
    [CalibrationDate]          DATETIME2 (7)  NULL,
    [CalibrationCustomDueDate] DATETIME2 (7)  NULL,
    [CalibrationNextDueDate]   DATETIME2 (7)  NULL,
    [TechnicianComment]        VARCHAR (500)  NULL,
    [TestPointNumber]          INT            NOT NULL,
    [HumidityUOMID]            INT            NOT NULL,
    [TechnicianID]             INT            NULL,
    [CurrentStatusID]          INT            NOT NULL,
    [ToleranceTypeID]          INT            NULL,
    [AccuracyPercentage]       FLOAT (53)     NOT NULL,
    [DecimalNumber]            INT            NOT NULL,
    [Resolution]               FLOAT (53)     NOT NULL,
    [Environment]              NVARCHAR (MAX) NULL,
    [WorkOrderDetailHash]      NVARCHAR (MAX) NULL,
    [AddressID]                INT            NULL,
    [CalibrationTypeID]        INT            NULL,
    [Name]                     NVARCHAR (MAX) NULL,
    [IsComercial]              BIT            CONSTRAINT [DF__WorkOrder__IsCom__7F2BE32F] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [Multiplier]               INT            CONSTRAINT [DF__WorkOrder__Multi__00200768] DEFAULT ((0)) NOT NULL,
    [ClassHB44]                INT            CONSTRAINT [DF__WorkOrder__Class__01142BA1] DEFAULT ((0)) NOT NULL,
    [OfflineID]                VARCHAR (100)  NULL,
    [OfflineStatus]            INT            CONSTRAINT [DF__WorkOrder__Offli__681373AD] DEFAULT ((0)) NOT NULL,
    [StatusDate]               DATETIME2 (7)  NULL,
    [HasBeenCompleted]         BIT            CONSTRAINT [DF__WorkOrder__HasBe__59904A2C] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [TemperatureAfter]         FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [IncludeASTM]              BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [IsUniversal]              BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CertificationID]          INT            NULL,
    [TemperatureStandardId]    NVARCHAR (MAX) NULL,
    [TestCodeID]               INT            NULL,
    [EndOfMonth]               BIT            NULL,
    [WorkOrderDetailUserID]    VARCHAR (500)  NULL,
    [CalibrationSubTypeID]     INT            NULL,
    [Configuration]            VARCHAR (500)  NULL,
    [FullScale]                BIT            NULL,
    [TolerancePercentage]      FLOAT (53)     NULL,
    [ToleranceValue]           FLOAT (53)     NULL,
    [ToleranceFixedValue]      FLOAT (53)     NULL,
    CONSTRAINT [PK_WorkOrderDetail] PRIMARY KEY CLUSTERED ([WorkOrderDetailID] ASC),
    CONSTRAINT [FK_WorkOrderDetail_Address_AddressID] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Address] ([AddressId]),
    CONSTRAINT [FK_WorkOrderDetail_CalibrationType_CalibrationTypeID] FOREIGN KEY ([CalibrationTypeID]) REFERENCES [dbo].[CalibrationType] ([CalibrationTypeId]),
    CONSTRAINT [FK_WorkOrderDetail_PieceOfEquipment_PieceOfEquipmentId] FOREIGN KEY ([PieceOfEquipmentId]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    CONSTRAINT [FK_WorkOrderDetail_Status_CurrentStatusID] FOREIGN KEY ([CurrentStatusID]) REFERENCES [dbo].[Status] ([StatusId]),
    CONSTRAINT [FK_WorkOrderDetail_TestCode_TestCodeID] FOREIGN KEY ([TestCodeID]) REFERENCES [dbo].[TestCode] ([TestCodeID]),
    CONSTRAINT [FK_WorkOrderDetail_User_TechnicianID] FOREIGN KEY ([TechnicianID]) REFERENCES [dbo].[User] ([UserID]),
    CONSTRAINT [FK_WorkOrderDetail_WorkOrderDetail] FOREIGN KEY ([WorkOrderDetailID]) REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]),
    CONSTRAINT [FK_WorkOrderDetail_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY ([WorkOderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderId]),
    CONSTRAINT [FK_WorkOrderDetail_WorkOrderDetail1] FOREIGN KEY ([WorkOrderDetailID]) REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
);


















GO
CREATE NONCLUSTERED INDEX [IX_WorkOrderDetail_CertificationID]
    ON [dbo].[WorkOrderDetail]([CertificationID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkOrderDetail_TestCodeID]
    ON [dbo].[WorkOrderDetail]([TestCodeID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkOrderDetail]
    ON [dbo].[WorkOrderDetail]([WorkOrderDetailID] ASC);

