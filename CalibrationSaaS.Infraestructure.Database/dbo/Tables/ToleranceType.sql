CREATE TABLE [dbo].[ToleranceType] (
    [Key]               VARCHAR (100) NOT NULL,
    [Value]             VARCHAR (100) NULL,
    [CalibrationTypeID] INT           NOT NULL,
    [Enable]            BIT           NULL,
    CONSTRAINT [FK_ToleranceType_CalibrationType_CalibrationTypeId] FOREIGN KEY ([CalibrationTypeID]) REFERENCES [dbo].[CalibrationType] ([CalibrationTypeId]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_ToleranceType_CalibrationTypeId]
    ON [dbo].[ToleranceType]([CalibrationTypeID] ASC);

