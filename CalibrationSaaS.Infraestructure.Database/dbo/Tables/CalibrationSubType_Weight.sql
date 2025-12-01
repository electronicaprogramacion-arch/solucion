CREATE TABLE [dbo].[CalibrationSubType_Weight] (
    [WorkOrderDetailID]                       INT            NULL,
    [WeightSetID]                             INT            NOT NULL,
    [CalibrationSubTypeID]                    INT            NOT NULL,
    [SecuenceID]                              INT            NOT NULL,
    [LinearityCalibrationSubTypeId]           INT            NULL,
    [LinearitySequenceID]                     INT            NULL,
    [LinearityWorkOrderDetailId]              INT            NULL,
    [RepeatabilityCalibrationSubTypeId]       INT            NULL,
    [RepeatabilityWorkOrderDetailId]          INT            NULL,
    [ForceCalibrationSubTypeId]               INT            NULL,
    [ForceSequenceID]                         INT            NULL,
    [ForceWorkOrderDetailId]                  INT            NULL,
    [RockwellSequenceID]                      INT            NULL,
    [RockwellCalibrationSubTypeId]            INT            NULL,
    [RockwellWorkOrderDetailId]               INT            NULL,
    [MicroCalibrationSubTypeId]               INT            NULL,
    [MicroSequenceID]                         INT            NULL,
    [MicroWorkOrderDetailId]                  INT            NULL,
    [GenericCalibrationCalibrationSubTypeId]  INT            NULL,
    [GenericCalibrationSequenceID]            INT            NULL,
    [GenericCalibrationWorkOrderDetailId]     INT            NULL,
    [ComponentID]                             VARCHAR (500)  NOT NULL DEFAULT -1,
    [EccentricityCalibrationSubTypeId]        INT            NULL,
    [EccentricityWorkOrderDetailId]           INT            NULL,
    [GenericCalibration2CalibrationSubTypeId] INT            NULL,
    [GenericCalibration2ComponentID]          NVARCHAR (450) NULL,
    [GenericCalibration2SequenceID]           INT            NULL,
    [Component]                               VARCHAR (500)  NULL,
    CONSTRAINT [PK_CalibrationSubType_Weight] PRIMARY KEY CLUSTERED ([ComponentID] ASC, [WeightSetID] ASC, [CalibrationSubTypeID] ASC, [SecuenceID] ASC),
    CONSTRAINT [FK_CalibrationSubType_Weight_CalibrationSubType_CalibrationSubTypeID] FOREIGN KEY ([CalibrationSubTypeID]) REFERENCES [dbo].[CalibrationSubType] ([CalibrationSubTypeId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_Eccentricity_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId] FOREIGN KEY ([EccentricityCalibrationSubTypeId], [EccentricityWorkOrderDetailId]) REFERENCES [dbo].[Eccentricity] ([CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId] FOREIGN KEY ([ForceSequenceID], [ForceCalibrationSubTypeId], [ForceWorkOrderDetailId]) REFERENCES [dbo].[Force] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCali~] FOREIGN KEY ([GenericCalibrationSequenceID], [GenericCalibrationCalibrationSubTypeId], [GenericCalibrationWorkOrderDetailId]) REFERENCES [dbo].[GenericCalibration] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericC~] FOREIGN KEY ([GenericCalibration2SequenceID], [GenericCalibration2CalibrationSubTypeId], [GenericCalibration2ComponentID]) REFERENCES [dbo].[GenericCalibration2] ([SequenceID], [CalibrationSubTypeId], [ComponentID]),
    CONSTRAINT [FK_CalibrationSubType_Weight_Linearity_LinearitySequenceID_LinearityCalibrationSubTypeId_LinearityWorkOrderDetailId] FOREIGN KEY ([LinearitySequenceID], [LinearityCalibrationSubTypeId], [LinearityWorkOrderDetailId]) REFERENCES [dbo].[Linearity] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId] FOREIGN KEY ([MicroSequenceID], [MicroCalibrationSubTypeId], [MicroWorkOrderDetailId]) REFERENCES [dbo].[Micro] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_Repeatability_RepeatabilityCalibrationSubTypeId_RepeatabilityWorkOrderDetailId] FOREIGN KEY ([RepeatabilityCalibrationSubTypeId], [RepeatabilityWorkOrderDetailId]) REFERENCES [dbo].[Repeatability] ([CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId] FOREIGN KEY ([RockwellSequenceID], [RockwellCalibrationSubTypeId], [RockwellWorkOrderDetailId]) REFERENCES [dbo].[Rockwell] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Weight_WeightSet_WeightSetID] FOREIGN KEY ([WeightSetID]) REFERENCES [dbo].[WeightSet] ([WeightSetID])
);











GO


GO


GO


GO


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Weight_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId]
    ON [dbo].[CalibrationSubType_Weight]([RockwellSequenceID] ASC, [RockwellCalibrationSubTypeId] ASC, [RockwellWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Weight_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId]
    ON [dbo].[CalibrationSubType_Weight]([MicroSequenceID] ASC, [MicroCalibrationSubTypeId] ASC, [MicroWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Weight_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDet~]
    ON [dbo].[CalibrationSubType_Weight]([GenericCalibrationSequenceID] ASC, [GenericCalibrationCalibrationSubTypeId] ASC, [GenericCalibrationWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Weight_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Component~]
    ON [dbo].[CalibrationSubType_Weight]([GenericCalibration2SequenceID] ASC, [GenericCalibration2CalibrationSubTypeId] ASC, [GenericCalibration2ComponentID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Weight_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId]
    ON [dbo].[CalibrationSubType_Weight]([EccentricityCalibrationSubTypeId] ASC, [EccentricityWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Weight_CalibrationSubTypeID]
    ON [dbo].[CalibrationSubType_Weight]([CalibrationSubTypeID] ASC);

