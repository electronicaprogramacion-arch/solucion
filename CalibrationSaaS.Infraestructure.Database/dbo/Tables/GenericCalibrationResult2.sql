CREATE TABLE [dbo].[GenericCalibrationResult2] (
    [SequenceID]           INT            NOT NULL,
    [CalibrationSubTypeId] INT            NOT NULL,
    [ComponentID]          NVARCHAR (450) NOT NULL,
    [WorkOrderDetailId]    INT            NOT NULL,
    [Position]             INT            NOT NULL,
    [Resolution]           FLOAT (53)     NOT NULL,
    [DecimalNumber]        INT            NOT NULL,
    [Object]               VARCHAR (MAX)  NULL,
    [ExtendedObject]       VARCHAR (MAX)  NULL,
    [PieceOfEquipmentID]   VARCHAR (500)  NULL,
    [Component]            VARCHAR (500)  NULL,
    [Updated]              BIGINT         NULL,
    CONSTRAINT [PK_GenericCalibrationResult2] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [ComponentID] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_GenericCalibrationResult2_PieceOfEquipmentID]
    ON [dbo].[GenericCalibrationResult2]([PieceOfEquipmentID] ASC);

