USE [evv]
GO
ALTER TABLE [dbo].[Stopovers] DROP CONSTRAINT [FK_Stopovers_Journeys]
GO
ALTER TABLE [dbo].[Stopovers] DROP CONSTRAINT [FK_Stopovers_Categories1]
GO
ALTER TABLE [dbo].[Journeys] DROP CONSTRAINT [FK_Journeys_Users1]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/6/2023 10:50:07 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Stopovers]    Script Date: 3/6/2023 10:50:07 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Stopovers]') AND type in (N'U'))
DROP TABLE [dbo].[Stopovers]
GO
/****** Object:  Table [dbo].[Journeys]    Script Date: 3/6/2023 10:50:07 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Journeys]') AND type in (N'U'))
DROP TABLE [dbo].[Journeys]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3/6/2023 10:50:07 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
DROP TABLE [dbo].[Categories]
GO
USE [master]
GO
/****** Object:  Database [evv]    Script Date: 3/6/2023 10:50:07 AM ******/
DROP DATABASE [evv]
GO
/****** Object:  Database [evv]    Script Date: 3/6/2023 10:50:07 AM ******/
CREATE DATABASE [evv]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'evv', FILENAME = N'C:\Users\kubil\evv.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'evv_log', FILENAME = N'C:\Users\kubil\evv_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [evv] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [evv].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [evv] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [evv] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [evv] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [evv] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [evv] SET ARITHABORT OFF 
GO
ALTER DATABASE [evv] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [evv] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [evv] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [evv] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [evv] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [evv] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [evv] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [evv] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [evv] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [evv] SET  DISABLE_BROKER 
GO
ALTER DATABASE [evv] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [evv] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [evv] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [evv] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [evv] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [evv] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [evv] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [evv] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [evv] SET  MULTI_USER 
GO
ALTER DATABASE [evv] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [evv] SET DB_CHAINING OFF 
GO
ALTER DATABASE [evv] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [evv] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [evv] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [evv] SET QUERY_STORE = OFF
GO
USE [evv]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [evv]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3/6/2023 10:50:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Vehicle_ID] [int] IDENTITY(1,1) NOT NULL,
	[Vehicle_Name] [nvarchar](255) NOT NULL,
	[Emission] [int] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Vehicle_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Journeys]    Script Date: 3/6/2023 10:50:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Journeys](
	[Journey_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Total_Distance] [int] NOT NULL,
	[Total_Emission] [int] NOT NULL,
	[Start] [varchar](255) NOT NULL,
	[End] [varchar](255) NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_Journeys] PRIMARY KEY CLUSTERED 
(
	[Journey_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stopovers]    Script Date: 3/6/2023 10:50:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stopovers](
	[Stopover_ID] [int] IDENTITY(1,1) NOT NULL,
	[Vehicle_ID] [int] NOT NULL,
	[Journey_ID] [int] NOT NULL,
	[Distance] [int] NOT NULL,
	[Start] [nvarchar](255) NOT NULL,
	[End] [nvarchar](255) NOT NULL,
	[Emission] [int] NOT NULL,
 CONSTRAINT [PK_Stopovers] PRIMARY KEY CLUSTERED 
(
	[Stopover_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/6/2023 10:50:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[UUID] [varchar](36) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Journeys]  WITH CHECK ADD  CONSTRAINT [FK_Journeys_Users1] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Journeys] CHECK CONSTRAINT [FK_Journeys_Users1]
GO
ALTER TABLE [dbo].[Stopovers]  WITH CHECK ADD  CONSTRAINT [FK_Stopovers_Categories1] FOREIGN KEY([Vehicle_ID])
REFERENCES [dbo].[Categories] ([Vehicle_ID])
GO
ALTER TABLE [dbo].[Stopovers] CHECK CONSTRAINT [FK_Stopovers_Categories1]
GO
ALTER TABLE [dbo].[Stopovers]  WITH CHECK ADD  CONSTRAINT [FK_Stopovers_Journeys] FOREIGN KEY([Journey_ID])
REFERENCES [dbo].[Journeys] ([Journey_ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stopovers] CHECK CONSTRAINT [FK_Stopovers_Journeys]
GO
USE [master]
GO
ALTER DATABASE [evv] SET  READ_WRITE 
GO
