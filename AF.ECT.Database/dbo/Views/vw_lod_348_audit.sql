
-- ============================================================================
-- Author:		Raymond Hall
-- Create date:  17- Sep-2021
-- Description:	Created view for ADHOC report for LOD Audit		
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	08-Nov-2021	
-- Description:		Modified to removed duplicates and included 
-- [Wing JA Sign Date] and [Wing JA]
-- ============================================================================
-- Modified By:		Raymond Hall
-- Modified Date:	08-Nov-2021	
-- Description:		Modified query cost cost of this view was 412. The limit was 
-- 300. it is now modified to 500.
-- Path: right click server name -> Properties -> connections -> "Use query 
-- governor to prevent..." then modify limit 
-- ============================================================================





CREATE VIEW [dbo].[vw_lod_348_audit]
AS


SELECT       DiSTINCT(a.lodId), a.case_id AS [Case Id], w.description AS Status, CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS Closed, CONVERT(char(11), ISNULL(track.ReceiveDate, a.created_date), 101) AS [Date Of Status], 
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
                         formal_board_admin_sig.date, 101) AS [Formal Board Admin Sign Date], formal_board_admin_sig.nameAndRank AS [Formal Board  Admin], CONVERT(char(11), formal_wing_ja_sig.date, 101) AS [Formal Wing JA Sign Date], 
						 formal_wing_ja_sig.nameAndRank AS [Formal Wing JA], CONVERT(char(11), med_off_sig.date, 101) 
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
                         w.statusId AS [Status Code], cs.STATE, a.from_unit AS Wing, f_bsg.explanation AS 'Remarks - Wing CC', CASE WHEN pas.priority >= 60000 THEN 'P1a' WHEN pas.priority BETWEEN 50000 AND 
                         59999 THEN 'P1b' WHEN pas.priority BETWEEN 40000 AND 49999 THEN 'P2' ELSE '' END AS Priority 

						,au.CaseID AS [Audit CaseID]      ,au.Workflow AS [Audit Workflow]      ,CASE au.MedicallyAppropriate WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - SG (Medically Appropriate)]      ,au.SG_DX AS [Audit - SG (DX)]
						,au.SG_ISupport AS [Audit - SG (Insufficient Records)]      ,au.SG_EPTS AS [Audit - SG (EPTS not Accurate)]      ,au.SG_Aggravation AS [Audit - SG (Aggravation not accurate)]      ,au.SG_Principles AS [Audit - SG (Principles)]
						,au.SG_Other AS [Audit - SG (Other)]      ,CASE au.SG_ProofApplied WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Standard of Proof applied) - SG]      ,CASE au.SG_CorrectStandard WHEN 1 THEN 'Clear and Unmistakable' WHEN 0 THEN 'Preponderance of Proof' ELSE '' END AS [Audit - Universal (Correct Standard of Proof) - SG]      ,CASE au.SG_ProofMet WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Standard of Proof met) - SG]
						,CASE au.SG_Evidence WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Evidence for LOD) - SG]      ,CASE au.SG_Misconduct WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Misconduct) - SG]      ,CASE au.SG_FormalInvestigation WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Formal warranted) - SG]      ,CASE au.LegallySufficient WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - JA (Legally Appropriate)]
						,au.JA_StandardOfProof AS [Audit - JA (Standard of Proof)]      ,au.JA_DeathAndMVA AS [Audit - JA (Death and MVA)]      ,au.JA_FormalPolicy AS [Audit - JA (Formal Required)]      ,au.JA_FormalPolicy AS [Audit - JA (AFI not adhered too)]
						,au.JA_Other AS [Audit - JA (Other)]      ,CASE au.JA_ProofApplied WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Standard of Proof applied) - JA]      ,CASE au.JA_CorrectStandard WHEN 1 THEN 'Clear and Unmistakable' WHEN 0 THEN 'Proponderance of Proof' ELSE '' END AS [Audit - Universal (Correct Standard of Proof) - JA]
						,CASE au.JA_ProofMet WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Standard of Proof met) - JA]      ,CASE au.JA_Evidence WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Evidence for LOD) - JA]      ,CASE au.JA_Misconduct WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Misconduct) - JA]      ,CASE au.JA_FormalInvestigation WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - Universal (Formal warranted) - JA]
						,CASE au.StatusValidated WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - A1 (Validated and Accurate)]      ,au.StatusOfMember AS [Audit - A1 (Correct Status of Member)]      ,au.Orders AS [Audit - A1 (Orders not verified)]      ,au.A1_EPTS AS [Audit - A1 (EPTS)]      ,au.IDT AS [Audit - A1 (IDT Status)]
						,au.PCARS AS [Audit - A1 (PCARS not attached)]      ,au.EightYearRule AS [Audit - A1 (8-year rule)]      ,au.A1_Other AS [Audit - A1 (Other)],  CASE au.determination WHEN 0 THEN 'NO' WHEN 1 THEN 'YES' ELSE '' END AS [Audit - A1 (Correct Finding)], CASE au.A1_DeterminationNotCorrect WHEN 0 THEN
						 'ILOD' WHEN 1 THEN 'NILOD' WHEN 2 THEN 'Direct Formal' ELSE '' END AS [Audit - A1 (Desired Finding)],     CASE au.LODInitiation WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - SAF/MRR (Member Signed Initiation)]      ,CASE au.WrittenDiagnosis WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END  AS [Audit - SAF/MRR (Written DX)]
						,CASE au.MemberRequest WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - SAF/MRR (Request by 180 days)]      ,CASE au.IncurredOrAggravated WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - SAF/MRR (Incurred or Aggravated)]      ,CASE au.IllnessOrDisease WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END AS [Audit - SAF/MRR (EPTS)]      ,CASE au.Activites WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE 'N/A' END AS [Audit - SAF/MRR (EPTS Worsened)]
						,au.A1_Comment AS [Audit - A1 (Comments)]      ,au.JA_Comment AS [Audit - JA (Comments)]      ,au.SG_Comment AS [Audit - SG (Comments)]

						--, jaCon.endDate AS [Wing JA Sign Date], jaCon.name AS [Wing JA]				--Modified 08Nov2021 start		
						--,CASE cd.role WHEN 'Board Medical' Then cd.comments ELSE '' END  AS [CD - Board Medical (Comments)]		
						--,CASE cd.role WHEN 'Board Medical' Then cd.created_date ELSE NULL END  AS [CD - Board Medical (Date)]		
						--,CASE cd.role WHEN 'Board Legal' Then cd.comments ELSE '' END  AS [CD - Board Legal (Comments)]		
						--,CASE cd.role WHEN 'Board Legal' Then cd.created_date ELSE NULL END  AS [CD - Board Legal (Date)]		
						,ISNULL((Select comments from dbo.CaseDialogue_Comments x where role = 'Board Medical' and x.lodid = cd.lodid), '') AS [CD - Board Medical (Comments)]
						,ISNULL((Select created_date from dbo.CaseDialogue_Comments x where role = 'Board Medical' and x.lodid = cd.lodid), '') AS [CD - Board Medical (Date)]
						,ISNULL((Select comments from dbo.CaseDialogue_Comments x where role = 'Board Legal' and x.lodid = cd.lodid), '') AS [CD - Board Legal (Comments)]
						,ISNULL((Select created_date from dbo.CaseDialogue_Comments x where role = 'Board Legal' and x.lodid = cd.lodid), '') AS [CD - Board Legal (Date)]
				--Modified 08Nov2021 end
		

FROM            dbo.Form348 AS a left JOIN
						 dbo.CaseDialogue_Comments AS cd on cd.lodid = a.lodid --AND cd.Role in ('Board Medical', 'Board Legal' ) 
						 INNER JOIN
						 dbo.FORM348_Audit AS au ON au.lod_Id = a.lodId INNER JOIN
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
						 dbo.core_lkUpFindings AS find_baa ON find_baa.Id = f_baa.finding 
--				--Modified 08Nov2021 removed start	
						
--						LEFT OUTTER JOIN dbo.core_WorkStatus_Tracking wst on wst.refId = a.lodId --and wst.
						
--						 LEFT OUTER JOIN 
--						 (Select CASE WHEN Form348.case_id IS NOT NULL THEN Form348.case_id ELSE CASE WHEN Form348_SARC.case_id IS NOT NULL THEN Form348_SARC.case_id ELSE CASE WHEN Form348_RR.Case_Id IS NOT NULL
--                          THEN Form348_RR.Case_Id ELSE CASE WHEN Form348_SC.Case_Id IS NOT NULL THEN Form348_SC.Case_Id ELSE CASE WHEN Form348_AP.Case_Id IS NOT NULL 
--                         THEN Form348_AP.Case_Id ELSE CASE WHEN Form348_AP_SARC.Case_Id IS NOT NULL THEN Form348_AP_SARC.Case_Id END END END END END END As CaseId,  MAX(wst.endDate)  as 'endDate', Max(wst.name) as 'name'
--FROM            dbo.core_WorkStatus_Tracking AS wst INNER JOIN
--                         dbo.core_WorkStatus AS ws ON wst.ws_id = ws.ws_id INNER JOIN
--                         dbo.core_StatusCodes AS sc ON ws.statusId = sc.statusId INNER JOIN
--                         dbo.core_Users AS u ON wst.completedBy = u.userID LEFT OUTER JOIN
--                         dbo.Form348 ON dbo.Form348.lodId = wst.refId AND wst.workflowId IN (1, 27) LEFT OUTER JOIN
--                         dbo.Form348_SARC ON dbo.Form348_SARC.sarc_id = wst.refId AND wst.workflowId IN (28) LEFT OUTER JOIN
--                         dbo.Form348_RR ON dbo.Form348_RR.request_id = wst.refId AND wst.workflowId IN (5) LEFT OUTER JOIN
--                         dbo.Form348_SC ON dbo.Form348_SC.SC_Id = wst.refId AND wst.workflowId IN (23, 15, 12, 6, 21, 7, 24, 18, 30, 19, 25, 13, 22, 16, 11, 20, 14, 8) LEFT OUTER JOIN
--                         dbo.Form348_AP ON dbo.Form348_AP.appeal_id = wst.refId AND wst.workflowId IN (26) LEFT OUTER JOIN
--                         dbo.Form348_AP_SARC ON dbo.Form348_AP_SARC.appeal_sarc_id = wst.refId AND wst.workflowId IN (29)
--						 Group By Form348.case_id, Form348_SARC.case_id, Form348_RR.Case_Id
--						 ,Form348_SC.Case_Id, Form348_AP.Case_Id, Form348_AP_SARC.Case_Id, wst.name) jaCon on jaCon.CaseId = a.case_id


						 
--				-Modified 08Nov2021 removed end

						 LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Med Officer Review (Pilot)')) AS med_off_sig ON med_off_sig.refId = a.lodId AND med_off_sig.workflowId = a.workflow LEFT OUTER JOIN
                             (SELECT        smd.date, smd.nameAndRank, smd.refId, smd.workflowId
                               FROM            dbo.core_SignatureMetaData AS smd INNER JOIN
                                                         dbo.vw_WorkStatus AS ws ON ws.ws_id = smd.workStatus
                               WHERE        (ws.description = 'Unit Commander Review (Pilot)')) AS unit_commander_sig ON unit_commander_sig.refId = a.lodId AND unit_commander_sig.workflowId = a.workflow LEFT OUTER JOIN
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
                               WHERE        (ws.description = 'Appointing Authority Action (Pilot)')) AS wing_cc_sig ON wing_cc_sig.refId = a.lodId AND wing_cc_sig.workflowId = a.workflow LEFT OUTER JOIN
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
WHERE        (w.isCancel = 0) --or 
--a.case_id = '20210511-001'
GO

