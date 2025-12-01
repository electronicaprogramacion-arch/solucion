CREATE TABLE [dbo].[DynamicProperty] (
    [DynamicPropertyID]  INT            NOT NULL,
    [ColPosition]        INT            NULL,
    [Name]               VARCHAR (500)  NULL,
    [DataType]           VARCHAR (500)  NULL,
    [CalibrationSubtype] INT            NOT NULL,
    [DefaultValue]       VARCHAR (5000) NULL,
    [ViewPropertyBaseID] INT            CONSTRAINT [DF__DynamicPr__ViewP__78F3E6EC] DEFAULT ((0)) NOT NULL,
    [Formula]            VARCHAR (MAX)  NULL,
    [ValidationFormula]  VARCHAR (MAX)  NULL,
    [Version]            INT            CONSTRAINT [DF__DynamicPr__Versi__17786E0C] DEFAULT ((0)) NOT NULL,
    [Enable]             BIT            CONSTRAINT [DF_DynamicProperty_Enable] DEFAULT ((0)) NULL,
    [Map]                VARCHAR (200)  NULL,
    [FormulaClass]       VARCHAR (500)  NULL,
    [GridLocation]       VARCHAR (50)   NOT NULL,
    [IsMaxField]         BIT            NULL,
    [IsRequired]         BIT            NULL,
    [unique]             BIT            NULL,
    [Pattern]            VARCHAR (500)  NULL,
    [JSONConfiguration]  VARCHAR (2000) NULL,
    CONSTRAINT [PK_DynamicProperty] PRIMARY KEY CLUSTERED ([DynamicPropertyID] ASC),
    CONSTRAINT [FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID] FOREIGN KEY ([ViewPropertyBaseID]) REFERENCES [dbo].[ViewPropertyBase] ([ViewPropertyID])
);






























GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DynamicProperty_ViewPropertyBaseID]
    ON [dbo].[DynamicProperty]([ViewPropertyBaseID] ASC);

