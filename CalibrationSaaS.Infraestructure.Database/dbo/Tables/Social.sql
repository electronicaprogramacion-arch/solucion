CREATE TABLE [dbo].[Social] (
    [SocialID]                     INT            IDENTITY (1, 1) NOT NULL,
    [Link]                         NVARCHAR (MAX) NOT NULL,
    [SocialTypeID]                 NVARCHAR (MAX) NOT NULL,
    [SocialType]                   NVARCHAR (MAX) NULL,
    [IsEnabled]                    BIT            NOT NULL,
    [Description]                  NVARCHAR (MAX) NULL,
    [Name]                         NVARCHAR (MAX) NULL,
    [CustomerAggregateAggregateID] INT            NULL CONSTRAINT [FK_Social_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID]),
    CONSTRAINT [PK_Social] PRIMARY KEY CLUSTERED ([SocialID] ASC)
);

