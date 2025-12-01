CREATE TABLE [dbo].[Component] (
    [ComponentID] INT           IDENTITY (1, 1) NOT NULL,
    [Route]       VARCHAR (100) NULL,
    [Name]        VARCHAR (100) NULL,
    [Group]       VARCHAR (200) NULL,
    [Permission]  INT           NOT NULL,
    [Disabled]    BIT           DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_Component] PRIMARY KEY CLUSTERED ([ComponentID] ASC)
);

