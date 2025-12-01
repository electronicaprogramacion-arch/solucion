CREATE TABLE [dbo].[Eccentricity] (
    [CalibrationSubTypeId]                INT            NOT NULL,
    [WorkOrderDetailId]                   INT            NOT NULL CONSTRAINT [FK_Eccentricity_BalanceAndScaleCalibration_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[BalanceAndScaleCalibration] ([WorkOrderDetailId]),
    [TenantId]                            INT            NOT NULL,
    [NumberOfSamples]                     INT            NOT NULL,
    [TestPointID]                         INT            NULL CONSTRAINT [FK_Eccentricity_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID]),
    [EccentricityMax]                     FLOAT (53)     NOT NULL,
    [EccentricityMin]                     FLOAT (53)     NOT NULL,
    [EccentricityVarianceAsLeft]          FLOAT (53)     NOT NULL,
    [EccentricityVarianceAsFound]         FLOAT (53)     NOT NULL,
    [EccentricityUncertaintyValueUOMID]   INT            NOT NULL,
    [EccentricityUncertaintyType]         NVARCHAR (MAX) NULL,
    [EccentricityUncertaintyDistribution] NVARCHAR (MAX) NULL,
    [EccentricityUncertaintyDivisor]      FLOAT (53)     NOT NULL,
    [EccentricityQuotient]                FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_Eccentricity] PRIMARY KEY CLUSTERED ([CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC)
);

