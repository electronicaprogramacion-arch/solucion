CREATE TABLE [dbo].[UnitOfMeasure] (
    [UnitOfMeasureID]            INT           NOT NULL,
    [Name]                       VARCHAR (50)  NOT NULL,
    [Abbreviation]               VARCHAR (10)  NOT NULL,
    [IsEnabled]                  BIT           NOT NULL,
    [UnitOfMeasureBaseID]        INT           NULL,
    [TypeID]                     INT           NOT NULL,
    [ConversionValue]            FLOAT (53)    NOT NULL,
    [UncertaintyUnitOfMeasureID] INT           NULL,
    [Description]                VARCHAR (200) NULL,
    CONSTRAINT [PK_UnitOfMeasure] PRIMARY KEY CLUSTERED ([UnitOfMeasureID] ASC),
    CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID] FOREIGN KEY ([UncertaintyUnitOfMeasureID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UnitOfMeasureBaseID] FOREIGN KEY ([UnitOfMeasureBaseID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UnitOfMeasureBaseUnitOfMeasureID] FOREIGN KEY ([UnitOfMeasureBaseID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasureType_TypeID] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[UnitOfMeasureType] ([Value])
);



