

CREATE VIEW [dbo].[vw_UserTracking]
AS

SELECT	TOP (100) PERCENT a.actionDate, s.description, a.actionId, la.actionName, la.hide, a.notes, vu.LastName, a.moduleId, mod.moduleName, 
		a.referenceId, a.logId, a.userId, la.logChanges,
		(
			CASE
				WHEN a.moduleId = 2 THEN (SELECT case_id FROM dbo.Form348 WHERE (lodId = a.referenceId))
				WHEN a.moduleId = 5 THEN (SELECT Case_Id FROM dbo.Form348_RR WHERE (request_id = a.referenceId))
				WHEN a.moduleId = 24 THEN (SELECT case_id FROM dbo.Form348_AP WHERE (appeal_id = a.referenceId))
				WHEN mod.isSpecialCase = 1 THEN (SELECT case_id FROM dbo.Form348_SC WHERE (SC_Id = a.referenceId))
				ELSE NULL
			END
		) AS caseId
FROM	dbo.core_LogAction AS a LEFT OUTER JOIN
		dbo.core_StatusCodes AS s ON s.statusId = a.status INNER JOIN
		dbo.core_lkupAction AS la ON la.actionId = a.actionId INNER JOIN
		dbo.vw_users AS vu ON vu.userID = a.userId INNER JOIN
		dbo.core_lkupModule AS mod ON mod.moduleId = a.moduleId
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[2] 2[52] 3) )"
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
         Begin Table = "s"
            Begin Extent = 
               Top = 6
               Left = 228
               Bottom = 121
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "la"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vu"
            Begin Extent = 
               Top = 126
               Left = 228
               Bottom = 241
               Right = 400
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "mod"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 331
               Right = 190
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_UserTracking';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_UserTracking';
GO

