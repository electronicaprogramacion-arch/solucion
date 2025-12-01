CREATE TABLE [dbo].[Repeatability] (
    [CalibrationSubTypeId]                 INT            NOT NULL,
    [WorkOrderDetailId]                    INT            NOT NULL CONSTRAINT [FK_Repeatability_BalanceAndScaleCalibration_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[BalanceAndScaleCalibration] ([WorkOrderDetailId]),
    [RepeatabilityId]                      INT            NOT NULL,
    [TenantId]                             INT            NOT NULL,
    [NumberOfSamples]                      INT            NOT NULL,
    [RepeatabilityStdDeviationAsLeft]      FLOAT (53)     NOT NULL,
    [RepeatabilityStdDeviationAsFound]     FLOAT (53)     NOT NULL,
    [RepeatabilityUncertaintyValueUOMID]   INT            NOT NULL,
    [RepeatabilityUncertaintyType]         NVARCHAR (MAX) NULL,
    [RepeatabilityUncertaintyDistribution] NVARCHAR (MAX) NULL,
    [RepeatabilityUncertaintyDivisor]      FLOAT (53)     NOT NULL,
    [RepeatabilityQuotient]                FLOAT (53)     NOT NULL,
    [TestPointID]                          INT            NULL CONSTRAINT [FK_Repeatability_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID]),
    CONSTRAINT [PK_Repeatability] PRIMARY KEY CLUSTERED ([CalibrationSubTypeId] ASC, [WorkOrderDetailId] ASC)
);

