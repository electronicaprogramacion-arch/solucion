CREATE TABLE [dbo].[WeightSet] (
    [WeightSetID]                             INT             IDENTITY (1, 1) NOT NULL,
    [WeightValue]                             INT             NOT NULL,
    [UnitOfMeasureID]                         INT             NOT NULL,
    [Note]                                    NVARCHAR (MAX)  NULL,
    [Name]                                    NVARCHAR (100)  NULL,
    [Description]                             NVARCHAR (100)  NULL,
    [PieceOfEquipmentStatus]                  NVARCHAR (MAX)  NULL,
    [PieceOfEquipmentId_new]                  INT             NULL,
    [WeightNominalValue]                      FLOAT (53)      NOT NULL,
    [WeightActualValue]                       FLOAT (53)      NOT NULL,
    [CalibrationUncertValue]                  FLOAT (53)      NOT NULL,
    [UncertaintyUnitOfMeasureId]              INT             NULL,
    [Divisor]                                 FLOAT (53)      NOT NULL,
    [Type]                                    NVARCHAR (MAX)  NULL,
    [Distribution]                            NVARCHAR (MAX)  NULL,
    [PieceOfEquipmentID]                      VARCHAR (500)   NULL,
    [EccentricityCalibrationSubTypeId]        INT             NULL,
    [EccentricityWorkOrderDetailId]           INT             NULL,
    [LinearityCalibrationSubTypeId]           INT             NULL,
    [LinearitySequenceID]                     INT             NULL,
    [LinearityWorkOrderDetailId]              INT             NULL,
    [RepeatabilityCalibrationSubTypeId]       INT             NULL,
    [RepeatabilityWorkOrderDetailId]          INT             NULL,
    [Reference]                               NVARCHAR (MAX)  NULL,
    [IsDelete]                                BIT             CONSTRAINT [DF__WeightSet__Delet__681373AD] DEFAULT ((0)) NOT NULL,
    [Serial]                                  VARCHAR (50)    NULL,
    [ForceCalibrationSubTypeId]               INT             NULL,
    [ForceSequenceID]                         INT             NULL,
    [ForceWorkOrderDetailId]                  INT             NULL,
    [WeightNominalValue2]                     FLOAT (53)      DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [Class]                                   DECIMAL (18, 2) DEFAULT ((0.0)) NOT NULL,
    [RockwellSequenceID]                      INT             NULL,
    [Resolution]                              FLOAT (53)      DEFAULT ((0)) NOT NULL,
    [RockwellCalibrationSubTypeId]            INT             NULL,
    [RockwellWorkOrderDetailId]               INT             NULL,
    [MicroCalibrationSubTypeId]               INT             NULL,
    [MicroSequenceID]                         INT             NULL,
    [MicroWorkOrderDetailId]                  INT             NULL,
    [GenericCalibrationCalibrationSubTypeId]  INT             NULL,
    [GenericCalibrationSequenceID]            INT             NULL,
    [GenericCalibrationWorkOrderDetailId]     INT             NULL,
    [GenericCalibration2CalibrationSubTypeId] INT             NULL,
    [GenericCalibration2ComponentID]          NVARCHAR (450)  NULL,
    [GenericCalibration2SequenceID]           INT             NULL,
    [Option]                                  VARCHAR (50)    NULL,
    CONSTRAINT [PK_WeightSet] PRIMARY KEY CLUSTERED ([WeightSetID] ASC),
    CONSTRAINT [FK_WeightSet_Eccentricity_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId] FOREIGN KEY ([EccentricityCalibrationSubTypeId], [EccentricityWorkOrderDetailId]) REFERENCES [dbo].[Eccentricity] ([CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId] FOREIGN KEY ([ForceSequenceID], [ForceCalibrationSubTypeId], [ForceWorkOrderDetailId]) REFERENCES [dbo].[Force] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrder~] FOREIGN KEY ([GenericCalibrationSequenceID], [GenericCalibrationCalibrationSubTypeId], [GenericCalibrationWorkOrderDetailId]) REFERENCES [dbo].[GenericCalibration] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Compo~] FOREIGN KEY ([GenericCalibration2SequenceID], [GenericCalibration2CalibrationSubTypeId], [GenericCalibration2ComponentID]) REFERENCES [dbo].[GenericCalibration2] ([SequenceID], [CalibrationSubTypeId], [ComponentID]),
    CONSTRAINT [FK_WeightSet_Linearity_LinearitySequenceID_LinearityCalibrationSubTypeId_LinearityWorkOrderDetailId] FOREIGN KEY ([LinearitySequenceID], [LinearityCalibrationSubTypeId], [LinearityWorkOrderDetailId]) REFERENCES [dbo].[Linearity] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId] FOREIGN KEY ([MicroSequenceID], [MicroCalibrationSubTypeId], [MicroWorkOrderDetailId]) REFERENCES [dbo].[Micro] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    CONSTRAINT [FK_WeightSet_Repeatability_RepeatabilityCalibrationSubTypeId_RepeatabilityWorkOrderDetailId] FOREIGN KEY ([RepeatabilityCalibrationSubTypeId], [RepeatabilityWorkOrderDetailId]) REFERENCES [dbo].[Repeatability] ([CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId] FOREIGN KEY ([RockwellSequenceID], [RockwellCalibrationSubTypeId], [RockwellWorkOrderDetailId]) REFERENCES [dbo].[Rockwell] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_WeightSet_UnitOfMeasure_UncertaintyUnitOfMeasureId] FOREIGN KEY ([UncertaintyUnitOfMeasureId]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    CONSTRAINT [FK_WeightSet_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY ([UnitOfMeasureID]) REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
);











GO


GO


GO


GO


GO


GO


GO


GO
CREATE NONCLUSTERED INDEX [IX_WeightSet_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId]
    ON [dbo].[WeightSet]([RockwellSequenceID] ASC, [RockwellCalibrationSubTypeId] ASC, [RockwellWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WeightSet_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId]
    ON [dbo].[WeightSet]([MicroSequenceID] ASC, [MicroCalibrationSubTypeId] ASC, [MicroWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WeightSet_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDetailId]
    ON [dbo].[WeightSet]([GenericCalibrationSequenceID] ASC, [GenericCalibrationCalibrationSubTypeId] ASC, [GenericCalibrationWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WeightSet_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2ComponentID]
    ON [dbo].[WeightSet]([GenericCalibration2SequenceID] ASC, [GenericCalibration2CalibrationSubTypeId] ASC, [GenericCalibration2ComponentID] ASC);

