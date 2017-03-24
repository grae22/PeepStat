-- Create TeamTracker database
-----------------------------------------------------------------------------------------------------

USE [master]
GO

/****** Object:  Database [TeamTracker]    Script Date: 2017/03/09 10:03:47 ******/
CREATE DATABASE [TeamTracker]
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
/****** Object: Table [dbo].[People] Script Date: 2017/03/09 10:07:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- Create people contact table
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  Table [dbo].[PeopleContact]    Script Date: 2017/03/16 4:08:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeopleContact](
	[peopleId] [int] NOT NULL,
	[statusTypeId] [int] NOT NULL,
	[address] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO


-- Create people status table
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  Table [dbo].[PeopleStatus]    Script Date: 2017/03/16 4:08:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeopleStatus](
	[peopleId] [int] NOT NULL,
	[statusTypeId] [int] NOT NULL
) ON [PRIMARY]
GO


-- Create setting table
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 2017/03/16 4:08:21 PM ******/
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


-- Create status types table
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  Table [dbo].[StatusTypes]    Script Date: 2017/03/16 4:08:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[sortOrder] [int] NOT NULL CONSTRAINT [DF_StatusTypes_sortOrder]  DEFAULT ((0)),
	[hyperlinkPrefix] [nvarchar](10) NOT NULL CONSTRAINT [DF_StatusTypes_hyperlinkPrefix]  DEFAULT (''),
 CONSTRAINT [PK_StatusTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_StatusTypes_UniqueName] ON [dbo].[StatusTypes]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_StatusTypes_UniqueSortOrder] ON [dbo].[StatusTypes]
(
	[sortOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


-- Create export people view
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  View [dbo].[ExportPeopleView]    Script Date: 2017/03/16 4:08:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ExportPeopleView]
AS
SELECT        dbo.People.name, dbo.StatusTypes.name AS statusName, dbo.PeopleContact.address
FROM            dbo.People INNER JOIN
                         dbo.PeopleContact ON dbo.People.id = dbo.PeopleContact.peopleId INNER JOIN
                         dbo.StatusTypes ON dbo.PeopleContact.statusTypeId = dbo.StatusTypes.id
GO


-- Create people contact view
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  View [dbo].[PeopleContactView]    Script Date: 2017/03/16 4:08:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PeopleContactView]
AS
SELECT        dbo.People.id AS peopleId, dbo.StatusTypes.name AS contactTypeName, dbo.PeopleContact.address AS contactAddress, dbo.StatusTypes.hyperlinkPrefix
FROM            dbo.People INNER JOIN
                         dbo.PeopleContact ON dbo.People.id = dbo.PeopleContact.peopleId INNER JOIN
                         dbo.StatusTypes ON dbo.PeopleContact.statusTypeId = dbo.StatusTypes.id
GO


-- Create people status view
-----------------------------------------------------------------------------------------------------
USE [TeamTracker]
GO
/****** Object:  View [dbo].[PeopleStatusView]    Script Date: 2017/03/16 4:08:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PeopleStatusView]
AS
SELECT        TOP (100) PERCENT dbo.People.name AS PersonName, dbo.StatusTypes.name AS StatusTypeName, dbo.People.id AS PersonId, dbo.StatusTypes.id AS StatusTypeId
FROM            dbo.StatusTypes FULL OUTER JOIN
                         dbo.PeopleStatus ON dbo.StatusTypes.id = dbo.PeopleStatus.statusTypeId FULL OUTER JOIN
                         dbo.People ON dbo.PeopleStatus.peopleId = dbo.People.id

GO
ALTER TABLE [dbo].[PeopleContact]  WITH CHECK ADD  CONSTRAINT [FK_PeopleContact_People] FOREIGN KEY([peopleId])
REFERENCES [dbo].[People] ([id])
GO
ALTER TABLE [dbo].[PeopleContact] CHECK CONSTRAINT [FK_PeopleContact_People]
GO
ALTER TABLE [dbo].[PeopleContact]  WITH CHECK ADD  CONSTRAINT [FK_PeopleContact_Status] FOREIGN KEY([statusTypeId])
REFERENCES [dbo].[StatusTypes] ([id])
GO
ALTER TABLE [dbo].[PeopleContact] CHECK CONSTRAINT [FK_PeopleContact_Status]
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
         Begin Table = "People"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 131
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PeopleContact"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 147
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "StatusTypes"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 158
               Right = 624
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ExportPeopleView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ExportPeopleView'
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
         Begin Table = "People"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PeopleContact"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 132
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "StatusTypes"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 159
               Right = 624
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
         Alias = 2100
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
         Column = 2085
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
USE [master]
GO
ALTER DATABASE [TeamTracker] SET  READ_WRITE 
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