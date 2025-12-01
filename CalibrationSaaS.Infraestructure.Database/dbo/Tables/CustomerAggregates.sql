CREATE TABLE [dbo].[CustomerAggregates] (
    [AggregateID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NULL,
    [Description] NVARCHAR (100) NULL,
    [CustomerID]  INT            NULL CONSTRAINT [FK_CustomerAggregates_Customer_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID]),
    CONSTRAINT [PK_CustomerAggregates] PRIMARY KEY CLUSTERED ([AggregateID] ASC)
);

