CREATE TABLE [dbo].[Contact] (
    [ContactID]                    INT            IDENTITY (1, 1) NOT NULL,
    [Name]                         VARCHAR (50)   NOT NULL,
    [LastName]                     VARCHAR (50)   NULL,
    [IsEnabled]                    BIT            NOT NULL,
    [UserName]                     NVARCHAR (MAX) NULL,
    [Email]                        NVARCHAR (MAX) NULL,
    [Occupation]                   NVARCHAR (MAX) NULL,
    [PasswordReset]                BIT            NOT NULL,
    [AggregateID]                  INT            NOT NULL,
    [Description]                  NVARCHAR (MAX) NULL,
    [CustomerAggregateAggregateID] INT            NULL,
    [PhoneNumber]                  NVARCHAR (13)  NULL,
    [IsDelete]                     BIT            CONSTRAINT [DF__Contact__Delete__671F4F74] DEFAULT ((0)) NOT NULL,
    [CellPhoneNumber]              NVARCHAR (13)  NULL,
    [Note]                         NVARCHAR (500) NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([ContactID] ASC)
);

