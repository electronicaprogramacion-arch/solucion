CREATE TABLE [dbo].[EquipmentType] (
    [EquipmentTypeID]                  INT            NOT NULL,
    [Name]                             NVARCHAR (500) NOT NULL,
    [IsEnabled]                        BIT            NOT NULL,
    [IsBalance]                        BIT            CONSTRAINT [DF__Equipment__IsBal__3B75D760] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasIndicator]                     BIT            CONSTRAINT [DF__Equipment__HasIn__40C49C62] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasPeripheral]                    BIT            CONSTRAINT [DF__Equipment__HasPe__41B8C09B] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasAccredited]                    BIT            CONSTRAINT [DF__Equipment__HasAc__4242D080] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasClass]                         BIT            CONSTRAINT [DF__Equipment__HasCl__4336F4B9] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasTestpoint]                     BIT            CONSTRAINT [DF__Equipment__HasTe__442B18F2] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasTolerance]                     BIT            CONSTRAINT [DF__Equipment__HasTo__451F3D2B] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasCertificate]                   BIT            CONSTRAINT [DF__Equipment__HasCe__34B3CB38] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasStandard]                      BIT            CONSTRAINT [DF__Equipment__HasSt__35A7EF71] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CalibrationTypeID]                INT            CONSTRAINT [DF__Equipment__Calib__7BB05806] DEFAULT ((0)) NULL,
    [HasStandardRange]                 BIT            CONSTRAINT [DF__Equipment__HasSt__71BCD978] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasUncertanity]                   BIT            CONSTRAINT [DF__Equipment__HasUn__72B0FDB1] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasCapacity]                      BIT            CONSTRAINT [DF__Equipment__HasCa__73A521EA] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasResolution]                    BIT            CONSTRAINT [DF__Equipment__HasRe__74994623] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasWorkOrderDetail]               BIT            CONSTRAINT [DF__Equipment__HasWo__758D6A5C] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasScales]                        BIT            CONSTRAINT [DF_EquipmentType_HasScales] DEFAULT ((0)) NOT NULL,
    [HardnessTestBlockInformation]     BIT            CONSTRAINT [DF__Equipment__Hardn__222B06A9] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [UseWorkOrderDetailStandard]       BIT            CONSTRAINT [DF__Equipment__UseWo__015422C3] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasWorkOrdeDetailStandard]        BIT            CONSTRAINT [DF__Equipment__HasWo__024846FC] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [HasStandardConfiguration]         BIT            CONSTRAINT [DF__Equipment__HasSt__450A2E92] DEFAULT ((1)) NOT NULL,
    [DynamicConfiguration]             BIT            CONSTRAINT [DF__Equipment__Dynam__695C9DA1] DEFAULT ((0)) NOT NULL,
    [DynamicConfiguration2]            BIT            NULL,
    [CalibrationInstrucctions]         VARCHAR (500)  NULL,
    [StandardComponent]                VARCHAR (500)  NULL,
    [HasRepeateabilityAndEccentricity] BIT            CONSTRAINT [DF__Equipment__HasRe__3EDC53F0] DEFAULT ((0)) NOT NULL,
    [HasReturnToZero]                  BIT            CONSTRAINT [DF__Equipment__HasRe__3CF40B7E] DEFAULT ((0)) NOT NULL,
    [DefaultCustomer]                  VARCHAR (500)  NULL,
    [JSONConfiguration]                VARCHAR (5000) NULL,
    CONSTRAINT [PK_EquipmentType] PRIMARY KEY CLUSTERED ([EquipmentTypeID] ASC),
    CONSTRAINT [FK_EquipmentType_CalibrationType_CalibrationTypeID] FOREIGN KEY ([CalibrationTypeID]) REFERENCES [dbo].[CalibrationType] ([CalibrationTypeId]) ON DELETE CASCADE
);
































GO
CREATE NONCLUSTERED INDEX [IX_EquipmentType_CalibrationTypeID]
    ON [dbo].[EquipmentType]([CalibrationTypeID] ASC);

