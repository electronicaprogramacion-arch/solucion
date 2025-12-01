CREATE TABLE [dbo].[Tenant] (
    [TenantId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED ([TenantId] ASC)
);

