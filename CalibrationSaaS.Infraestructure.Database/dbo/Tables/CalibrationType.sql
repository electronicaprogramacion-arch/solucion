CREATE TABLE [dbo].[CalibrationType] (
    [CalibrationTypeId] INT            NOT NULL,
    [Name]              NVARCHAR (MAX) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [ShowType]          BIT            NULL,
    [UrlReport]         NVARCHAR (500) NULL,
    [HasNew]            BIT            NOT NULL,
    CONSTRAINT [PK_CalibrationType] PRIMARY KEY CLUSTERED ([CalibrationTypeId] ASC)
);


















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llave primaria de la tabla Consecutivo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CalibrationType', @level2type = N'COLUMN', @level2name = N'CalibrationTypeId';

