CREATE TABLE [dbo].[Address] (
    [AddressId]                    INT            IDENTITY (1, 1) NOT NULL,
    [StreetAddress1]               NVARCHAR (MAX) NOT NULL,
    [StreetAddress2]               NVARCHAR (MAX) NULL,
    [StreetAddress3]               NVARCHAR (MAX) NULL,
    [CityID]                       NVARCHAR (MAX) NOT NULL,
    [City]                         NVARCHAR (MAX) NULL,
    [StateID]                      NVARCHAR (MAX) NOT NULL,
    [State]                        NVARCHAR (MAX) NULL,
    [ZipCode]                      NVARCHAR (MAX) NOT NULL,
    [CountryID]                    NVARCHAR (MAX) NOT NULL,
    [Country]                      NVARCHAR (MAX) NULL,
    [Description]                  NVARCHAR (MAX) NULL,
    [AggregateID]                  INT            NOT NULL CONSTRAINT [FK_Address_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID]),
    [IsDefault]                    BIT            NOT NULL,
    [IsEnable]                     BIT            NOT NULL,
    [Name]                         NVARCHAR (MAX) NULL,
    [County]                       NVARCHAR (MAX) NULL,
    [CustomerAggregateAggregateID] INT            NULL,
    [IsDelete]                     BIT            CONSTRAINT [DF__Address__Delete__662B2B3B] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);