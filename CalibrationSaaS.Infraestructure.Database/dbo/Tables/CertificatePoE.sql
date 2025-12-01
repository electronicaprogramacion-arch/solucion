CREATE TABLE [dbo].[CertificatePoE] (
    [CertificateNumber]   NVARCHAR (50)  NOT NULL,
    [DueDate]             DATETIME2 (7)  NOT NULL,
    [CalibrationDate]     DATETIME2 (7)  NOT NULL,
    [Company]             NVARCHAR (MAX) NULL,
    [AffectDueDate]       BIT            NOT NULL,
    [Name]                VARCHAR (100)  NULL,
    [Description]         VARCHAR (500)  NULL,
    [PieceOfEquipmentID]  VARCHAR (500)  DEFAULT ('') NOT NULL,
    [Version]             INT            NOT NULL,
    [CalibrationProvider] VARCHAR (500)  NULL,
    CONSTRAINT [PK_CertificatePoE] PRIMARY KEY CLUSTERED ([CertificateNumber] ASC, [PieceOfEquipmentID] ASC),
    CONSTRAINT [FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
);



