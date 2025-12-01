CREATE TABLE [dbo].[TestPoint] (
    [TestPointID]                   INT            IDENTITY (1, 1) NOT NULL,
    [NominalTestPoit]               FLOAT (53)     NOT NULL,
    [LowerTolerance]                FLOAT (53)     NOT NULL,
    [UpperTolerance]                FLOAT (53)     NOT NULL,
    [Resolution]                    FLOAT (53)     NOT NULL,
    [DecimalNumber]                 INT            NOT NULL,
    [TestPointGroupTestPoitGroupID] INT            NULL CONSTRAINT [FK_TestPoint_TestPointGroup_TestPointGroupTestPoitGroupID] FOREIGN KEY([TestPointGroupTestPoitGroupID])
REFERENCES [dbo].[TestPointGroup] ([TestPoitGroupID]),
    [UnitOfMeasurementID]           INT            NOT NULL CONSTRAINT [FK_TestPointsin_UnitOfMeasureIn_UnitOfMeasureInID] FOREIGN KEY([UnitOfMeasurementID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    [UnitOfMeasureOutID]            INT            NOT NULL CONSTRAINT [FK_TestPointsOut_UnitOfMeasureOut_UnitOfMeasureOutID] FOREIGN KEY([UnitOfMeasureOutID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID]),
    [CalibrationType]               NVARCHAR (MAX) NULL,
    [Description]                   NVARCHAR (MAX) NULL,
    [TestPointTarget]               INT            DEFAULT ((0)) NOT NULL,
    [IsDescendant]                  BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [Position]                      INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TestPoint] PRIMARY KEY CLUSTERED ([TestPointID] ASC)
);

