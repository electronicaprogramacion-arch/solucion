CREATE TABLE [dbo].[UserInformation] (
    [UserInformationID] INT            IDENTITY (1, 1) NOT NULL,
    [Title]             NVARCHAR (20)  NOT NULL,
    [FirstName]         NVARCHAR (40)  NOT NULL,
    [LastName]          NVARCHAR (40)  NOT NULL,
    [DateOfBirth]       DATETIME2 (7)  NOT NULL,
    [Email]             NVARCHAR (MAX) NOT NULL,
    [Password]          NVARCHAR (MAX) NOT NULL,
    [ConfirmPassword]   NVARCHAR (MAX) NOT NULL,
    [AcceptTerms]       BIT            NOT NULL,
    CONSTRAINT [PK_UserInformation] PRIMARY KEY CLUSTERED ([UserInformationID] ASC)
);

