CREATE TABLE [dbo].[Manufacturer] (
    [ManufacturerID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (500) NULL,
    [ImageUrl]       NVARCHAR (500) NULL,
    [Abbreviation]   NVARCHAR (500) NULL,
    [IsEnabled]      BIT            NOT NULL,
    [IsDelete]       BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED ([ManufacturerID] ASC)
);

