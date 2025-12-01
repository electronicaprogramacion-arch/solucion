CREATE TABLE [dbo].[CalibrationSubType_ViewProperty] (
    [CalibrationSubTypeId] INT NOT NULL,
    [ViewPropertyID]       INT NOT NULL,
    CONSTRAINT [PK_CalibrationSubType_ViewProperty] PRIMARY KEY CLUSTERED ([CalibrationSubTypeId] ASC, [ViewPropertyID] ASC),
    CONSTRAINT [FK_CalibrationSubType_ViewProperty_CalibrationSubType_ViewPropertyID] FOREIGN KEY ([ViewPropertyID]) REFERENCES [dbo].[CalibrationSubType] ([CalibrationSubTypeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_ViewProperty_ViewPropertyID]
    ON [dbo].[CalibrationSubType_ViewProperty]([ViewPropertyID] ASC);

