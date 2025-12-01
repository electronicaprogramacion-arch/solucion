CREATE TABLE [dbo].[EmailAddress] (
    [EmailAddressID]               INT            IDENTITY (1, 1) NOT NULL,
    [Address]                      NVARCHAR (MAX) NOT NULL,
    [TypeID]                       NVARCHAR (MAX) NOT NULL,
    [Type]                         NVARCHAR (MAX) NOT NULL,
    [IsEnabled]                    BIT            NOT NULL,
    [AggregateID]                  NVARCHAR (MAX) NOT NULL,
    [Name]                         NVARCHAR (MAX) NULL,
    [Description]                  NVARCHAR (MAX) NULL,
    [CustomerAggregateAggregateID] INT            NULL CONSTRAINT [FK_EmailAddress_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID]),
    CONSTRAINT [PK_EmailAddress] PRIMARY KEY CLUSTERED ([EmailAddressID] ASC)
);

