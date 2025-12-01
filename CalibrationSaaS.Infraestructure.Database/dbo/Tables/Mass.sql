CREATE TABLE [dbo].[Mass] (
    [MassID]        INT        IDENTITY (1, 1) NOT NULL,
    [Metric]        INT        NOT NULL,
    [UnitOfMeasure] INT        NOT NULL,
    [Class1]        FLOAT (53) NULL,
    [Class1UoM]     INT        NULL,
    [Class2]        FLOAT (53) NULL,
    [Class2UoM]     INT        NULL,
    [Class3]        FLOAT (53) NULL,
    [Class3UoM]     INT        NULL,
    [Class4]        FLOAT (53) NULL,
    [Class4UoM]     INT        NULL,
    [Class5]        FLOAT (53) NULL,
    [Class5UoM]     INT        NULL,
    [Class6]        FLOAT (53) NULL,
    [Class6UoM]     INT        NULL,
    [Class7]        FLOAT (53) NULL,
    [Class7UoM]     INT        NULL,
    CONSTRAINT [PK_Mass] PRIMARY KEY CLUSTERED ([MassID] ASC)
);

