CREATE TABLE [dbo].[Procedure] (
    [ProcedureID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [DocumentUrl] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Procedure] PRIMARY KEY CLUSTERED ([ProcedureID] ASC)
);









