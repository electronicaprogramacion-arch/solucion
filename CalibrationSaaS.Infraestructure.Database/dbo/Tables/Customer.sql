CREATE TABLE [dbo].[Customer] (
    [CustomerID]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [TenantId]    INT           NOT NULL,
    [Description] VARCHAR (500) NULL,
    [IsDelete]    BIT           CONSTRAINT [DF__Customer__IsDele__01D345B0] DEFAULT ((0)) NULL,
    [CustomID]    VARCHAR (500) NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);



