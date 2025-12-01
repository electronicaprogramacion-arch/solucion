GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CalibrationType]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CalibrationType](
	[CalibrationTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_CalibrationType] PRIMARY KEY CLUSTERED 
(
	[CalibrationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate](
	[CertificateNumber] [nvarchar](50) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[CalibrationDate] [datetime2](7) NOT NULL,
	[Company] [nvarchar](max) NULL,
	[AffectDueDate] [bit] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[WorkOrderDetailId] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[WorkOrderDetailSerialized] [nvarchar](max) NULL,
	[CertificateID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Certificate] PRIMARY KEY CLUSTERED 
(
	[CertificateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certification]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certification](
	[CertificationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Certification] PRIMARY KEY CLUSTERED 
(
	[CertificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentType]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentType](
	[EquipmentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsBalance] [bit] NOT NULL,
 CONSTRAINT [PK_EquipmentType] PRIMARY KEY CLUSTERED 
(
	[EquipmentTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 11/10/2021 2:29:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[RolID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [varchar](200) NULL,
	[DefaultPermissions] [varchar](200) NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[RolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210225183638_itdw', N'3.1.10')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210902154244_inicial450', N'5.0.7')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210922162227_inicial451', N'5.0.7')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210922162637_inicial453', N'5.0.7')
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (6, N'e0466e64-b0c7-4abf-90ff-d6e6e4831ed9', N'location', N'somewhere')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (7, N'e0466e64-b0c7-4abf-90ff-d6e6e4831ed9', N'website', N'http://bob.com')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (8, N'e0466e64-b0c7-4abf-90ff-d6e6e4831ed9', N'family_name', N'Smith')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (9, N'e0466e64-b0c7-4abf-90ff-d6e6e4831ed9', N'given_name', N'Bob')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (10, N'e0466e64-b0c7-4abf-90ff-d6e6e4831ed9', N'name', N'Bob Smith')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (11, N'8416aa28-7c8d-4f07-a7cf-8120779741b2', N'name', N'paulo')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (12, N'8416aa28-7c8d-4f07-a7cf-8120779741b2', N'given_name', N'Paulo')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (13, N'8416aa28-7c8d-4f07-a7cf-8120779741b2', N'family_name', N'Burgos')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (14, N'8416aa28-7c8d-4f07-a7cf-8120779741b2', N'website', N'http://alice.com')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (15, N'8416aa28-7c8d-4f07-a7cf-8120779741b2', N'ApplicationRole', N'Owner')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (16, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'name', N'alice')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (17, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'given_name', N'Alice')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (18, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'family_name', N'Smith')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (19, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'website', N'http://alice.com')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (20, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'role', N'admin')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (21, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'ApplicationRole', N'Owner')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (22, N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'role', N'pepe')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (23, N'c85bf857-689c-4d8e-a2c7-a6c5be90f9a5', N'name', N'roger')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (24, N'c85bf857-689c-4d8e-a2c7-a6c5be90f9a5', N'given_name', N'roger')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (25, N'c85bf857-689c-4d8e-a2c7-a6c5be90f9a5', N'role', N'admin')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (26, N'b86fd027-c906-4988-bae0-4dd7132608e1', N'name', N'matt')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (27, N'b86fd027-c906-4988-bae0-4dd7132608e1', N'given_name', N'matt')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (28, N'b86fd027-c906-4988-bae0-4dd7132608e1', N'role', N'admin')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (29, N'd39f1c93-7075-4c7e-a6c6-77d33af3295e', N'name', N'Chris')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (30, N'd39f1c93-7075-4c7e-a6c6-77d33af3295e', N'given_name', N'Chris')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (31, N'03071bba-f74b-493c-a16c-b289150d9414', N'name', N'taylor')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (32, N'03071bba-f74b-493c-a16c-b289150d9414', N'given_name', N'taylor')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (33, N'aad26661-b59c-4f5f-a6f4-1c33333ac43b', N'name', N'mike')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (34, N'aad26661-b59c-4f5f-a6f4-1c33333ac43b', N'given_name', N'mike')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (35, N'd39f1c93-7075-4c7e-a6c6-77d33af3295e', N'role', N'tech')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (36, N'03071bba-f74b-493c-a16c-b289150d9414', N'role', N'tech')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (37, N'03071bba-f74b-493c-a16c-b289150d9414', N'role', N'tech.HasView')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (38, N'03071bba-f74b-493c-a16c-b289150d9414', N'role', N'tech.HasEdit')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (39, N'03071bba-f74b-493c-a16c-b289150d9414', N'role', N'tech.HasSelect')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (40, N'aad26661-b59c-4f5f-a6f4-1c33333ac43b', N'role', N'admin')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (41, N'b858dcf5-b62b-4347-872d-6e41d7a7a272', N'name', N'test3')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (42, N'b858dcf5-b62b-4347-872d-6e41d7a7a272', N'given_name', N'test3')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (43, N'b858dcf5-b62b-4347-872d-6e41d7a7a272', N'role', N'admin')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (44, N'b858dcf5-b62b-4347-872d-6e41d7a7a272', N'role', N'admin.HasFullacces')
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'03071bba-f74b-493c-a16c-b289150d9414', N'taylor', N'TAYLOR', N'taylor@bittermascales.com ', N'TAYLOR@BITTERMASCALES.COM ', 1, N'AQAAAAEAACcQAAAAECECYvRJKSKUogJ96CMKJo+XKZPgFGa71Nk3yDua4R68kzjFWnr/hBsoYqnGGfWyog==', N'WCXDRBYXSTKQDEGTO5RXOPKCNXKBK4CA', N'5aa71376-ea4b-4fec-bff8-c5e370e0ed1c', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8416aa28-7c8d-4f07-a7cf-8120779741b2', N'paulo', N'PAULO', N'pburgos@kavoku.com', N'PBURGOS@KAVOKU.COM', 1, N'AQAAAAEAACcQAAAAECpfc/lEcF6p5wEkdDo1htNduq6z5psUfSosNdymTG9pa2Jkr7tmYpYLcUsfln5ZUA==', N'AUJ6YGLKDTWPG3SSYSNCCAG3ZQ7VDERD', N'e10b4796-695a-41df-876c-3f50012f2fea', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'aad26661-b59c-4f5f-a6f4-1c33333ac43b', N'mike', N'MIKE', N'mike2@bittermanscales.com ', N'MIKE2@BITTERMANSCALES.COM ', 1, N'AQAAAAEAACcQAAAAEJ1NBZuCmTGGhbxXBJAVsSu3/6QyE/5AvPcu/Q3FcwtyfeMKw4rBoqBlbfBu8dTl5Q==', N'ZDTQFBZ3BG5EKTQOL6IZXGW4MKDNFCYT', N'da445ce1-e843-4167-891b-614cf25bbab0', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'b858dcf5-b62b-4347-872d-6e41d7a7a272', N'test3', N'TEST3', N'test3@test.com', N'TEST3@TEST.COM', 1, N'AQAAAAEAACcQAAAAEDS1IxGB7SKYar1TK2CKMvATqAZyy39sel4HGvTxI5sTZTwpnXs32VibjBRaTfj14A==', N'MPWBFIS2VERT2ODYD3BT7GBOOHLCBST4', N'563821c4-d93c-47f2-a52b-5e11a91e1bf1', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'b86fd027-c906-4988-bae0-4dd7132608e1', N'matt', N'MATT', N'matt@bittermanscales.com', N'MATT@BITTERMANSCALES.COM', 1, N'AQAAAAEAACcQAAAAEKWG2RY9OJNGx1od72M+VsuCbdVZ9F5059a/yZ7+15k2c9f6eP+cpZXMKXvTMJ7YzA==', N'JYPRJMDUDGS47MC6BMWQC2MB2VI5ZDY6', N'989f5528-5aa3-4e2a-8c1c-0760c092c670', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'c85bf857-689c-4d8e-a2c7-a6c5be90f9a5', N'roger', N'ROGER', N'roger@bittermanscales.com', N'ROGER@BITTERMANSCALES.COM', 1, N'AQAAAAEAACcQAAAAEGvawXpJAtCzFKJTPDxFRCU617zQTJCGMEOEdZustcUCRwdmKCeZdoOSfqK3B5b1yw==', N'TQ3YKHJTMMH4OVMPWDZHWP4A74NTR2CX', N'c4743dd8-c1ac-4565-8439-83c0e4cfbae5', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'cb828e8f-68cb-4e63-ae90-5de6cd3cb354', N'alice', N'ALICE', N'AliceSmith@email.com', N'ALICESMITH@EMAIL.COM', 1, N'AQAAAAEAACcQAAAAEEdB5DlaJpRw25JnmQZss8AxOaLq1r0OaGz0B63eGyBLncJdXYrtd3PY9ghnVWxb1Q==', N'YX6Y6XC7OB5YFOEK2AFNHGQRCT6UOAGN', N'ed581758-919b-45b1-9f94-65eab422a5e1', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd39f1c93-7075-4c7e-a6c6-77d33af3295e', N'Chris', N'CHRIS', N'chris@bittermanscales.com', N'CHRIS@BITTERMANSCALES.COM', 1, N'AQAAAAEAACcQAAAAEA8V4+Wn495XaYS0q1vB/tTmPUUUDSPLqx4ItkOnMiYzrGl8bS0dcgEDC+oRJo9jyg==', N'KAIJNZOGDLKU7LZNA4RH2XORSURUXLER', N'82790632-f6bd-4f71-8b69-1f7356ae4ddc', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'e0466e64-b0c7-4abf-90ff-d6e6e4831ed9', N'bob', N'BOB', N'BobSmith@email.com', N'BOBSMITH@EMAIL.COM', 1, N'AQAAAAEAACcQAAAAED6AaWfUkdiXQVR697kRYzDMy8TJTvccyy8QWgy6BH4RUzCBKQ7fv5Ork+e20ThAzQ==', N'WB6MLK3KIQWXTJXTDFIV4CBDVDNIMNFM', N'eee6a263-bab2-4c75-9df2-0ac3cc1838b0', NULL, 0, 0, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[CalibrationType] ON 
GO
INSERT [dbo].[CalibrationType] ([CalibrationTypeId], [Name], [Description]) VALUES (1, N'17025 Calibration Type', N'17025 Calibration Type')
GO
SET IDENTITY_INSERT [dbo].[CalibrationType] OFF
GO
SET IDENTITY_INSERT [dbo].[Certification] ON 
GO
INSERT [dbo].[Certification] ([CertificationID], [Name], [Description]) VALUES (1, N'17025 Calibration Type', N'17025 Calibration Type')
GO
SET IDENTITY_INSERT [dbo].[Certification] OFF
GO
SET IDENTITY_INSERT [dbo].[EquipmentType] ON 
GO
INSERT [dbo].[EquipmentType] ([EquipmentTypeID], [Name], [IsEnabled], [IsBalance]) VALUES (1, N'Indicator', 1, 0)
GO
INSERT [dbo].[EquipmentType] ([EquipmentTypeID], [Name], [IsEnabled], [IsBalance]) VALUES (2, N'Weight Set/Kit', 1, 0)
GO
INSERT [dbo].[EquipmentType] ([EquipmentTypeID], [Name], [IsEnabled], [IsBalance]) VALUES (3, N'Scale', 1, 1)
GO
INSERT [dbo].[EquipmentType] ([EquipmentTypeID], [Name], [IsEnabled], [IsBalance]) VALUES (4, N'Peripheral', 1, 0)
GO
SET IDENTITY_INSERT [dbo].[EquipmentType] OFF
GO
SET IDENTITY_INSERT [dbo].[Rol] ON 
GO
INSERT [dbo].[Rol] ([RolID], [Name], [Description], [DefaultPermissions]) VALUES (1, N'tech', N'', N'HasSelect,HasView,')
GO
INSERT [dbo].[Rol] ([RolID], [Name], [Description], [DefaultPermissions]) VALUES (2, N'admin', N'tech', NULL)
GO
INSERT [dbo].[Rol] ([RolID], [Name], [Description], [DefaultPermissions]) VALUES (3, N'job', NULL, N'ContractReview,HasSave,Override,HasView,HasEdit,')
GO
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 11/10/2021 2:29:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EquipmentType] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsBalance]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO


GO
/****** Object:  Table [dbo].[Address]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[StreetAddress1] [nvarchar](max) NOT NULL,
	[StreetAddress2] [nvarchar](max) NULL,
	[StreetAddress3] [nvarchar](max) NULL,
	[CityID] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NULL,
	[StateID] [nvarchar](max) NOT NULL,
	[State] [nvarchar](max) NULL,
	[ZipCode] [nvarchar](max) NOT NULL,
	[CountryID] [nvarchar](max) NOT NULL,
	[Country] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[AggregateID] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[County] [nvarchar](max) NULL,
	[CustomerAggregateAggregateID] [int] NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BalanceAndScaleCalibration]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BalanceAndScaleCalibration](
	[WorkOrderDetailId] [int] NOT NULL,
	[CalibrationTypeId] [int] NOT NULL,
	[TenantId] [int] NOT NULL,
	[HasLinearity] [bit] NOT NULL,
	[HasEccentricity] [bit] NOT NULL,
	[HasRepeatability] [bit] NOT NULL,
	[EnvironmentalFactor] [float] NOT NULL,
	[EnvironmentalUncertaintyValueUOMID] [int] NOT NULL,
	[EnvironmentalUncertaintyType] [nvarchar](max) NULL,
	[EnvironmentalUncertaintyDistribution] [nvarchar](max) NULL,
	[EnvironmentalUncertaintyDivisor] [float] NOT NULL,
	[Resolution] [float] NOT NULL,
	[ResolutionFormatted] [float] NOT NULL,
	[ResolutionNumber] [float] NOT NULL,
	[ResolutionUncertaintyValueUOMID] [int] NOT NULL,
	[ResolutionUncertaintyType] [nvarchar](max) NULL,
	[ResolutionUncertaintyDistribution] [nvarchar](max) NULL,
	[ResolutionUncertaintyDivisor] [float] NOT NULL,
 CONSTRAINT [PK_BalanceAndScaleCalibration] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BasicCalibrationResult]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BasicCalibrationResult](
	[SequenceID] [int] NOT NULL,
	[CalibrationSubTypeId] [int] NOT NULL,
	[WorkOrderDetailId] [int] NOT NULL,
	[AsFound] [float] NOT NULL,
	[AsLeft] [float] NOT NULL,
	[WeightApplied] [float] NOT NULL,
	[ReadingStandard] [float] NOT NULL,
	[FinalReadingStandard] [float] NOT NULL,
	[TestResultID] [int] NOT NULL,
	[Uncertainty] [float] NULL,
	[Position] [int] NOT NULL,
	[UnitOfMeasureID] [int] NOT NULL,
	[InToleranceFound] [nvarchar](max) NULL,
	[InToleranceLeft] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_BasicCalibrationResult] PRIMARY KEY CLUSTERED 
(
	[SequenceID] ASC,
	[CalibrationSubTypeId] ASC,
	[WorkOrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CalibrationSubType]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CalibrationSubType](
	[CalibrationSubTypeId] [int] NOT NULL,
	[CalibrationTypeId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_CalibrationSubType] PRIMARY KEY CLUSTERED 
(
	[CalibrationSubTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CalibrationSubType_Weight]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CalibrationSubType_Weight](
	[WorkOrderDetailID] [int] NOT NULL,
	[WeightSetID] [int] NOT NULL,
	[CalibrationSubTypeID] [int] NOT NULL,
	[SecuenceID] [int] NOT NULL,
	[LinearityCalibrationSubTypeId] [int] NULL,
	[LinearitySequenceID] [int] NULL,
	[LinearityWorkOrderDetailId] [int] NULL,
	[RepeatabilityCalibrationSubTypeId] [int] NULL,
	[RepeatabilityWorkOrderDetailId] [int] NULL,
 CONSTRAINT [PK_CalibrationSubType_Weight] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailID] ASC,
	[WeightSetID] ASC,
	[CalibrationSubTypeID] ASC,
	[SecuenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Component]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Component](
	[ComponentID] [int] IDENTITY(1,1) NOT NULL,
	[Route] [varchar](100) NULL,
	[Name] [varchar](100) NULL,
	[Group] [varchar](200) NULL,
	[Permission] [int] NOT NULL,
	[Disabled] [bit] NOT NULL,
 CONSTRAINT [PK_Component] PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NULL,
	[IsEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Occupation] [nvarchar](max) NULL,
	[PasswordReset] [bit] NOT NULL,
	[AggregateID] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CustomerAggregateAggregateID] [int] NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[IsDelete] [bit] NOT NULL,
	[CellPhoneNumber] [nvarchar](max) NULL,
	[Note] [nvarchar](500) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[TenantId] [int] NOT NULL,
	[Description] [varchar](500) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerAggregates]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAggregates](
	[AggregateID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[CustomerID] [int] NULL,
 CONSTRAINT [PK_CustomerAggregates] PRIMARY KEY CLUSTERED 
(
	[AggregateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceCodes]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceCodes](
	[UserCode] [nvarchar](200) NOT NULL,
	[DeviceCode] [nvarchar](200) NOT NULL,
	[SubjectId] [nvarchar](200) NULL,
	[SessionId] [nvarchar](100) NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Expiration] [datetime2](7) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DeviceCodes] PRIMARY KEY CLUSTERED 
(
	[UserCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Eccentricity]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Eccentricity](
	[CalibrationSubTypeId] [int] NOT NULL,
	[WorkOrderDetailId] [int] NOT NULL,
	[TenantId] [int] NOT NULL,
	[NumberOfSamples] [int] NOT NULL,
	[TestPointID] [int] NULL,
	[EccentricityMax] [float] NOT NULL,
	[EccentricityMin] [float] NOT NULL,
	[EccentricityVarianceAsLeft] [float] NOT NULL,
	[EccentricityVarianceAsFound] [float] NOT NULL,
	[EccentricityUncertaintyValueUOMID] [int] NOT NULL,
	[EccentricityUncertaintyType] [nvarchar](max) NULL,
	[EccentricityUncertaintyDistribution] [nvarchar](max) NULL,
	[EccentricityUncertaintyDivisor] [float] NOT NULL,
	[EccentricityQuotient] [float] NOT NULL,
 CONSTRAINT [PK_Eccentricity] PRIMARY KEY CLUSTERED 
(
	[CalibrationSubTypeId] ASC,
	[WorkOrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailAddress]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailAddress](
	[EmailAddressID] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[TypeID] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[AggregateID] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CustomerAggregateAggregateID] [int] NULL,
 CONSTRAINT [PK_EmailAddress] PRIMARY KEY CLUSTERED 
(
	[EmailAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentCondition]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentCondition](
	[EquipmentConditionId] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderDetailId] [int] NOT NULL,
	[IsAsFound] [bit] NOT NULL,
	[Value] [bit] NOT NULL,
	[Label] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_EquipmentCondition] PRIMARY KEY CLUSTERED 
(
	[EquipmentConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentTemplate]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentTemplate](
	[EquipmentTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](100) NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[EquipmentTypeID] [int] NOT NULL,
	[ManufacturerID] [int] NOT NULL,
	[ToleranceTypeID] [int] NULL,
	[AccuracyPercentage] [float] NOT NULL,
	[Resolution] [float] NOT NULL,
	[DecimalNumber] [int] NOT NULL,
	[UnitofmeasurementID] [int] NULL,
	[StatusID] [nvarchar](max) NOT NULL,
	[Model] [nvarchar](max) NOT NULL,
	[Capacity] [float] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Manufacturer] [nvarchar](max) NULL,
	[EquipmentType] [nvarchar](max) NULL,
	[IsComercial] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[ClassHB44] [int] NOT NULL,
	[IsGeneric] [bit] NOT NULL,
	[OldID] [int] NULL,
 CONSTRAINT [PK_EquipmentTemplate] PRIMARY KEY CLUSTERED 
(
	[EquipmentTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Linearity]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Linearity](
	[SequenceID] [int] NOT NULL,
	[CalibrationSubTypeId] [int] NOT NULL,
	[WorkOrderDetailId] [int] NOT NULL,
	[Tolerance] [float] NOT NULL,
	[NumberOfSamples] [int] NOT NULL,
	[TestPointID] [int] NOT NULL,
	[TotalNominal] [float] NOT NULL,
	[TotalActual] [float] NOT NULL,
	[SumUncertainty] [float] NOT NULL,
	[Quotient] [float] NOT NULL,
	[Square] [float] NOT NULL,
	[SumOfSquares] [float] NOT NULL,
	[TotalUncertainty] [float] NOT NULL,
	[ExpandedUncertainty] [float] NOT NULL,
	[CalibrationUncertaintyType] [nvarchar](max) NULL,
	[CalibrationUncertaintyDistribution] [nvarchar](max) NULL,
	[CalibrationUncertaintyDivisor] [float] NOT NULL,
	[UnitOfMeasureId] [int] NULL,
	[CalibrationUncertaintyValueUnitOfMeasureId] [int] NULL,
	[MinTolerance] [float] NOT NULL,
	[MaxTolerance] [float] NOT NULL,
	[MaxToleranceAsLeft] [float] NOT NULL,
	[MinToleranceAsLeft] [float] NOT NULL,
 CONSTRAINT [PK_Linearity] PRIMARY KEY CLUSTERED 
(
	[SequenceID] ASC,
	[CalibrationSubTypeId] ASC,
	[WorkOrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturer]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturer](
	[ManufacturerID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[Abbreviation] [nvarchar](500) NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED 
(
	[ManufacturerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersistedGrants]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersistedGrants](
	[Key] [nvarchar](200) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[SubjectId] [nvarchar](200) NULL,
	[SessionId] [nvarchar](100) NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Expiration] [datetime2](7) NULL,
	[ConsumedTime] [datetime2](7) NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PersistedGrants] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneNumber]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneNumber](
	[PhoneNumberID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](50) NOT NULL,
	[TypeID] [varchar](50) NOT NULL,
	[CountryID] [varchar](50) NULL,
	[IsEnabled] [bit] NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[CustomerAggregateAggregateID] [int] NULL,
 CONSTRAINT [PK_PhoneNumber] PRIMARY KEY CLUSTERED 
(
	[PhoneNumberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PieceOfEquipment]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PieceOfEquipment](
	[PieceOfEquipmentID] [nvarchar](450) NOT NULL,
	[SerialNumber] [varchar](500) NOT NULL,
	[Capacity] [float] NOT NULL,
	[AddressId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[InstallLocation] [varchar](500) NULL,
	[TenantId] [int] NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Class] [varchar](5) NULL,
	[IsStandard] [bit] NOT NULL,
	[IsWeigthSet] [bit] NOT NULL,
	[IsForAccreditedCal] [bit] NOT NULL,
	[CalibrationDate] [datetime2](7) NOT NULL,
	[EquipmentTemplateId] [int] NOT NULL,
	[WorOrderDetailID] [int] NULL,
	[WorkOrderID] [int] NULL,
	[IndicatorPieceOfEquipmentID] [nvarchar](450) NULL,
	[UnitOfMeasureID] [int] NULL,
	[EquipmentTypeID] [int] NULL,
	[AccuracyPercentage] [float] NOT NULL,
	[DecimalNumber] [int] NOT NULL,
	[Resolution] [float] NOT NULL,
	[ToleranceTypeID] [int] NOT NULL,
	[IsToleranceImport] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[ClassHB44] [int] NOT NULL,
	[IsTestPointImport] [bit] NOT NULL,
	[OfflineID] [varchar](100) NULL,
	[Notes] [nvarchar](max) NULL,
	[CustomerToolId] [varchar](200) NULL,
 CONSTRAINT [PK_PieceOfEquipment] PRIMARY KEY CLUSTERED 
(
	[PieceOfEquipmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POE_POE]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POE_POE](
	[PieceOfEquipmentID] [nvarchar](450) NOT NULL,
	[PieceOfEquipmentID2] [nvarchar](450) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POE_User]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POE_User](
	[UserID] [int] NOT NULL,
	[PieceOfEquipmentID] [nvarchar](450) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POE_WorkOrder]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POE_WorkOrder](
	[PieceOfEquipmentID] [nvarchar](450) NOT NULL,
	[WorkOrderID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RangeTolerance]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RangeTolerance](
	[RangeToleranceID] [int] IDENTITY(1,1) NOT NULL,
	[MinValue] [float] NOT NULL,
	[MaxValue] [float] NOT NULL,
	[Percent] [float] NOT NULL,
	[Resolution] [float] NOT NULL,
	[EquipmentTemplateID] [int] NULL,
	[ToleranceTypeID] [int] NOT NULL,
	[PieceOfEquipmentID] [nvarchar](450) NULL,
	[WorkOrderDetailID] [int] NULL,
 CONSTRAINT [PK_RangeTolerance] PRIMARY KEY CLUSTERED 
(
	[RangeToleranceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Repeatability]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Repeatability](
	[CalibrationSubTypeId] [int] NOT NULL,
	[WorkOrderDetailId] [int] NOT NULL,
	[RepeatabilityId] [int] NOT NULL,
	[TenantId] [int] NOT NULL,
	[NumberOfSamples] [int] NOT NULL,
	[RepeatabilityStdDeviationAsLeft] [float] NOT NULL,
	[RepeatabilityStdDeviationAsFound] [float] NOT NULL,
	[RepeatabilityUncertaintyValueUOMID] [int] NOT NULL,
	[RepeatabilityUncertaintyType] [nvarchar](max) NULL,
	[RepeatabilityUncertaintyDistribution] [nvarchar](max) NULL,
	[RepeatabilityUncertaintyDivisor] [float] NOT NULL,
	[RepeatabilityQuotient] [float] NOT NULL,
	[TestPointID] [int] NULL,
 CONSTRAINT [PK_Repeatability] PRIMARY KEY CLUSTERED 
(
	[CalibrationSubTypeId] ASC,
	[WorkOrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Social]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Social](
	[SocialID] [int] IDENTITY(1,1) NOT NULL,
	[Link] [nvarchar](max) NOT NULL,
	[SocialTypeID] [nvarchar](max) NOT NULL,
	[SocialType] [nvarchar](max) NULL,
	[IsEnabled] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[CustomerAggregateAggregateID] [int] NULL,
 CONSTRAINT [PK_Social] PRIMARY KEY CLUSTERED 
(
	[SocialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsDefault] [bit] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[Possibilities] [nvarchar](max) NOT NULL,
	[WorkOrderDetailID] [int] NULL,
	[IsLast] [bit] NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TechnicianCode]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TechnicianCode](
	[Code] [nvarchar](100) NOT NULL,
	[StateID] [nvarchar](100) NOT NULL,
	[CertificationID] [int] NOT NULL,
	[TechnicianCodeID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_TechnicianCode] PRIMARY KEY CLUSTERED 
(
	[Code] ASC,
	[StateID] ASC,
	[CertificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenant](
	[TenantId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED 
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestPoint]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestPoint](
	[TestPointID] [int] IDENTITY(1,1) NOT NULL,
	[NominalTestPoit] [float] NOT NULL,
	[LowerTolerance] [float] NOT NULL,
	[UpperTolerance] [float] NOT NULL,
	[Resolution] [float] NOT NULL,
	[DecimalNumber] [int] NOT NULL,
	[TestPointGroupTestPoitGroupID] [int] NULL,
	[UnitOfMeasurementID] [int] NOT NULL,
	[UnitOfMeasureOutID] [int] NOT NULL,
	[CalibrationType] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[TestPointTarget] [int] NOT NULL,
	[IsDescendant] [bit] NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_TestPoint] PRIMARY KEY CLUSTERED 
(
	[TestPointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestPointGroup]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestPointGroup](
	[TestPoitGroupID] [int] IDENTITY(1,1) NOT NULL,
	[OutUnitOfMeasurementID] [int] NOT NULL,
	[TypeID] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[UnitOfMeasurementOutUnitOfMeasureID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[EquipmentTemplateID] [int] NULL,
	[WorkOrderDetailID] [int] NULL,
	[PieceOfEquipmentID] [nvarchar](450) NULL,
 CONSTRAINT [PK_TestPointGroup] PRIMARY KEY CLUSTERED 
(
	[TestPoitGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UnitOfMeasure]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnitOfMeasure](
	[UnitOfMeasureID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Abbreviation] [varchar](10) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[UnitOfMeasureBaseID] [int] NULL,
	[TypeID] [int] NOT NULL,
	[ConversionValue] [float] NOT NULL,
	[UncertaintyUnitOfMeasureID] [int] NULL,
	[Description] [varchar](200) NULL,
 CONSTRAINT [PK_UnitOfMeasure] PRIMARY KEY CLUSTERED 
(
	[UnitOfMeasureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UnitOfMeasureType]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnitOfMeasureType](
	[Value] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_UnitOfMeasureType] PRIMARY KEY CLUSTERED 
(
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[UserTypeID] [nvarchar](50) NULL,
	[PasswordReset] [bit] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Email] [nvarchar](450) NOT NULL,
	[Roles] [nvarchar](max) NULL,
	[Occupation] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[PieceOfEquipmentID] [nvarchar](450) NULL,
	[WorkOrderId] [int] NULL,
	[IdentityID] [nvarchar](max) NULL,
	[PassWord] [nvarchar](max) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Rol]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Rol](
	[UserID] [int] NOT NULL,
	[RolID] [int] NOT NULL,
	[Permissions] [varchar](200) NULL,
 CONSTRAINT [PK_User_Rol] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[RolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_WorkOrder]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_WorkOrder](
	[UserID] [int] NOT NULL,
	[WorkOrderID] [int] NOT NULL,
 CONSTRAINT [PK_User_WorkOrder] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[WorkOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInformation]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInformation](
	[UserInformationID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](40) NOT NULL,
	[LastName] [nvarchar](40) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[ConfirmPassword] [nvarchar](max) NOT NULL,
	[AcceptTerms] [bit] NOT NULL,
 CONSTRAINT [PK_UserInformation] PRIMARY KEY CLUSTERED 
(
	[UserInformationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeightSet]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeightSet](
	[WeightSetID] [int] IDENTITY(1,1) NOT NULL,
	[WeightValue] [int] NOT NULL,
	[UnitOfMeasureID] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[PieceOfEquipmentStatus] [nvarchar](max) NULL,
	[PieceOfEquipmentId_new] [int] NULL,
	[WeightNominalValue] [float] NOT NULL,
	[WeightActualValue] [float] NOT NULL,
	[CalibrationUncertValue] [float] NOT NULL,
	[UncertaintyUnitOfMeasureId] [int] NULL,
	[Divisor] [float] NOT NULL,
	[Type] [nvarchar](max) NULL,
	[Distribution] [nvarchar](max) NULL,
	[PieceOfEquipmentID] [nvarchar](450) NULL,
	[EccentricityCalibrationSubTypeId] [int] NULL,
	[EccentricityWorkOrderDetailId] [int] NULL,
	[LinearityCalibrationSubTypeId] [int] NULL,
	[LinearitySequenceID] [int] NULL,
	[LinearityWorkOrderDetailId] [int] NULL,
	[RepeatabilityCalibrationSubTypeId] [int] NULL,
	[RepeatabilityWorkOrderDetailId] [int] NULL,
	[Reference] [nvarchar](max) NULL,
	[IsDelete] [bit] NOT NULL,
	[Serial] [varchar](50) NULL,
 CONSTRAINT [PK_WeightSet] PRIMARY KEY CLUSTERED 
(
	[WeightSetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WOD_TestPoint]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WOD_TestPoint](
	[WorkOrderDetailID] [int] NOT NULL,
	[TestPointID] [int] NOT NULL,
	[SecuenceID] [int] NOT NULL,
	[CalibrationSubTypeID] [int] NOT NULL,
 CONSTRAINT [PK_WOD_TestPoint] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailID] ASC,
	[TestPointID] ASC,
	[CalibrationSubTypeID] ASC,
	[SecuenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WOD_Weight]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WOD_Weight](
	[WorkOrderDetailID] [int] NOT NULL,
	[WeightSetID] [int] NOT NULL,
 CONSTRAINT [PK_WOD_Weight] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailID] ASC,
	[WeightSetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkDetailHistory]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkDetailHistory](
	[WorkDetailHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[StatusId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[TechnicianID] [int] NULL,
	[TechnicianName] [nvarchar](max) NULL,
	[Date] [datetime2](7) NOT NULL,
	[Action] [nvarchar](max) NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Version] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Data] [nvarchar](max) NULL,
	[WorkOrderDetailID] [int] NOT NULL,
 CONSTRAINT [PK_WorkDetailHistory] PRIMARY KEY CLUSTERED 
(
	[WorkDetailHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrder]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrder](
	[WorkOrderId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[WorkOrderDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ControlNumber] [nvarchar](max) NULL,
	[TenantId] [int] NOT NULL,
	[CalibrationType] [int] NOT NULL,
	[AddressId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED 
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderDetail]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderDetail](
	[WorkOrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[WorkOderID] [int] NOT NULL,
	[TenantId] [int] NOT NULL,
	[PieceOfEquipmentId] [nvarchar](450) NOT NULL,
	[IsAccredited] [bit] NOT NULL,
	[IsService] [bit] NOT NULL,
	[IsRepair] [bit] NOT NULL,
	[SelectedNewStatus] [smallint] NOT NULL,
	[CertificateComment] [varchar](500) NULL,
	[Humidity] [float] NOT NULL,
	[Temperature] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[TemperatureUOMID] [int] NOT NULL,
	[StandarID] [int] NOT NULL,
	[CalibrationIntervalID] [int] NOT NULL,
	[CalibrationIntervalName] [int] NOT NULL,
	[CalibrationDate] [datetime2](7) NULL,
	[CalibrationCustomDueDate] [datetime2](7) NULL,
	[CalibrationNextDueDate] [datetime2](7) NULL,
	[TechnicianComment] [varchar](500) NULL,
	[TestPointNumber] [int] NOT NULL,
	[HumidityUOMID] [int] NOT NULL,
	[TechnicianID] [int] NULL,
	[CurrentStatusID] [int] NOT NULL,
	[ToleranceTypeID] [int] NOT NULL,
	[AccuracyPercentage] [float] NOT NULL,
	[DecimalNumber] [int] NOT NULL,
	[Resolution] [float] NOT NULL,
	[Environment] [nvarchar](max) NULL,
	[WorkOrderDetailHash] [nvarchar](max) NULL,
	[AddressID] [int] NULL,
	[CalibrationTypeID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[IsComercial] [bit] NOT NULL,
	[Multiplier] [int] NOT NULL,
	[ClassHB44] [int] NOT NULL,
	[OfflineID] [varchar](100) NULL,
	[OfflineStatus] [int] NOT NULL,
	[StatusDate] [datetime2](7) NULL,
	[HasBeenCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_WorkOrderDetail] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderDetailByEquipment]    Script Date: 11/10/2021 2:38:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderDetailByEquipment](
	[WorkOrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[Company] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
	[SerialNumber] [nvarchar](max) NULL,
	[EquipmentType] [nvarchar](max) NULL,
	[WorkOrderId] [int] NOT NULL,
	[WorkOrderReceiveDate] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[EquipmentTypeID] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[StatusDate] [datetime2](7) NULL,
 CONSTRAINT [PK_WorkOrderDetailByEquipment] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF__Address__Delete__662B2B3B]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Component] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Disabled]
GO
ALTER TABLE [dbo].[Contact] ADD  CONSTRAINT [DF__Contact__Delete__671F4F74]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[EquipmentTemplate] ADD  CONSTRAINT [DF__Equipment__IsCom__7F2BE32F]  DEFAULT (CONVERT([bit],(0))) FOR [IsComercial]
GO
ALTER TABLE [dbo].[EquipmentTemplate] ADD  CONSTRAINT [DF__Equipment__Delet__6442E2C9]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[EquipmentTemplate] ADD  DEFAULT ((0)) FOR [ClassHB44]
GO
ALTER TABLE [dbo].[EquipmentTemplate] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsGeneric]
GO
ALTER TABLE [dbo].[Linearity] ADD  DEFAULT ((0.000000000000000e+000)) FOR [MaxToleranceAsLeft]
GO
ALTER TABLE [dbo].[Linearity] ADD  DEFAULT ((0.000000000000000e+000)) FOR [MinToleranceAsLeft]
GO
ALTER TABLE [dbo].[Manufacturer] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDelete]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__Accur__51300E55]  DEFAULT ((0.000000000000000e+000)) FOR [AccuracyPercentage]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__Decim__5224328E]  DEFAULT ((0)) FOR [DecimalNumber]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__Resol__531856C7]  DEFAULT ((0.000000000000000e+000)) FOR [Resolution]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__Toler__540C7B00]  DEFAULT ((0)) FOR [ToleranceTypeID]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__IsTol__56E8E7AB]  DEFAULT (CONVERT([bit],(0))) FOR [IsToleranceImport]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__Delet__690797E6]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__Class__0C85DE4D]  DEFAULT ((0)) FOR [ClassHB44]
GO
ALTER TABLE [dbo].[PieceOfEquipment] ADD  CONSTRAINT [DF__PieceOfEq__IsTes__0D7A0286]  DEFAULT (CONVERT([bit],(0))) FOR [IsTestPointImport]
GO
ALTER TABLE [dbo].[Status] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsLast]
GO
ALTER TABLE [dbo].[TechnicianCode] ADD  CONSTRAINT [DF__Technicia__Delet__634EBE90]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[TestPoint] ADD  DEFAULT ((0)) FOR [TestPointTarget]
GO
ALTER TABLE [dbo].[TestPoint] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDescendant]
GO
ALTER TABLE [dbo].[TestPoint] ADD  DEFAULT ((0)) FOR [Position]
GO
ALTER TABLE [dbo].[WeightSet] ADD  CONSTRAINT [DF__WeightSet__Delet__681373AD]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[WorkOrderDetail] ADD  CONSTRAINT [DF__WorkOrder__IsCom__7F2BE32F]  DEFAULT (CONVERT([bit],(0))) FOR [IsComercial]
GO
ALTER TABLE [dbo].[WorkOrderDetail] ADD  CONSTRAINT [DF__WorkOrder__Multi__00200768]  DEFAULT ((0)) FOR [Multiplier]
GO
ALTER TABLE [dbo].[WorkOrderDetail] ADD  CONSTRAINT [DF__WorkOrder__Class__01142BA1]  DEFAULT ((0)) FOR [ClassHB44]
GO
ALTER TABLE [dbo].[WorkOrderDetail] ADD  CONSTRAINT [DF__WorkOrder__Offli__681373AD]  DEFAULT ((0)) FOR [OfflineStatus]
GO
ALTER TABLE [dbo].[WorkOrderDetail] ADD  CONSTRAINT [DF__WorkOrder__HasBe__59904A2C]  DEFAULT (CONVERT([bit],(0))) FOR [HasBeenCompleted]
GO
ALTER TABLE [dbo].[Address]  WITH NOCHECK ADD  CONSTRAINT [FK_Address_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_CustomerAggregates_CustomerAggregateAggregateID]
GO
ALTER TABLE [dbo].[BalanceAndScaleCalibration]  WITH CHECK ADD  CONSTRAINT [FK_BalanceAndScaleCalibration_WorkOrderDetail_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
GO
ALTER TABLE [dbo].[BalanceAndScaleCalibration] CHECK CONSTRAINT [FK_BalanceAndScaleCalibration_WorkOrderDetail_WorkOrderDetailId]
GO
ALTER TABLE [dbo].[BasicCalibrationResult]  WITH CHECK ADD  CONSTRAINT [FK_BasicCalibrationResult_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY([UnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[BasicCalibrationResult] CHECK CONSTRAINT [FK_BasicCalibrationResult_UnitOfMeasure_UnitOfMeasureID]
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight]  WITH CHECK ADD  CONSTRAINT [FK_CalibrationSubType_Weight_CalibrationSubType_CalibrationSubTypeID] FOREIGN KEY([CalibrationSubTypeID])
REFERENCES [dbo].[CalibrationSubType] ([CalibrationSubTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight] CHECK CONSTRAINT [FK_CalibrationSubType_Weight_CalibrationSubType_CalibrationSubTypeID]
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight]  WITH CHECK ADD  CONSTRAINT [FK_CalibrationSubType_Weight_Linearity_LinearitySequenceID_LinearityCalibrationSubTypeId_LinearityWorkOrderDetailId] FOREIGN KEY([LinearitySequenceID], [LinearityCalibrationSubTypeId], [LinearityWorkOrderDetailId])
REFERENCES [dbo].[Linearity] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight] CHECK CONSTRAINT [FK_CalibrationSubType_Weight_Linearity_LinearitySequenceID_LinearityCalibrationSubTypeId_LinearityWorkOrderDetailId]
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight]  WITH CHECK ADD  CONSTRAINT [FK_CalibrationSubType_Weight_Repeatability_RepeatabilityCalibrationSubTypeId_RepeatabilityWorkOrderDetailId] FOREIGN KEY([RepeatabilityCalibrationSubTypeId], [RepeatabilityWorkOrderDetailId])
REFERENCES [dbo].[Repeatability] ([CalibrationSubTypeId], [WorkOrderDetailId])
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight] CHECK CONSTRAINT [FK_CalibrationSubType_Weight_Repeatability_RepeatabilityCalibrationSubTypeId_RepeatabilityWorkOrderDetailId]
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight]  WITH CHECK ADD  CONSTRAINT [FK_CalibrationSubType_Weight_WeightSet_WeightSetID] FOREIGN KEY([WeightSetID])
REFERENCES [dbo].[WeightSet] ([WeightSetID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CalibrationSubType_Weight] CHECK CONSTRAINT [FK_CalibrationSubType_Weight_WeightSet_WeightSetID]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_CustomerAggregates_CustomerAggregateAggregateID]
GO
ALTER TABLE [dbo].[CustomerAggregates]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAggregates_Customer_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[CustomerAggregates] CHECK CONSTRAINT [FK_CustomerAggregates_Customer_CustomerID]
GO
ALTER TABLE [dbo].[Eccentricity]  WITH CHECK ADD  CONSTRAINT [FK_Eccentricity_BalanceAndScaleCalibration_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[BalanceAndScaleCalibration] ([WorkOrderDetailId])
GO
ALTER TABLE [dbo].[Eccentricity] CHECK CONSTRAINT [FK_Eccentricity_BalanceAndScaleCalibration_WorkOrderDetailId]
GO
ALTER TABLE [dbo].[Eccentricity]  WITH CHECK ADD  CONSTRAINT [FK_Eccentricity_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID])
GO
ALTER TABLE [dbo].[Eccentricity] CHECK CONSTRAINT [FK_Eccentricity_TestPoint_TestPointID]
GO
ALTER TABLE [dbo].[EmailAddress]  WITH CHECK ADD  CONSTRAINT [FK_EmailAddress_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID])
GO
ALTER TABLE [dbo].[EmailAddress] CHECK CONSTRAINT [FK_EmailAddress_CustomerAggregates_CustomerAggregateAggregateID]
GO
ALTER TABLE [dbo].[EquipmentCondition]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentCondition_WorkOrderDetail_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EquipmentCondition] CHECK CONSTRAINT [FK_EquipmentCondition_WorkOrderDetail_WorkOrderDetailId]
GO
ALTER TABLE [dbo].[Linearity]  WITH CHECK ADD  CONSTRAINT [FK_Linearity_BalanceAndScaleCalibration_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[BalanceAndScaleCalibration] ([WorkOrderDetailId])
GO
ALTER TABLE [dbo].[Linearity] CHECK CONSTRAINT [FK_Linearity_BalanceAndScaleCalibration_WorkOrderDetailId]
GO
ALTER TABLE [dbo].[Linearity]  WITH CHECK ADD  CONSTRAINT [FK_Linearity_BasicCalibrationResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId] FOREIGN KEY([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
REFERENCES [dbo].[BasicCalibrationResult] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
GO
ALTER TABLE [dbo].[Linearity] CHECK CONSTRAINT [FK_Linearity_BasicCalibrationResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId]
GO
ALTER TABLE [dbo].[Linearity]  WITH CHECK ADD  CONSTRAINT [FK_Linearity_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID])
GO
ALTER TABLE [dbo].[Linearity] CHECK CONSTRAINT [FK_Linearity_TestPoint_TestPointID]
GO
ALTER TABLE [dbo].[Linearity]  WITH CHECK ADD  CONSTRAINT [FK_Linearity_UnitOfMeasure_CalibrationUncertaintyValueUnitOfMeasureId] FOREIGN KEY([CalibrationUncertaintyValueUnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[Linearity] CHECK CONSTRAINT [FK_Linearity_UnitOfMeasure_CalibrationUncertaintyValueUnitOfMeasureId]
GO
ALTER TABLE [dbo].[Linearity]  WITH CHECK ADD  CONSTRAINT [FK_Linearity_UnitOfMeasure_UnitOfMeasureId] FOREIGN KEY([UnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[Linearity] CHECK CONSTRAINT [FK_Linearity_UnitOfMeasure_UnitOfMeasureId]
GO
ALTER TABLE [dbo].[PhoneNumber]  WITH CHECK ADD  CONSTRAINT [FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID])
GO
ALTER TABLE [dbo].[PhoneNumber] CHECK CONSTRAINT [FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID]
GO
ALTER TABLE [dbo].[PieceOfEquipment]  WITH CHECK ADD  CONSTRAINT [FK_PieceOfEquipment_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PieceOfEquipment] CHECK CONSTRAINT [FK_PieceOfEquipment_Customer_CustomerId]
GO
ALTER TABLE [dbo].[PieceOfEquipment]  WITH CHECK ADD  CONSTRAINT [FK_PieceOfEquipment_EquipmentTemplate_EquipmentTemplateId] FOREIGN KEY([EquipmentTemplateId])
REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PieceOfEquipment] CHECK CONSTRAINT [FK_PieceOfEquipment_EquipmentTemplate_EquipmentTemplateId]
GO
ALTER TABLE [dbo].[PieceOfEquipment]  WITH CHECK ADD  CONSTRAINT [FK_PieceOfEquipment_EquipmentType_EquipmentTypeID] FOREIGN KEY([EquipmentTypeID])
REFERENCES [dbo].[EquipmentType] ([EquipmentTypeID])
GO
ALTER TABLE [dbo].[PieceOfEquipment] CHECK CONSTRAINT [FK_PieceOfEquipment_EquipmentType_EquipmentTypeID]
GO
ALTER TABLE [dbo].[PieceOfEquipment]  WITH CHECK ADD  CONSTRAINT [FK_PieceOfEquipment_PieceOfEquipment_IndicatorPieceOfEquipmentID] FOREIGN KEY([IndicatorPieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[PieceOfEquipment] CHECK CONSTRAINT [FK_PieceOfEquipment_PieceOfEquipment_IndicatorPieceOfEquipmentID]
GO
ALTER TABLE [dbo].[PieceOfEquipment]  WITH CHECK ADD  CONSTRAINT [FK_PieceOfEquipment_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY([UnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[PieceOfEquipment] CHECK CONSTRAINT [FK_PieceOfEquipment_UnitOfMeasure_UnitOfMeasureID]
GO
ALTER TABLE [dbo].[POE_POE]  WITH CHECK ADD  CONSTRAINT [FK_POE_POE_PieceOfEquipment_PieceOfEquipmentID2] FOREIGN KEY([PieceOfEquipmentID2])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[POE_POE] CHECK CONSTRAINT [FK_POE_POE_PieceOfEquipment_PieceOfEquipmentID2]
GO
ALTER TABLE [dbo].[POE_User]  WITH CHECK ADD  CONSTRAINT [FK_POE_User_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[POE_User] CHECK CONSTRAINT [FK_POE_User_PieceOfEquipment_PieceOfEquipmentID]
GO
ALTER TABLE [dbo].[POE_User]  WITH CHECK ADD  CONSTRAINT [FK_POE_User_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[POE_User] CHECK CONSTRAINT [FK_POE_User_User_UserID]
GO
ALTER TABLE [dbo].[POE_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_POE_WorkOrder_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[POE_WorkOrder] CHECK CONSTRAINT [FK_POE_WorkOrder_PieceOfEquipment_PieceOfEquipmentID]
GO
ALTER TABLE [dbo].[POE_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_POE_WorkOrder_WorkOrder_WorkOrderID] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrder] ([WorkOrderId])
GO
ALTER TABLE [dbo].[POE_WorkOrder] CHECK CONSTRAINT [FK_POE_WorkOrder_WorkOrder_WorkOrderID]
GO
ALTER TABLE [dbo].[RangeTolerance]  WITH CHECK ADD  CONSTRAINT [FK_RangeTolerance_EquipmentTemplate_EquipmentTemplateID] FOREIGN KEY([EquipmentTemplateID])
REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID])
GO
ALTER TABLE [dbo].[RangeTolerance] CHECK CONSTRAINT [FK_RangeTolerance_EquipmentTemplate_EquipmentTemplateID]
GO
ALTER TABLE [dbo].[RangeTolerance]  WITH CHECK ADD  CONSTRAINT [FK_RangeTolerance_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[RangeTolerance] CHECK CONSTRAINT [FK_RangeTolerance_PieceOfEquipment_PieceOfEquipmentID]
GO
ALTER TABLE [dbo].[RangeTolerance]  WITH CHECK ADD  CONSTRAINT [FK_RangeTolerance_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
GO
ALTER TABLE [dbo].[RangeTolerance] CHECK CONSTRAINT [FK_RangeTolerance_WorkOrderDetail_WorkOrderDetailID]
GO
ALTER TABLE [dbo].[Repeatability]  WITH CHECK ADD  CONSTRAINT [FK_Repeatability_BalanceAndScaleCalibration_WorkOrderDetailId] FOREIGN KEY([WorkOrderDetailId])
REFERENCES [dbo].[BalanceAndScaleCalibration] ([WorkOrderDetailId])
GO
ALTER TABLE [dbo].[Repeatability] CHECK CONSTRAINT [FK_Repeatability_BalanceAndScaleCalibration_WorkOrderDetailId]
GO
ALTER TABLE [dbo].[Repeatability]  WITH CHECK ADD  CONSTRAINT [FK_Repeatability_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID])
GO
ALTER TABLE [dbo].[Repeatability] CHECK CONSTRAINT [FK_Repeatability_TestPoint_TestPointID]
GO
ALTER TABLE [dbo].[Social]  WITH CHECK ADD  CONSTRAINT [FK_Social_CustomerAggregates_CustomerAggregateAggregateID] FOREIGN KEY([CustomerAggregateAggregateID])
REFERENCES [dbo].[CustomerAggregates] ([AggregateID])
GO
ALTER TABLE [dbo].[Social] CHECK CONSTRAINT [FK_Social_CustomerAggregates_CustomerAggregateAggregateID]
GO
ALTER TABLE [dbo].[Status]  WITH CHECK ADD  CONSTRAINT [FK_Status_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
GO
ALTER TABLE [dbo].[Status] CHECK CONSTRAINT [FK_Status_WorkOrderDetail_WorkOrderDetailID]
GO
ALTER TABLE [dbo].[TechnicianCode]  WITH CHECK ADD  CONSTRAINT [FK_TechnicianCode_Certification_CertificationID] FOREIGN KEY([CertificationID])
REFERENCES [dbo].[Certification] ([CertificationID])
GO
ALTER TABLE [dbo].[TechnicianCode] CHECK CONSTRAINT [FK_TechnicianCode_Certification_CertificationID]
GO
ALTER TABLE [dbo].[TechnicianCode]  WITH CHECK ADD  CONSTRAINT [FK_TechnicianCode_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[TechnicianCode] CHECK CONSTRAINT [FK_TechnicianCode_User_UserID]
GO
ALTER TABLE [dbo].[TestPoint]  WITH CHECK ADD  CONSTRAINT [FK_TestPoint_TestPointGroup_TestPointGroupTestPoitGroupID] FOREIGN KEY([TestPointGroupTestPoitGroupID])
REFERENCES [dbo].[TestPointGroup] ([TestPoitGroupID])
GO
ALTER TABLE [dbo].[TestPoint] CHECK CONSTRAINT [FK_TestPoint_TestPointGroup_TestPointGroupTestPoitGroupID]
GO
ALTER TABLE [dbo].[TestPoint]  WITH CHECK ADD  CONSTRAINT [FK_TestPointsin_UnitOfMeasureIn_UnitOfMeasureInID] FOREIGN KEY([UnitOfMeasurementID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[TestPoint] CHECK CONSTRAINT [FK_TestPointsin_UnitOfMeasureIn_UnitOfMeasureInID]
GO
ALTER TABLE [dbo].[TestPoint]  WITH CHECK ADD  CONSTRAINT [FK_TestPointsOut_UnitOfMeasureOut_UnitOfMeasureOutID] FOREIGN KEY([UnitOfMeasureOutID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[TestPoint] CHECK CONSTRAINT [FK_TestPointsOut_UnitOfMeasureOut_UnitOfMeasureOutID]
GO
ALTER TABLE [dbo].[TestPointGroup]  WITH CHECK ADD  CONSTRAINT [FK_TestPointGroup_EquipmentTemplate_EquipmentTemplateID] FOREIGN KEY([EquipmentTemplateID])
REFERENCES [dbo].[EquipmentTemplate] ([EquipmentTemplateID])
GO
ALTER TABLE [dbo].[TestPointGroup] CHECK CONSTRAINT [FK_TestPointGroup_EquipmentTemplate_EquipmentTemplateID]
GO
ALTER TABLE [dbo].[TestPointGroup]  WITH CHECK ADD  CONSTRAINT [FK_TestPointGroup_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[TestPointGroup] CHECK CONSTRAINT [FK_TestPointGroup_PieceOfEquipment_PieceOfEquipmentID]
GO
ALTER TABLE [dbo].[TestPointGroup]  WITH CHECK ADD  CONSTRAINT [FK_TestPointGroup_UnitOfMeasure_UnitOfMeasurementOutUnitOfMeasureID] FOREIGN KEY([UnitOfMeasurementOutUnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[TestPointGroup] CHECK CONSTRAINT [FK_TestPointGroup_UnitOfMeasure_UnitOfMeasurementOutUnitOfMeasureID]
GO
ALTER TABLE [dbo].[TestPointGroup]  WITH CHECK ADD  CONSTRAINT [FK_TestPointGroup_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
GO
ALTER TABLE [dbo].[TestPointGroup] CHECK CONSTRAINT [FK_TestPointGroup_WorkOrderDetail_WorkOrderDetailID]
GO
ALTER TABLE [dbo].[UnitOfMeasure]  WITH CHECK ADD  CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID] FOREIGN KEY([UncertaintyUnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[UnitOfMeasure] CHECK CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID]
GO
ALTER TABLE [dbo].[UnitOfMeasure]  WITH CHECK ADD  CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UnitOfMeasureBaseID] FOREIGN KEY([UnitOfMeasureBaseID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[UnitOfMeasure] CHECK CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UnitOfMeasureBaseID]
GO
ALTER TABLE [dbo].[UnitOfMeasure]  WITH CHECK ADD  CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UnitOfMeasureBaseUnitOfMeasureID] FOREIGN KEY([UnitOfMeasureBaseID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[UnitOfMeasure] CHECK CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasure_UnitOfMeasureBaseUnitOfMeasureID]
GO
ALTER TABLE [dbo].[UnitOfMeasure]  WITH CHECK ADD  CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasureType_TypeID] FOREIGN KEY([TypeID])
REFERENCES [dbo].[UnitOfMeasureType] ([Value])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UnitOfMeasure] CHECK CONSTRAINT [FK_UnitOfMeasure_UnitOfMeasureType_TypeID]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_PieceOfEquipment_PieceOfEquipmentID]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_WorkOrder_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrder] ([WorkOrderId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_WorkOrder_WorkOrderId]
GO
ALTER TABLE [dbo].[User_Rol]  WITH CHECK ADD  CONSTRAINT [FK_User_Rol_Rol_RolID] FOREIGN KEY([RolID])
REFERENCES [dbo].[Rol] ([RolID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User_Rol] CHECK CONSTRAINT [FK_User_Rol_Rol_RolID]
GO
ALTER TABLE [dbo].[User_Rol]  WITH CHECK ADD  CONSTRAINT [FK_User_Rol_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User_Rol] CHECK CONSTRAINT [FK_User_Rol_User_UserID]
GO
ALTER TABLE [dbo].[User_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_User_WorkOrder_User_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User_WorkOrder] CHECK CONSTRAINT [FK_User_WorkOrder_User_UserID]
GO
ALTER TABLE [dbo].[User_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_User_WorkOrder_WorkOrder_WorkOrderID] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrder] ([WorkOrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User_WorkOrder] CHECK CONSTRAINT [FK_User_WorkOrder_WorkOrder_WorkOrderID]
GO
ALTER TABLE [dbo].[WeightSet]  WITH CHECK ADD  CONSTRAINT [FK_WeightSet_Eccentricity_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId] FOREIGN KEY([EccentricityCalibrationSubTypeId], [EccentricityWorkOrderDetailId])
REFERENCES [dbo].[Eccentricity] ([CalibrationSubTypeId], [WorkOrderDetailId])
GO
ALTER TABLE [dbo].[WeightSet] CHECK CONSTRAINT [FK_WeightSet_Eccentricity_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId]
GO
ALTER TABLE [dbo].[WeightSet]  WITH CHECK ADD  CONSTRAINT [FK_WeightSet_Linearity_LinearitySequenceID_LinearityCalibrationSubTypeId_LinearityWorkOrderDetailId] FOREIGN KEY([LinearitySequenceID], [LinearityCalibrationSubTypeId], [LinearityWorkOrderDetailId])
REFERENCES [dbo].[Linearity] ([SequenceID], [CalibrationSubTypeId], [WorkOrderDetailId])
GO
ALTER TABLE [dbo].[WeightSet] CHECK CONSTRAINT [FK_WeightSet_Linearity_LinearitySequenceID_LinearityCalibrationSubTypeId_LinearityWorkOrderDetailId]
GO
ALTER TABLE [dbo].[WeightSet]  WITH CHECK ADD  CONSTRAINT [FK_WeightSet_PieceOfEquipment_PieceOfEquipmentID] FOREIGN KEY([PieceOfEquipmentID])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[WeightSet] CHECK CONSTRAINT [FK_WeightSet_PieceOfEquipment_PieceOfEquipmentID]
GO
ALTER TABLE [dbo].[WeightSet]  WITH CHECK ADD  CONSTRAINT [FK_WeightSet_Repeatability_RepeatabilityCalibrationSubTypeId_RepeatabilityWorkOrderDetailId] FOREIGN KEY([RepeatabilityCalibrationSubTypeId], [RepeatabilityWorkOrderDetailId])
REFERENCES [dbo].[Repeatability] ([CalibrationSubTypeId], [WorkOrderDetailId])
GO
ALTER TABLE [dbo].[WeightSet] CHECK CONSTRAINT [FK_WeightSet_Repeatability_RepeatabilityCalibrationSubTypeId_RepeatabilityWorkOrderDetailId]
GO
ALTER TABLE [dbo].[WeightSet]  WITH CHECK ADD  CONSTRAINT [FK_WeightSet_UnitOfMeasure_UncertaintyUnitOfMeasureId] FOREIGN KEY([UncertaintyUnitOfMeasureId])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[WeightSet] CHECK CONSTRAINT [FK_WeightSet_UnitOfMeasure_UncertaintyUnitOfMeasureId]
GO
ALTER TABLE [dbo].[WeightSet]  WITH CHECK ADD  CONSTRAINT [FK_WeightSet_UnitOfMeasure_UnitOfMeasureID] FOREIGN KEY([UnitOfMeasureID])
REFERENCES [dbo].[UnitOfMeasure] ([UnitOfMeasureID])
GO
ALTER TABLE [dbo].[WeightSet] CHECK CONSTRAINT [FK_WeightSet_UnitOfMeasure_UnitOfMeasureID]
GO
ALTER TABLE [dbo].[WOD_TestPoint]  WITH CHECK ADD  CONSTRAINT [FK_WOD_TestPoint_TestPoint_TestPointID] FOREIGN KEY([TestPointID])
REFERENCES [dbo].[TestPoint] ([TestPointID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WOD_TestPoint] CHECK CONSTRAINT [FK_WOD_TestPoint_TestPoint_TestPointID]
GO
ALTER TABLE [dbo].[WOD_TestPoint]  WITH CHECK ADD  CONSTRAINT [FK_WOD_TestPoint_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WOD_TestPoint] CHECK CONSTRAINT [FK_WOD_TestPoint_WorkOrderDetail_WorkOrderDetailID]
GO
ALTER TABLE [dbo].[WOD_Weight]  WITH CHECK ADD  CONSTRAINT [FK_WOD_Weight_WeightSet_WeightSetID] FOREIGN KEY([WeightSetID])
REFERENCES [dbo].[WeightSet] ([WeightSetID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WOD_Weight] CHECK CONSTRAINT [FK_WOD_Weight_WeightSet_WeightSetID]
GO
ALTER TABLE [dbo].[WOD_Weight]  WITH CHECK ADD  CONSTRAINT [FK_WOD_Weight_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WOD_Weight] CHECK CONSTRAINT [FK_WOD_Weight_WorkOrderDetail_WorkOrderDetailID]
GO
ALTER TABLE [dbo].[WorkDetailHistory]  WITH CHECK ADD  CONSTRAINT [FK_WorkDetailHistory_User_TechnicianID] FOREIGN KEY([TechnicianID])
REFERENCES [dbo].[User] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkDetailHistory] CHECK CONSTRAINT [FK_WorkDetailHistory_User_TechnicianID]
GO
ALTER TABLE [dbo].[WorkDetailHistory]  WITH CHECK ADD  CONSTRAINT [FK_WorkDetailHistory_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOrderDetailID])
REFERENCES [dbo].[WorkOrderDetail] ([WorkOrderDetailID])
GO
ALTER TABLE [dbo].[WorkDetailHistory] CHECK CONSTRAINT [FK_WorkDetailHistory_WorkOrderDetail_WorkOrderDetailID]
GO
ALTER TABLE [dbo].[WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrder_Address_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrder] CHECK CONSTRAINT [FK_WorkOrder_Address_AddressId]
GO
ALTER TABLE [dbo].[WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrder_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrder] CHECK CONSTRAINT [FK_WorkOrder_Customer_CustomerId]
GO
ALTER TABLE [dbo].[WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderDetail_Address_AddressID] FOREIGN KEY([AddressID])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[WorkOrderDetail] CHECK CONSTRAINT [FK_WorkOrderDetail_Address_AddressID]
GO
ALTER TABLE [dbo].[WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderDetail_CalibrationType_CalibrationTypeID] FOREIGN KEY([CalibrationTypeID])
REFERENCES [dbo].[CalibrationType] ([CalibrationTypeId])
GO
ALTER TABLE [dbo].[WorkOrderDetail] CHECK CONSTRAINT [FK_WorkOrderDetail_CalibrationType_CalibrationTypeID]
GO
ALTER TABLE [dbo].[WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderDetail_PieceOfEquipment_PieceOfEquipmentId] FOREIGN KEY([PieceOfEquipmentId])
REFERENCES [dbo].[PieceOfEquipment] ([PieceOfEquipmentID])
GO
ALTER TABLE [dbo].[WorkOrderDetail] CHECK CONSTRAINT [FK_WorkOrderDetail_PieceOfEquipment_PieceOfEquipmentId]
GO
ALTER TABLE [dbo].[WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderDetail_Status_CurrentStatusID] FOREIGN KEY([CurrentStatusID])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[WorkOrderDetail] CHECK CONSTRAINT [FK_WorkOrderDetail_Status_CurrentStatusID]
GO
ALTER TABLE [dbo].[WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderDetail_User_TechnicianID] FOREIGN KEY([TechnicianID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[WorkOrderDetail] CHECK CONSTRAINT [FK_WorkOrderDetail_User_TechnicianID]
GO
ALTER TABLE [dbo].[WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderDetail_WorkOrderDetail_WorkOrderDetailID] FOREIGN KEY([WorkOderID])
REFERENCES [dbo].[WorkOrder] ([WorkOrderId])
GO
ALTER TABLE [dbo].[WorkOrderDetail] CHECK CONSTRAINT [FK_WorkOrderDetail_WorkOrderDetail_WorkOrderDetailID]
GO

/****** Object:  View [dbo].[WorkOrderDetailByCustomer]    Script Date: 11/10/2021 2:41:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WorkOrderDetailByCustomer]
AS
SELECT        dbo.WorkOrderDetail.WorkOrderDetailID, et.Manufacturer, et.Model, dbo.PieceOfEquipment.SerialNumber, et.EquipmentType, dbo.WorkOrderDetail.WorkOderID AS WorkOrderId, 
                         dbo.WorkOrder.WorkOrderDate AS WorkOrderReceiveDate, dbo.Status.Name AS Status, et.EquipmentTypeID, dbo.WorkOrderDetail.CurrentStatusID AS StatusId, dbo.WorkOrderDetail.StatusDate, '' AS Name, '' AS Description, 
                         dbo.Customer.Name AS Company, Com.Address, Com.City, Com.State, Com.ZipCode, dbo.WorkOrderDetail.CalibrationDate AS DueDate, dbo.WorkOrderDetail.CalibrationNextDueDate AS DateEnd, Com.County
FROM            dbo.WorkOrderDetail INNER JOIN
                         dbo.PieceOfEquipment ON dbo.PieceOfEquipment.PieceOfEquipmentID = dbo.WorkOrderDetail.PieceOfEquipmentId INNER JOIN
                             (SELECT        PieceOfEquipment_1.PieceOfEquipmentID, dbo.EquipmentTemplate.Model, dbo.EquipmentType.Name AS EquipmentType, dbo.EquipmentTemplate.EquipmentTypeID, 
                                                         dbo.Manufacturer.Name AS Manufacturer
                               FROM            dbo.PieceOfEquipment AS PieceOfEquipment_1 INNER JOIN
                                                         dbo.EquipmentTemplate ON PieceOfEquipment_1.EquipmentTemplateId = dbo.EquipmentTemplate.EquipmentTemplateID INNER JOIN
                                                         dbo.EquipmentType ON dbo.EquipmentType.EquipmentTypeID = dbo.EquipmentTemplate.EquipmentTypeID INNER JOIN
                                                         dbo.Manufacturer ON dbo.Manufacturer.ManufacturerID = dbo.EquipmentTemplate.ManufacturerID) AS et ON et.PieceOfEquipmentID = dbo.WorkOrderDetail.PieceOfEquipmentId INNER JOIN
                         dbo.WorkOrder ON dbo.WorkOrder.WorkOrderId = dbo.WorkOrderDetail.WorkOderID INNER JOIN
                         dbo.Status ON dbo.Status.StatusId = dbo.WorkOrderDetail.CurrentStatusID INNER JOIN
                         dbo.Customer ON dbo.Customer.CustomerID = dbo.PieceOfEquipment.CustomerId INNER JOIN
                             (SELECT        dbo.CustomerAggregates.CustomerID, dbo.Address.StreetAddress1 AS Address, dbo.Address.CityID AS City, dbo.Address.StateID AS State, dbo.Address.ZipCode, dbo.Address.County
                               FROM            dbo.CustomerAggregates INNER JOIN
                                                         dbo.Address ON dbo.Address.CustomerAggregateAggregateID = dbo.CustomerAggregates.AggregateID) AS Com ON dbo.Customer.CustomerID = Com.CustomerID
GO
/****** Object:  View [dbo].[WorkOrderDetailByStatus]    Script Date: 11/10/2021 2:41:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[WorkOrderDetailByStatus]
AS
SELECT        dbo.WorkOrderDetail.WorkOrderDetailID, et.Manufacturer, et.Model, dbo.PieceOfEquipment.SerialNumber, et.EquipmentType, dbo.WorkOrderDetail.WorkOderID AS WorkOrderId, 
                         dbo.WorkOrder.WorkOrderDate AS WorkOrderReceiveDate, dbo.Status.Name AS Status, et.EquipmentTypeID, dbo.WorkOrderDetail.CurrentStatusID AS StatusId, dbo.WorkOrderDetail.StatusDate, '' AS Name, '' AS Description, 
                         dbo.Customer.Name AS Company
FROM            dbo.WorkOrderDetail INNER JOIN
                         dbo.PieceOfEquipment ON dbo.PieceOfEquipment.PieceOfEquipmentID = dbo.WorkOrderDetail.PieceOfEquipmentId INNER JOIN
                             (SELECT        PieceOfEquipment_1.PieceOfEquipmentID, dbo.EquipmentTemplate.Model, dbo.EquipmentType.Name AS EquipmentType, dbo.EquipmentTemplate.EquipmentTypeID, 
                                                         dbo.Manufacturer.Name AS Manufacturer
                               FROM            dbo.PieceOfEquipment AS PieceOfEquipment_1 INNER JOIN
                                                         dbo.EquipmentTemplate ON PieceOfEquipment_1.EquipmentTemplateId = dbo.EquipmentTemplate.EquipmentTemplateID INNER JOIN
                                                         dbo.EquipmentType ON dbo.EquipmentType.EquipmentTypeID = dbo.EquipmentTemplate.EquipmentTypeID INNER JOIN
                                                         dbo.Manufacturer ON dbo.Manufacturer.ManufacturerID = dbo.EquipmentTemplate.ManufacturerID) AS et ON et.PieceOfEquipmentID = dbo.WorkOrderDetail.PieceOfEquipmentId INNER JOIN
                         dbo.WorkOrder ON dbo.WorkOrder.WorkOrderId = dbo.WorkOrderDetail.WorkOderID INNER JOIN
                         dbo.Status ON dbo.Status.StatusId = dbo.WorkOrderDetail.CurrentStatusID INNER JOIN
                         dbo.Customer ON dbo.Customer.CustomerID = dbo.PieceOfEquipment.CustomerId
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[7] 2[34] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "WorkOrderDetail"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PieceOfEquipment"
            Begin Extent = 
               Top = 6
               Left = 310
               Bottom = 136
               Right = 556
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "et"
            Begin Extent = 
               Top = 6
               Left = 594
               Bottom = 136
               Right = 793
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkOrder"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Status"
            Begin Extent = 
               Top = 138
               Left = 249
               Bottom = 268
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Customer"
            Begin Extent = 
               Top = 138
               Left = 475
               Bottom = 268
               Right = 645
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Com"
            Begin Extent = 
               Top = 138
               Left = 683
               Bottom = 268
               Right = 853
            End
            DisplayFlag' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WorkOrderDetailByCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N's = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WorkOrderDetailByCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WorkOrderDetailByCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[17] 2[30] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = -910
      End
      Begin Tables = 
         Begin Table = "WorkOrderDetail"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 209
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 31
         End
         Begin Table = "PieceOfEquipment"
            Begin Extent = 
               Top = 260
               Left = 51
               Bottom = 390
               Right = 297
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "et"
            Begin Extent = 
               Top = 6
               Left = 521
               Bottom = 136
               Right = 720
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WorkOrder"
            Begin Extent = 
               Top = 6
               Left = 310
               Bottom = 210
               Right = 483
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Status"
            Begin Extent = 
               Top = 400
               Left = 431
               Bottom = 590
               Right = 619
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Customer"
            Begin Extent = 
               Top = 138
               Left = 521
               Bottom = 268
               Right = 691
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WorkOrderDetailByStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WorkOrderDetailByStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'WorkOrderDetailByStatus'
GO

GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasureType] ON 
GO
INSERT [dbo].[UnitOfMeasureType] ([Value], [Name], [Description]) VALUES (1, N'Temperature', NULL)
GO
INSERT [dbo].[UnitOfMeasureType] ([Value], [Name], [Description]) VALUES (2, N'Humidity', NULL)
GO
INSERT [dbo].[UnitOfMeasureType] ([Value], [Name], [Description]) VALUES (3, N'Weight', NULL)
GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasureType] OFF
GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasure] ON 
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (3, N'Pounds', N'lb', 1, NULL, 3, 0, 3, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (4, N'Kilograms', N'kg', 1, NULL, 3, 0, 4, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (5, N'Ounces', N'oz', 1, NULL, 3, 0, 5, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (6, N'Grams', N'g', 1, NULL, 3, 0, 6, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (7, N'Gallon', N'gal', 1, NULL, 3, 0, 7, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (8, N'Gallon', N'gallon', 1, NULL, 3, 0, 8, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (9, N'Tons', N'tons', 1, NULL, 3, 0, 9, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (10, N'TPH', N'TPH', 1, NULL, 3, 0, 10, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (11, N'Celcius', N'C', 1, NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (12, N'Farenheit', N'F', 1, NULL, 1, 0, 12, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (13, N'Humidity', N'H', 1, NULL, 2, 0, 13, NULL)
GO
INSERT [dbo].[UnitOfMeasure] ([UnitOfMeasureID], [Name], [Abbreviation], [IsEnabled], [UnitOfMeasureBaseID], [TypeID], [ConversionValue], [UncertaintyUnitOfMeasureID], [Description]) VALUES (14, N'test', N't', 1, 11, 3, 1, 11, NULL)
GO
SET IDENTITY_INSERT [dbo].[UnitOfMeasure] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 
GO
INSERT [dbo].[Status] ([StatusId], [Name], [Description], [IsDefault], [IsEnable], [Possibilities], [WorkOrderDetailID], [IsLast]) VALUES (1, N'Contract Review', N'', 1, 1, N'2', NULL, 0)
GO
INSERT [dbo].[Status] ([StatusId], [Name], [Description], [IsDefault], [IsEnable], [Possibilities], [WorkOrderDetailID], [IsLast]) VALUES (2, N'Ready for Calibration', N'', 0, 1, N'1;3', NULL, 0)
GO
INSERT [dbo].[Status] ([StatusId], [Name], [Description], [IsDefault], [IsEnable], [Possibilities], [WorkOrderDetailID], [IsLast]) VALUES (3, N'Technical Review', N'', 0, 1, N'1;2;4', NULL, 0)
GO
INSERT [dbo].[Status] ([StatusId], [Name], [Description], [IsDefault], [IsEnable], [Possibilities], [WorkOrderDetailID], [IsLast]) VALUES (4, N'Completed', NULL, 0, 1, N'3;2', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (1, N'Jon', N'Bitterman', NULL, 0, 1, NULL, N'jon@bittermanscales.com', N'admin ,', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (2, N'Matt', N'Preston', NULL, 0, 1, N'matt', N'matt@bittermanscales.com', N'admin', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (3, N'CJ', N'Bitterman', NULL, 0, 1, NULL, N'cj@bittermanscales.com', N'admin ,', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (4, N'Roger', N'Bailey', NULL, 0, 1, N'roger', N'roger@bittermanscales.com', N'admin', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (5, N'Craig', N'Bitterman', NULL, 0, 1, NULL, N'craig@bittermanscales.com', N'admin ,', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (9, N'Matt2', N'Test', NULL, 0, 1, NULL, N'matt@bittermanscales1.com', N'', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (11, N'Paulo', N'Burgos', NULL, 0, 1, NULL, N'pburgos@kavoku.com', N'', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (13, N'Jon2', N'Test', NULL, 0, 1, NULL, N'jon@bittermanscales1.com', N'', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (14, N'Paulo Test 3', N'Paulo', NULL, 0, 1, NULL, N'pburgos@kavoku2.com', N'', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (15, N'Michael', N'Schlott', NULL, 0, 1, NULL, N'mike@bittermanscales.com', N'tech ,', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (16, N'Chris', N'Chris', NULL, 0, 1, N'Chris', N'chris@bittermanscales.com', N'tech', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (17, N'Taylor ', N'Taylor ', NULL, 0, 1, N'taylor', N'taylor@bittermascales.com ', N'tech,tech.HasView,tech.HasEdit,tech.HasSelect', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (18, N'Mike', N'Mike ', NULL, 0, 1, N'mike', N'mike2@bittermanscales.com ', N'admin', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (19, N'Test', N'Test', NULL, 0, 1, N'test', N'test@test.com', N'admin,admin.HasFullacces', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (20, N'Test2', N'Test2', NULL, 0, 1, N'test2', N'test2@test.com', N'admin,admin.HasFullacces', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
INSERT [dbo].[User] ([UserID], [Name], [LastName], [UserTypeID], [PasswordReset], [IsEnabled], [UserName], [Email], [Roles], [Occupation], [Description], [PieceOfEquipmentID], [WorkOrderId], [IdentityID], [PassWord]) VALUES (21, N'Test3', N'Test3', NULL, 0, 1, N'test3', N'test3@test.com', N'admin,admin.HasFullacces', NULL, NULL, NULL, NULL, NULL, N'Pass123$')
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO

