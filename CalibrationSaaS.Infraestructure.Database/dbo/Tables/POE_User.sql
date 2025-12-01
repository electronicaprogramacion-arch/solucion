CREATE TABLE [dbo].[POE_User] (
    [UserID]             INT           NOT NULL CONSTRAINT [FK_POE_User_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID]),
    [PieceOfEquipmentID] VARCHAR (500) NOT NULL CONSTRAINT [FK_POE_User_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
);

