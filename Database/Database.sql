CREATE DATABASE [evv]

USE [evv]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 4/4/2023 11:23:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Vehicle_ID] [int] IDENTITY(1,1) NOT NULL,
	[Vehicle_Name] [nvarchar](255) NOT NULL,
	[Emission] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Vehicle_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Journeys]    Script Date: 4/4/2023 11:23:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Journeys](
	[Journey_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Total_Distance] [int] NULL,
	[Total_Emission] [int] NULL,
	[Start] [varchar](255) NULL,
	[End] [varchar](255) NULL,
	[Date] [date] NOT NULL,
	[Score] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Journey_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stopovers]    Script Date: 4/4/2023 11:23:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stopovers](
	[Stopover_ID] [int] IDENTITY(1,1) NOT NULL,
	[Vehicle_ID] [int] NOT NULL,
	[Journey_ID] [int] NOT NULL,
	[Distance] [int] NOT NULL,
	[Start] [varchar](255) NOT NULL,
	[End] [varchar](255) NOT NULL,
	[Emission] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Stopover_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/4/2023 11:23:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[UUID] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Score] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Stopovers]  WITH CHECK ADD FOREIGN KEY([Journey_ID])
REFERENCES [dbo].[Journeys] ([Journey_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stopovers]  WITH CHECK ADD FOREIGN KEY([Vehicle_ID])
REFERENCES [dbo].[Categories] ([Vehicle_ID])
ON DELETE CASCADE
GO
/****** Object:  Trigger [dbo].[trg_UpdateUserScore]    Script Date: 4/4/2023 11:23:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trg_UpdateUserScore]
ON [dbo].[Journeys]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- If the Score column is updated in Journey table
    IF UPDATE(Score)
    BEGIN
        -- Update the corresponding user's score in the User table
        UPDATE u
        SET u.Score = u.Score + i.ScoreDifference
        FROM Users u
        JOIN (
            SELECT i.User_ID, SUM(i.Score - d.Score) AS ScoreDifference
            FROM inserted i
            JOIN deleted d ON i.Journey_ID = d.Journey_ID
            GROUP BY i.User_ID
        ) i ON i.User_ID = u.User_ID
    END
END;
GO
ALTER TABLE [dbo].[Journeys] ENABLE TRIGGER [trg_UpdateUserScore]
GO
USE [master]
GO
ALTER DATABASE [evv] SET  READ_WRITE 
GO
