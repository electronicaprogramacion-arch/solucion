CREATE TABLE [dbo].[CalibrationSubType_Standard] (
    [WorkOrderDetailID]                       INT            NULL,
    [PieceOfEquipmentID]                      VARCHAR (500)  NOT NULL,
    [CalibrationSubTypeID]                    INT            NOT NULL,
    [SecuenceID]                              INT            NOT NULL,
    [RockwellCalibrationSubTypeId]            INT            NULL,
    [RockwellSequenceID]                      INT            NULL,
    [RockwellWorkOrderDetailId]               INT            NULL,
    [MicroCalibrationSubTypeId]               INT            NULL,
    [MicroSequenceID]                         INT            NULL,
    [MicroWorkOrderDetailId]                  INT            NULL,
    [GenericCalibrationCalibrationSubTypeId]  INT            NULL,
    [GenericCalibrationSequenceID]            INT            NULL,
    [GenericCalibrationWorkOrderDetailId]     INT            NULL,
    [ComponentID]                             VARCHAR (500)  NOT NULL DEFAULT -1,
    [GenericCalibration2CalibrationSubTypeId] INT            NULL,
    [GenericCalibration2ComponentID]          NVARCHAR (450) NULL,
    [GenericCalibration2SequenceID]           INT            NULL,
    [Component]                               VARCHAR (500)  NULL,
    CONSTRAINT [PK_CalibrationSubType_Standard] PRIMARY KEY CLUSTERED ([ComponentID] ASC, [PieceOfEquipmentID] ASC, [CalibrationSubTypeID] ASC, [SecuenceID] ASC),
    CONSTRAINT [FK_CalibrationSubType_Standard_CalibrationSubType_CalibrationSubTypeID] FOREIGN KEY ([CalibrationSubTypeID]) REFERENCES [dbo].[CalibrationSubType] ([CalibrationSubTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_CalibrationSubType_Standard_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCa~] FOREIGN KEY ([GenericCalibrationSequenceID], [GenericCalibrationCalibrationSubTypeId], [GenericCalibrationWorkOrderDetailId]) REFERENCES [dbo].[GenericCalibration] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Standard_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_Generi~] FOREIGN KEY ([GenericCalibration2SequenceID], [GenericCalibration2CalibrationSubTypeId], [GenericCalibration2ComponentID]) REFERENCES [dbo].[GenericCalibration2] ([SequenceID], [CalibrationSubTypeId], [ComponentID]),
    CONSTRAINT [FK_CalibrationSubType_Standard_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId] FOREIGN KEY ([MicroSequenceID], [MicroCalibrationSubTypeId], [MicroWorkOrderDetailId]) REFERENCES [dbo].[Micro] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId]),
    CONSTRAINT [FK_CalibrationSubType_Standard_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]) ON DELETE CASCADE,
    CONSTRAINT [FK_CalibrationSubType_Standard_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId] FOREIGN KEY ([RockwellSequenceID], [RockwellCalibrationSubTypeId], [RockwellWorkOrderDetailId]) REFERENCES [dbo].[Rockwell] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
);




GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Standard_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId]
    ON [dbo].[CalibrationSubType_Standard]([MicroSequenceID] ASC, [MicroCalibrationSubTypeId] ASC, [MicroWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Standard_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderD~]
    ON [dbo].[CalibrationSubType_Standard]([GenericCalibrationSequenceID] ASC, [GenericCalibrationCalibrationSubTypeId] ASC, [GenericCalibrationWorkOrderDetailId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_Standard_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Compone~]
    ON [dbo].[CalibrationSubType_Standard]([GenericCalibration2SequenceID] ASC, [GenericCalibration2CalibrationSubTypeId] ASC, [GenericCalibration2ComponentID] ASC);

