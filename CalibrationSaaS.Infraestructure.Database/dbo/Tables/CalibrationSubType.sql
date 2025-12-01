CREATE TABLE [dbo].[CalibrationSubType] (
    [CalibrationSubTypeId]     INT            NOT NULL,
    [CalibrationTypeId]        INT            NOT NULL,
    [Name]                     NVARCHAR (MAX) NULL,
    [NameToShow]               NVARCHAR (MAX) NULL,
    [CreateClass]              VARCHAR (MAX)  NULL,
    [GetClass]                 VARCHAR (500)  NULL,
    [CalibrationSubTypeViewID] INT            NULL,
    [Enabled]                  BIT            CONSTRAINT [DF__Calibrati__Enabl__50FB042B] DEFAULT ((1)) NOT NULL,
    [Position]                 INT            CONSTRAINT [DF__Calibrati__Posit__5006DFF2] DEFAULT ((1)) NOT NULL,
    [SelectStandarClass]       VARCHAR (500)  NULL,
    [OnChangeClass]            VARCHAR (500)  NULL,
    [NewClass]                 VARCHAR (500)  NULL,
    [Select2StandarClass]      VARCHAR (500)  NULL,
    [StandardAssignComponent]  VARCHAR (500)  NULL,
    [StandardAssignComponent2] VARCHAR (500)  NULL,
    [Mandatory]                BIT            CONSTRAINT [DF_CalibrationSubType_Mandatory] DEFAULT ((1)) NOT NULL,
    [SupportCSV]               BIT            NULL,
    CONSTRAINT [PK_CalibrationSubType] PRIMARY KEY CLUSTERED ([CalibrationSubTypeId] ASC),
    CONSTRAINT [FK_CalibrationSubType_CalibrationSubTypeView_CalibrationSubTypeViewID] FOREIGN KEY ([CalibrationSubTypeViewID]) REFERENCES [dbo].[CalibrationSubTypeView] ([CalibrationSubTypeViewID]),
    CONSTRAINT [FK_CalibrationSubType_CalibrationType_CalibrationTypeId] FOREIGN KEY ([CalibrationTypeId]) REFERENCES [dbo].[CalibrationType] ([CalibrationTypeId])
);






























GO
CREATE NONCLUSTERED INDEX [IX_CalibrationSubType_CalibrationTypeId]
    ON [dbo].[CalibrationSubType]([CalibrationTypeId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CalibrationSubType_CalibrationSubTypeViewID]
    ON [dbo].[CalibrationSubType]([CalibrationSubTypeViewID] ASC) WHERE ([CalibrationSubTypeViewID] IS NOT NULL);

