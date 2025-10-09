


CREATE VIEW [dbo].[vw_appeal]
AS
SELECT	ap.case_id AS [Case Id], 
		ap.appeal_id AS [Appeal Id], 
		ap.initial_lod_id AS [LOD Id], 
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
		CONVERT(char(11), a.member_DOB, 101) AS [Member DOB], 
		CONVERT(char(11), ap.created_date, 101) AS [Date Created], 
		CONVERT(char(11), ap.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(ap.created_by) AS [Created By], 
		dbo.FormatName(ap.modified_by) AS [Modified By],
		comp.CompletedDate AS [Date Completed],
		 
		CONVERT(char(11), LOD_pm_sig.date, 101) AS [LOD PM date], 
		LOD_pm_sig.nameAndRank AS [LOD PM Name],

		CONVERT(char(11), board_tech_sig.date, 101) AS [Board Tech Date], 
		board_tech_sig.nameAndRank AS [Board Tech Name],

		CONVERT(char(11), board_medical_sig.date, 101) AS [Board Medical Date], 
		board_medical_sig.nameAndRank AS [Board Medical Name],

		CONVERT(char(11), board_legal_sig.date, 101) AS [Board Legal Date], 
		board_legal_sig.nameAndRank AS [Board Legal Name],

		CONVERT(char(11), board_admin_sig.date, 101) AS [Board Admin Date], 
		board_admin_sig.nameAndRank AS [Board Admin Name],

		CONVERT(char(11), approval_sig.date, 101) AS [Approval Authority Date], 
		approval_sig.nameAndRank AS [Approval Authority Name],

		CONVERT(char(11), appellate_sig.date, 101) AS [Appellate Authority Date], 
		appellate_sig.nameAndRank AS [Appellate Authority Name],

		finding_BoardMed.Id AS [BoardMedFindingId],
		finding_BoardMed.Description AS [Finding - Board Medical],
		f_BoardMed.explanation AS [Board Medical Comment],
		CASE f_BoardMed.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Medical - Is Legacy Finding],

		finding_BoardJA.Id AS [BoardLegalFindingId],
		finding_BoardJA.Description AS [Finding - Board Legal],
		f_BoardJA.explanation AS [Board Legal Comment],
		CASE f_BoardJA.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Legal - Is Legacy Finding],

		finding_BoardAdmin.Id AS [BoardAdminFindingId],
		finding_BoardAdmin.Description AS [Finding - Board Admin],
		f_BoardAdmin.explanation AS [Board Admin Comment],
		CASE f_BoardAdmin.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Board Admin - Is Legacy Finding],

		finding_ApprovingAuthority.Id AS [ApprovalAuthorityFindingId],
		finding_ApprovingAuthority.Description AS [Finding - Approval Authority],
		f_ApprovingAuthority.explanation AS [Approval Authority Comment], 
		CASE f_ApprovingAuthority.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Approval Authority - Is Legacy Finding],

		finding_AppellateAuthority.Id AS [AppellateAuthorityFindingId],
		finding_AppellateAuthority.Description AS [Finding - Appellate Authority],
		f_AppellateAuthority.explanation AS [Appellate Authority Comment], 
		CASE f_AppellateAuthority.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Appellate Authority - Is Legacy Finding],

		CASE ap.member_notified WHEN 1 THEN 'Yes' ELSE 'No' END AS [Member Notified], 
		w.ws_id, 
		ap.created_by, 
		ap.member_unit_id, 
		w.isFinal, 
		ap.workflow AS workflowId,
		w.statusId AS [Status Code]

FROM	dbo.Form348_AP ap
		INNER JOIN dbo.vw_WorkStatus AS w ON w.ws_id = ap.status
		INNER JOIN dbo.Form348 AS a ON a.lodId = ap.initial_lod_id
		LEFT OUTER JOIN dbo.core_lkupGrade AS g ON g.CODE = ap.member_grade
		LEFT OUTER JOIN(
							SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
							FROM	dbo.core_WorkStatus_Tracking
							WHERE	module = 24
							GROUP BY ws_id, refId
						) AS track ON track.refId = ap.appeal_id AND track.ws_id = ap.status
		LEFT OUTER JOIN(
							SELECT	refId, MAX(endDate) AS CompletedDate
							FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1
									INNER JOIN dbo.vw_WorkStatus AS ws ON core_WorkStatus_Tracking_1.ws_id = ws.ws_id
							WHERE	module = 24 AND ws.isFinal = 1 AND ws.description NOT LIKE '%Canceled'
							GROUP BY refId
						) AS comp ON comp.refId = ap.appeal_id
		
		LEFT JOIN Form348_AP_Findings	AS f_BoardMed		ON ap.appeal_id = f_BoardMed.appeal_id AND f_BoardMed.ptype = 8	-- (Board Medical Officer)
		LEFT JOIN core_lkUpFindings		AS finding_BoardMed	ON f_BoardMed.finding = finding_BoardMed.Id

		LEFT JOIN Form348_AP_Findings	AS f_BoardJA		ON ap.appeal_id = f_BoardJA.appeal_id AND f_BoardJA.ptype = 7	-- (Board JA)
		LEFT JOIN core_lkUpFindings		AS finding_BoardJA	ON f_BoardJA.finding = finding_BoardJA.Id

		LEFT JOIN Form348_AP_Findings	AS f_BoardAdmin			ON ap.appeal_id = f_BoardAdmin.appeal_id AND f_BoardAdmin.ptype = 22	-- (Board Admin (formerly Board A1))
		LEFT JOIN core_lkUpFindings		AS finding_BoardAdmin	ON f_BoardAdmin.finding = finding_BoardAdmin.Id

		LEFT JOIN Form348_AP_Findings	AS f_ApprovingAuthority			ON ap.appeal_id = f_ApprovingAuthority.appeal_id AND f_ApprovingAuthority.ptype = 10	-- (Approving Authority)
		LEFT JOIN core_lkUpFindings		AS finding_ApprovingAuthority	ON f_ApprovingAuthority.finding = finding_ApprovingAuthority.Id

		LEFT JOIN Form348_AP_Findings	AS f_AppellateAuthority			ON ap.appeal_id = f_AppellateAuthority.appeal_id AND f_AppellateAuthority.ptype = 24	-- (Appellate Authority)
		LEFT JOIN core_lkUpFindings		AS finding_AppellateAuthority	ON f_AppellateAuthority.finding = finding_AppellateAuthority.Id

		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Initiation'
					) AS LOD_pm_sig ON LOD_pm_sig.refid = ap.appeal_id AND LOD_pm_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Board Tech Review'
					) AS board_tech_sig ON board_tech_sig.refid = ap.appeal_id AND board_tech_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Board Medical Review'
					) AS board_medical_sig ON board_medical_sig.refid = ap.appeal_id AND board_medical_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Board Legal Review'
					) AS board_legal_sig ON board_legal_sig.refid = ap.appeal_id AND board_legal_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Board Admin Review'
					) AS board_admin_sig ON board_admin_sig.refid = ap.appeal_id AND board_admin_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Approving Authority Review'
					) AS approval_sig ON approval_sig.refid = ap.appeal_id AND approval_sig.workflowid = ap.workflow
		LEFT JOIN (
						SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
						FROM dbo.core_signaturemetadata smd
						JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
						WHERE ws.description = 'Appeal Appellate Authority Review'
					) AS appellate_sig ON appellate_sig.refid = ap.appeal_id AND appellate_sig.workflowid = ap.workflow
WHERE	w.description not like '%Cancelled'
GO

