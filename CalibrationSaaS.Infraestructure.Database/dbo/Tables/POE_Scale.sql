CREATE TABLE [dbo].[POE_Scale] (
    [PieceOfEquipmentID] VARCHAR (500) NOT NULL,
    [Scale]              VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_POE_Scale] PRIMARY KEY CLUSTERED ([PieceOfEquipmentID] ASC, [Scale] ASC),
    CONSTRAINT [FK_POE_Scale_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY ([PieceOfEquipmentID]) REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID]) ON DELETE CASCADE
);

