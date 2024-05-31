USE [master]
GO
/****** Object:  Database [Trelloclone]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[Board]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[BoardMember]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[Card]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[CardActivity]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[CardLabel]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[CardMember]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[Comment]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[Label]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[List]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[Task]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[ToDo]    Script Date: 5/31/2024 10:28:56 AM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 5/31/2024 10:28:56 AM ******/
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
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'ca11839e-cbeb-4383-8d9b-0494a1857093', N'Board 2', CAST(N'2024-05-28T10:32:27.933' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T08:15:48.770' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'3fc0df58-0612-4155-88fb-068b4d3cf5d6', N'Phong Board', CAST(N'2024-05-29T10:26:43.013' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-29T16:22:21.367' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'c7eaaac1-6376-45c0-95b2-3b8eb1fb048e', N'Board-P3', CAST(N'2024-05-24T09:04:03.150' AS DateTime), N'82873cb2-8e4f-4791-b532-643fbe7db7e8', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'c4afe8f2-0a43-4426-871e-3cfa652b4d4c', N'Board-P1', CAST(N'2024-05-24T09:03:37.963' AS DateTime), N'7650966b-b192-4118-b6d1-11b950ca6969', NULL, NULL, 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'96bd8fe8-f0af-4427-a86e-727f35d548d0', N'Astrox', CAST(N'2024-05-30T14:52:15.717' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'6dfb67e5-ed90-4e14-83ce-94d1fdef8ce1', N'Board-P2', CAST(N'2024-05-24T09:03:53.203' AS DateTime), N'18483c35-3a2f-49e1-979c-84a5d0250713', NULL, NULL, 0, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'43a1661e-f38e-49a6-9105-9d1f364e63f9', N'Toan Boa', CAST(N'2024-05-29T10:28:22.730' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'e05acbdb-f134-45eb-805f-bfa8bdf8b100', N'board4', CAST(N'2024-05-28T10:49:41.320' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-29T16:00:43.173' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'6332bf52-7eac-4dc0-bd6b-cd8407452cce', N'Board 3', CAST(N'2024-05-28T10:33:39.533' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-29T15:54:51.037' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'cd5f865a-d897-44d9-8bb2-dc80deaf5c14', N'Board 22', CAST(N'2024-05-30T09:25:44.683' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, NULL, 1, 1)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'84392df2-3f94-457f-b226-e323992e09db', N'Bỏad1', CAST(N'2024-05-23T16:54:54.320' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, NULL, 0, 0)
INSERT [dbo].[Board] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsPublic], [IsActive]) VALUES (N'7931a89a-3b2e-4cfd-b113-e45ffff30fef', N'Board 1', CAST(N'2024-05-28T09:24:07.220' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-29T16:23:37.720' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1, 1)
GO
INSERT [dbo].[BoardMember] ([Id], [UserId], [BoardId], [RoleId], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'994ee499-e631-4a5b-a381-3ac4ca38e74e', N'84376a34-9987-40fa-abe2-88b08d75c4d6', N'84392df2-3f94-457f-b226-e323992e09db', N'4f7e6075-f1a1-40ac-be6f-339122eed920', CAST(N'2024-05-27T16:06:42.817' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'd54e6055-8011-400f-b1d9-14d5829304e4', N'ee1d3784-ab4d-47e8-ab78-5429fa010583', N'Card 1', NULL, CAST(N'2024-05-31T02:52:10.767' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'54c7af98-b687-4e7f-8881-262aeae0abeb', N'2f876d24-1040-4c28-813d-7785b5c4f4c2', N'test', N'4 484 84 51 48 515', CAST(N'2024-05-24T07:43:42.507' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-24T15:45:02.927' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, CAST(N'2024-05-24T00:00:00.000' AS DateTime), CAST(N'2024-05-27T00:00:00.000' AS DateTime), 0)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'e8379cfb-683b-4d34-85fa-7424928b3717', N'ee1d3784-ab4d-47e8-ab78-5429fa010583', N'Card 2', N'alo 123', CAST(N'2024-05-31T02:52:13.367' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-31T09:55:34.650' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-31T00:00:00.000' AS DateTime), CAST(N'2024-06-01T00:00:00.000' AS DateTime), CAST(N'2024-06-01T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'd95bc3a9-2e8e-44a9-a9aa-7ff0e0f9e222', N'2f876d24-1040-4c28-813d-7785b5c4f4c2', N'card1', NULL, CAST(N'2024-05-23T10:00:15.383' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
INSERT [dbo].[Card] ([Id], [ListId], [Title], [Description], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [StartDate], [EndDate], [ReminderDate], [IsActive]) VALUES (N'634694c5-4068-45e1-8fa5-ee18ae7bd1b1', N'2f876d24-1040-4c28-813d-7785b5c4f4c2', N'Card2', NULL, CAST(N'2024-05-23T10:05:16.313' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'0431b60d-c3d5-45ad-b1f5-1bb8943e90f0', N'96bd8fe8-f0af-4427-a86e-727f35d548d0', N'100zz', 2, CAST(N'2024-05-30T14:54:54.273' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T14:57:55.267' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'5004765d-3bbb-43ed-a0fd-364170b89810', N'84392df2-3f94-457f-b226-e323992e09db', N'list 3', 3, CAST(N'2024-05-30T14:35:29.140' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'ca29c4eb-8d65-4347-9c9c-52c51cb907e7', N'7931a89a-3b2e-4cfd-b113-e45ffff30fef', N'List1', 2, CAST(N'2024-05-30T10:49:36.870' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T14:20:52.263' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'ee1d3784-ab4d-47e8-ab78-5429fa010583', N'96bd8fe8-f0af-4427-a86e-727f35d548d0', N'88d pro', 1, CAST(N'2024-05-30T14:55:36.423' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T14:57:27.227' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'6f1de357-236e-463b-8f3a-6ca42a1541cf', N'84392df2-3f94-457f-b226-e323992e09db', N'List2', 1, CAST(N'2024-05-30T10:49:56.333' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T14:32:24.060' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'2f876d24-1040-4c28-813d-7785b5c4f4c2', N'84392df2-3f94-457f-b226-e323992e09db', N'List 1', 2, CAST(N'2024-05-23T16:55:24.410' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T14:32:24.060' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1)
INSERT [dbo].[List] ([Id], [BoardId], [Name], [Position], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'7d3b5313-b891-436f-95eb-bf7412c0012e', N'7931a89a-3b2e-4cfd-b113-e45ffff30fef', N'List 222', 1, CAST(N'2024-05-30T10:27:51.273' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', CAST(N'2024-05-30T14:33:14.417' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', 1)
GO
INSERT [dbo].[Role] ([Id], [Name], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'4f7e6075-f1a1-40ac-be6f-339122eed920', N'Member', CAST(N'2024-05-27T15:49:01.093' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', 1)
GO
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'7650966b-b192-4118-b6d1-11b950ca6969', N'phong1@gmail.com', N'KpJl4ds6nfqV2qW3Od37Tw==:uqzle1LR/xBpj/dTxc0tHfPwJa+T4TcCVzwW0HbRWhM=', N'phong1', NULL, CAST(N'2024-05-24T01:54:41.720' AS DateTime), N'7650966b-b192-4118-b6d1-11b950ca6969', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'82873cb2-8e4f-4791-b532-643fbe7db7e8', N'phong3@gmail.com', N'4Brfks1c/v1n+RHciTg0WQ==:2AGaFVO7XRdCBGh+2NpTvPXPU60xEQkKs5/mO39Zj2Y=', N'phong3', NULL, CAST(N'2024-05-24T01:54:54.893' AS DateTime), N'82873cb2-8e4f-4791-b532-643fbe7db7e8', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'18483c35-3a2f-49e1-979c-84a5d0250713', N'phong2@gmail.com', N'vNBfwDrB9aB/LhkPo3gGhA==:bjTBTbbXe7YlYUOG8OLG4NlM6+8rbzo67swaNrSbLvE=', N'phong2', NULL, CAST(N'2024-05-24T01:54:49.660' AS DateTime), N'18483c35-3a2f-49e1-979c-84a5d0250713', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'84376a34-9987-40fa-abe2-88b08d75c4d6', N'phongnguyen@gmail.com', N'loSnUcAMMy2G+npo1C+JaQ==:8vFvD6qv6ZQ6BsWtGMzQtfUM+L9TthP5YFNGOKc2YKk=', N'PhongNguyen', NULL, CAST(N'2024-05-24T03:56:35.093' AS DateTime), N'84376a34-9987-40fa-abe2-88b08d75c4d6', NULL, N'00000000-0000-0000-0000-000000000000', 0)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'b47db9ff-89a2-448c-8146-d5dc4234ee54', N'phong@gmail.com', N'exJv7u0SjP8bSzegJRQKYA==:mVYuUsYB2bLWW6kI0oT2WwQnix4/pXHSViqWUL89I5c=', N'Phong', NULL, CAST(N'2024-05-23T09:54:04.220' AS DateTime), N'b47db9ff-89a2-448c-8146-d5dc4234ee54', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'92a29371-009f-42d6-a857-e9f3fd34fb53', N'toan@gmail.com', N'rE0sL6Yjkojf3FEY0WBI0w==:TQcbZTLutv7fGAv9XKB8O4z7hB0dWYA8aVOFwpSg5II=', N'Toan', NULL, CAST(N'2024-05-29T02:59:59.123' AS DateTime), N'92a29371-009f-42d6-a857-e9f3fd34fb53', NULL, N'00000000-0000-0000-0000-000000000000', 1)
INSERT [dbo].[User] ([Id], [Email], [Password], [Name], [Gender], [CreatedDate], [CreatedUser], [UpdatedDate], [UpdatedUser], [IsActive]) VALUES (N'952f69a1-b984-487f-8605-ead2328b8072', N'toan1@gmail.com', N'nDMkgtsmH/GB5Ie0NlL/TQ==:IHO8BFa3damLoGb66qn8TG7W9mwkl9Vpgvvysp2lkaw=', N'Toan', NULL, CAST(N'2024-05-29T03:14:43.130' AS DateTime), N'952f69a1-b984-487f-8605-ead2328b8072', NULL, N'00000000-0000-0000-0000-000000000000', 1)
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
