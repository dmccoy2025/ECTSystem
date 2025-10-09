


CREATE VIEW [dbo].[vw_appeal_sarc]
AS
SELECT	ap.case_id AS [Case Id], 
		ap.appeal_sarc_id AS [Appeal Id], 
		ap.initial_id AS [Initial Id],
		ap.initial_workflow AS [Initial workflow], 
		w.description AS Status, 
		CASE w.isFinal WHEN NULL THEN 'Yes' ELSE 'No' END AS Closed,
		CONVERT(char(11), ISNULL(track.ReceiveDate, ap.created_date), 101) AS [Date Of Status],
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, ap.created_date), GETDATE()) ELSE 0 END AS Days,
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(ap.created_date, GETDATE()), GETDATE()) ELSE 0 END AS [Days Open],
		CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(ap.created_date, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days],
		ap.member_name AS [Member Name], 
		RIGHT(ap.member_ssn, 4) AS [Member SSN], 
		dbo.InitCap(g.RANK) AS [Member Rank],
		g.GRADE AS [Member Grade], 
		ap.member_unit AS [Member Unit], 
		ap.member_unit_id AS [Member Unit ID], 
		CONVERT(char(11), a.member_DOB, 101) AS [Member DOB], 
		CONVERT(char(11), ap.created_date, 101) AS [Date Created], 
		CONVERT(char(11), ap.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(ap.created_by) AS [Created By], 
		dbo.FormatName(ap.modified_by) AS [Modified By],
		comp.CompletedDate AS [Date Completed], 
		CASE WHEN ap.cancel_reason IS NULL THEN 'No' ELSE 'Yes' END AS [Cancelled], 
		canc.Description AS [Cancel Reason], 

		CASE w.isFinal WHEN 0 THEN NULL ELSE find_AppellateAuthority.Description END AS [Final Decision],
		CASE w.isFinal WHEN 0 THEN NULL ELSE find_AppellateAuthority.Id END AS [FinalDecisionId],

		CONVERT(char(11), wing_sarc_sig.date, 101) AS [Wing SARC/RSL Sign Date], 
		wing_sarc_sig.date AS [Wing SARC/RSL],

		CONVERT(char(11), sarc_admin_sig.date, 101) AS [SARC Administrator Sign Date], 
		sarc_admin_sig.nameAndRank AS [SARC Administrator],

		CONVERT(char(11), appellate_auth_sig.date, 101) AS [Appellate Authority Sign Date], 
		appellate_auth_sig.nameAndRank AS [Approving Authority],

		CONVERT(char(11), board_medical_sig.date, 101) AS [Board Medical Sign Date], 
		board_medical_sig.nameAndRank AS [Board Medical],

		CONVERT(char(11), board_legal_sig.date, 101) AS [Board JA Sign Date], 
		board_legal_sig.nameAndRank AS [Board JA],

		CONVERT(char(11), board_admin_sig.date, 101) AS [Board Administrator Sign Date], 
		board_admin_sig.nameAndRank AS [Board Administrator],

		find_SARCAdmin.Id AS [SARCAdminFindingId],
		find_SARCAdmin.Description AS [Finding - SARC Admin],
		f_SARCAdmin.remarks AS [SARC Admin Comment],
		CASE f_SARCAdmin.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [SARC Admin - Is Legacy Finding],

		find_BoardMed.Id AS [BoardMedFindingId],
		find_BoardMed.Description AS [Finding - Board Medical],
		f_BoardMed.remarks AS [Board Medical Comment],
		CASE f_BoardMed.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Medical - Is Legacy Finding],

		find_BoardJA.Id AS [BoardJAFindingId],
		find_BoardJA.Description AS [Finding - Board JA],
		f_BoardJA.remarks AS [Board JA Comment],
		CASE f_BoardJA.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board JA - Is Legacy Finding],

		find_BoardAdmin.Id AS [BoardAdminFindingId],
		find_BoardAdmin.Description AS [Finding - Board Admin],
		f_BoardAdmin.remarks AS [Board Admin Comment],
		CASE f_BoardAdmin.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Admin - Is Legacy Finding],

		find_AppellateAuthority.Id AS [AppellateAuthorityFindingId],
		find_AppellateAuthority.Description AS [Finding - Appellate Authority],
		f_AppellateAuthority.remarks AS [Appellate Authority Comment],
		CASE f_AppellateAuthority.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Appellate Authority - Is Legacy Finding],

		CASE ap.member_notified WHEN 1 THEN 'Yes' ELSE 'No' END AS [Member Notified], 
		w.ws_id AS [WorkStatusId], 
		ap.created_by, 
		ap.member_unit_id, 
		w.isFinal, 
		ap.workflow AS workflowId,
		w.statusId AS [Status Code]

FROM	dbo.Form348_AP_SARC ap
		INNER JOIN dbo.vw_WorkStatus AS w ON w.ws_id = ap.status
		INNER JOIN dbo.Form348 AS a ON a.lodId = ap.initial_id AND a.workflow = ap.initial_workflow
		LEFT OUTER JOIN dbo.core_lkupGrade AS g ON g.CODE = ap.member_grade
		LEFT OUTER JOIN(
			SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
			FROM	dbo.core_WorkStatus_Tracking
			WHERE	module = 24
			GROUP BY ws_id, refId
		) AS track ON track.refId = ap.appeal_sarc_id AND track.ws_id = ap.status
		LEFT OUTER JOIN(
			SELECT	refId, MAX(endDate) AS CompletedDate
			FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1
					INNER JOIN dbo.vw_WorkStatus AS ws ON core_WorkStatus_Tracking_1.ws_id = ws.ws_id
			WHERE	module = 26 AND ws.isFinal = 1 AND ws.description NOT LIKE '%Canceled'
			GROUP BY refId
		) AS comp ON comp.refId = ap.appeal_sarc_id

		LEFT JOIN FORM348_AP_SARC_findings AS f_SARCAdmin ON ap.appeal_sarc_id = f_SARCAdmin.appeal_id AND f_SARCAdmin.ptype = 26
		LEFT JOIN core_lkUpFindings AS find_SARCAdmin ON f_SARCAdmin.finding = find_SARCAdmin.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_BoardMed ON ap.appeal_sarc_id = f_BoardMed.appeal_id AND f_BoardMed.ptype = 8
		LEFT JOIN core_lkUpFindings AS find_BoardMed ON f_BoardMed.finding = find_BoardMed.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_BoardJA ON ap.appeal_sarc_id = f_BoardJA.appeal_id AND f_BoardJA.ptype = 7
		LEFT JOIN core_lkUpFindings AS find_BoardJA ON f_BoardJA.finding = find_BoardJA.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_BoardAdmin ON ap.appeal_sarc_id = f_BoardAdmin.appeal_id AND f_BoardAdmin.ptype = 22
		LEFT JOIN core_lkUpFindings AS find_BoardAdmin ON f_BoardAdmin.finding = find_BoardAdmin.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_AppellateAuthority ON ap.appeal_sarc_id = f_AppellateAuthority.appeal_id AND f_AppellateAuthority.ptype = 24
		LEFT JOIN core_lkUpFindings AS find_AppellateAuthority ON f_AppellateAuthority.finding = find_AppellateAuthority.Id

		LEFT JOIN core_lkupCancelReason AS canc ON ap.cancel_reason = canc.Id
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Wing SARC Initiate'
					) AS wing_sarc_sig ON wing_sarc_sig.refid = ap.appeal_sarc_id AND wing_sarc_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'SARC Admin Review'
					) AS sarc_admin_sig ON sarc_admin_sig.refid = ap.appeal_sarc_id AND sarc_admin_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board Medical Review'
					) AS board_medical_sig ON board_medical_sig.refid = ap.appeal_sarc_id AND board_medical_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board JA Review'
					) AS board_legal_sig ON board_legal_sig.refid = ap.appeal_sarc_id AND board_legal_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appellate Authority Review'
					) AS appellate_auth_sig ON appellate_auth_sig.refid = ap.appeal_sarc_id AND appellate_auth_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board Administrator Review'
					) AS board_admin_sig ON board_admin_sig.refid = ap.appeal_sarc_id AND board_admin_sig.workflowid = ap.workflow
		
WHERE	w.description not like '%Cancelled'

UNION

SELECT	ap.case_id AS [Case Id], 
		ap.appeal_sarc_id AS [Appeal Id], 
		ap.initial_id AS [Initial Id],
		ap.initial_workflow AS [Initial workflow], 
		w.description AS Status, 
		CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS Closed,
		CONVERT(char(11), ISNULL(track.ReceiveDate, ap.created_date), 101) AS [Date Of Status],
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, ap.created_date), GETDATE()) ELSE 0 END AS Days,
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(ap.created_date, GETDATE()), GETDATE()) ELSE 0 END AS [Days Open],
		CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(ap.created_date, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days],
		ap.member_name AS [Member Name], 
		RIGHT(ap.member_ssn, 4) AS [Member SSN], 
		dbo.InitCap(g.RANK) AS [Member Rank],
		g.GRADE AS [Member Grade], 
		ap.member_unit AS [Member Unit], 
		ap.member_unit_id AS [Member Unit ID], 
		CONVERT(char(11), sarc.member_DOB, 101) AS [Member DOB], 
		CONVERT(char(11), ap.created_date, 101) AS [Date Created], 
		CONVERT(char(11), ap.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(ap.created_by) AS [Created By], 
		dbo.FormatName(ap.modified_by) AS [Modified By],
		comp.CompletedDate AS [Date Completed], 
		CASE WHEN ap.cancel_reason IS NULL THEN 'No' ELSE 'Yes' END AS [Cancelled], 
		canc.Description AS [Cancel Reason], 

		CASE w.isFinal WHEN 0 THEN NULL ELSE find_AppellateAuthority.Description END AS [Final Decision],
		CASE w.isFinal WHEN 0 THEN NULL ELSE find_AppellateAuthority.Id END AS [FinalDecisionId],

		CONVERT(char(11), wing_sarc_sig.date, 101) AS [Wing SARC/RSL Sign Date], 
		wing_sarc_sig.date AS [Wing SARC/RSL],

		CONVERT(char(11), sarc_admin_sig.date, 101) AS [SARC Administrator Sign Date], 
		sarc_admin_sig.nameAndRank AS [SARC Administrator],

		CONVERT(char(11), appellate_auth_sig.date, 101) AS [Appellate Authority Sign Date], 
		appellate_auth_sig.nameAndRank AS [Approving Authority],

		CONVERT(char(11), board_medical_sig.date, 101) AS [Board Medical Sign Date], 
		board_medical_sig.nameAndRank AS [Board Medical],

		CONVERT(char(11), board_legal_sig.date, 101) AS [Board JA Sign Date], 
		board_legal_sig.nameAndRank AS [Board JA],

		CONVERT(char(11), board_admin_sig.date, 101) AS [Board Administrator Sign Date], 
		board_admin_sig.nameAndRank AS [Board Administrator],

		find_SARCAdmin.Id AS [SARCAdminFindingId],
		find_SARCAdmin.Description AS [Finding - SARC Admin],
		f_SARCAdmin.remarks AS [SARC Admin Comment],
		CASE f_SARCAdmin.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [SARC Admin - Is Legacy Finding],

		find_BoardMed.Id AS [BoardMedFindingId],
		find_BoardMed.Description AS [Finding - Board Medical],
		f_BoardMed.remarks AS [Board Medical Comment],
		CASE f_BoardMed.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Medical - Is Legacy Finding],

		find_BoardJA.Id AS [BoardJAFindingId],
		find_BoardJA.Description AS [Finding - Board JA],
		f_BoardJA.remarks AS [Board JA Comment],
		CASE f_BoardJA.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board JA - Is Legacy Finding],

		find_BoardAdmin.Id AS [BoardAdminFindingId],
		find_BoardAdmin.Description AS [Finding - Board Admin],
		f_BoardAdmin.remarks AS [Board Admin Comment],
		CASE f_BoardAdmin.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Admin - Is Legacy Finding],

		find_AppellateAuthority.Id AS [AppellateAuthorityFindingId],
		find_AppellateAuthority.Description AS [Finding - Appellate Authority],
		f_AppellateAuthority.remarks AS [Appellate Authority Comment],
		CASE f_AppellateAuthority.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Appellate Authority - Is Legacy Finding],

		CASE ap.member_notified WHEN 1 THEN 'Yes' ELSE 'No' END AS [Member Notified], 
		w.ws_id AS [WorkStatusId], 
		ap.created_by, 
		ap.member_unit_id, 
		w.isFinal, 
		ap.workflow AS workflowId,
		w.statusId AS [Status Code]

FROM	dbo.Form348_AP_SARC ap
		INNER JOIN dbo.vw_WorkStatus AS w ON w.ws_id = ap.status
		INNER JOIN dbo.Form348_SARC AS sarc ON sarc.sarc_id = ap.initial_id AND sarc.workflow = ap.initial_workflow
		LEFT OUTER JOIN dbo.core_lkupGrade AS g ON g.CODE = ap.member_grade
		LEFT OUTER JOIN(
			SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
			FROM	dbo.core_WorkStatus_Tracking
			WHERE	module = 24
			GROUP BY ws_id, refId
		) AS track ON track.refId = ap.appeal_sarc_id AND track.ws_id = ap.status
		LEFT OUTER JOIN(
			SELECT	refId, MAX(endDate) AS CompletedDate
			FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1
					INNER JOIN dbo.vw_WorkStatus AS ws ON core_WorkStatus_Tracking_1.ws_id = ws.ws_id
			WHERE	module = 26 AND ws.isFinal = 1 AND ws.description NOT LIKE '%Canceled'
			GROUP BY refId
		) AS comp ON comp.refId = ap.appeal_sarc_id

		LEFT JOIN FORM348_AP_SARC_findings AS f_SARCAdmin ON ap.appeal_sarc_id = f_SARCAdmin.appeal_id AND f_SARCAdmin.ptype = 26
		LEFT JOIN core_lkUpFindings AS find_SARCAdmin ON f_SARCAdmin.finding = find_SARCAdmin.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_BoardMed ON ap.appeal_sarc_id = f_BoardMed.appeal_id AND f_BoardMed.ptype = 8
		LEFT JOIN core_lkUpFindings AS find_BoardMed ON f_BoardMed.finding = find_BoardMed.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_BoardJA ON ap.appeal_sarc_id = f_BoardJA.appeal_id AND f_BoardJA.ptype = 7
		LEFT JOIN core_lkUpFindings AS find_BoardJA ON f_BoardJA.finding = find_BoardJA.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_BoardAdmin ON ap.appeal_sarc_id = f_BoardAdmin.appeal_id AND f_BoardAdmin.ptype = 22
		LEFT JOIN core_lkUpFindings AS find_BoardAdmin ON f_BoardAdmin.finding = find_BoardAdmin.Id

		LEFT JOIN FORM348_AP_SARC_findings AS f_AppellateAuthority ON ap.appeal_sarc_id = f_AppellateAuthority.appeal_id AND f_AppellateAuthority.ptype = 24
		LEFT JOIN core_lkUpFindings AS find_AppellateAuthority ON f_AppellateAuthority.finding = find_AppellateAuthority.Id

		LEFT JOIN core_lkupCancelReason AS canc ON ap.cancel_reason = canc.Id
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Wing SARC Initiate'
					) AS wing_sarc_sig ON wing_sarc_sig.refid = ap.appeal_sarc_id AND wing_sarc_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'SARC Admin Review'
					) AS sarc_admin_sig ON sarc_admin_sig.refid = ap.appeal_sarc_id AND sarc_admin_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board Medical Review'
					) AS board_medical_sig ON board_medical_sig.refid = ap.appeal_sarc_id AND board_medical_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board JA Review'
					) AS board_legal_sig ON board_legal_sig.refid = ap.appeal_sarc_id AND board_legal_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appellate Authority Review'
					) AS appellate_auth_sig ON appellate_auth_sig.refid = ap.appeal_sarc_id AND appellate_auth_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board Administrator Review'
					) AS board_admin_sig ON board_admin_sig.refid = ap.appeal_sarc_id AND board_admin_sig.workflowid = ap.workflow
WHERE	w.description not like '%Cancelled'
GO

