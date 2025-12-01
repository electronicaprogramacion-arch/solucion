CREATE TABLE [dbo].[POE_POE] (
    [PieceOfEquipmentID]  VARCHAR (500) NOT NULL,
    [PieceOfEquipmentID2] VARCHAR (500) NOT NULL CONSTRAINT [FK_POE_POE_PieceOfEquipment_PieceOfEquipmentID2] FOREIGN KEY([PieceOfEquipmentID2])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
);

