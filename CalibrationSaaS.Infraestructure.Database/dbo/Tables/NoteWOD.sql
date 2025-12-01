CREATE TABLE [dbo].[NoteWOD] (
    [NoteWODId]            INT            IDENTITY (1, 1) NOT NULL,
    [Text]                 NVARCHAR (MAX) NULL,
    [WorkOrderDetailId]    INT            NULL,
    [Position]             INT            CONSTRAINT [DF_NoteWOD__Position] DEFAULT ((0)) NOT NULL,
    [CalibrationSubtypeId] INT            NULL,
    CONSTRAINT [PK_NoteWOD] PRIMARY KEY CLUSTERED ([NoteWODId] ASC),
    CONSTRAINT [FK_NoteWOD_WorkOrderDetail_WorkOrderDetailId] FOREIGN KEY ([WorkOrderDetailId]) REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
);


GO


GO


GO


GO
