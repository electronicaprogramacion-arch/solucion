CREATE TABLE [dbo].[Note] (
    [NoteId]               INT            IDENTITY (1, 1) NOT NULL,
    [Text]                 NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [Commnet]              INT            NULL,
    [Condition]            INT            NOT NULL,
    [EquipmnetTypeId]      INT            NULL,
    [TestCodeID]           INT            NULL,
    [Position]             INT            CONSTRAINT [DF__Note__Position__29CC2871] DEFAULT ((0)) NOT NULL,
    [CalibrationSubtypeId] INT            NULL,
    [Validation]           TEXT           NULL,
    CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED ([NoteId] ASC),
    CONSTRAINT [FK_Note_EquipmentType_EquipmnetTypeId] FOREIGN KEY ([EquipmnetTypeId]) REFERENCES [dbo].[EquipmentType] ([EquipmentTypeID]),
    CONSTRAINT [FK_Note_TestCode_TestCodeID] FOREIGN KEY ([TestCodeID]) REFERENCES [dbo].[TestCode] ([TestCodeID])
);












GO
CREATE NONCLUSTERED INDEX [IX_Note_EquipmnetTypeId]
    ON [dbo].[Note]([EquipmnetTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Note_TestCodeID]
    ON [dbo].[Note]([TestCodeID] ASC);

