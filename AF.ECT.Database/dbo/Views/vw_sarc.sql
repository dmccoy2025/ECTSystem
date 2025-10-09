

CREATE VIEW [dbo].[vw_sarc]
AS
SELECT	sarc.sarc_id,
		sarc.case_id AS [Case Id],
		w.description AS [Status],
		CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS [Closed], 
		CONVERT(char(11), ISNULL(track.ReceiveDate, sarc.created_date), 101) AS [Date Of Status], 
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, sarc.created_date), GETDATE()) ELSE 0 END AS [Days], 
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(sarc.created_date, GETDATE()), GETDATE()) ELSE 0 END AS [Days Open],
		CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(sarc.created_date, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days],
		sarc.member_name AS [Member Name], 
		RIGHT(sarc.member_ssn, 4) AS [Member SSN], 
		dbo.InitCap(g.RANK) AS [Member Rank], 
		g.GRADE AS [Member Grade], 
		sarc.member_unit AS [Member Unit],
		CONVERT(char(11), sarc.member_DOB, 101) AS [Member DOB], 
		CONVERT(char(11), sarc.created_date, 101) AS [Date Created], 
		CONVERT(char(11), sarc.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(sarc.created_by) AS [Created By], 
		dbo.FormatName(sarc.modified_by) AS [Modified By], 
		comp.CompletedDate AS [Date Completed], 
		
		CASE w.isCancel WHEN 0 THEN 'No' ELSE 'Yes' END AS [Cancelled], 
		canc.Description AS [Cancel Reason], 

		CONVERT(char(11), sarc.incident_date, 101) AS [Incident Date],
		CONVERT(char(11), sarc.duration_start_date, 101) + ' ' + CONVERT(char(11), sarc.duration_start_date, 108) AS [Duration Of Orders Start],
		CONVERT(char(11), sarc.duration_end_date, 101) + ' ' + CONVERT(char(11), sarc.duration_end_date, 108) AS [Duration Of Orders End],
		mc.component_description AS [Duty Status],
		CASE sarc.in_duty_status WHEN 1 THEN 'Yes' ELSE 'No' END AS [In Duty Status],

		CASE sarc.ICD_E968_8 WHEN 1 THEN 'Yes' ELSE 'No' END AS [E968.8],
		CASE ICD_E969_9 WHEN 1 THEN 'Yes' ELSE 'No' END AS [E969.9],
		CASE ICD_other WHEN 1 THEN 'Yes' ELSE 'No' END AS [Other ICDs],

		CONVERT(char(11), wing_sarc_sig.date, 101) AS [Wing SARC/RSL Sign Date], 
		wing_sarc_sig.nameAndRank AS [Wing SARC/RSL],

		CONVERT(char(11), sarc_admin_sig.date, 101) AS [SARC Administrator Sign Date], 
		sarc_admin_sig.nameAndRank AS [SARC Administrator],

		CONVERT(char(11), approval_sig.date, 101) AS [Approving Authority Sign Date], 
		approval_sig.nameAndRank AS [Approving Authority],

		CONVERT(char(11), board_medical_sig.nameAndRank, 101) AS [Board Medical Sign Date], 
		board_medical_sig.nameAndRank AS [Board Medical],

		CONVERT(char(11), board_legal_sig.date, 101) AS [Board JA Sign Date], 
		board_legal_sig.nameAndRank AS [Board JA],

		CONVERT(char(11), board_admin_sig.date, 101) AS [Board Administrator Sign Date], 
		board_admin_sig.nameAndRank AS [Board Administrator],

		CASE w.isFinal WHEN 0 THEN NULL ELSE find_ApprovingAuthority.Description END AS [Final Decision],
		CASE w.isFinal WHEN 0 THEN NULL ELSE find_ApprovingAuthority.Id END AS [FinalDecisionId],

		find_SARCAdmin.Id AS [SARCAdminFindingId],
		find_SARCAdmin.Description AS [Finding - SARC Admin],

		find_ApprovingAuthority.Id AS [ApprovingAuthorityFindingId],
		find_ApprovingAuthority.Description AS [Finding - Approving Authority],

		find_BoardAdmin.Id AS [BoardAdminFindingId],
		find_BoardAdmin.Description AS [Finding - Board Admin],

		find_BoardMed.Id AS [BoardMedFindingId],
		find_BoardMed.Description AS [Finding - Board Medical],

		find_BoardJA.Id AS [BoardJAFindingId],
		find_BoardJA.Description AS [Finding - Board JA],
		
		sarc.workflow AS [WorkflowId],
		sarc.created_by,
		sarc.member_unit_id,
		sarc.in_duty_status AS [InDutyStatusFlag],
		sarc.duty_status AS [DutyStatusComponentId],
		sarc.ICD_E968_8 AS [E968Flag],
		sarc.ICD_E969_9 AS [E969Flag],
		sarc.ICD_other AS [ICDOtherFlag],
		OtherICDs.ICD_IDs_CSV,
		OtherICDs.ICD_Descriptions_CSV AS [Other ICD Descriptions],

		w.isFinal AS [IsFinal],
		w.ws_id AS [WorkStatusId],
		w.statusId AS [Status Code]

FROM	Form348_SARC sarc
		JOIN vw_WorkStatus AS w ON sarc.status = w.ws_id

		LEFT JOIN FORM348_SARC_findings AS f_SARCAdmin ON sarc.sarc_id = f_SARCAdmin.SARC_ID AND f_SARCAdmin.ptype = 26
		LEFT JOIN core_lkUpFindings AS find_SARCAdmin ON f_SARCAdmin.finding = find_SARCAdmin.Id

		LEFT JOIN FORM348_SARC_findings AS f_ApprovingAuthority ON sarc.sarc_id = f_ApprovingAuthority.SARC_ID AND f_ApprovingAuthority.ptype = 10
		LEFT JOIN core_lkUpFindings AS find_ApprovingAuthority ON f_ApprovingAuthority.finding = find_ApprovingAuthority.Id

		LEFT JOIN FORM348_SARC_findings AS f_BoardAdmin ON sarc.sarc_id = f_BoardAdmin.SARC_ID AND f_BoardAdmin.ptype = 22
		LEFT JOIN core_lkUpFindings AS find_BoardAdmin ON f_BoardAdmin.finding = find_BoardAdmin.Id

		LEFT JOIN FORM348_SARC_findings AS f_BoardMed ON sarc.sarc_id = f_BoardMed.SARC_ID AND f_BoardMed.ptype = 8
		LEFT JOIN core_lkUpFindings AS find_BoardMed ON f_BoardMed.finding = find_BoardMed.Id

		LEFT JOIN FORM348_SARC_findings AS f_BoardJA ON sarc.sarc_id = f_BoardJA.SARC_ID AND f_BoardJA.ptype = 7
		LEFT JOIN core_lkUpFindings AS find_BoardJA ON f_BoardJA.finding = find_BoardJA.Id

		LEFT JOIN core_lkupGrade AS g ON sarc.member_grade = g.CODE
		LEFT JOIN core_lkupCancelReason AS canc ON sarc.cancel_reason = canc.Id
		LEFT JOIN core_lkupComponent AS mc ON sarc.duty_status = mc.component_id

		LEFT JOIN (
			SELECT	distinct t1.SARCId,
					SUBSTRING(
						(
							SELECT	',' + CONVERT(VARCHAR(10), t2.ICDCodeId)
							FROM	Form348_SARC_ICD AS t2
							WHERE	t2.SARCId = t1.SARCId
							FOR XML PATH('')
						), 
						2, 200000
					) ICD_IDs_CSV,
					SUBSTRING(
						(
							SELECT	'; ' + code.text
							FROM	Form348_SARC_ICD AS t2
									JOIN core_lkupICD9 AS code ON t2.ICDCodeId = code.ICD9_ID
							WHERE	t2.SARCId = t1.SARCId
							FOR XML PATH('')
						), 
						2, 200000
					) ICD_Descriptions_CSV
			FROM	Form348_SARC_ICD AS t1
		) AS OtherICDs ON sarc.sarc_id = OtherICDs.SARCId
		LEFT JOIN (
						SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
						FROM	dbo.core_WorkStatus_Tracking
						WHERE	module = 25
						GROUP BY ws_id, refId
					) AS track ON track.refId = sarc.sarc_id AND track.ws_id = sarc.status
		LEFT JOIN (
						SELECT	refId, MAX(endDate) AS CompletedDate
						FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 
								INNER JOIN dbo.vw_WorkStatus w ON core_WorkStatus_Tracking_1.ws_id = w.ws_id
						WHERE	module = 25 AND w.isFinal = 1 AND w.isCancel = 0
						GROUP BY refId
					) AS comp ON comp.refId = sarc.sarc_id
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Wing SARC Initiate'
					) AS wing_sarc_sig ON wing_sarc_sig.refid = sarc.sarc_id AND wing_sarc_sig.workflowid = sarc.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'SARC Admin Review'
					) AS sarc_admin_sig ON sarc_admin_sig.refid = sarc.sarc_id AND sarc_admin_sig.workflowid = sarc.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board Medical Review'
					) AS board_medical_sig ON board_medical_sig.refid = sarc.sarc_id AND board_medical_sig.workflowid = sarc.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board JA Review'
					) AS board_legal_sig ON board_legal_sig.refid = sarc.sarc_id AND board_legal_sig.workflowid = sarc.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Board Administrator Review'
					) AS board_admin_sig ON board_admin_sig.refid = sarc.sarc_id AND board_admin_sig.workflowid = sarc.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Approving Authority Review'
					) AS approval_sig ON approval_sig.refid = sarc.sarc_id AND approval_sig.workflowid = sarc.workflow
GO

