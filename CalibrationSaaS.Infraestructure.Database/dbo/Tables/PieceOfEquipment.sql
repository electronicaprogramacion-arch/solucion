CREATE TABLE [dbo].[PieceOfEquipment] (
    [PieceOfEquipmentID]          VARCHAR (500)  NOT NULL,
    [SerialNumber]                VARCHAR (500)  NOT NULL,
    [Capacity]                    FLOAT (53)     NOT NULL,
    [AddressId]                   INT            NOT NULL,
    [Status]                      INT            NOT NULL,
    [InstallLocation]             VARCHAR (500)  NULL,
    [TenantId]                    INT            NOT NULL,
    [DueDate]                     DATETIME2 (7)  NOT NULL,
    [CustomerId]                  INT            NOT NULL,
    [Class]                       VARCHAR (10)   NULL,
    [IsStandard]                  BIT            NOT NULL,
    [IsWeigthSet]                 BIT            NOT NULL,
    [IsForAccreditedCal]          BIT            NOT NULL,
    [CalibrationDate]             DATETIME2 (7)  NOT NULL,
    [EquipmentTemplateId]         INT            NOT NULL,
    [WorOrderDetailID]            INT            NULL,
    [WorkOrderID]                 INT            NULL,
    [IndicatorPieceOfEquipmentID] VARCHAR (500)  NULL,
    [UnitOfMeasureID]             INT            NULL,
    [EquipmentTypeID]             INT            NULL,
    [AccuracyPercentage]          FLOAT (53)     CONSTRAINT [DF__PieceOfEq__Accur__51300E55] DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [DecimalNumber]               INT            CONSTRAINT [DF__PieceOfEq__Decim__5224328E] DEFAULT ((0)) NOT NULL,
    [Resolution]                  FLOAT (53)     CONSTRAINT [DF__PieceOfEq__Resol__531856C7] DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [ToleranceTypeID]             INT            CONSTRAINT [DF__PieceOfEq__Toler__540C7B00] DEFAULT ((0)) NULL,
    [IsToleranceImport]           BIT            CONSTRAINT [DF__PieceOfEq__IsTol__56E8E7AB] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [IsDelete]                    BIT            CONSTRAINT [DF__PieceOfEq__Delet__690797E6] DEFAULT ((0)) NOT NULL,
    [ClassHB44]                   INT            CONSTRAINT [DF__PieceOfEq__Class__0C85DE4D] DEFAULT ((0)) NOT NULL,
    [IsTestPointImport]           BIT            CONSTRAINT [DF__PieceOfEq__IsTes__0D7A0286] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [OfflineID]                   VARCHAR (500)  NULL,
    [Notes]                       NVARCHAR (MAX) NULL,
    [CustomerToolId]              VARCHAR (500)  NULL,
    [Scale]                       NVARCHAR (MAX) NULL,
    [ToleranceValue]              FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [UncertaintyValue]            FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [ToleranceValueISO]           FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [Hardness]                    FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [MicronValue]                 FLOAT (53)     DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [TypeMicro]                   INT            NULL,
    [LoadKGF]                     FLOAT (53)     CONSTRAINT [DF_PieceOfEquipment_LoadKGF] DEFAULT ((0)) NULL,
    [ToleranceHV]                 FLOAT (53)     NULL,
    [TestCodeID]                  INT            NULL,
    [CalibrationTypeID]           INT            NULL,
    [AccuracyDescription]         VARCHAR (MAX)  NULL,
    [Configuration]               VARCHAR (500)  NULL,
    [FullScale]                   BIT            NULL,
    [TolerancePercentage]         FLOAT (53)     NULL,
    [ToleranceFixedValue]         FLOAT (53)     NULL,
    CONSTRAINT [PK_PieceOfEquipment] PRIMARY KEY CLUSTERED ([PieceOfEquipmentID] ASC),
    CONSTRAINT [FK_PieceOfEquipment_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerID]),
    CONSTRAINT [FK_PieceOfEquipment_EquipmentTemplate_EquipmentTemplateId] FOREIGN KEY ([EquipmentTemplateId]) REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID]),
    CONSTRAINT [FK_PieceOfEquipment_EquipmentType_EquipmentTypeID] FOREIGN KEY ([EquipmentTypeID]) REFERENCES [dbo].[EquipmentType] ([EquipmentTypeID]),
    CONSTRAINT [FK_PieceOfEquipment_PieceOfEquipment_IndicatorPieceOfEquipmentID] FOREIGN KEY ([IndicatorPieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    CONSTRAINT [FK_PieceOfEquipment_TestCode_TestCodeID] FOREIGN KEY ([TestCodeID]) REFERENCES [dbo].[TestCode] ([TestCodeID]),
    CONSTRAINT [FK_PieceOfEquipment_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY ([UnitOfMeasureID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
);



















