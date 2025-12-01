CREATE TABLE [dbo].[CalibrationResultContributor] (
    [CalibrationResultContributorID] NVARCHAR (450) NOT NULL,
    [EquipmentTemplateID]            INT            NULL,
    [PieceOfEquipmentID]             VARCHAR (500)  NULL,
    [Type]                           INT            NOT NULL,
    [UnitOfMeasureID]                INT            NOT NULL,
    [Divisor]                        FLOAT (53)     NOT NULL,
    [SensitiveCoefficient]           FLOAT (53)     NOT NULL,
    [ConfidenceLevel]                FLOAT (53)     NOT NULL,
    [Quotient]                       FLOAT (53)     NOT NULL,
    [Square]                         FLOAT (53)     NOT NULL,
    [Distribution]                   NVARCHAR (MAX) NULL,
    [Comment]                        NVARCHAR (MAX) NULL,
    [TypeContributor]                NVARCHAR (MAX) NULL,
    [Magnitude]                      FLOAT (53)     NOT NULL,
    [Description]                    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CalibrationResultContributor] PRIMARY KEY CLUSTERED ([CalibrationResultContributorID] ASC),
    CONSTRAINT [FK_CalibrationResultContributor_EquipmentTemplate_EquipmentTemplateID] FOREIGN KEY ([EquipmentTemplateID]) REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID]),
    CONSTRAINT [FK_CalibrationResultContributor_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
);




GO
CREATE NONCLUSTERED INDEX [IX_CalibrationResultContributor_EquipmentTemplateID]
    ON [dbo].[CalibrationResultContributor]([EquipmentTemplateID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationResultContributor_PieceOfEquipmentID]
    ON [dbo].[CalibrationResultContributor]([PieceOfEquipmentID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationResultContributor_UnitOfMeasureID]
    ON [dbo].[CalibrationResultContributor]([UnitOfMeasureID] ASC);

