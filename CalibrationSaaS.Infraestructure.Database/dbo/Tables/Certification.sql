CREATE TABLE [dbo].[Certification] (
    [CertificationID]   INT            NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [CalibrationTypeID] INT            CONSTRAINT [DF__Certifica__Calib__1A9EF37A] DEFAULT ((0)) NOT NULL,
    [ID]                INT            CONSTRAINT [DF__Certificatio__ID__1B9317B3] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Certification] PRIMARY KEY CLUSTERED ([CertificationID] ASC)
);



