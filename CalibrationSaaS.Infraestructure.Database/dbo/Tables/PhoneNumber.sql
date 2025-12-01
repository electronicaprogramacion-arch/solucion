CREATE TABLE [dbo].[PhoneNumber] (
    [PhoneNumberID]                INT            IDENTITY (1, 1) NOT NULL,
    [Number]                       VARCHAR (50)   NOT NULL,
    [TypeID]                       VARCHAR (50)   NOT NULL,
    [CountryID]                    VARCHAR (50)   NULL,
    [IsEnabled]                    BIT            NOT NULL,
    [Name]                         VARCHAR (50)   NULL,
    [Description]                  NVARCHAR (MAX) NULL,
    [CustomerAggregateAggregateID] INT            NULL CONSTRAINT [FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID]),
    CONSTRAINT [PK_PhoneNumber] PRIMARY KEY CLUSTERED ([PhoneNumberID] ASC)
);

