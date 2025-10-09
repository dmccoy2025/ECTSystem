
CREATE VIEW [dbo].[vw_lod]
AS
SELECT        a.lodId, a.case_id AS [Case Id], w.description AS Status, CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS Closed, CONVERT(char(11), ISNULL(track.ReceiveDate, a.created_date), 101) AS [Date Of Status], 
                         CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, a.created_date), GETDATE()) ELSE 0 END AS Days, CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(a.created_date, GETDATE()), GETDATE()) 
                         ELSE 0 END AS [Days Open], CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(a.created_date, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days], a.member_name AS [Member Name], 
                         RIGHT(a.member_ssn, 4) AS [Member SSN], dbo.InitCap(g.RANK) AS [Member Rank], g.GRADE AS [Member Grade], a.member_unit AS [Member Unit], CONVERT(char(11), a.member_DOB, 101) AS [Member DOB], 
                         CONVERT(char(11), a.created_date, 101) AS [Date Created], CONVERT(char(11), a.modified_date, 101) AS [Date Modified], dbo.FormatName(a.created_by) AS [Created By], dbo.FormatName(a.modified_by) AS [Modified By], 
                         comp.CompletedDate AS [Date Completed], CASE a.formal_inv WHEN 1 THEN 'Yes' ELSE 'No' END AS Formal, a.med_tech_comments AS [Med Tech Comments], CONVERT(char(11), wing_cc_sig.date, 101) 
                         AS [Wing CC Sign Date], wing_cc_sig.nameAndRank AS [Wing CC], CONVERT(char(11), formal_wing_cc_sig.date, 101) AS [Formal Wing  CC Sign Date], formal_wing_cc_sig.nameAndRank AS [Formal Wing CC], CONVERT(char(11),
                          approving_sig.date, 101) AS [Approval Sign Date], approving_sig.nameAndRank AS [Approval Authority], CONVERT(char(11), formal_approving_sig.date, 101) AS [Formal Approval Sign Date], 
                         formal_approving_sig.nameAndRank AS [Formal Approval Authority], CONVERT(char(11), board_tech_sig.date, 101) AS [Board Tech  Sign Date], board_tech_sig.nameAndRank AS [Board Tech], CONVERT(char(11), 
                         formal_board_tech_sig.date, 101) AS [Formal Board Tech Sign Date], formal_board_tech_sig.nameAndRank AS [Formal Board Tech], CONVERT(char(11), board_legal_sig.date, 101) AS [Board JA Sign Date], 
                         board_legal_sig.nameAndRank AS [Board JA], CONVERT(char(11), formal_board_legal_sig.date, 101) AS [Formal Board JA Sign Date], formal_board_legal_sig.nameAndRank AS [Formal Board JA], CONVERT(char(11), 
                         board_medical_sig.date, 101) AS [Board SG Sign Date], board_medical_sig.nameAndRank AS [Board SG], CONVERT(char(11), formal_board_medical_sig.date, 101) AS [Formal Board SG Sign Date], 
                         formal_board_medical_sig.date AS [Formal Board SG], CONVERT(char(11), board_admin_sig.date, 101) AS [Board Admin Sign Date], board_admin_sig.nameAndRank AS [Board Admin], CONVERT(char(11), 
                         formal_board_admin_sig.date, 101) AS [Formal Board Admin Sign Date], formal_board_admin_sig.nameAndRank AS [Formal Board  Admin], CONVERT(char(11), wing_ja_sig.date, 101) AS [Wing JA Sign Date], 
                         wing_ja_sig.nameAndRank AS [Wing JA], CONVERT(char(11), formal_wing_ja_sig.date, 101) AS [Formal Wing JA Sign Date], formal_wing_ja_sig.nameAndRank AS [Formal Wing JA], CONVERT(char(11), med_off_sig.date, 101) 
                         AS [Med Officer Sign Date], med_off_sig.nameAndRank AS [Med Officer], CONVERT(char(11), unit_commander_sig.date, 101) AS [Unit CC Sign Date], unit_commander_sig.nameAndRank AS [Unit CC], 
                         a.FinalDecision AS [Final Decision], CASE a.memberNotified WHEN 1 THEN 'Yes' ELSE 'No' END AS [Member Notified], 
                         (CASE m.member_status WHEN 'Active' THEN m.member_status WHEN 'Inactive' THEN m.member_status ELSE NULL END) AS [Member  Status], m.event_nature_type AS [Event Type], 
                         m.medical_facility_type AS [Medical Facility Type], m.medical_facility AS [Medical Facility], CONVERT(char(11), m.treatment_date, 101) AS [Treatment Date], m.death_involved_yn AS Death, m.mva_involved_yn AS MVA, 
                         m.icd9Id AS [ICD-9], i.value AS [ICD Code], m.diagnosis_text AS Diagnosis, u.cmdr_duty_determination AS duty_status, dbo.InitCap(u.cmdr_duty_determination) AS [Duty Status], u.cmdr_duty_from AS [Duty Start], 
                         u.cmdr_duty_to AS [Duty End], CASE u.cmdr_activated_yn WHEN 'Y' THEN 'Yes' WHEN 'N' THEN 'No' ELSE '' END AS [Member  Activated], '[' + i.value + '] ' + i.text AS [ICD Text], 
                         CASE f.investigationOf WHEN 1 THEN 'Disease' WHEN 2 THEN 'Illness' WHEN 3 THEN 'Death' WHEN 4 THEN 'Injury' WHEN 5 THEN 'Injury (MVA)' ELSE '' END AS [Investigation Of], CONVERT(char(11), f.reportDate, 101) 
                         AS [IO Report Date], 
                         CASE f.status WHEN 1 THEN 'Regular or EAD' WHEN 2 THEN 'Called or Ordered to AD More than 30 days' WHEN 3 THEN 'Called or  Ordered to AD less than 30 days' WHEN 4 THEN 'Inactive Duty Training' WHEN 5 THEN 'Short Tour of AD for Training'
                          ELSE '' END AS [IO Member Status], CASE f.mentallySound WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Mentally Sound], 
                         CASE f.intentionalMisconduct WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Intentional Misconduct], CASE f.presentForDuty WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Present For Duty], 
                         CONVERT(char(11), f.sig_date_io, 101) AS [IO Sign Date], IOUser.RANK + ' ' + IOUser.LastName + ' ' + IOUser.FirstName AS [Investigating Officer], dbo.InitCap(CONVERT(varchar(100), 
                         f.sig_info_appointing.query('data(/PersonnelData/Grade)'))) + ' ' + CONVERT(varchar(100), f.sig_info_appointing.query('data (/PersonnelData/Name)')) AS [Appointing Authority], 
                         CASE m.physician_cancel_reason WHEN 0 THEN 'No' ELSE 'Yes' END AS Cancelled, canc.Description AS [Reason Cancelled], find.Description AS [Finding - Final], find.Id AS finding_final, 
                         find_bja.Description AS [Finding - HQ JA], find_bja.Id AS finding_hq_jq, find_baa.Description AS [Finding - HQ AA], find_baa.Id AS finding_hq_aa, find_bsg.Description AS [Finding - HQ SG], find_bsg.Id AS finding_hq_sg, 
                         find_ucc.Description AS [Finding - Unit CC], find_ucc.Id AS finding_unit_cc, find_wja.Description AS [Finding - Wing JA], find_wja.Id AS finding_wing_ja, find_wcc.Description AS [Finding - Wing CC], find_wcc.Id AS finding_wing_cc, 
                         find_io.Description AS [Finding - IO], find_io.Id AS finding_io, find_wccf.Description AS [Finding - Formal Wing CC], find_wccf.Id AS finding_formal_wing_cc, find_wjaf.Description AS [Finding - Formal Wing JA], 
                         find_wjaf.Id AS finding_formal_wing_ja, w.ws_id, wf.workflowId, w.isFinal, a.created_by, a.member_unit, a.member_unit_id, a.sarc, a.restricted, a.deleted, 
                         CASE a.tmn WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Timely Manner Notify], CASE a.tms WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Timely Manner Submit], 
                         info.source_description AS [Information Source], u.source_information_specify AS [Other Information Source], 
                         CASE u.proximate_cause WHEN 13 THEN u.proximate_cause_specify ELSE pc.cause_description END AS [Proximate Cause of Death], 
                         (CASE u.workflow WHEN 27 THEN u.member_on_orders ELSE (CASE a.wasMemberOnOrders WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END) END) AS [Member on Orders], 
                         (CASE u.workflow WHEN 27 THEN u.member_credible ELSE (CASE a.hasCredibleService WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END) END) AS [Member Credible Service], 
                         m.member_condition AS [Member Condition], mi.influence_description AS [Member under influence], m.alcohol_test_done AS [Alcohol Test Done], m.drug_test_done AS [Drug Test Done], 
                         m.member_responsible AS [Member Mentally Responsible], m.psych_eval AS [Psychiatric Evaluation Done], m.psych_date AS [Psychiatric Evaluation Date], m.relevant_condition AS [Other Relevant Condition], 
                         m.other_test AS [Other tests done], m.other_test_date AS [Other test date], m.deployed_location AS [Member in Deployed  Location], c.component_description AS [Member Component], 
                         (CASE a.workflow WHEN 27 THEN mc.Member_Status_Desc ELSE (CASE m.member_status WHEN 'Active' THEN NULL WHEN 'Inactive' THEN NULL ELSE
                             (SELECT        TOP 1 Member_Status_Desc
                               FROM            core_lkupMemberCategory b
                               WHERE        b.Member_Status_Desc = m.member_status) END) END) AS [Member Category], fl.location_description AS [Member  Location], od.occurrence_description AS [Member Occurrence], u.absent_from AS [Absent From], 
                         u.absent_to AS [Absent To], CASE m.condition_epts WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [EPTS Condition], 
                         CASE m.service_aggravated WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Service Aggravated], m.mobility_standards AS [Mobility Standards], m.board_finalization AS [Board Finalization], 
                         w.statusId AS [Status Code], cs.STATE, a.from_unit AS Wing, f_bsg.explanation AS Explanation, CASE WHEN pas.priority >= 60000 THEN 'P1a' WHEN pas.priority BETWEEN 50000 AND 
                         59999 THEN 'P1b' WHEN pas.priority BETWEEN 40000 AND 49999 THEN 'P2' ELSE '' END AS Priority
FROM            dbo.Form348 AS a INNER JOIN
                         dbo.Form348_Medical AS m ON m.lodid = a.lodId INNER JOIN
                         dbo.Form348_Unit AS u ON u.lodid = a.lodId LEFT OUTER JOIN
                         dbo.core_lkupICD9 AS i ON i.ICD9_ID = m.icd9Id LEFT OUTER JOIN
                         dbo.Form261 AS f ON f.lodId = a.lodId LEFT OUTER JOIN
                         dbo.core_lkupGrade AS g ON g.CODE = a.member_grade INNER JOIN
                         dbo.vw_WorkStatus AS w ON w.ws_id = a.status INNER JOIN
                         dbo.core_Workflow AS wf ON wf.workflowId = w.workflowId INNER JOIN
                         dbo.core_Users AS created ON created.userID = a.created_by INNER JOIN
                         dbo.core_Users AS updated ON updated.userID = a.modified_by INNER JOIN
                         dbo.Command_Struct AS cs ON cs.CS_ID = a.member_unit_id LEFT OUTER JOIN
                         dbo.core_lkupPAS AS pas ON pas.pas = cs.PAS_CODE LEFT OUTER JOIN
                         dbo.vw_users AS IOUser ON f.IoUserId = IOUser.userID LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find ON find.Id = CASE WHEN f.final_approval_findings IS NULL THEN a.FinalFindings ELSE f.final_approval_findings END LEFT OUTER JOIN
                         dbo.core_lkupInfoSource AS info ON info.source_id = u.source_information LEFT OUTER JOIN
                         dbo.core_lkupProximateCause AS pc ON pc.cause_id = u.proximate_cause LEFT OUTER JOIN
                         dbo.core_lkupMemberInfluence AS mi ON mi.influence_id = m.influence LEFT OUTER JOIN
                         dbo.core_lkupMemberCategory AS mc ON mc.Member_Status_ID = m.member_category LEFT OUTER JOIN
                         dbo.core_lkupComponent AS c ON c.component_id = m.member_component LEFT OUTER JOIN
                         dbo.core_lkupFromLocation AS fl ON fl.location_id = m.member_from LEFT OUTER JOIN
                         dbo.core_lkupCancelReason AS canc ON canc.Id = m.physician_cancel_reason LEFT OUTER JOIN
                         dbo.core_lkupOccurrenceDescription AS od ON od.occurrence_id = u.member_occurrence LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_ucc ON f_ucc.LODID = a.lodId AND f_ucc.ptype = 3 LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_ucc ON find_ucc.Id = f_ucc.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_io ON f_io.LODID = a.lodId AND f_io.ptype = 19 LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_io ON find_io.Id = f_io.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_wja ON f_wja.LODID = a.lodId AND f_wja.ptype = 3 LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_wja ON find_wja.Id = f_wja.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_wcc ON f_wcc.LODID = a.lodId AND f_wcc.ptype = 5 LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_wcc ON find_wcc.Id = f_wcc.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_wccf ON f_wccf.LODID = a.lodId AND f_wccf.ptype = 13 LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_wccf ON find_wccf.Id = f_wccf.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_wjaf ON f_wjaf.LODID = a.lodId AND f_wjaf.ptype = 12 LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_wjaf ON find_wjaf.Id = f_wjaf.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_bja ON f_bja.LODID = a.lodId AND f_bja.ptype = CASE a.formal_inv WHEN 1 THEN 15 ELSE 7 END LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_bja ON find_bja.Id = f_bja.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_bsg ON f_bsg.LODID = a.lodId AND f_bsg.ptype = CASE a.formal_inv WHEN 1 THEN 16 ELSE 8 END LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_bsg ON find_bsg.Id = f_bsg.finding LEFT OUTER JOIN
                         dbo.FORM348_findings AS f_baa ON f_baa.LODID = a.lodId AND f_baa.ptype = CASE a.formal_inv WHEN 1 THEN 18 ELSE 10 END LEFT OUTER JOIN
                         dbo.core_lkUpFindings AS find_baa ON find_baa.Id = f_baa.finding LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Medical Officer Review')) AS med_off_sig ON med_off_sig.refId = a.lodId AND med_off_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Unit Commander Review')) AS unit_commander_sig ON unit_commander_sig.refId = a.lodId AND unit_commander_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Wing JA Review')) AS wing_ja_sig ON wing_ja_sig.refId = a.lodId AND wing_ja_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal Action by Wing JA')) AS formal_wing_ja_sig ON formal_wing_ja_sig.refId = a.lodId AND formal_wing_ja_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Appointing Authority Review')) AS wing_cc_sig ON wing_cc_sig.refId = a.lodId AND wing_cc_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal Action by Appointing Authority')) AS formal_wing_cc_sig ON formal_wing_cc_sig.refId = a.lodId AND formal_wing_cc_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'AFRC LOD Board Review')) AS board_tech_sig ON board_tech_sig.refId = a.lodId AND board_tech_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal AFRC LOD Board Review')) AS formal_board_tech_sig ON formal_board_tech_sig.refId = a.lodId AND formal_board_tech_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'LOD Board Medical Review')) AS board_medical_sig ON board_medical_sig.refId = a.lodId AND board_medical_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal LOD Board Medical Review')) AS formal_board_medical_sig ON formal_board_medical_sig.refId = a.lodId AND formal_board_medical_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'LOD Board Legal Review')) AS board_legal_sig ON board_legal_sig.refId = a.lodId AND board_legal_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal LOD Board Legal Review')) AS formal_board_legal_sig ON formal_board_legal_sig.refId = a.lodId AND formal_board_legal_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'LOD Board Personnel Review')) AS board_admin_sig ON board_admin_sig.refId = a.lodId AND board_admin_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal LOD Board Personnel Review')) AS formal_board_admin_sig ON formal_board_admin_sig.refId = a.lodId AND formal_board_admin_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Approving Authority Action')) AS approving_sig ON approving_sig.refId = a.lodId AND approving_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Formal Approving Authority Action')) AS formal_approving_sig ON formal_approving_sig.refId = a.lodId AND formal_approving_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        MAX(startDate) AS ReceiveDate, ws_id, refId
                               FROM            dbo.core_WorkStatus_Tracking
                               WHERE        (module = 2)
                               GROUP BY ws_id, refId) AS track ON track.refId = a.lodId AND track.ws_id = a.status LEFT OUTER JOIN
                             (SELECT        core_WorkStatus_Tracking_1.refId, MAX(core_WorkStatus_Tracking_1.endDate) AS CompletedDate
                               FROM            dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON core_WorkStatus_Tracking_1.ws_id = ws.ws_id
                               WHERE        (core_WorkStatus_Tracking_1.module = 2) AND (ws.isFinal = 1) AND (ws.isCancel = 0)
                               GROUP BY core_WorkStatus_Tracking_1.refId) AS comp ON comp.refId = a.lodId
WHERE        (w.isCancel = 0)
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[21] 4[40] 2[20] 3) )"
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
               Right = 249
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 265
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "u"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 361
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "i"
            Begin Extent = 
               Top = 6
               Left = 287
               Bottom = 121
               Right = 439
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f"
            Begin Extent = 
               Top = 246
               Left = 280
               Bottom = 361
               Right = 472
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "g"
            Begin Extent = 
               Top = 126
               Left = 303
               Bottom = 241
               Right = 460
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "w"
            Begin Extent = 
               Top = 366
               Left = 38
               Bottom = 481
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_lod';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'        Begin Table = "wf"
            Begin Extent = 
               Top = 366
               Left = 228
               Bottom = 496
               Right = 398
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "created"
            Begin Extent = 
               Top = 366
               Left = 228
               Bottom = 481
               Right = 382
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "updated"
            Begin Extent = 
               Top = 486
               Left = 38
               Bottom = 601
               Right = 192
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cs"
            Begin Extent = 
               Top = 6
               Left = 477
               Bottom = 136
               Right = 718
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "IOUser"
            Begin Extent = 
               Top = 498
               Left = 230
               Bottom = 628
               Right = 436
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find"
            Begin Extent = 
               Top = 486
               Left = 230
               Bottom = 586
               Right = 382
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "info"
            Begin Extent = 
               Top = 606
               Left = 38
               Bottom = 719
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pc"
            Begin Extent = 
               Top = 630
               Left = 264
               Bottom = 743
               Right = 447
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "mi"
            Begin Extent = 
               Top = 720
               Left = 38
               Bottom = 833
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "mc"
            Begin Extent = 
               Top = 834
               Left = 38
               Bottom = 947
               Right = 239
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 948
               Left = 38
               Bottom = 1061
               Right = 253
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fl"
            Begin Extent = 
               Top = 744
               Left = 278
               Bottom = 857
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "canc"
            Begin Extent = 
               Top = 588
               Left = 230
               Bottom = 688
               Right = 382
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "od"
            Begin Extent = 
               Top = 1062
               Left = 38
               Bottom = 1175
               Right = 250
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_ucc"
            Begin Extent = 
               Top = 606
               Left = 38
           ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_lod';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane3', @value = N'    Bottom = 721
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_ucc"
            Begin Extent = 
               Top = 690
               Left = 228
               Bottom = 790
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_io"
            Begin Extent = 
               Top = 726
               Left = 38
               Bottom = 841
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_io"
            Begin Extent = 
               Top = 792
               Left = 228
               Bottom = 892
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_wja"
            Begin Extent = 
               Top = 846
               Left = 38
               Bottom = 961
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_wja"
            Begin Extent = 
               Top = 894
               Left = 228
               Bottom = 994
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_wcc"
            Begin Extent = 
               Top = 966
               Left = 38
               Bottom = 1081
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_wcc"
            Begin Extent = 
               Top = 996
               Left = 228
               Bottom = 1096
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_wccf"
            Begin Extent = 
               Top = 1086
               Left = 38
               Bottom = 1201
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_wccf"
            Begin Extent = 
               Top = 1098
               Left = 228
               Bottom = 1198
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_wjaf"
            Begin Extent = 
               Top = 1200
               Left = 228
               Bottom = 1315
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_wjaf"
            Begin Extent = 
               Top = 1206
               Left = 38
               Bottom = 1306
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_bja"
            Begin Extent = 
               Top = 1308
               Left = 38
               Bottom = 1423
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_bja"
            Begin Extent = 
               Top = 1320
               Left = 228
               Bottom = 1420
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_bsg"
            Begin Extent = 
               Top = 1422
               Left = 228
               Bottom = 1537
               Right = 380
            End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_lod';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane4', @value = N'
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_bsg"
            Begin Extent = 
               Top = 1428
               Left = 38
               Bottom = 1528
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f_baa"
            Begin Extent = 
               Top = 1530
               Left = 38
               Bottom = 1645
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "find_baa"
            Begin Extent = 
               Top = 1542
               Left = 228
               Bottom = 1642
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "med_off_sig"
            Begin Extent = 
               Top = 252
               Left = 510
               Bottom = 382
               Right = 680
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "unit_commander_sig"
            Begin Extent = 
               Top = 384
               Left = 436
               Bottom = 514
               Right = 606
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wing_ja_sig"
            Begin Extent = 
               Top = 384
               Left = 644
               Bottom = 514
               Right = 814
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_wing_ja_sig"
            Begin Extent = 
               Top = 516
               Left = 474
               Bottom = 646
               Right = 644
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wing_cc_sig"
            Begin Extent = 
               Top = 648
               Left = 485
               Bottom = 778
               Right = 655
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_wing_cc_sig"
            Begin Extent = 
               Top = 780
               Left = 512
               Bottom = 910
               Right = 682
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "board_tech_sig"
            Begin Extent = 
               Top = 912
               Left = 418
               Bottom = 1042
               Right = 588
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_board_tech_sig"
            Begin Extent = 
               Top = 912
               Left = 626
               Bottom = 1042
               Right = 796
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "board_medical_sig"
            Begin Extent = 
               Top = 1044
               Left = 418
               Bottom = 1174
               Right = 588
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_board_medical_sig"
            Begin Extent = 
               Top = 1044
               Left = 626
               Bottom = 1174
               Right = 796
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "board_legal_sig"
            Begin Extent = 
               Top = 1176
               Left = 418
               Bottom = 1306', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_lod';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane5', @value = N'
               Right = 588
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_board_legal_sig"
            Begin Extent = 
               Top = 1176
               Left = 626
               Bottom = 1306
               Right = 796
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "board_admin_sig"
            Begin Extent = 
               Top = 1308
               Left = 418
               Bottom = 1438
               Right = 588
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_board_admin_sig"
            Begin Extent = 
               Top = 1308
               Left = 626
               Bottom = 1438
               Right = 796
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "approving_sig"
            Begin Extent = 
               Top = 1440
               Left = 418
               Bottom = 1570
               Right = 588
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "formal_approving_sig"
            Begin Extent = 
               Top = 1440
               Left = 626
               Bottom = 1570
               Right = 796
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "track"
            Begin Extent = 
               Top = 1572
               Left = 418
               Bottom = 1685
               Right = 588
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "comp"
            Begin Extent = 
               Top = 1572
               Left = 626
               Bottom = 1668
               Right = 798
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pas"
            Begin Extent = 
               Top = 6
               Left = 756
               Bottom = 119
               Right = 926
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_lod';
GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 5, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_lod';
GO

