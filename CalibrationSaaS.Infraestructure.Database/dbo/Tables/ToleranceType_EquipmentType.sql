CREATE TABLE [dbo].[ToleranceType_EquipmentType] (
    [Key]             NVARCHAR (450) NOT NULL,
    [EquipmentTypeID] INT            NOT NULL,
    CONSTRAINT [PK_ToleranceType_EquipmentType] PRIMARY KEY CLUSTERED ([Key] ASC, [EquipmentTypeID] ASC),
    CONSTRAINT [FK_ToleranceType_EquipmentType_EquipmentType_EquipmentTypeID] FOREIGN KEY ([EquipmentTypeID]) REFERENCES [dbo].[EquipmentType] ([EquipmentTypeID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ToleranceType_EquipmentType_EquipmentTypeID]
    ON [dbo].[ToleranceType_EquipmentType]([EquipmentTypeID] ASC);

