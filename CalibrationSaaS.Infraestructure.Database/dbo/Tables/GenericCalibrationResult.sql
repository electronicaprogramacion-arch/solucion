CREATE TABLE [dbo].[GenericCalibrationResult] (
    [SequenceID]           INT           NOT NULL,
    [CalibrationSubTypeId] INT           NOT NULL,
    [WorkOrderDetailId]    INT           NOT NULL,
    [Position]             INT           NOT NULL,
    [Resolution]           FLOAT (53)    CONSTRAINT [DF__GenericCa__Resol__7D98A078] DEFAULT ((0.0000000000000000e+000)) NOT NULL,
    [DecimalNumber]        INT           CONSTRAINT [DF__GenericCa__Decim__7CA47C3F] DEFAULT ((0)) NOT NULL,
    [Object]               VARCHAR (MAX) NULL,
    [ExtendedObject]       VARCHAR (MAX) NULL,
    [ComponentID]          VARCHAR (500) NULL,
    [Updated]              BIGINT        NULL,
    CONSTRAINT [PK_GenericCalibrationResult] PRIMARY KEY CLUSTERED ([SequenceID] ASC, [CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC)
);















