CREATE TABLE [dbo].[User] (
    [UserID]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (50)  NOT NULL,
    [LastName]           NVARCHAR (50)  NOT NULL,
    [UserTypeID]         NVARCHAR (50)  NULL,
    [PasswordReset]      BIT            NULL,
    [IsEnabled]          BIT            NULL,
    [UserName]           NVARCHAR (MAX) NULL,
    [Email]              NVARCHAR (450) NOT NULL,
    [Roles]              NVARCHAR (MAX) NULL,
    [Occupation]         NVARCHAR (MAX) NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [PieceOfEquipmentID] VARCHAR (500)  NULL,
    [WorkOrderId]        INT            NULL,
    [IdentityID]         NVARCHAR (MAX) NULL,
    [PassWord]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_User_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]),
    CONSTRAINT [FK_User_WorkOrder_WorkOrderId] FOREIGN KEY ([WorkOrderId]) REFERENCES [dbo].[WorkOrder] ([WorkOrderId])
);



