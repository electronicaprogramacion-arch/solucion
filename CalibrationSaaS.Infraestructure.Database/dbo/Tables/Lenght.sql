CREATE TABLE [dbo].[Lenght] (
    [LenghtID]        INT        IDENTITY (1, 1) NOT NULL,
    [From]            FLOAT (53) NOT NULL,
    [To]              FLOAT (53) NOT NULL,
    [UnitOfMeasure]   INT        NOT NULL,
    [Tolerance]       FLOAT (53) NOT NULL,
    [CertificationID] INT        NOT NULL,
    CONSTRAINT [PK_Lenght] PRIMARY KEY CLUSTERED ([LenghtID] ASC)
);



