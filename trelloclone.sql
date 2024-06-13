USE [master]
GO
/****** Object:  Database [Trelloclone]    Script Date: 6/13/2024 2:42:43 PM ******/
CREATE DATABASE [Trelloclone]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Trelloclone', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Trelloclone.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Trelloclone_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Trelloclone_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Trelloclone] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Trelloclone].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Trelloclone] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Trelloclone] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Trelloclone] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Trelloclone] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Trelloclone] SET ARITHABORT OFF 
GO
ALTER DATABASE [Trelloclone] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Trelloclone] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Trelloclone] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Trelloclone] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Trelloclone] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Trelloclone] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Trelloclone] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Trelloclone] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Trelloclone] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Trelloclone] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Trelloclone] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Trelloclone] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Trelloclone] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Trelloclone] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Trelloclone] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Trelloclone] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Trelloclone] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Trelloclone] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Trelloclone] SET  MULTI_USER 
GO
ALTER DATABASE [Trelloclone] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Trelloclone] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Trelloclone] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Trelloclone] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Trelloclone] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Trelloclone] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Trelloclone] SET QUERY_STORE = ON
GO
ALTER DATABASE [Trelloclone] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Trelloclone]
GO
/****** Object:  Table [dbo].[Board]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Board](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NULL,
	[IsPublic] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Board] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BoardMember]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BoardMember](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[BoardId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_BoardMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Card]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Card](
	[Id] [uniqueidentifier] NOT NULL,
	[ListId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ReminderDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CardActivity]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardActivity](
	[Id] [uniqueidentifier] NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Activity] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CardActivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CardLabel]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardLabel](
	[Id] [uniqueidentifier] NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[LabelId] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CardLabel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CardMember]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardMember](
	[Id] [uniqueidentifier] NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CardMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[Id] [uniqueidentifier] NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Label]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Label](
	[Id] [uniqueidentifier] NOT NULL,
	[BoardId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Color] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Label] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[List]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[List](
	[Id] [uniqueidentifier] NOT NULL,
	[BoardId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Position] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_List] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [uniqueidentifier] NOT NULL,
	[TodoId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsChecked] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDo]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDo](
	[Id] [uniqueidentifier] NOT NULL,
	[CardId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ToDo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/13/2024 2:42:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Gender] [nvarchar](20) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'PMS', CAST(N'2024-06-13T09:58:34.407' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'A Hiring & Recruiting', CAST(N'2024-06-13T10:10:28.253' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Nonprofit Volunteer', CAST(N'2024-06-13T10:08:05.693' AS DateTime), N'7dd04903-980d-403a-9b79-80e8afce11e5', NULL, NULL, 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'fbad3cf5-1316-4150-b483-4ef70b8eaddb', N'Grant Tracking', CAST(N'2024-06-13T10:08:36.993' AS DateTime), N'7dd04903-980d-403a-9b79-80e8afce11e5', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'CoMS', CAST(N'2024-06-13T10:01:07.440' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Agile Sprint Board', CAST(N'2024-06-13T10:02:45.100' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'1ec544dc-9da1-4667-880f-efffd59d89dd', N'Bug Tracker', CAST(N'2024-06-13T10:02:26.263' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, 1, 1)
GO
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'676ec6e5-f1dd-4bab-aef6-17bc21bd30fb', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'Product backlog
', NULL, CAST(N'2024-06-13T03:20:23.493' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'login', NULL, CAST(N'2024-06-13T03:20:59.447' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'1e46a9ca-5284-456f-9d17-8e5bbc384fad', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'SRS', NULL, CAST(N'2024-06-13T03:20:28.323' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'70a5a781-4ee7-4d35-8310-a21b7c82474a', N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'78', NULL, CAST(N'2024-06-13T03:26:06.910' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-13T10:26:10.687' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'390d87a2-5307-4e74-a3b2-c900c6cbf95e', N'db15adb5-34cf-4676-89bf-c85c318b7799', N'user registration', NULL, CAST(N'2024-06-13T03:21:32.110' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'7fd83e9a-fa66-45ea-b2ca-c9d007566297', N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'Create an endpoint (API) and handle the service for create board', NULL, CAST(N'2024-06-13T03:20:55.363' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-13T10:24:20.873' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'944533ae-3e4f-4eb4-adc1-e1dd73e5d67c', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'Database', NULL, CAST(N'2024-06-13T03:20:44.060' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'70f75543-5d43-4d66-b338-006d53a62d2b', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Done', 4, CAST(N'2024-06-13T10:20:07.497' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'049a8982-5872-45ee-b98a-0a4d5047800c', N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Kick-start', 3, CAST(N'2024-06-13T10:14:51.823' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'780578d9-cfeb-46ad-a964-33478a9606e7', N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Organization ', 1, CAST(N'2024-06-13T10:14:25.677' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5b50357f-2090-4e44-aaae-375881fcda6e', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'Offer sent', 4, CAST(N'2024-06-13T10:13:22.117' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0411e8dc-9233-4bd8-b0c3-472d2a804c06', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'Incoming', 2, CAST(N'2024-06-13T10:16:34.020' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'66281925-bed6-458b-b64e-4ebcb71c7c4a', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'InProgress', 3, CAST(N'2024-06-13T10:17:44.143' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f0333af6-ef7c-47a0-b275-59233d74aaaa', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'Interview process', 3, CAST(N'2024-06-13T10:13:07.567' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3e93af4f-0211-4d8f-95d7-67ac1c29ed6c', N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Role', 2, CAST(N'2024-06-13T10:14:30.423' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0b0f3d01-d541-4196-8669-67b945630751', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'Backlog', 1, CAST(N'2024-06-13T10:16:20.483' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'66874d18-37e7-4e9e-9683-80fa622244b3', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'New Applicants', 1, CAST(N'2024-06-13T10:12:07.410' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6bdb8e37-db51-483b-b5da-8e2b549cac35', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Sprint backlog', 2, CAST(N'2024-06-13T10:17:25.533' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'01f15719-2e2f-4069-b972-90c31d0bbba4', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Backlog', 1, CAST(N'2024-06-13T10:17:14.087' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Document', 1, CAST(N'2024-06-13T10:19:32.440' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e46b98d0-2950-453d-b093-950d69ca60d9', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'Doing', 3, CAST(N'2024-06-13T10:16:42.527' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Backlog', 2, CAST(N'2024-06-13T10:19:38.380' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'db15adb5-34cf-4676-89bf-c85c318b7799', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Todo', 3, CAST(N'2024-06-13T10:19:52.350' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'40a933e4-ec6d-4148-94fc-cf8b98104a90', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'Interview schedule', 2, CAST(N'2024-06-13T10:12:42.640' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'52bc1785-781f-495f-a9ed-e811324e64f3', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Complete', 4, CAST(N'2024-06-13T10:18:00.970' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'97b6e635-2ef9-4d3b-94cb-379099884d75', N'Admin', CAST(N'2024-06-13T09:57:24.467' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', N'Member', CAST(N'2024-06-13T09:57:33.013' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'phongnt@gmail.com', N'02ajJZlciZ/Pl6FCiRPqkQ==:8TFwBxbvIBxqkYFFuZ781ECgJjlhdS7zPP4elA7NT2g=', N'PhongNT', NULL, CAST(N'2024-06-13T02:53:56.113' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'toannb@gmail.com', N'ifGlHVsn7wQZ/xFuNOOtzg==:fIOirxBImZq7IvcmSQRKnP67uqcffa+d3P8ugYF9hgw=', N'ToanNB', NULL, CAST(N'2024-06-13T02:55:43.337' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'7dd04903-980d-403a-9b79-80e8afce11e5', N'tamnt@gmail.com', N'5g4vAyI1QS9HGUHF23MVeA==:2uw9EFkkVi434jcjGxftxXvNIRkf1VRcJU+AihYsT3I=', N'TamNT', NULL, CAST(N'2024-06-13T02:54:42.957' AS DateTime), N'7dd04903-980d-403a-9b79-80e8afce11e5', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
ALTER TABLE [dbo].[BoardMember]  WITH CHECK ADD  CONSTRAINT [FK_BoardMember_Board] FOREIGN KEY([BoardId])
REFERENCES [dbo].[Board] ([Id])
GO
ALTER TABLE [dbo].[BoardMember] CHECK CONSTRAINT [FK_BoardMember_Board]
GO
ALTER TABLE [dbo].[BoardMember]  WITH CHECK ADD  CONSTRAINT [FK_BoardMember_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[BoardMember] CHECK CONSTRAINT [FK_BoardMember_Role]
GO
ALTER TABLE [dbo].[BoardMember]  WITH CHECK ADD  CONSTRAINT [FK_BoardMember_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[BoardMember] CHECK CONSTRAINT [FK_BoardMember_User]
GO
ALTER TABLE [dbo].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_List] FOREIGN KEY([ListId])
REFERENCES [dbo].[List] ([Id])
GO
ALTER TABLE [dbo].[Card] CHECK CONSTRAINT [FK_Card_List]
GO
ALTER TABLE [dbo].[CardActivity]  WITH CHECK ADD  CONSTRAINT [FK_CardActivity_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([Id])
GO
ALTER TABLE [dbo].[CardActivity] CHECK CONSTRAINT [FK_CardActivity_Card]
GO
ALTER TABLE [dbo].[CardActivity]  WITH CHECK ADD  CONSTRAINT [FK_CardActivity_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CardActivity] CHECK CONSTRAINT [FK_CardActivity_User]
GO
ALTER TABLE [dbo].[CardLabel]  WITH CHECK ADD  CONSTRAINT [FK_CardLabel_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([Id])
GO
ALTER TABLE [dbo].[CardLabel] CHECK CONSTRAINT [FK_CardLabel_Card]
GO
ALTER TABLE [dbo].[CardLabel]  WITH CHECK ADD  CONSTRAINT [FK_CardLabel_Label] FOREIGN KEY([LabelId])
REFERENCES [dbo].[Label] ([Id])
GO
ALTER TABLE [dbo].[CardLabel] CHECK CONSTRAINT [FK_CardLabel_Label]
GO
ALTER TABLE [dbo].[CardMember]  WITH CHECK ADD  CONSTRAINT [FK_CardMember_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([Id])
GO
ALTER TABLE [dbo].[CardMember] CHECK CONSTRAINT [FK_CardMember_Card]
GO
ALTER TABLE [dbo].[CardMember]  WITH CHECK ADD  CONSTRAINT [FK_CardMember_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CardMember] CHECK CONSTRAINT [FK_CardMember_User]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([Id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Card]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_User]
GO
ALTER TABLE [dbo].[Label]  WITH CHECK ADD  CONSTRAINT [FK_Label_Board] FOREIGN KEY([BoardId])
REFERENCES [dbo].[Board] ([Id])
GO
ALTER TABLE [dbo].[Label] CHECK CONSTRAINT [FK_Label_Board]
GO
ALTER TABLE [dbo].[List]  WITH CHECK ADD  CONSTRAINT [FK_List_Board] FOREIGN KEY([BoardId])
REFERENCES [dbo].[Board] ([Id])
GO
ALTER TABLE [dbo].[List] CHECK CONSTRAINT [FK_List_Board]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_ToDo] FOREIGN KEY([TodoId])
REFERENCES [dbo].[ToDo] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_ToDo]
GO
ALTER TABLE [dbo].[ToDo]  WITH CHECK ADD  CONSTRAINT [FK_ToDo_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([Id])
GO
ALTER TABLE [dbo].[ToDo] CHECK CONSTRAINT [FK_ToDo_Card]
GO
USE [master]
GO
ALTER DATABASE [Trelloclone] SET  READ_WRITE 
GO
