CREATE TABLE [dbo].[EquipmentTemplate] (
    [EquipmentTemplateID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (MAX) NULL,
    [Description]         NVARCHAR (100) NULL,
    [ImageUrl]            NVARCHAR (500) NULL,
    [EquipmentTypeID]     INT            NOT NULL,
    [ManufacturerID]      INT            NOT NULL,
    [ToleranceTypeID]     INT            NULL,
    [AccuracyPercentage]  FLOAT (53)     NOT NULL,
    [Resolution]          FLOAT (53)     NOT NULL,
    [DecimalNumber]       INT            NOT NULL,
    [UnitofmeasurementID] INT            NULL,
    [StatusID]            NVARCHAR (MAX) NOT NULL,
    [Model]               NVARCHAR (MAX) NOT NULL,
    [Capacity]            FLOAT (53)     NOT NULL,
    [IsEnabled]           BIT            NOT NULL,
    [Manufacturer]        NVARCHAR (MAX) NULL,
    [EquipmentType]       NVARCHAR (MAX) NULL,
    [IsComercial]         BIT            CONSTRAINT [DF__Equipment__IsCom__7F2BE32F] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [IsDelete]            BIT            CONSTRAINT [DF__Equipment__Delet__6442E2C9] DEFAULT ((0)) NOT NULL,
    [ClassHB44]           INT            DEFAULT ((0)) NOT NULL,
    [IsGeneric]           BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [OldID]               INT            NULL,
    [DeviceClass]         VARCHAR (100)  NULL,
    [PlatformSize]        VARCHAR (100)  NULL,
    [FullScale]           BIT            NULL,
    [TolerancePercentage] FLOAT (53)     NULL,
    [ToleranceValue]      FLOAT (53)     NULL,
    [ToleranceFixedValue] FLOAT (53)     NULL,
    CONSTRAINT [PK_EquipmentTemplate] PRIMARY KEY CLUSTERED ([EquipmentTemplateID] ASC)
);



