CREATE TABLE [dbo].[Uncertainty] (
    [UncertaintyID]        INT            IDENTITY (1, 1) NOT NULL,
    [EquipmentTemplateID]  INT            NULL CONSTRAINT [FK_Uncertainty_EquipmentTemplate_EquipmentTemplateID] FOREIGN KEY([EquipmentTemplateID])
REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID]),
    [PieceOfEquipmentID]   VARCHAR (500)  NULL CONSTRAINT [FK_Uncertainty_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    [Type]                 INT            NOT NULL,
    [RangeMin]             FLOAT (53)     NOT NULL,
    [RangeMax]             FLOAT (53)     NOT NULL,
    [Value]                FLOAT (53)     NOT NULL,
    [Description]          NVARCHAR (MAX) NULL,
    [UnitOfMeasureID]      INT            NOT NULL CONSTRAINT [FK_Uncertainty_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY([UnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    [Divisor]              FLOAT (53)     NOT NULL,
    [SensitiveCoefficient] FLOAT (53)     NOT NULL,
    [ConfidenceLevel]      FLOAT (53)     NOT NULL,
    [Quotient]             FLOAT (53)     NOT NULL,
    [Square]               FLOAT (53)     NOT NULL,
    [Distribution]         NVARCHAR (MAX) NULL,
    [Comment]              NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Uncertainty] PRIMARY KEY CLUSTERED ([UncertaintyID] ASC)
);

