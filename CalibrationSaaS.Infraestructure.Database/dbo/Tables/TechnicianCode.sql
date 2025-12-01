CREATE TABLE [dbo].[TechnicianCode] (
    [Code]             NVARCHAR (100) NOT NULL,
    [StateID]          NVARCHAR (100) NOT NULL,
    [CertificationID]  INT            NOT NULL CONSTRAINT [FK_TechnicianCode_Certification_CertificationID] FOREIGN KEY([CertificationID])
REFERENCES [dbo].[Certification] ([CertificationID]),
    [TechnicianCodeID] INT            IDENTITY (1, 1) NOT NULL CONSTRAINT [FK_TechnicianCode_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID]),
    [UserID]           INT            NOT NULL,
    [IsDelete]         BIT            CONSTRAINT [DF__Technicia__Delet__634EBE90] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TechnicianCode] PRIMARY KEY CLUSTERED ([Code] ASC, [StateID] ASC, [CertificationID] ASC)
);

