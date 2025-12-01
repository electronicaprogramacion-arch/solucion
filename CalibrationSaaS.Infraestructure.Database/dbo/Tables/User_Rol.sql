CREATE TABLE [dbo].[User_Rol] (
    [UserID]      INT           NOT NULL CONSTRAINT [FK_User_Rol_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID]),
    [RolID]       INT           NOT NULL CONSTRAINT [FK_User_Rol_Rol_RolID] FOREIGN KEY([RolID])
REFERENCES [dbo].[Rol] ([RolID]),
    [Permissions] VARCHAR (200) NULL,
    CONSTRAINT [PK_User_Rol] PRIMARY KEY CLUSTERED ([UserID] ASC, [RolID] ASC)
);

