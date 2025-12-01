CREATE TABLE [dbo].[CalibrationSubType_DynamicProperty] (
    [CalibrationSubTypeId] INT NOT NULL,
    [DynamicPropertyID]    INT NOT NULL,
    CONSTRAINT [PK_CalibrationSubType_DynamicProperty] PRIMARY KEY CLUSTERED ([CalibrationSubTypeId] ASC, [DynamicPropertyID] ASC),
    CONSTRAINT [FK_CalibrationSubType_DynamicProperty_CalibrationSubType_DynamicPropertyID] FOREIGN KEY ([DynamicPropertyID]) REFERENCES [dbo].[CalibrationSubType] ([CalibrationSubTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_DynamicProperty_DynamicPropertyID]
    ON [dbo].[CalibrationSubType_DynamicProperty]([DynamicPropertyID] ASC);

