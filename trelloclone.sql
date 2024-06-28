USE [master]
GO
/****** Object:  Database [Trelloclone]    Script Date: 6/28/2024 3:56:29 PM ******/
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
/****** Object:  Table [dbo].[Board]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[BoardMember]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[Card]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[CardActivity]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[CardLabel]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[CardMember]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[Comment]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[Label]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[List]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[Task]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[ToDo]    Script Date: 6/28/2024 3:56:30 PM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 6/28/2024 3:56:30 PM ******/
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
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'PMS - Trellone', CAST(N'2024-06-21T14:09:33.273' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'PMS', CAST(N'2024-06-13T09:58:34.407' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T13:47:08.613' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'2d10b30b-44c2-45ff-85f9-1cfe4509ce21', N'anho', CAST(N'2024-06-20T10:47:42.303' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T15:10:33.507' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'de53d7db-58a5-4c49-ab54-2b8359fde21e', N'Test board member', CAST(N'2024-06-17T08:41:07.617' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T15:18:57.227' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'A Hiring & Recruiting', CAST(N'2024-06-13T10:10:28.253' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'888c9404-fb9c-470c-9be6-4b1962cc6ff3', N'test new board', CAST(N'2024-06-21T08:25:01.043' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T13:46:27.200' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Nonprofit Volunteer', CAST(N'2024-06-13T10:08:05.693' AS DateTime), N'7dd04903-980d-403a-9b79-80e8afce11e5', NULL, NULL, 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'PMS- Trellone 1', CAST(N'2024-06-20T14:08:50.687' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-28T15:02:50.400' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'fbad3cf5-1316-4150-b483-4ef70b8eaddb', N'Grant Tracking', CAST(N'2024-06-13T10:08:36.993' AS DateTime), N'7dd04903-980d-403a-9b79-80e8afce11e5', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'alo', CAST(N'2024-06-21T10:51:00.567' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', CAST(N'2024-06-21T11:00:13.483' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'd2ba316d-969a-4d85-9034-897da70319df', N'alo', CAST(N'2024-06-19T14:47:26.290' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, NULL, 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'c613bcb0-b003-4fc6-bc55-8e38ba78239e', N'Test ver 2', CAST(N'2024-06-17T08:42:12.910' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T15:10:19.830' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'c1932f1f-2bcb-4b7f-8ed0-9e1fb7e010c0', N'phong', CAST(N'2024-06-28T14:53:06.100' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'219cecd8-0124-4454-b57a-b63768ad68ea', N'Test ver 3', CAST(N'2024-06-17T15:36:01.327' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T08:29:20.007' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'CoMS', CAST(N'2024-06-13T10:01:07.440' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T15:33:29.550' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'a3d602d2-d0ce-444a-950d-c77b7454d614', N'isTop', CAST(N'2024-06-21T10:43:52.090' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T10:45:42.790' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Agile Sprint Board', CAST(N'2024-06-13T10:02:45.100' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-17T08:43:36.293' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'651612db-6db8-49ce-8a72-d87138b76694', N'Boom', CAST(N'2024-06-20T16:36:08.810' AS DateTime), N'8da3122d-cb65-4d98-8054-a9a2e4e77446', CAST(N'2024-06-20T16:55:46.847' AS DateTime), N'8da3122d-cb65-4d98-8054-a9a2e4e77446', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'1ec544dc-9da1-4667-880f-efffd59d89dd', N'Bug Tracker', CAST(N'2024-06-13T10:02:26.263' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-17T08:42:58.807' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'6bf4598d-559c-4ac6-a1fa-f8efb5e022c7', N'Phongnt', CAST(N'2024-06-28T14:56:52.977' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, 1, 1)
GO
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'08f12f41-176d-4187-a2dd-067e404c817d', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T09:17:19.237' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T10:06:33.117' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'94d604a6-62b9-425b-acac-0e55e2fb6175', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'a3d602d2-d0ce-444a-950d-c77b7454d614', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-21T10:43:52.093' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1a03ac31-8153-47a7-9e61-13079bf119dc', N'bf9eb7e3-e9ca-4c10-a5a0-37dc7c6071a5', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-20T15:35:45.800' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'09e9f49e-75f6-4c28-b086-1500cadd90a3', N'8da3122d-cb65-4d98-8054-a9a2e4e77446', N'651612db-6db8-49ce-8a72-d87138b76694', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-20T16:36:08.817' AS DateTime), N'8da3122d-cb65-4d98-8054-a9a2e4e77446', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f70cbe8b-9506-4f7f-9221-152cb6306782', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T14:21:38.413' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'af21f219-8092-46b2-aaba-1dae0ba98e0a', N'970ad5c7-3f15-4379-aeb2-c1e6e8317adc', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-19T14:20:33.537' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0b1d03c8-a357-4c66-83f3-259d2c9d88d4', N'eb029ad5-7704-4f78-8600-4b7c368e5617', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-28T14:54:44.767' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'46047b0b-d671-4e0d-9046-2859d0f186a7', N'9386ed34-308e-432a-bbb3-0cbf266aefb7', N'219cecd8-0124-4454-b57a-b63768ad68ea', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-17T15:36:12.277' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'06af2aba-a26b-45f7-8dd2-2cddef122143', N'7dd04903-980d-403a-9b79-80e8afce11e5', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T11:15:22.173' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e9de7b02-3263-4762-aaa6-3db6ee52807f', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-21T10:52:58.853' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a7e621c2-1a5f-43ed-b58e-45cf90a07c94', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'1ec544dc-9da1-4667-880f-efffd59d89dd', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T14:21:24.743' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9d9664cb-cb0e-4c0a-a5ef-49d7175058fe', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-20T14:10:56.847' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9e85b687-4de1-4aee-9a34-4dfde00ab800', N'7dd04903-980d-403a-9b79-80e8afce11e5', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T14:34:33.243' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-19T11:06:35.357' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'209fbef9-f024-4de3-977f-54255b7638e9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-14T09:16:22.553' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b336a6d9-76af-4c56-9fbf-59d48bccdc2e', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'219cecd8-0124-4454-b57a-b63768ad68ea', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-17T15:36:01.450' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5620ec1c-22f6-4a21-92cd-5dd937538491', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'888c9404-fb9c-470c-9be6-4b1962cc6ff3', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-21T08:25:01.070' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f91a2fa3-0d5a-4c34-a9c7-5e17b79a6f3b', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'c613bcb0-b003-4fc6-bc55-8e38ba78239e', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-17T08:42:12.913' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'138011eb-f3c4-4aa1-9043-6505935fcfa8', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-20T14:08:50.803' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'94349b66-26d2-409b-8f55-67e4fa7f65a0', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'c613bcb0-b003-4fc6-bc55-8e38ba78239e', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-17T09:00:21.617' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'd813f139-6f4e-4827-a562-72fcd3905aca', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'd2ba316d-969a-4d85-9034-897da70319df', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-19T14:47:26.293' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c481cafd-3853-4fef-871a-7c60bc594f9c', N'6ab597c9-f578-44c5-b539-1fc969bc9d37', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-17T09:30:18.807' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-19T11:10:11.823' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6f845591-a578-48e0-a0b5-7e1ad161b156', N'2cf1f1a8-a868-46c3-a480-68333817c77b', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T09:55:49.730' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b3498486-183c-4ea3-a7d3-7f830dd6f18d', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-21T14:10:00.457' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1082caef-75e0-48a2-b85e-8413226b7e42', N'bf9eb7e3-e9ca-4c10-a5a0-37dc7c6071a5', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-21T10:53:07.260' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5a10569c-6052-4103-9d21-85ae24ad9e57', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-20T14:10:56.847' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T15:08:43.520' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b7962080-b7bb-40bd-bdff-8cb80d2514f8', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-19T14:50:30.997' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0175b42b-2a85-4288-ae68-9bacb4d70521', N'9f09d341-88d1-44e3-8a02-e638b821c356', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-19T14:20:08.157' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'58dac56b-3f6e-4448-aa3c-9d4596dd8b07', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'2d10b30b-44c2-45ff-85f9-1cfe4509ce21', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-20T10:47:42.327' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f7c59230-5ad7-4ed6-bcdc-a521ec677256', N'ed9cba81-e638-42af-9198-c8575a6f9126', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-19T14:20:25.973' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'873a613b-d619-4adb-ada0-a844a6936fb3', N'df1e4e23-9547-4340-9a13-2b4a98d63576', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-21T14:09:33.277' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'338f8971-f9ff-44c6-88d5-c039cc4fc375', N'9e205d0a-4775-4c70-85db-e1c0144a3b7f', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-19T14:20:17.573' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b675ffae-c3fd-46f3-9b3e-dcb6bee6935c', N'42ddd23d-9513-4245-9e16-260a74ab9460', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-21T09:58:03.260' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3c2e3736-2f75-414c-b683-dfcc0801b4cb', N'9f09d341-88d1-44e3-8a02-e638b821c356', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-19T14:20:08.167' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'65658f1e-6d91-490b-a4c5-e30cdc772c2a', N'eb029ad5-7704-4f78-8600-4b7c368e5617', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-21T10:51:01.410' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'01a693b2-3d04-4cdb-82b7-e56754360eef', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-19T14:49:32.397' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'bc17eabe-ab87-4425-b6ec-f01be6a48fc7', N'5c14f7ee-a16a-475f-b5f4-5f88063998bb', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-17T10:01:53.417' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-19T11:30:08.077' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'61b67ecf-b5d7-486b-95bd-f3b890161374', N'370bb23a-d47f-45a5-823a-367e0fd2b681', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-17T09:57:57.590' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-19T11:28:12.797' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'032f6daa-ffa7-4c40-bdf5-f92e051b83fc', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'de53d7db-58a5-4c49-ab54-2b8359fde21e', N'97b6e635-2ef9-4d3b-94cb-379099884d75', CAST(N'2024-06-17T08:41:07.737' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'cf6ea158-7c72-49d6-9b1c-ff0563a25c7f', N'f10c308a-b941-4776-a73f-404fd3e01f55', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', CAST(N'2024-06-14T11:07:24.803' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-19T14:19:34.883' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1)
GO
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'4930d762-ee2e-4845-a64b-0ab18e551f23', N'70f75543-5d43-4d66-b338-006d53a62d2b', N'alo
', NULL, CAST(N'2024-06-21T02:19:09.477' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:19:10.967' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'676ec6e5-f1dd-4bab-aef6-17bc21bd30fb', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'Product backlog', N'This is product back log description ', CAST(N'2024-06-13T03:20:23.493' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-13T08:52:17.587' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'ab9b0e2a-1e38-40d0-af08-2ca84a7f6610', N'95799dbf-e969-4709-a188-3d3b22d9f203', N'SRS', NULL, CAST(N'2024-06-21T07:13:01.703' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'16e073d3-d0b4-49af-aa87-348770b02853', N'1033b7f2-42ec-4de1-9b76-41257938def2', N'Product backlog', NULL, CAST(N'2024-06-20T07:14:14.107' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T07:26:24.877' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'7b715da0-bd7a-4ddb-8c4c-3740e5bb45f9', N'52bc1785-781f-495f-a9ed-e811324e64f3', N'1
', NULL, CAST(N'2024-06-25T09:22:37.023' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'2e5863ce-d5d7-40eb-9d98-3e1a69d845e7', N'24bb9eaa-af8c-4fb4-adfb-94c774342470', N'Login', N'{"blocks":[{"key":"15rc","text":" ","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-21T03:56:40.463' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', CAST(N'2024-06-21T03:57:24.030' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'a598683d-3e6e-4880-af40-41abffea2277', N'66281925-bed6-458b-b64e-4ebcb71c7c4a', N'6', NULL, CAST(N'2024-06-25T09:26:33.323' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'login', NULL, CAST(N'2024-06-13T03:20:59.447' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'b096b906-68b1-4b65-827c-49c1c07b4049', N'1033b7f2-42ec-4de1-9b76-41257938def2', N'Login', N'{"blocks":[{"key":"44sof","text":"create api login","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-20T07:15:49.153' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T02:05:51.783' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'ea2b404d-088f-4fff-8517-511b77a19ad2', N'24bb9eaa-af8c-4fb4-adfb-94c774342470', N'Registration', N'{"blocks":[{"key":"cijq9","text":"Implement API Register user ","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-21T03:56:47.150' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', CAST(N'2024-06-21T10:59:00.813' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'91bf4f53-bcae-4c27-8c04-575062294cb7', N'41a64c2e-0757-4c84-beb1-2a010e7ffb4c', N'ad
', NULL, CAST(N'2024-06-20T07:43:12.827' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'6af50381-21ca-4f9f-a557-5ce87369e5f2', N'6bdb8e37-db51-483b-b5da-8e2b549cac35', N'2', NULL, CAST(N'2024-06-25T09:24:57.577' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'0fc7786e-a6e9-4103-8df6-6d7e2936a5f5', N'66281925-bed6-458b-b64e-4ebcb71c7c4a', N'do', NULL, CAST(N'2024-06-25T09:06:12.717' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T09:29:22.470' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'0038456b-01d3-46dc-bf51-746fd6150d13', N'95799dbf-e969-4709-a188-3d3b22d9f203', N'Login user', NULL, CAST(N'2024-06-21T07:13:07.227' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-21T14:13:54.793' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'cc1ef440-aa41-427d-82ee-76806349eb87', N'01f15719-2e2f-4069-b972-90c31d0bbba4', N'lo
', NULL, CAST(N'2024-06-24T09:41:42.473' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'4b6bbe02-10e9-4e4a-9c13-88055ae96176', N'24bb9eaa-af8c-4fb4-adfb-94c774342470', N'SRS', NULL, CAST(N'2024-06-21T03:56:15.327' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'1e46a9ca-5284-456f-9d17-8e5bbc384fad', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'SRS', NULL, CAST(N'2024-06-13T03:20:28.323' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'a1d4599b-d781-4baa-897d-9474886ea322', N'1033b7f2-42ec-4de1-9b76-41257938def2', N'register', N'{"blocks":[{"key":"9pk9","text":"toan will do","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-20T07:15:56.713' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:17:44.300' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'795b13e6-942a-482e-b88f-61ed0baaec42', N'ca1', NULL, CAST(N'2024-06-24T07:19:00.307' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T02:06:59.417' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'70a5a781-4ee7-4d35-8310-a21b7c82474a', N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'78', NULL, CAST(N'2024-06-13T03:26:06.910' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-13T10:26:10.687' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'8c4d9902-38b4-4510-aa43-b68b2cd2c965', N'70f75543-5d43-4d66-b338-006d53a62d2b', N'lo
', NULL, CAST(N'2024-06-21T02:10:56.537' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:10:58.790' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'81d6fcce-5948-4a20-bc90-ba2de01a965e', N'alo', NULL, CAST(N'2024-06-21T09:30:13.957' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T07:59:02.660' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'390d87a2-5307-4e74-a3b2-c900c6cbf95e', N'db15adb5-34cf-4676-89bf-c85c318b7799', N'user registration', NULL, CAST(N'2024-06-13T03:21:32.110' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'7fd83e9a-fa66-45ea-b2ca-c9d007566297', N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'Create an endpoint (API) and handle the service for create board', NULL, CAST(N'2024-06-13T03:20:55.363' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-13T10:24:20.873' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'f2be90d3-749c-4f55-acef-cc7273060972', N'e4ddde7b-eec7-4868-a67a-242251464a67', N'alo', N'{"blocks":[{"key":"7vphg","text":"ogc","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-21T04:00:43.983' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', CAST(N'2024-06-21T11:01:06.293' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'65bc7dfb-f555-4e04-9b42-d65ddc348d36', N'0b0f3d01-d541-4196-8669-67b945630751', N'Backlog', NULL, CAST(N'2024-06-21T02:03:23.597' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'0fe96260-1327-46c1-810a-d6c0670753a5', N'52bc1785-781f-495f-a9ed-e811324e64f3', N'phong', NULL, CAST(N'2024-06-25T09:20:29.033' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'd0f136c3-1f16-44f5-a1b6-d999d11e849d', N'24bb9eaa-af8c-4fb4-adfb-94c774342470', N'Testcase', NULL, CAST(N'2024-06-21T03:56:28.783' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'74a3067b-b3d5-45c1-8b61-dcbb0176f9bc', N'55ee378e-f8d4-4dec-96e3-179344af08d0', N'alo
', NULL, CAST(N'2024-06-25T07:30:53.533' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'944533ae-3e4f-4eb4-adc1-e1dd73e5d67c', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'Database', N'{"blocks":[{"key":"6acfr","text":"Create ERD ","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-13T03:20:44.060' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T02:01:26.897' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'e912de75-2c43-4c66-aae2-e4eeeef7fb76', N'1033b7f2-42ec-4de1-9b76-41257938def2', N'SRS', N'{"blocks":[{"key":"44sof","text":"","type":"unstyled","depth":0,"inlineStyleRanges":[],"entityRanges":[],"data":{}}],"entityMap":{}}', CAST(N'2024-06-20T07:15:43.040' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T02:06:36.310' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'cc1dfc6f-0219-4ed4-b3eb-e5122762621b', N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'hello', NULL, CAST(N'2024-06-21T02:10:07.663' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:10:11.690' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'ff9bf030-ff71-4ec3-8677-f3498cb07b55', N'95799dbf-e969-4709-a188-3d3b22d9f203', N'Register', NULL, CAST(N'2024-06-21T07:13:11.960' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-25T16:00:36.583' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, NULL, NULL, 0)
GO
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'accc134c-6962-4a76-b64a-023f31c20db1', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'9f09d341-88d1-44e3-8a02-e638b821c356', CAST(N'2024-06-25T17:11:06.790' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:11:48.147' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8f967416-4cec-4296-9152-0652b44810bf', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'42ddd23d-9513-4245-9e16-260a74ab9460', CAST(N'2024-06-25T17:11:07.883' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:11:53.420' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'52b97787-3fb7-475c-9fec-0bddb5a1f779', N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T11:17:26.363' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'30251383-f2b4-45e0-a55b-0c10acbfdc49', N'0fe96260-1327-46c1-810a-d6c0670753a5', N'7dd04903-980d-403a-9b79-80e8afce11e5', CAST(N'2024-06-25T16:20:36.727' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b9841ba7-da62-406a-a52f-0cbc9e1af155', N'a598683d-3e6e-4880-af40-41abffea2277', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T16:47:21.213' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T16:47:58.973' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f20f5eae-4ec6-4ea2-9314-1083d11c1e2c', N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'bf9eb7e3-e9ca-4c10-a5a0-37dc7c6071a5', CAST(N'2024-06-24T11:40:40.217' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'cf4081fc-6e39-4dbf-8702-126f5b97b8fd', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T09:21:25.390' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8930d349-ce87-40fe-8e8e-169209664279', N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T15:40:52.007' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'cee0312a-fc3a-44da-81cc-1b4c363a2cd1', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'7dd04903-980d-403a-9b79-80e8afce11e5', CAST(N'2024-06-25T17:02:43.983' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:11:55.340' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'44ade6b1-add5-4a26-a486-2f35b6f25617', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-26T09:21:12.917' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T09:21:13.927' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5b6f3c89-6d06-4dba-8f9d-3086adcf62eb', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:11:05.990' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:11:55.483' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'29ecb501-d4c3-4527-8160-3fcaa1edea9c', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'970ad5c7-3f15-4379-aeb2-c1e6e8317adc', CAST(N'2024-06-25T17:02:39.813' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:12:10.737' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3ec9edc0-2075-4c02-a3f8-41989644f247', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'9f09d341-88d1-44e3-8a02-e638b821c356', CAST(N'2024-06-25T17:11:52.080' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:12:12.383' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9e9760c5-5ea0-48c5-8edc-46a3d4e3b8e6', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-26T09:21:26.513' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'14073ed8-fd26-4389-b4e3-55d4f96ab97a', N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T14:58:21.677' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'510481cc-5af1-4043-860f-67672eb4833e', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'9e205d0a-4775-4c70-85db-e1c0144a3b7f', CAST(N'2024-06-25T17:11:07.383' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T17:12:13.610' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c07b1fd4-1d69-4ec7-a3bd-69ab48b26058', N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T08:59:31.010' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'19f1cc0c-bbc6-42c9-a77a-6ca1feb761dc', N'ab9b0e2a-1e38-40d0-af08-2ca84a7f6610', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T15:41:03.123' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'16a73c58-24a7-4c68-9fc9-79d4d6f600c2', N'816ae763-bb3d-4f49-8482-bd8bc4372b42', N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-25T17:01:17.470' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6bf7daf4-732a-434c-8aaf-7dc820288bf3', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T14:41:14.400' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:51:51.730' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8f0d0de0-b971-462a-8b7d-7e6a2681f1fd', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:51:55.337' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:52:44.823' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c864aa5e-dbb2-467a-bd5b-8c0d31ad9572', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'5c14f7ee-a16a-475f-b5f4-5f88063998bb', CAST(N'2024-06-25T17:11:08.280' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9e924f50-0a1a-4ff7-be3e-8d7668624690', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-26T08:51:55.957' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:52:45.887' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b1aad319-3ead-4c62-9f51-8ec94c643527', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'bf9eb7e3-e9ca-4c10-a5a0-37dc7c6071a5', CAST(N'2024-06-25T17:02:38.763' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'4af1b259-4eca-453d-ac7e-95d93975487d', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T09:21:12.197' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T09:21:14.497' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'492ee3f3-cd45-46ca-b555-97e491b8b721', N'74a3067b-b3d5-45c1-8b61-dcbb0176f9bc', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', CAST(N'2024-06-25T14:32:59.153' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'57d9effe-63e1-4aee-b54c-9962c53cb49d', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'ed9cba81-e638-42af-9198-c8575a6f9126', CAST(N'2024-06-25T17:11:07.117' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9f031060-1a84-4cb1-a76e-9b2da57d622e', N'cc1ef440-aa41-427d-82ee-76806349eb87', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T16:02:19.223' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'35605b8f-e6d8-4b4a-810b-9cddcf6e307b', N'ab9b0e2a-1e38-40d0-af08-2ca84a7f6610', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T09:02:11.167' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9097b589-c655-4e54-b250-a0165575a8c6', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'f10c308a-b941-4776-a73f-404fd3e01f55', CAST(N'2024-06-25T17:11:09.690' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'294eb153-59d9-4244-b21c-af3bb1353789', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-25T15:59:17.140' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:51:52.297' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'cdb736b6-3ff4-4a34-9273-b7a6c3f26bec', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T15:40:40.537' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:51:52.780' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'490cb977-cf3d-49ca-9f82-bc8ca6100f43', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'c8483243-beeb-481f-9ddc-7a06d0d5e001', CAST(N'2024-06-25T17:02:37.677' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'948ec2da-631d-49aa-9136-c41fcc10b593', N'ff9bf030-ff71-4ec3-8677-f3498cb07b55', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T09:02:48.263' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'161111f2-2115-42b0-a7b0-c45651a99225', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'6ab597c9-f578-44c5-b539-1fc969bc9d37', CAST(N'2024-06-25T17:11:06.403' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0576e8fc-31de-4a2a-9f13-c5efcb65fb8b', N'7b715da0-bd7a-4ddb-8c4c-3740e5bb45f9', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T16:22:41.110' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8ff1643d-1440-401d-85e4-d372bc5ecba2', N'7b715da0-bd7a-4ddb-8c4c-3740e5bb45f9', N'7dd04903-980d-403a-9b79-80e8afce11e5', CAST(N'2024-06-25T16:22:42.037' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8f7d07c3-d211-4959-9b61-defd3983a6e9', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'370bb23a-d47f-45a5-823a-367e0fd2b681', CAST(N'2024-06-25T17:11:09.203' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'64cf1e91-821b-459d-8729-e2f0d430d6b8', N'ab9b0e2a-1e38-40d0-af08-2ca84a7f6610', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-24T15:41:00.397' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1b97ec28-352b-4398-9c47-e4af030c8c49', N'0fe96260-1327-46c1-810a-d6c0670753a5', N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-25T16:20:35.587' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'4ddf0150-2fce-4205-b664-ee957f9fdd9e', N'087ab3bb-52aa-46c9-b516-496cc1a2442f', N'5c14f7ee-a16a-475f-b5f4-5f88063998bb', CAST(N'2024-06-25T17:11:08.613' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'89661bb1-627d-4e2b-8634-f3a1b1e53d6c', N'6865c5c9-325f-4dfc-832a-98aa330f17c9', N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-25T15:59:01.260' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-26T08:51:53.183' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6e967a96-7a4d-4603-9337-f425ae72257b', N'cc1ef440-aa41-427d-82ee-76806349eb87', N'7dd04903-980d-403a-9b79-80e8afce11e5', CAST(N'2024-06-25T16:24:43.647' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'70f75543-5d43-4d66-b338-006d53a62d2b', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Done', 4, CAST(N'2024-06-13T10:20:07.497' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'983d2eb6-c2ad-4e4e-84a8-0857cd61245a', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'odo', 5, CAST(N'2024-06-20T14:42:02.163' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:42:17.153' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'049a8982-5872-45ee-b98a-0a4d5047800c', N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Kick-start', 3, CAST(N'2024-06-13T10:14:51.823' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'99629448-47eb-43ae-b77a-0e81485cda17', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'Inprogress', 3, CAST(N'2024-06-21T10:55:21.457' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'55ee378e-f8d4-4dec-96e3-179344af08d0', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'aloc', 7, CAST(N'2024-06-20T14:43:00.053' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e4ddde7b-eec7-4868-a67a-242251464a67', N'888c9404-fb9c-470c-9be6-4b1962cc6ff3', N'alo', 1, CAST(N'2024-06-21T11:00:38.720' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'41a64c2e-0757-4c84-beb1-2a010e7ffb4c', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'ado', 8, CAST(N'2024-06-20T14:43:04.440' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:43:25.370' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6eeb605b-bb67-4cc0-95f7-30b783c3f695', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'Review', 4, CAST(N'2024-06-21T10:55:32.400' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'780578d9-cfeb-46ad-a964-33478a9606e7', N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Organization ', 1, CAST(N'2024-06-13T10:14:25.677' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5b50357f-2090-4e44-aaae-375881fcda6e', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'Offer sent', 4, CAST(N'2024-06-13T10:13:22.117' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a2ec04dd-36cb-41d4-aa19-39015c56a6ed', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'zo', 9, CAST(N'2024-06-21T09:18:24.213' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:18:26.517' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'95799dbf-e969-4709-a188-3d3b22d9f203', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'backlog', 1, CAST(N'2024-06-21T14:12:33.613' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1033b7f2-42ec-4de1-9b76-41257938def2', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'Todo', 1, CAST(N'2024-06-20T14:14:03.517' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3fca3101-3383-4c31-ae4c-44c3d5ca6f0d', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'To Do', 2, CAST(N'2024-06-21T10:55:04.730' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0411e8dc-9233-4bd8-b0c3-472d2a804c06', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'Incoming', 2, CAST(N'2024-06-13T10:16:34.020' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'66281925-bed6-458b-b64e-4ebcb71c7c4a', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'InProgress', 3, CAST(N'2024-06-13T10:17:44.143' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'107a6d2d-6e7c-4e2f-9b6e-5765118e7f00', N'651612db-6db8-49ce-8a72-d87138b76694', N'phong', 1, CAST(N'2024-06-20T16:50:34.233' AS DateTime), N'8da3122d-cb65-4d98-8054-a9a2e4e77446', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f0333af6-ef7c-47a0-b275-59233d74aaaa', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'Interview process', 3, CAST(N'2024-06-13T10:13:07.567' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'795b13e6-942a-482e-b88f-61ed0baaec42', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'list', 4, CAST(N'2024-06-24T14:18:51.800' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'95b88380-c634-49ff-9df7-62f82581c097', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'dev done', 3, CAST(N'2024-06-20T14:14:53.777' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:41:38.810' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3e93af4f-0211-4d8f-95d7-67ac1c29ed6c', N'bdacb475-7b87-49c0-bdf8-4df6c38e6930', N'Role', 2, CAST(N'2024-06-13T10:14:30.423' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0b0f3d01-d541-4196-8669-67b945630751', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'Backlog', 1, CAST(N'2024-06-13T10:16:20.483' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'571067e6-3f9c-4666-88bc-6b6c2e23a437', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'alo', 7, CAST(N'2024-06-21T09:08:14.243' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:08:16.760' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2a8be7cb-9347-4a7a-9f5b-6fc5de7d3670', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'alo alo', 6, CAST(N'2024-06-20T14:42:38.027' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:42:40.287' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a3e199f3-547f-4be8-81e2-7b853a8ec810', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'in progress', 2, CAST(N'2024-06-20T14:14:46.740' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:18:06.677' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c6db95a9-4510-4011-89ea-800f3548d641', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'doing ', 3, CAST(N'2024-06-21T14:12:44.817' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', CAST(N'2024-06-21T14:14:05.863' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'66874d18-37e7-4e9e-9683-80fa622244b3', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'New Applicants', 1, CAST(N'2024-06-13T10:12:07.410' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6bdb8e37-db51-483b-b5da-8e2b549cac35', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Sprint backlog', 2, CAST(N'2024-06-13T10:17:25.533' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'01f15719-2e2f-4069-b972-90c31d0bbba4', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Backlog', 1, CAST(N'2024-06-13T10:17:14.087' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'37216b6a-9669-4eff-a5f5-91c0d0bd4cd8', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Document', 1, CAST(N'2024-06-13T10:19:32.440' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'24bb9eaa-af8c-4fb4-adfb-94c774342470', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'Backlog', 1, CAST(N'2024-06-21T10:54:54.603' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e46b98d0-2950-453d-b093-950d69ca60d9', N'66032bfb-8c24-4c2f-807f-c1c4ffc31181', N'Doing', 3, CAST(N'2024-06-13T10:16:42.527' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'99e08ff6-edce-41bf-b379-9a796e53c38c', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Backlog', 2, CAST(N'2024-06-13T10:19:38.380' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'765db1a9-6ec0-4249-8dc1-9d5d95b70fdb', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'do', 6, CAST(N'2024-06-20T15:18:18.490' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:08:09.533' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'81d6fcce-5948-4a20-bc90-ba2de01a965e', N'56dbd37a-8ada-499f-8fec-01c135be0ede', N'todo', 2, CAST(N'2024-06-21T14:12:39.057' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'db15adb5-34cf-4676-89bf-c85c318b7799', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'Todo', 3, CAST(N'2024-06-13T10:19:52.350' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T15:16:44.097' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'ad52f197-88ae-4f89-b9c3-c883fdd4b7f1', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'phong', 5, CAST(N'2024-06-20T15:18:07.800' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:08:07.733' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'40a933e4-ec6d-4148-94fc-cf8b98104a90', N'bc2ae841-9204-4e32-a48e-2ebde172c96c', N'Interview schedule', 2, CAST(N'2024-06-13T10:12:42.640' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'cd5e981b-b1a1-4e47-aaf2-d3feac106b5a', N'916ae784-15f6-4d34-99a9-4ec6fbc5aa63', N'closed
', 4, CAST(N'2024-06-20T14:15:07.787' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-20T14:41:42.220' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'52bc1785-781f-495f-a9ed-e811324e64f3', N'ac3468dd-0cea-4b6b-ad38-d1cb85101c81', N'Complete', 4, CAST(N'2024-06-13T10:18:00.970' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6e3eb507-879f-437b-afae-ebde3d894a04', N'c1e58859-6b79-498c-9f50-0d930b1121b3', N'mot', 8, CAST(N'2024-06-21T09:08:57.200' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-21T09:09:03.860' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'fc26b66b-6692-4871-a0d7-f573bb7e79a3', N'ffb55629-1440-46d7-a4e1-84c99f647ac5', N'Done', 5, CAST(N'2024-06-21T10:55:35.127' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'97b6e635-2ef9-4d3b-94cb-379099884d75', N'Admin', CAST(N'2024-06-13T09:57:24.467' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e58e6c57-ebf4-4cb3-a1de-cfbc3dca7019', N'Member', CAST(N'2024-06-13T09:57:33.013' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'a69a7be6-0115-4ed5-9da9-13bb34dea4cf', N'339a369c-383c-4f59-9b8e-be5ead1c29b4', N'cân 2', CAST(N'2024-06-28T10:36:21.817' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-28T10:41:08.690' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'3cd9707b-12c5-4756-a785-2e74e5aee22b', N'339a369c-383c-4f59-9b8e-be5ead1c29b4', N'khong can 1', CAST(N'2024-06-28T10:18:16.270' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 0, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'7ce7d129-1a2a-41ea-b75d-3170b33b153e', N'2f7feac2-3136-4fba-a1eb-0fe7296eaf4e', N'todo 1', CAST(N'2024-06-28T13:55:38.537' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 0, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'e154eca5-7665-4f91-bbfd-3af6c666260e', N'da61d4a0-c9ad-4ded-a28f-185556cf9bd3', N'la la', CAST(N'2024-06-27T15:05:34.060' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 0, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'cf92cc11-78c9-40b4-9c4e-3cb9d07f77ab', N'da61d4a0-c9ad-4ded-a28f-185556cf9bd3', N'yo yo', CAST(N'2024-06-27T15:05:28.613' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 0, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'f1373861-ed29-4e5a-9692-efb5074d7099', N'2f7feac2-3136-4fba-a1eb-0fe7296eaf4e', N'Todo 2', CAST(N'2024-06-28T13:55:56.437' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-28T13:56:46.753' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1, 0)
INSERT [dbo].[Task] ([Id], [TodoId], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsChecked], [IsActive]) VALUES (N'f45eae6a-cab5-44c1-965b-f43fc9e0a69a', N'339a369c-383c-4f59-9b8e-be5ead1c29b4', N'khong can 1', CAST(N'2024-06-28T10:18:30.010' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 0, 1)
GO
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2f7feac2-3136-4fba-a1eb-0fe7296eaf4e', N'2e5863ce-d5d7-40eb-9d98-3e1a69d845e7', N'Things todo', CAST(N'2024-06-28T13:53:30.750' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-28T13:54:49.483' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'937c8384-e7b4-4c08-bb80-136ea1b40845', N'2e5863ce-d5d7-40eb-9d98-3e1a69d845e7', N'viec can lam', CAST(N'2024-06-27T15:04:32.167' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8edc144e-882b-4eb6-a4ab-1461855868cc', N'676ec6e5-f1dd-4bab-aef6-17bc21bd30fb', N'Create Service', CAST(N'2024-06-27T16:45:17.383' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'da61d4a0-c9ad-4ded-a28f-185556cf9bd3', N'2e5863ce-d5d7-40eb-9d98-3e1a69d845e7', N'yoyo wassup', CAST(N'2024-06-27T15:04:50.407' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-28T09:10:45.560' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 0)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b51269d4-611c-4103-9b98-1c4fff9a731f', N'676ec6e5-f1dd-4bab-aef6-17bc21bd30fb', N'Create API', CAST(N'2024-06-27T16:45:36.147' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'71ebcf18-e61d-4bcb-af1a-5a407dcae6bc', N'2e5863ce-d5d7-40eb-9d98-3e1a69d845e7', N'Change title', CAST(N'2024-06-28T09:08:17.093' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'339a369c-383c-4f59-9b8e-be5ead1c29b4', N'2e5863ce-d5d7-40eb-9d98-3e1a69d845e7', N'Việc không cần làm', CAST(N'2024-06-28T09:24:43.827' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', CAST(N'2024-06-28T09:25:43.817' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', 1)
GO
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9386ed34-308e-432a-bbb3-0cbf266aefb7', N'tam21@gmail.com', N'tK07UqmoaJIsGVaUOmYrBw==:IOSXXIqda9CEVoMWnW8qZVs2+uKbaoX7uXn/YaS8VKc=', N'tam', NULL, CAST(N'2024-06-17T02:11:32.667' AS DateTime), N'9386ed34-308e-432a-bbb3-0cbf266aefb7', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6ab597c9-f578-44c5-b539-1fc969bc9d37', N'tam4@gmail.com', N'cZ7CqNKcipuINQo1P17vrQ==:qtMPAqwm4xa0+j1t+mkXkVAXgs1fXJnOjPKmyON6mig=', N'tam', NULL, CAST(N'2024-06-17T02:09:38.030' AS DateTime), N'6ab597c9-f578-44c5-b539-1fc969bc9d37', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'42ddd23d-9513-4245-9e16-260a74ab9460', N'tam12@gmail.com', N'MgZL/V3J8C4+ha37mljOqw==:SZXuHdQQkqMER7Q6qAhD9pDPQGkEKvacT5eZSabe0B8=', N'tam', NULL, CAST(N'2024-06-17T02:10:14.667' AS DateTime), N'42ddd23d-9513-4245-9e16-260a74ab9460', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'df1e4e23-9547-4340-9a13-2b4a98d63576', N'phongthain@gmail.com', N'aQdVWr8+OSu6RdJJm3E2vg==:TTxIEebNW8koiLpdPfqv4Aeki+O87tp5c+qnJBZK1dA=', N'Nguyen Thai Phong ', NULL, CAST(N'2024-06-21T07:09:03.140' AS DateTime), N'df1e4e23-9547-4340-9a13-2b4a98d63576', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'370bb23a-d47f-45a5-823a-367e0fd2b681', N'tam18@gmail.com', N'HjucD6UdM8q8iMn7Js5rGw==:oXabu4rzO7lgsixJ1LzCqZArnl+FGYjGc9pTY+8NJY4=', N'tam', NULL, CAST(N'2024-06-17T02:11:14.530' AS DateTime), N'370bb23a-d47f-45a5-823a-367e0fd2b681', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'bf9eb7e3-e9ca-4c10-a5a0-37dc7c6071a5', N'phongnguyen@gmail.com', N'SQhdw0IRmyTsLTlZkJBemg==:FCfp5ENfTNoQyjzXz9dX1JZFqWbPDySK/V089UFTqr0=', N'Nguyen Thai Phong', NULL, CAST(N'2024-06-20T08:05:42.020' AS DateTime), N'bf9eb7e3-e9ca-4c10-a5a0-37dc7c6071a5', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'24d25698-3111-42d2-b640-39d0b86d481c', N'tam6@gmail.com', N'KTNa1p4Na5PYmOsjcKWHSw==:ILrEc87+nT7xmHCMbA6AmaDwO1vM/ybhGBxy5KB3aLM=', N'tam', NULL, CAST(N'2024-06-17T02:09:46.893' AS DateTime), N'24d25698-3111-42d2-b640-39d0b86d481c', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f4e3b61d-3773-4708-be9e-3d60fa03476f', N'tam3@gmail.com', N'QD1IDeiEruD9NAin9sIaLA==:eW4d/kaWBce3ZIL4JnOj6xkvmR0emhcaKhnjMCSH/s0=', N'tam', NULL, CAST(N'2024-06-17T02:09:34.153' AS DateTime), N'f4e3b61d-3773-4708-be9e-3d60fa03476f', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f10c308a-b941-4776-a73f-404fd3e01f55', N'tam1@gmail.com', N'VPFr/4fa3oxAwxqWV8fRow==:oT5YMJHjeJLy9zsvPjnZGwWMQcmHLAsdyi86UItY39A=', N'Tam 1', NULL, CAST(N'2024-06-14T02:31:55.633' AS DateTime), N'f10c308a-b941-4776-a73f-404fd3e01f55', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2ba9c4b1-046f-40b9-bccb-40999e17f184', N'phongnt@gmail.com', N'02ajJZlciZ/Pl6FCiRPqkQ==:8TFwBxbvIBxqkYFFuZ781ECgJjlhdS7zPP4elA7NT2g=', N'PhongNT', NULL, CAST(N'2024-06-13T02:53:56.113' AS DateTime), N'2ba9c4b1-046f-40b9-bccb-40999e17f184', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'eb029ad5-7704-4f78-8600-4b7c368e5617', N'phuc.hau@gmail.com', N'dNlz4YjhHU8xvsCUonQasg==:SZUFC8/BMIjuvm70mpmCTNqbPBNykNJyMkfxLXq34VE=', N'Hau Phuc', NULL, CAST(N'2024-06-21T03:49:16.847' AS DateTime), N'eb029ad5-7704-4f78-8600-4b7c368e5617', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'bfd81393-d3f0-4c65-b2ad-56bc7f443546', N'tam19@gmail.com', N'9A29vL313qC/c7InxuVkgQ==:M5fYPXIf+q5i9kE8zXWAjl/LPQjcc8DA4RsE21i1FYM=', N'tam', NULL, CAST(N'2024-06-17T02:11:18.037' AS DateTime), N'bfd81393-d3f0-4c65-b2ad-56bc7f443546', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5c14f7ee-a16a-475f-b5f4-5f88063998bb', N'tam17@gmail.com', N'd7iF9fmBoeW/mMfqUZL/2A==:85106VxFPVZQvOtZCrH2t7NqwKVbMgS4qD+ylNhsnuI=', N'tam', NULL, CAST(N'2024-06-17T02:11:10.717' AS DateTime), N'5c14f7ee-a16a-475f-b5f4-5f88063998bb', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a722b4a5-c298-44ac-a440-674f2796a9e5', N'tam2@gmail.com', N'Igl1IkWK5pNCRc7pBHpJaA==:e9jiIVMHGDEGJqgHgeWFbajH0gxruzxvQmTBbv3TIgY=', N'tam', NULL, CAST(N'2024-06-17T02:09:29.273' AS DateTime), N'a722b4a5-c298-44ac-a440-674f2796a9e5', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'aeb8003d-31fa-43c0-aea5-6777ea7f90bf', N'tam9@gmail.com', N'48X3EUNsm5ObP4TlBq1B5Q==:6CKwl3lYUYcCPePrxBJAWOaNpvCwdeGibEILjWRL09k=', N'tam', NULL, CAST(N'2024-06-17T02:10:01.803' AS DateTime), N'aeb8003d-31fa-43c0-aea5-6777ea7f90bf', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2cf1f1a8-a868-46c3-a480-68333817c77b', N'tamthanh@gmail.com', N'XwqylrpXjl6+k0JUk2tbYw==:jZ1uh2n9d857AC6ifu2qtZAlYKrlV3og1txVzu9uRrY=', N'Tam Thanh', NULL, CAST(N'2024-06-14T02:31:46.300' AS DateTime), N'2cf1f1a8-a868-46c3-a480-68333817c77b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c4148c78-12b3-4793-870f-76ace70a3571', N'tam15@gmail.com', N'us5DjYO9Vadk+/EolhAt5Q==:C+v+4HvjBwD52DeDmCMpFZf+4MOMTa2nh8HCIL1ZBnc=', N'tam', NULL, CAST(N'2024-06-17T02:11:01.807' AS DateTime), N'c4148c78-12b3-4793-870f-76ace70a3571', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c8483243-beeb-481f-9ddc-7a06d0d5e001', N'toannb@gmail.com', N'ifGlHVsn7wQZ/xFuNOOtzg==:fIOirxBImZq7IvcmSQRKnP67uqcffa+d3P8ugYF9hgw=', N'ToanNB', NULL, CAST(N'2024-06-13T02:55:43.337' AS DateTime), N'c8483243-beeb-481f-9ddc-7a06d0d5e001', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'7dd04903-980d-403a-9b79-80e8afce11e5', N'tamnt@gmail.com', N'5g4vAyI1QS9HGUHF23MVeA==:2uw9EFkkVi434jcjGxftxXvNIRkf1VRcJU+AihYsT3I=', N'TamNT', NULL, CAST(N'2024-06-13T02:54:42.957' AS DateTime), N'7dd04903-980d-403a-9b79-80e8afce11e5', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9a6a5569-53bc-4c11-924f-8321911b835b', N'tam14@gmail.com', N'CemREhfZV9INtmK6A74c4g==:+cx/jClPup0m+c+1yyY68uUADZOIfS2MvDd6vKZTK7k=', N'tam', NULL, CAST(N'2024-06-17T02:10:57.300' AS DateTime), N'9a6a5569-53bc-4c11-924f-8321911b835b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'65b09cae-4846-459d-add4-8773d3d839f9', N'tam20@gmail.com', N'Eh+bKYy4c0ybZly9OINYjA==:zWx3v5RAPVYfo5Y7TGSy2xWYoqWj6xmnMnmoBbmMyXA=', N'tam', NULL, CAST(N'2024-06-17T02:11:28.203' AS DateTime), N'65b09cae-4846-459d-add4-8773d3d839f9', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5b865918-2900-42f3-abd7-9f5ea2d187e1', N'tam10@gmail.com', N'LsPelS0ZFf45/k8mrIvatw==:2ZGPiDu6M8TzCB0ZOVN68mXWR1vgpCYqbFc26Vggee0=', N'tam', NULL, CAST(N'2024-06-17T02:10:06.473' AS DateTime), N'5b865918-2900-42f3-abd7-9f5ea2d187e1', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e2cf3d41-7366-45c4-a1d8-a10f5c5b75a6', N'tam8@gmail.com', N'SUldOQHWjgcfEftg8muLWQ==:BKHmcT+feFKawOuFJQv4SS45sCNQ7zlQvTR274B4ig0=', N'tam', NULL, CAST(N'2024-06-17T02:09:56.960' AS DateTime), N'e2cf3d41-7366-45c4-a1d8-a10f5c5b75a6', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8da3122d-cb65-4d98-8054-a9a2e4e77446', N'nguoibattu@gmail.com', N'8D1fbHX1u99xN74F0njpbw==:1/IdURh8F+e7skMpGf99bMjSjJqfqrpyeDKknPXd1OU=', N'Bat Tu', NULL, CAST(N'2024-06-20T09:21:19.090' AS DateTime), N'8da3122d-cb65-4d98-8054-a9a2e4e77446', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'13812770-7052-4af6-be7a-b6584e2cd653', N'tam13@gmail.com', N'15vs82dMj/1hcn/JMGjlPA==:q3qX7BggQBUzYxmcEJe1A4SVgfSeCSdt8T9J+Xi3CAY=', N'tam', NULL, CAST(N'2024-06-17T02:10:53.943' AS DateTime), N'13812770-7052-4af6-be7a-b6584e2cd653', NULL, N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'970ad5c7-3f15-4379-aeb2-c1e6e8317adc', N'tam5@gmail.com', N'lKPCZzwCMTmfuUrg7eGnog==:F1C3D+Dn+QkaKr4mRwM1JFsOKyhq5Q+GQdxzBalZQuM=', N'tam', NULL, CAST(N'2024-06-17T02:09:42.927' AS DateTime), N'970ad5c7-3f15-4379-aeb2-c1e6e8317adc', NULL, N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'ed9cba81-e638-42af-9198-c8575a6f9126', N'tam16@gmail.com', N'OqKNvY/fyh5xddMWo9RuxA==:8uxH3KmxrkHV8VKED3LqvnMnSNnZBhI8qCcBEFZWQKE=', N'tam', NULL, CAST(N'2024-06-17T02:11:07.760' AS DateTime), N'ed9cba81-e638-42af-9198-c8575a6f9126', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9e205d0a-4775-4c70-85db-e1c0144a3b7f', N'tam11@gmail.com', N'KC9pLOJmrsXceFTdiq67LQ==:c0k/aS9tfeyUGITlx2FnhqS125DVp/oOFMbabE5Br9A=', N'tam', NULL, CAST(N'2024-06-17T02:10:11.247' AS DateTime), N'9e205d0a-4775-4c70-85db-e1c0144a3b7f', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9f09d341-88d1-44e3-8a02-e638b821c356', N'tam7@gmail.com', N'FAiMbdUSyB21iykA/cTppA==:DEnJdKGgoR7utJS/pAcVDlqYkHuQ2fPaclleuvbU31c=', N'tam', NULL, CAST(N'2024-06-17T02:09:50.867' AS DateTime), N'9f09d341-88d1-44e3-8a02-e638b821c356', NULL, N'00000000-0000-0000-0000-000000000000', 1)
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
