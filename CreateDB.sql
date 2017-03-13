-- Create TeamTracker database
-----------------------------------------------------------------------------------------------------

USE [master]
GO

/****** Object:  Database [TeamTracker]    Script Date: 2017/03/09 10:03:47 ******/
CREATE DATABASE [TeamTracker]

ALTER DATABASE [TeamTracker] SET COMPATIBILITY_LEVEL = 80
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TeamTracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TeamTracker] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TeamTracker] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TeamTracker] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TeamTracker] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TeamTracker] SET ARITHABORT OFF 
GO

ALTER DATABASE [TeamTracker] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [TeamTracker] SET AUTO_SHRINK ON 
GO

ALTER DATABASE [TeamTracker] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TeamTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TeamTracker] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TeamTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TeamTracker] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TeamTracker] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TeamTracker] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TeamTracker] SET  DISABLE_BROKER 
GO

ALTER DATABASE [TeamTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TeamTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TeamTracker] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TeamTracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TeamTracker] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TeamTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TeamTracker] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TeamTracker] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [TeamTracker] SET  MULTI_USER 
GO

ALTER DATABASE [TeamTracker] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TeamTracker] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TeamTracker] SET  READ_WRITE 
GO

-- Create people table
-----------------------------------------------------------------------------------------------------

USE [TeamTracker]
GO

/****** Object:  Table [dbo].[People]    Script Date: 2017/03/09 10:07:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[People](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[contact] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Create status types table
-----------------------------------------------------------------------------------------------------

USE [TeamTracker]
GO

/****** Object:  Table [dbo].[StatusTypes]    Script Date: 2017/03/09 10:07:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StatusTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
  [sortOrder] [int] NOT NULL CONSTRAINT [DF_StatusTypes_sortOrder] DEFAULT ((0)),
 CONSTRAINT [PK_StatusTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Create people status table
-----------------------------------------------------------------------------------------------------

USE [TeamTracker]
GO

/****** Object:  Table [dbo].[PeopleStatus]    Script Date: 2017/03/09 10:08:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PeopleStatus](
	[peopleId] [int] NOT NULL,
	[statusTypeId] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PeopleStatus]  WITH CHECK ADD  CONSTRAINT [FK_PeopleStatus_People1] FOREIGN KEY([peopleId])
REFERENCES [dbo].[People] ([id])
GO

ALTER TABLE [dbo].[PeopleStatus] CHECK CONSTRAINT [FK_PeopleStatus_People1]
GO

ALTER TABLE [dbo].[PeopleStatus]  WITH CHECK ADD  CONSTRAINT [FK_PeopleStatus_StatusTypes1] FOREIGN KEY([statusTypeId])
REFERENCES [dbo].[StatusTypes] ([id])
GO

ALTER TABLE [dbo].[PeopleStatus] CHECK CONSTRAINT [FK_PeopleStatus_StatusTypes1]
GO

-- Populate status types table
-----------------------------------------------------------------------------------------------------

USE [TeamTracker]
GO

INSERT INTO [dbo].[StatusTypes]
           ([name],[SortOrder])
     VALUES
           ('Presence',0)
GO

INSERT INTO [dbo].[StatusTypes]
           ([name],[SortOrder])
     VALUES
           ('Phone',1)
GO

INSERT INTO [dbo].[StatusTypes]
           ([name],[SortOrder])
     VALUES
           ('Email',2)
GO

INSERT INTO [dbo].[StatusTypes]
           ([name],[SortOrder])
     VALUES
           ('Skype',3)
GO

-- Create people status view
-----------------------------------------------------------------------------------------------------

USE [TeamTracker]
GO

/****** Object:  View [dbo].[PeopleStatusView]    Script Date: 2017/03/09 11:21:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[PeopleStatusView]
AS
SELECT        TOP (100) PERCENT dbo.People.name AS PersonName, dbo.StatusTypes.name AS StatusTypeName, dbo.People.id AS PersonId, dbo.StatusTypes.id AS StatusTypeId, dbo.People.contact AS PersonContact
FROM            dbo.StatusTypes FULL OUTER JOIN
                         dbo.PeopleStatus ON dbo.StatusTypes.id = dbo.PeopleStatus.statusTypeId FULL OUTER JOIN
                         dbo.People ON dbo.PeopleStatus.peopleId = dbo.People.id

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "StatusTypes"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 102
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PeopleStatus"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "People"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 138
               Right = 208
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
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1800
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1770
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PeopleStatusView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PeopleStatusView'
GO

-- Create TeamTracker login, user and set permissions, etc
-----------------------------------------------------------------------------------------------------

CREATE LOGIN [TeamTrackerUser] WITH PASSWORD=N'TeamTrackerUser', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
/*
-- Sysadmin role just in case, disabled by default
EXEC sys.sp_addsrvrolemember @loginame = N'TeamTrackerUser', @rolename = N'sysadmin'
GO
*/
USE [TeamTracker]
GO
CREATE USER [TeamTrackerUser] FOR LOGIN [TeamTrackerUser]
GO

USE [TeamTracker]
GO
EXEC sp_addrolemember N'db_owner', N'TeamTrackerUser'
GO

-- Create setting table
-----------------------------------------------------------------------------------------------------

USE [TeamTracker]
GO

/****** Object:  Table [dbo].[Setting]    Script Date: 03/12/2017 16:52:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Setting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[key] [nvarchar](50) NOT NULL,
	[value] [nvarchar](100) NULL,
 CONSTRAINT [PK_Setting_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Setting_UniqueKey] UNIQUE NONCLUSTERED 
(
	[key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Populate setting table with defaults
-----------------------------------------------------------------------------------------------------

INSERT INTO [TeamTracker].[dbo].[Setting]
           ([key],[value])
     VALUES
           ('PageHeader','Team availability matrix'),
           ('SettingsPassword','admin')
GO

-- Create ContactType table
-----------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ContactType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[hyperlinkPrefix] [nvarchar](10) NULL,
 CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Populate ContactType table with defaults
-----------------------------------------------------------------------------------------------------

INSERT INTO [TeamTracker].[dbo].[ContactType]
           ([name],[hyperlinkPrefix])
     VALUES
           ('SIP','sip'),
           ('Email','mailto'),
           ('Skype','tel')
GO

-- Create PeopleContact table
-----------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PeopleContact](
	[peopleId] [int] NOT NULL,
	[contactTypeId] [int] NOT NULL,
	[address] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PeopleContact]  WITH CHECK ADD  CONSTRAINT [FK_PeopleContact_ContactType] FOREIGN KEY([contactTypeId])
REFERENCES [dbo].[ContactType] ([id])
GO

ALTER TABLE [dbo].[PeopleContact] CHECK CONSTRAINT [FK_PeopleContact_ContactType]
GO

ALTER TABLE [dbo].[PeopleContact]  WITH CHECK ADD  CONSTRAINT [FK_PeopleContact_People] FOREIGN KEY([peopleId])
REFERENCES [dbo].[People] ([id])
GO

ALTER TABLE [dbo].[PeopleContact] CHECK CONSTRAINT [FK_PeopleContact_People]
GO

-- Create PeopleContactView table
-----------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[PeopleContactView]
AS
SELECT        dbo.People.id AS peopleId, dbo.ContactType.name AS contactName, dbo.PeopleContact.address AS contactAddress, dbo.ContactType.hyperlinkPrefix
FROM            dbo.ContactType INNER JOIN
                         dbo.PeopleContact ON dbo.ContactType.id = dbo.PeopleContact.contactTypeId INNER JOIN
                         dbo.People ON dbo.PeopleContact.peopleId = dbo.People.id

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "ContactType"
            Begin Extent = 
               Top = 139
               Left = 862
               Bottom = 268
               Right = 1032
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PeopleContact"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 122
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "People"
            Begin Extent = 
               Top = 181
               Left = 208
               Bottom = 303
               Right = 378
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
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PeopleContactView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PeopleContactView'
GO