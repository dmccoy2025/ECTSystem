
CREATE VIEW [dbo].[case_tracking]
AS
SELECT        wst.workflowId, CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE CASE WHEN Form348_RR.Case_Id IS NOT NULL
                          THEN Form348_RR.Case_Id ELSE CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE CASE WHEN Form348_AP.Case_Id IS NOT NULL 
                         THEN Form348_AP.Case_Id ELSE CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END END END END END END AS CaseId, CASE WHEN Form348.member_unit_id IS NOT NULL 
                         THEN Form348.member_unit_id ELSE CASE WHEN Form348_SARC.member_unit_id IS NOT NULL THEN Form348_SARC.member_unit_id ELSE CASE WHEN Form348_RR.member_unit_id IS NOT NULL 
                         THEN Form348_RR.member_unit_id ELSE CASE WHEN Form348_SC.member_unit_id IS NOT NULL THEN Form348_SC.member_unit_id ELSE CASE WHEN Form348_AP.member_unit_id IS NOT NULL 
                         THEN Form348_AP.member_unit_id ELSE CASE WHEN Form348_AP_SARC.member_unit_id IS NOT NULL THEN Form348_AP_SARC.member_unit_id END END END END END END AS member_unit_id, 
                         CASE WHEN Form348.member_ssn IS NOT NULL THEN Form348.member_ssn ELSE CASE WHEN Form348_SARC.member_ssn IS NOT NULL 
                         THEN Form348_SARC.member_ssn ELSE CASE WHEN Form348_RR.member_ssn IS NOT NULL THEN Form348_RR.member_ssn ELSE CASE WHEN Form348_SC.member_ssn IS NOT NULL 
                         THEN Form348_SC.member_ssn ELSE CASE WHEN Form348_AP.member_ssn IS NOT NULL THEN Form348_AP.member_ssn ELSE CASE WHEN Form348_AP_SARC.member_ssn IS NOT NULL 
                         THEN Form348_AP_SARC.member_ssn END END END END END END AS member_ssn, sc.description, wst.name, wst.startDate, wst.endDate
FROM            dbo.core_WorkStatus_Tracking AS wst INNER JOIN
                         dbo.core_WorkStatus AS ws ON wst.ws_id = ws.ws_id INNER JOIN
                         dbo.core_StatusCodes AS sc ON ws.statusId = sc.statusId INNER JOIN
                         dbo.core_Users AS u ON wst.completedBy = u.userID LEFT OUTER JOIN
                         dbo.Form348 ON dbo.Form348.lodId = wst.refId AND wst.workflowId IN (1, 27) LEFT OUTER JOIN
                         dbo.Form348_SARC ON dbo.Form348_SARC.sarc_id = wst.refId AND wst.workflowId IN (28) LEFT OUTER JOIN
                         dbo.Form348_RR ON dbo.Form348_RR.request_id = wst.refId AND wst.workflowId IN (5) LEFT OUTER JOIN
                         dbo.Form348_SC ON dbo.Form348_SC.SC_Id = wst.refId AND wst.workflowId IN (23, 15, 12, 6, 21, 7, 24, 18, 30, 19, 25, 13, 22, 16, 11, 20, 14, 8) LEFT OUTER JOIN
                         dbo.Form348_AP ON dbo.Form348_AP.appeal_id = wst.refId AND wst.workflowId IN (26) LEFT OUTER JOIN
                         dbo.Form348_AP_SARC ON dbo.Form348_AP_SARC.appeal_sarc_id = wst.refId AND wst.workflowId IN (29)
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'case_tracking';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
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
      ActivePaneConfig = 10
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "wst"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ws"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sc"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "u"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 868
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Form348"
            Begin Extent = 
               Top = 6
               Left = 906
               Bottom = 136
               Right = 1168
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Form348_SARC"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 298
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Form348_RR"
            Begin Extent = 
               Top = 138
               Left = 336
               Bottom = 268
               Right = 611
            End
            DisplayFlags = 280
            To', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'case_tracking';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'pColumn = 0
         End
         Begin Table = "Form348_SC"
            Begin Extent = 
               Top = 138
               Left = 649
               Bottom = 268
               Right = 1017
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Form348_AP"
            Begin Extent = 
               Top = 138
               Left = 1055
               Bottom = 268
               Right = 1330
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Form348_AP_SARC"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 298
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'case_tracking';
GO

