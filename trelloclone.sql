USE [master]
GO
/****** Object:  Database [Trelloclone]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[Board]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[BoardMember]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[Card]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[CardActivity]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[CardLabel]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[CardMember]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[Comment]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[Label]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[List]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[Notification]    Script Date: 8/7/2024 4:37:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Body] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[Task]    Script Date: 8/7/2024 4:37:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [uniqueidentifier] NOT NULL,
	[TodoId] [uniqueidentifier] NOT NULL,
	[AssignedUserId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[PriorityLevel] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUser] [uniqueidentifier] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUser] [uniqueidentifier] NOT NULL,
	[CompletedDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[Status] [varchar](50) NOT NULL,
	[IsChecked] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDo]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 8/7/2024 4:37:48 PM ******/
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
/****** Object:  Table [dbo].[UserFcmToken]    Script Date: 8/7/2024 4:37:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFcmToken](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[FcmToken] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserFcmToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'da645e44-8132-4ac0-aeb8-24096b79c622', N'Bug Tracker', CAST(N'2024-08-07T08:58:13.077' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, NULL, 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'53533043-a91c-49d4-91ab-47cc96915754', N'Candy Shop System', CAST(N'2024-08-07T09:19:18.543' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, NULL, 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'1dcc1e09-25b4-42de-9a1c-6c8b77f5958f', N'Bug Tracker', CAST(N'2024-08-07T09:03:31.127' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, NULL, 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'50c78ccc-1280-4013-aa9c-7282d8143bb7', N'Risk Report', CAST(N'2024-08-07T09:14:47.340' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', CAST(N'2024-08-07T09:17:58.247' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'Bug Tracker', CAST(N'2024-08-07T09:06:43.417' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', CAST(N'2024-08-07T09:06:57.090' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'db0aad3f-ba11-461d-ace3-d3de4f0b4382', N'eCourse Workflow', CAST(N'2024-08-07T09:12:16.830' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', CAST(N'2024-08-07T09:14:54.540' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'4e5b1144-a1a3-4876-b10d-ed0e2fc251aa', N'Bug Tracker ', CAST(N'2024-08-07T09:03:57.117' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, NULL, 0, 0)
GO
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'31094a3b-6c60-4d3e-8b31-5466445885b2', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'db0aad3f-ba11-461d-ace3-d3de4f0b4382', N'a63d991f-3be4-4666-b370-055a2b25a3ee', CAST(N'2024-08-07T09:12:16.833' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a8ff8ed6-374c-4950-8fd1-8108bd0e1a5d', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'a63d991f-3be4-4666-b370-055a2b25a3ee', CAST(N'2024-08-07T09:06:43.423' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'94f68eef-b274-47b1-a8f2-83f288890cfe', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'50c78ccc-1280-4013-aa9c-7282d8143bb7', N'a63d991f-3be4-4666-b370-055a2b25a3ee', CAST(N'2024-08-07T09:14:47.343' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f6c11c23-7858-4867-af95-d5aaf9a7d31a', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'53533043-a91c-49d4-91ab-47cc96915754', N'a63d991f-3be4-4666-b370-055a2b25a3ee', CAST(N'2024-08-07T09:19:18.547' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c0128e34-c412-4606-90c6-dec4450eb7cc', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'53533043-a91c-49d4-91ab-47cc96915754', N'609e659a-9186-4c30-8571-c9f39adad436', CAST(N'2024-08-07T09:19:32.437' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'91d38951-aebb-4c95-8e32-28599472c3e4', N'c1b413e5-8be5-4bc9-880c-595c39b6ddce', N'Can not create list', NULL, CAST(N'2024-08-07T09:10:44.233' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'fc278cdc-a357-4f0e-9688-31c3e3124166', N'afdf5db9-bf05-4e01-bfd1-5ee8501702fe', N'Inspiration', NULL, CAST(N'2024-08-07T09:13:43.560' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'91c349b9-aa85-453b-bff2-3c216c6105a7', N'4f209d26-fdc7-49b3-a4ed-766870c5bbd6', N'Unable to open Dropdown menu', NULL, CAST(N'2024-08-07T09:11:35.733' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'd27f22d4-7cd2-45f0-9f3d-49eb3c20283a', N'bf3fc6bf-e5bc-4e1b-98af-2b06cb97695e', N'Concept', NULL, CAST(N'2024-08-07T09:13:49.443' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'a4ff370e-618b-48b1-b506-5afc82e952aa', N'c1b413e5-8be5-4bc9-880c-595c39b6ddce', N'Can not click on Create button', NULL, CAST(N'2024-08-07T09:11:18.780' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'd07e85da-4a4e-44ce-8771-6e929e4193a9', N'9143bef2-b07f-430b-8c64-bb0386d2bb2d', N'Inactive user failed', NULL, CAST(N'2024-08-07T09:10:08.703' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'5ac1a182-b0e7-4245-8372-7c24d31035dc', N'28a751f3-9eb8-4fbe-9454-a7bed44b10a3', N'Mood Board ', NULL, CAST(N'2024-08-07T09:14:14.593' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'9a3f62f4-11a2-4f7c-830e-7dca9f3c74b1', N'f676721f-165b-4ebf-97f8-f0874531e631', N'Create SRS document', NULL, CAST(N'2024-08-07T09:21:28.177' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'fce33720-e661-441b-b925-8b1af9b775e9', N'7ce73b4d-21d5-42dc-8b37-7ff7d3a4d367', N'Create Login service and API', N'<p>Create <strong>Login </strong>api and handle service</p>', CAST(N'2024-08-07T09:20:50.193' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:31:29.323' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, CAST(N'2024-08-09T09:20:00.000' AS DateTime), NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'6010fe2e-4864-4c6f-926c-8fc2962a14fa', N'28a751f3-9eb8-4fbe-9454-a7bed44b10a3', N'Branding checklist', NULL, CAST(N'2024-08-07T09:14:07.083' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'3c59a25b-e000-4fc8-ae4c-96bc952f3863', N'98de138b-f908-4b63-9e62-352a76d90808', N'Login Error', NULL, CAST(N'2024-08-07T09:08:39.213' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'7dd04196-c717-4cea-8c95-add33a38b5fd', N'43a021ef-7749-45cb-8d72-22e4a077589b', N'Social Media', NULL, CAST(N'2024-08-07T09:14:26.460' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'8379a4f4-c9dc-4a9b-b280-b7e2a22ec9bc', N'afdf5db9-bf05-4e01-bfd1-5ee8501702fe', N'Progress report', NULL, CAST(N'2024-08-07T09:13:33.217' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'45766df5-87c3-4308-8553-c13205e8adeb', N'bf3fc6bf-e5bc-4e1b-98af-2b06cb97695e', N'Assests', NULL, CAST(N'2024-08-07T09:13:58.747' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'e30ce0e5-5d81-432e-9345-c6dde3339204', N'bf3fc6bf-e5bc-4e1b-98af-2b06cb97695e', N'Title', NULL, CAST(N'2024-08-07T09:13:53.067' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'2a5df82a-ee66-4c9d-a67d-c99dfe1b040e', N'9143bef2-b07f-430b-8c64-bb0386d2bb2d', N'Update card error', NULL, CAST(N'2024-08-07T09:09:50.913' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'f676721f-165b-4ebf-97f8-f0874531e631', N'Create Registration service and API', NULL, CAST(N'2024-08-07T09:21:04.507' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'5e44617b-2a06-43f6-bd93-e52102dbc61a', N'98de138b-f908-4b63-9e62-352a76d90808', N'Unable to view notifications', NULL, CAST(N'2024-08-07T09:09:22.977' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'23138cfe-32ab-488a-88b7-e9ee582443ba', N'98de138b-f908-4b63-9e62-352a76d90808', N'Can not create new board ', NULL, CAST(N'2024-08-07T09:08:48.497' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'58e23342-4c6c-4877-a845-ea4060bcefc6', N'336b4b7f-27a2-4ed1-9d0b-5b24489b38fd', N'Create board member failed', NULL, CAST(N'2024-08-07T09:10:28.720' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'cea4f6fa-30b8-444a-a2ec-fac8a93475bb', N'98de138b-f908-4b63-9e62-352a76d90808', N'Failed to load user ', NULL, CAST(N'2024-08-07T09:08:58.193' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c512868c-a591-4278-ab72-01c8450740cd', N'5e44617b-2a06-43f6-bd93-e52102dbc61a', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Reported', CAST(N'2024-08-07T16:09:22.977' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c1651477-52fa-4ee7-9be5-1075daa48880', N'8379a4f4-c9dc-4a9b-b280-b7e2a22ec9bc', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Resources', CAST(N'2024-08-07T16:13:33.220' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'd4a3f0b3-9f03-4e29-99ae-1109e555d146', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Add todo list Handle Services to this card', CAST(N'2024-08-07T16:26:16.657' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2ece322a-054b-463b-b41b-135316fc0b0c', N'cea4f6fa-30b8-444a-a2ec-fac8a93475bb', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Reported', CAST(N'2024-08-07T16:08:58.193' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0b44166c-8753-42b5-8563-14040cf4e75e', N'9a3f62f4-11a2-4f7c-830e-7dca9f3c74b1', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Add this card to Backlog', CAST(N'2024-08-07T16:21:28.177' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'41727644-28cd-444b-a60b-1760dcdb48f5', N'6010fe2e-4864-4c6f-926c-8fc2962a14fa', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Branding', CAST(N'2024-08-07T16:14:07.083' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3aa56570-99fe-4822-8b06-1f4d6873de8c', N'e30ce0e5-5d81-432e-9345-c6dde3339204', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Planning & Strategy', CAST(N'2024-08-07T16:13:53.067' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a0ddca6c-2cad-450f-8502-2cf78ba1a5d3', N'5ac1a182-b0e7-4245-8372-7c24d31035dc', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Branding', CAST(N'2024-08-07T16:14:14.593' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'68c9063a-2d5c-485d-929f-2d26a69e9ad1', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Moved this card from In Progress to Review', CAST(N'2024-08-07T16:30:49.083' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'51784c9a-b7fc-4574-8df6-4f9f5b38e986', N'fce33720-e661-441b-b925-8b1af9b775e9', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Joined this card', CAST(N'2024-08-07T16:21:54.447' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5ba85867-2b2d-430f-96ac-4fe7ca4c4f51', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Moved this card from Review to Done', CAST(N'2024-08-07T16:31:29.323' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8b55781c-90ad-4dcc-b3a2-56e23b43efbd', N'fc278cdc-a357-4f0e-9688-31c3e3124166', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Resources', CAST(N'2024-08-07T16:13:43.560' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9f08df4f-ce4c-4671-bd61-5c2f410a6691', N'23138cfe-32ab-488a-88b7-e9ee582443ba', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Reported', CAST(N'2024-08-07T16:08:48.497' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5584f4d3-2faa-4c0b-8305-5c82f031b88f', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Joined this card', CAST(N'2024-08-07T16:32:02.527' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9b10103b-03fc-48b4-8ffd-601541491478', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Completed Implement JWT  on this card', CAST(N'2024-08-07T16:30:24.997' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'92515ff3-7442-4616-99a2-7074947b451e', N'91d38951-aebb-4c95-8e32-28599472c3e4', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Confirm Fix', CAST(N'2024-08-07T16:10:44.233' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1a196354-c093-43f8-8fb4-73320d499766', N'd27f22d4-7cd2-45f0-9f3d-49eb3c20283a', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Planning & Strategy', CAST(N'2024-08-07T16:13:49.443' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b38a02fd-0310-41d7-8cf0-7bb2c5c36592', N'58e23342-4c6c-4877-a845-ea4060bcefc6', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to In Progress', CAST(N'2024-08-07T16:10:28.720' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1e3518e4-407f-48ab-abc5-7deee665747a', N'd07e85da-4a4e-44ce-8771-6e929e4193a9', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Planned', CAST(N'2024-08-07T16:10:08.703' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'd700478f-541f-4210-87be-95911fbf0f08', N'91c349b9-aa85-453b-bff2-3c216c6105a7', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Done', CAST(N'2024-08-07T16:11:35.733' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'd47b6aa9-61d0-4c17-867e-abd31adb3eea', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Joined this card', CAST(N'2024-08-07T16:21:50.253' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a46177d1-de3a-439d-b643-adf3cfb8dd9b', N'45766df5-87c3-4308-8553-c13205e8adeb', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Planning & Strategy', CAST(N'2024-08-07T16:13:58.747' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'ef62defd-cb4a-4c37-a6d7-bd7b66d3b924', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Moved this card from Backlog to In Progress', CAST(N'2024-08-07T16:24:54.370' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5f21c31f-f70a-4c09-ba74-c5afb893913a', N'a4ff370e-618b-48b1-b506-5afc82e952aa', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Confirm Fix', CAST(N'2024-08-07T16:11:18.780' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'01ac1841-738b-4269-a9da-cafdb7e51733', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Add todo list Create API to this card', CAST(N'2024-08-07T16:28:49.763' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'cc223f9c-4d5f-4395-a9ca-d9c8c7c9287c', N'7dd04196-c717-4cea-8c95-add33a38b5fd', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Launching', CAST(N'2024-08-07T16:14:26.460' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'225410a5-0b2b-41a2-b423-dd64e19863a6', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Add this card to Backlog', CAST(N'2024-08-07T16:21:04.507' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'fb4382e1-7b73-4f85-b0f1-e5bed0919b80', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Completed Implement hash password using SHA256 on this card', CAST(N'2024-08-07T16:30:42.527' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b3efed5e-717e-47bd-93cd-eae1ee4a1a82', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Removed todo list Create API from this card', CAST(N'2024-08-07T16:29:34.420' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9f4d1251-d7ad-4abe-8a05-ed5e05fd96cd', N'2a5df82a-ee66-4c9d-a67d-c99dfe1b040e', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Planned', CAST(N'2024-08-07T16:09:50.913' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'3f94daa6-d15e-4689-adc4-f2b15989114a', N'3c59a25b-e000-4fc8-ae4c-96bc952f3863', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Add this card to Reported', CAST(N'2024-08-07T16:08:39.233' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardActivity] ([Id], [CardId], [UserId], [Activity], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'4453926c-b94a-4a05-9216-fc70fb5cb6ca', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Add this card to Backlog', CAST(N'2024-08-07T16:20:50.193' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[CardLabel] ([Id], [CardId], [LabelId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a78796a6-2fd4-4b43-9caa-3cce09bfaaa2', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'e393f330-4949-4fa3-9e71-7f33dab7df0b', CAST(N'2024-08-07T16:33:34.910' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardLabel] ([Id], [CardId], [LabelId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'8be2de87-8123-4478-ba4f-a2ddcfcb33af', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'a0d6e10d-168b-4bfa-8e4e-c7a6c7a9e606', CAST(N'2024-08-07T16:33:44.830' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'7f7a0f4b-5c95-4201-8a75-92d3461c1a11', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', CAST(N'2024-08-07T09:32:02.000' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b238a3e1-0eb2-409f-b22b-db05401acbae', N'fce33720-e661-441b-b925-8b1af9b775e9', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', CAST(N'2024-08-07T09:21:54.093' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[CardMember] ([Id], [CardId], [UserId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5fbe24a3-a591-4139-82d2-fe0d436b536c', N'fce33720-e661-441b-b925-8b1af9b775e9', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:21:49.967' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Comment] ([Id], [CardId], [UserId], [Content], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'06d40a87-a41b-4d0c-a221-47a634b00e27', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'<p>Hi Phong, can you handle this task</p>', CAST(N'2024-08-07T09:32:20.933' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[Comment] ([Id], [CardId], [UserId], [Content], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'dc069928-5ae1-46ba-a7ca-cbb9bf0e92c6', N'2ffe9ae1-130d-4949-b208-d602e69b10a7', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'<p>It is barely simple for you, i believe you can handle it easily</p>', CAST(N'2024-08-07T09:33:07.727' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Label] ([Id], [BoardId], [Name], [Color], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'e393f330-4949-4fa3-9e71-7f33dab7df0b', N'53533043-a91c-49d4-91ab-47cc96915754', N'Impact: Medium', N'#f5cd47', CAST(N'2024-08-07T16:33:34.877' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[Label] ([Id], [BoardId], [Name], [Color], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a0d6e10d-168b-4bfa-8e4e-c7a6c7a9e606', N'53533043-a91c-49d4-91ab-47cc96915754', N'Priority: Low', N'#4bce97', CAST(N'2024-08-07T16:33:44.823' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'43a021ef-7749-45cb-8d72-22e4a077589b', N'db0aad3f-ba11-461d-ace3-d3de4f0b4382', N'Launching', 4, CAST(N'2024-08-07T09:13:19.503' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'bf3fc6bf-e5bc-4e1b-98af-2b06cb97695e', N'db0aad3f-ba11-461d-ace3-d3de4f0b4382', N'Planning & Strategy', 2, CAST(N'2024-08-07T09:12:53.367' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'26aea120-1c8a-42a6-b661-2b7d3562e55e', N'50c78ccc-1280-4013-aa9c-7282d8143bb7', N'Medium Risk Level', 2, CAST(N'2024-08-07T09:15:25.327' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'98de138b-f908-4b63-9e62-352a76d90808', N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'Reported', 1, CAST(N'2024-08-07T09:07:26.543' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a82b12fa-e2c0-4823-bcc1-35fc9fa0ec8f', N'50c78ccc-1280-4013-aa9c-7282d8143bb7', N'High Risk Level', 1, CAST(N'2024-08-07T09:15:15.813' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c1b413e5-8be5-4bc9-880c-595c39b6ddce', N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'Confirm Fix', 4, CAST(N'2024-08-07T09:07:57.193' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'336b4b7f-27a2-4ed1-9d0b-5b24489b38fd', N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'In Progress', 3, CAST(N'2024-08-07T09:07:49.403' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'afdf5db9-bf05-4e01-bfd1-5ee8501702fe', N'db0aad3f-ba11-461d-ace3-d3de4f0b4382', N'Resources', 1, CAST(N'2024-08-07T09:12:34.180' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'aa16f1cf-2a25-41ee-8ce7-6ad31a3fcdb8', N'50c78ccc-1280-4013-aa9c-7282d8143bb7', N'Low Risk Level', 3, CAST(N'2024-08-07T09:15:29.440' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1f5e80b5-345b-49ab-8b5d-7041f1d06231', N'53533043-a91c-49d4-91ab-47cc96915754', N'Review', 3, CAST(N'2024-08-07T09:20:15.880' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'4f209d26-fdc7-49b3-a4ed-766870c5bbd6', N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'Done', 5, CAST(N'2024-08-07T09:08:00.257' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'7ce73b4d-21d5-42dc-8b37-7ff7d3a4d367', N'53533043-a91c-49d4-91ab-47cc96915754', N'Done', 4, CAST(N'2024-08-07T09:20:19.863' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'28a751f3-9eb8-4fbe-9454-a7bed44b10a3', N'db0aad3f-ba11-461d-ace3-d3de4f0b4382', N'Branding', 3, CAST(N'2024-08-07T09:13:01.020' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'9143bef2-b07f-430b-8c64-bb0386d2bb2d', N'82b62d15-69b8-4507-b583-93ce89ca3eae', N'Planned', 2, CAST(N'2024-08-07T09:07:33.307' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'57d91c48-b27a-44f7-9b7b-d2a7f97c9013', N'50c78ccc-1280-4013-aa9c-7282d8143bb7', N'Under Evaluation', 4, CAST(N'2024-08-07T09:15:45.140' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'f676721f-165b-4ebf-97f8-f0874531e631', N'53533043-a91c-49d4-91ab-47cc96915754', N'Backlog', 1, CAST(N'2024-08-07T09:19:56.010' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2c92abc7-e37e-4a92-bc9a-fac357327d3e', N'53533043-a91c-49d4-91ab-47cc96915754', N'In Progress', 2, CAST(N'2024-08-07T09:20:06.153' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'19f83aba-1360-4105-bfe6-06d04ce2f06c', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Added to a new card', N'
You have been added to the card: Create Registration service and API.', CAST(N'2024-08-07T09:32:02.000' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'6a25af3a-fef5-4abc-85b4-267d7e9ca494', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Task Checked', N'
Implement hash password using SHA256  has been checked!', CAST(N'2024-08-07T09:30:42.053' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'95cb11b4-1eb2-4508-aeff-2aba38a26dc5', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Added to a new card', N'
You have been added to the card: Create Login service and API.', CAST(N'2024-08-07T09:21:54.093' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'ee584fb2-0863-438c-9ff6-30c2d4a41013', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Task Reminder', N'Reminder: The task ''Implement hash password using SHA256'' is due tomorrow.', CAST(N'2024-08-07T09:36:09.360' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'bca4be62-b922-4529-980d-5786ce4b8bb5', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Invited to a new board', N'
You have been invited to the board: Candy Shop System.', CAST(N'2024-08-07T09:19:32.440' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'a11eb85c-618a-4f29-8bb9-622bcc99193c', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Assigned to a new task', N'
You have assigned to the task: Implement JWT .', CAST(N'2024-08-07T09:28:31.520' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'416f3c16-89f8-4fca-9ae3-757b1669929e', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Task Checked', N'
Implement JWT   has been checked!', CAST(N'2024-08-07T09:30:24.847' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'34cae5ac-0d5a-4470-800e-7a4c73d2c956', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Task Reminder', N'Reminder: The task ''Implement JWT '' is due tomorrow.', CAST(N'2024-08-07T09:36:09.013' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'7f77c4a4-8ac0-4f76-827b-a82037c7ecd8', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Task Updated', N'
Implement hash password using SHA256  has been updated!', CAST(N'2024-08-07T09:30:12.017' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'35829aa1-4704-4cb3-a9d8-c52736324dec', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Added to a new card', N'
You have been added to the card: Create Login service and API.', CAST(N'2024-08-07T09:21:49.977' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'64c96793-d3f1-43a6-aa83-d544f481b4f1', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Task Updated', N'
Implement hash password using SHA256  has been updated!', CAST(N'2024-08-07T09:30:15.703' AS DateTime), NULL, 0)
INSERT [dbo].[Notification] ([Id], [UserId], [Title], [Body], [CreatedDate], [UpdatedDate], [IsRead]) VALUES (N'8f77792e-7597-4807-8362-f298a5b1095a', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Assigned to a new task', N'
You have assigned to the task: Implement hash password using SHA256.', CAST(N'2024-08-07T09:27:36.970' AS DateTime), NULL, 0)
GO
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a63d991f-3be4-4666-b370-055a2b25a3ee', N'Admin', CAST(N'2024-08-07T09:05:42.340' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'609e659a-9186-4c30-8571-c9f39adad436', N'Member', CAST(N'2024-08-07T09:05:56.243' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Task] ([Id], [TodoId], [AssignedUserId], [Name], [Description], [PriorityLevel], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [CompletedDate], [DueDate], [Status], [IsChecked], [IsActive]) VALUES (N'23a373c7-f7ff-4d6b-8559-6b17e7c5d51f', N'1a5bfc1b-700c-46cb-8b7b-395fe062359f', NULL, N'Create Login API', NULL, N'High', CAST(N'2024-08-07T09:29:13.687' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, N'New', 0, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [AssignedUserId], [Name], [Description], [PriorityLevel], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [CompletedDate], [DueDate], [Status], [IsChecked], [IsActive]) VALUES (N'bf48b031-f9c4-4546-b915-799570ed2f9b', N'c4f6d0b7-869f-4a19-a7d6-cfeb7adf84ab', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'Implement JWT ', N'Implement JWT', N'High', CAST(N'2024-08-07T09:28:31.517' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:30:24.837' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:30:24.837' AS DateTime), CAST(N'2024-08-08T00:00:00.000' AS DateTime), N'Resolved', 1, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [AssignedUserId], [Name], [Description], [PriorityLevel], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [CompletedDate], [DueDate], [Status], [IsChecked], [IsActive]) VALUES (N'8b40f39e-aa4a-4519-a8de-bb2af5c03a4a', N'c4f6d0b7-869f-4a19-a7d6-cfeb7adf84ab', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'Implement hash password using SHA256', N'Implement hash password using SHA256 for sercurity', N'High', CAST(N'2024-08-07T09:27:36.947' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:30:42.050' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:30:42.050' AS DateTime), CAST(N'2024-08-08T00:00:00.000' AS DateTime), N'Resolved', 1, 1)
INSERT [dbo].[Task] ([Id], [TodoId], [AssignedUserId], [Name], [Description], [PriorityLevel], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [CompletedDate], [DueDate], [Status], [IsChecked], [IsActive]) VALUES (N'76f3b2c7-8e8b-4a1b-90dd-f29cf5e1320f', N'c4f6d0b7-869f-4a19-a7d6-cfeb7adf84ab', NULL, N'Create second section of SRS', NULL, N'Medium', CAST(N'2024-08-07T09:31:15.967' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:31:20.870' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, NULL, N'New', 0, 0)
GO
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'1a5bfc1b-700c-46cb-8b7b-395fe062359f', N'fce33720-e661-441b-b925-8b1af9b775e9', N'Create API', CAST(N'2024-08-07T09:28:49.763' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', CAST(N'2024-08-07T09:29:34.420' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', 0)
INSERT [dbo].[ToDo] ([Id], [CardId], [Title], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'c4f6d0b7-869f-4a19-a7d6-cfeb7adf84ab', N'fce33720-e661-441b-b925-8b1af9b775e9', N'Handle Services', CAST(N'2024-08-07T09:26:16.647' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'phongnt@gmail.com', N'szG1d8pn0+7J0Ujgg1NudA==:/m1ZWw3PNaOlKLBNqibvuWovLwkXEAX67RzzqBBJKQQ=', N'PhongNT', NULL, CAST(N'2024-08-07T08:56:48.907' AS DateTime), N'a47ae073-1df7-42bd-94c1-5d4935881ba8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'toannguyen@gmail.com', N'VPFILFmSK+d5O9qI+vXaYA==:OzTx9BgV0P70OLZ0kDxu5Q3+3IVHczDpn/IQ9wc+rrY=', N'Bao Toan', NULL, CAST(N'2024-08-07T09:17:18.237' AS DateTime), N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[UserFcmToken] ([Id], [UserId], [FcmToken], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (N'78011b3f-2f7f-4781-abef-0a738a167d4a', N'27e09f64-15ed-487c-a7e1-93f7ab6bd42b', N'fyTqDhUyPnhu3qDQ3-9wLp:APA91bFUP4wLIE15OzJ7cuxtO93ipd2H5IcG5zfJtal23r5jiejqB4R5gcr-Ebn9AXk8_NRbb2H30oo-Wkyx_iegxBdji015F5BHsZmbOuVzFZiQ04ILkdZdQeTfywIe_Du1IsSuSgIS', CAST(N'2024-08-07T09:17:18.307' AS DateTime), NULL, 1)
INSERT [dbo].[UserFcmToken] ([Id], [UserId], [FcmToken], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (N'ca188b8f-b151-412a-bf0f-6639a8313747', N'a47ae073-1df7-42bd-94c1-5d4935881ba8', N'd-w6lnkxvy0ejE9bng2CCV:APA91bHQRjwfva2GJE3Y2W1xxn_HkdajT4KGsAfm7fgGCgozO9Pc2fY36ePWXwJi-uODN53HJlqvxT9oteuskE503uDME3dr99HcUSgNa6rco7mMsL8HawRKPImjgAPUvvLxkQ4iq0J2', CAST(N'2024-08-07T08:56:49.157' AS DateTime), NULL, 1)
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
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_User]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_ToDo] FOREIGN KEY([TodoId])
REFERENCES [dbo].[ToDo] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_ToDo]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_User] FOREIGN KEY([AssignedUserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_User]
GO
ALTER TABLE [dbo].[ToDo]  WITH CHECK ADD  CONSTRAINT [FK_ToDo_Card] FOREIGN KEY([CardId])
REFERENCES [dbo].[Card] ([Id])
GO
ALTER TABLE [dbo].[ToDo] CHECK CONSTRAINT [FK_ToDo_Card]
GO
ALTER TABLE [dbo].[UserFcmToken]  WITH CHECK ADD  CONSTRAINT [FK_UserFcmToken_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserFcmToken] CHECK CONSTRAINT [FK_UserFcmToken_User]
GO
USE [master]
GO
ALTER DATABASE [Trelloclone] SET  READ_WRITE 
GO
