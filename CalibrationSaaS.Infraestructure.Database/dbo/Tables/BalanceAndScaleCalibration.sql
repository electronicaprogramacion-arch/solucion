CREATE TABLE [dbo].[BalanceAndScaleCalibration] (
    [WorkOrderDetailId]                    INT            NOT NULL,
    [CalibrationTypeId]                    INT            NOT NULL,
    [TenantId]                             INT            NOT NULL,
    [HasLinearity]                         BIT            NOT NULL,
    [HasEccentricity]                      BIT            NOT NULL,
    [HasRepeatability]                     BIT            NOT NULL,
    [EnvironmentalFactor]                  FLOAT (53)     NOT NULL,
    [EnvironmentalUncertaintyValueUOMID]   INT            NOT NULL,
    [EnvironmentalUncertaintyType]         NVARCHAR (MAX) NULL,
    [EnvironmentalUncertaintyDistribution] NVARCHAR (MAX) NULL,
    [EnvironmentalUncertaintyDivisor]      FLOAT (53)     NOT NULL,
    [Resolution]                           FLOAT (53)     NOT NULL,
    [ResolutionFormatted]                  FLOAT (53)     NOT NULL,
    [ResolutionNumber]                     FLOAT (53)     NOT NULL,
    [ResolutionUncertaintyValueUOMID]      INT            NOT NULL,
    [ResolutionUncertaintyType]            NVARCHAR (MAX) NULL,
    [ResolutionUncertaintyDistribution]    NVARCHAR (MAX) NULL,
    [ResolutionUncertaintyDivisor]         FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_BalanceAndScaleCalibration] PRIMARY KEY CLUSTERED ([WorkOrderDetailId] ASC)
);

GO
ALTER TABLE [dbo].[BalanceAndScaleCalibration] ADD CONSTRAINT [FK_BalanceAndScaleCalibration_WorkOrderDetail_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID]);
