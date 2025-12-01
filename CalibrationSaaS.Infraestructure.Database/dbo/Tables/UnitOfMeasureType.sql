CREATE TABLE [dbo].[UnitOfMeasureType] (
    [Value]             INT            NOT NULL,
    [Name]              NVARCHAR (MAX) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [CalibrationTypeID] INT            NULL,
    CONSTRAINT [PK_UnitOfMeasureType] PRIMARY KEY CLUSTERED ([Value] ASC)
);



