
CREATE VIEW [dbo].[vw_users]
AS
SELECT     a.userID, a.Title, a.FirstName, a.MiddleName, a.LastName, vw.RANK, a.Phone, a.DSN, a.receiveEmail, a.accessStatus, 
                      e.description AS AccessStatusDescr, a.workCompo AS compo, a.expirationDate, a.created_date, a.comment, a.work_street, a.work_city, a.work_state, 
                      a.work_zip, a.Email, a.DateOfBirth, a.SSN, a.workCompo, vw.GRADE, b.groupId, c.name AS role, ISNULL(a.ada_cs_id, a.cs_id) AS unit_id, 
                      cs.LONG_NAME AS unit_description, cs.PAS_CODE, a.lastAccessDate, coalesce(a.reportView, c.reportView) AS ViewType, a.currentRole, a.username, c.accessScope, a.Email2, 
                      a.Email3, a.modified_date, a.modified_by, a.receiveReminderEmail
FROM         dbo.core_Users AS a LEFT OUTER JOIN
                      dbo.core_lkupAccessStatus AS e ON e.statusId = a.accessStatus INNER JOIN
                      dbo.core_lkupGrade AS vw ON vw.CODE = a.rank_code LEFT OUTER JOIN
                      dbo.core_UserRoles AS b ON b.userRoleID = a.currentRole LEFT OUTER JOIN
                      dbo.core_UserGroups AS c ON c.groupId = b.groupId LEFT OUTER JOIN
                      dbo.Command_Struct AS cs ON cs.CS_ID = ISNULL(a.ada_cs_id, a.cs_id)
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_users';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[17] 2[32] 3) )"
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
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 211
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw"
            Begin Extent = 
               Top = 126
               Left = 418
               Bottom = 241
               Right = 570
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 102
               Left = 228
               Bottom = 217
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 216
               Left = 38
               Bottom = 331
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cs"
            Begin Extent = 
               Top = 246
               Left = 228
               Bottom = 361
               Right = 446
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
         Column = 1440
         Alias = 900
         Table = 1170
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_users';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'         Output = 720
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_users';
GO

