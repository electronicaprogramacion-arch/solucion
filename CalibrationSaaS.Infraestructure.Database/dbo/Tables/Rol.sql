CREATE TABLE [dbo].[Rol] (
    [RolID]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (MAX) NOT NULL,
    [Description]        VARCHAR (200)  NULL,
    [DefaultPermissions] VARCHAR (200)  NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([RolID] ASC)
);

