

CREATE VIEW [dbo].[vw_rr]
AS
SELECT	a.request_id, 
		a.InitialLodId,
		a.ReinvestigationLodId,
		a.case_id AS [Case Id], 
		w.description AS Status, 
		CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS Closed, 
		CONVERT(char(11), ISNULL(track.ReceiveDate, a.CreatedDate), 101) AS [Date Of Status], 
		CASE w.isFinal WHEN 0 THEN datediff(d, ISNULL(track.ReceiveDate, a.CreatedDate), getdate()) ELSE 0 END AS Days, 
		CASE w.isFinal WHEN 0 THEN datediff(d, ISNULL(a.CreatedDate, getdate()), getdate()) ELSE 0 END AS [Days Open],
		CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(a.CreatedDate, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days],
		a.member_name AS [Member Name], 
		RIGHT(a.member_ssn, 4) AS [Member SSN], 
		dbo.InitCap(g.RANK) AS [Member Rank], 
		g.GRADE AS [Member Grade], 
		a.member_unit AS [Member Unit], 
		CONVERT(char(11), olod.member_DOB, 101) AS [Member DOB], 
		CONVERT(char(11), a.CreatedDate, 101)AS [Date Created], 
		CONVERT(char(11), a.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(a.CreatedBy) AS [Created By], 
		dbo.FormatName(a.modified_by) AS [Modified By], 
		CONVERT(char(11), comp.CompletedDate, 101) AS [Date Completed], 
		olodm.icd9Id AS [ICD-9],
		i.value AS [ICD Code],
		'[' + i.value + '] ' + i.text AS [ICD Text], 

		CONVERT(char(11), mpf_sig.date, 101) AS [MPF Sign Date], 
		mpf_sig.nameAndRank AS [MPF],
		
		CONVERT(char(11), LOD_pm_sig.date, 101) AS [LOD-PM Sign Date], 
		LOD_pm_sig.nameAndRank AS [LOD-PM],
		
		CONVERT(char(11), wing_ja_sig.date, 101) AS [Wing JA Sign Date], 
		wing_ja_sig.nameAndRank AS [Wing JA],
		
		CONVERT(char(11), wing_cc_sig.date, 101) AS [Wing CC Sign Date], 
		wing_cc_sig.nameAndRank AS [Wing CC],
		
		CONVERT(char(11), board_tech_sig.date, 101) AS [Board Tech Sign Date], 
		board_tech_sig.nameAndRank AS [Board Tech],
		
		CONVERT(char(11), board_medical_sig.date, 101) AS [Board SG Sign Date], 
		board_medical_sig.nameAndRank AS [Board SG],
		
		CONVERT(char(11), board_legal_sig.date, 101) AS [Board JA Sign Date], 
		board_legal_sig.nameAndRank AS [Board JA],
		
		CONVERT(char(11), approval_sig.date, 101) AS [Approval Sign Date], 
		approval_sig.nameAndRank AS [Approval Authority],

		finding_WingJA.Id AS [WingJAFindingId],
		finding_WingJA.Description AS [Finding - Wing JA],
		f_WingJA.explanation AS [Wing JA Comment],
		CASE f_WingJA.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Wing JA - Is Legacy Finding],

		finding_WingCC.Id AS [WingCCFindingId],
		finding_WingCC.Description AS [Finding - Wing CC],
		f_WingCC.explanation AS [Wing CC Comment],
		CASE f_WingCC.is_legacy_finding WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END AS [Wing CC - Is Legacy Finding],

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

		w.ws_id, 
		wf.workflowId,
		w.isFinal, 
		a.CreatedBy AS [created_by], 
		a.member_unit, 
		a.member_unit_id,
		w.statusId AS [Status Code]
FROM	dbo.Form348_RR AS a 
		INNER JOIN dbo.Form348 AS olod ON olod.lodId = a.InitialLodId 
		INNER JOIN dbo.Form348_Medical AS olodm ON olodm.lodid = olod.lodId 
		LEFT OUTER JOIN dbo.core_lkupICD9 AS i ON i.ICD9_ID = olodm.icd9Id 
		INNER JOIN dbo.core_lkupGrade AS g ON g.CODE = a.member_grade 
		INNER JOIN dbo.vw_WorkStatus AS w ON w.ws_id = a.status 
		INNER JOIN dbo.core_Workflow AS wf ON wf.workflowId = w.workflowId 
		INNER JOIN dbo.core_Users AS created ON created.userID = a.CreatedBy 
		INNER JOIN dbo.core_Users AS updated ON updated.userID = a.modified_by 
		LEFT OUTER JOIN (
							SELECT     MAX(startDate) AS ReceiveDate, ws_id, refId
							FROM          dbo.core_WorkStatus_Tracking
							WHERE	 module = 5
							GROUP BY ws_id, refId
						) AS track ON track.refId = a.request_id AND track.ws_id = a.status 
		LEFT OUTER JOIN (
							SELECT	refId, MAX(endDate) AS CompletedDate
							FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 INNER JOIN
									dbo.vw_WorkStatus ws on core_WorkStatus_Tracking_1.ws_id = ws.ws_id
							WHERE	module = 5 AND ((ws.description like '%Complete%') or (ws.description like '%Approve%') or (ws.description like '%Denied%'))
							GROUP BY refId
						) AS comp ON comp.refId = a.request_id
		LEFT JOIN Form348_RR_Findings	AS f_WingJA			ON a.request_id = f_WingJA.request_id AND f_WingJA.ptype = 4	-- (Wing JA)
		LEFT JOIN core_lkUpFindings		AS finding_WingJA	ON f_WingJA.finding = finding_WingJA.Id

		LEFT JOIN Form348_RR_Findings	AS f_WingCC			ON a.request_id = f_WingCC.request_id AND f_WingCC.ptype = 5	-- (Wing CC)
		LEFT JOIN core_lkUpFindings		AS finding_WingCC	ON f_WingCC.finding = finding_WingCC.Id

		LEFT JOIN Form348_RR_Findings	AS f_BoardMed		ON a.request_id = f_BoardMed.request_id AND f_BoardMed.ptype = 8	-- (Board Medical Officer)
		LEFT JOIN core_lkUpFindings		AS finding_BoardMed	ON f_BoardMed.finding = finding_BoardMed.Id

		LEFT JOIN Form348_RR_Findings	AS f_BoardJA		ON a.request_id = f_BoardJA.request_id AND f_BoardJA.ptype = 7	-- (Board JA)
		LEFT JOIN core_lkUpFindings		AS finding_BoardJA	ON f_BoardJA.finding = finding_BoardJA.Id

		LEFT JOIN Form348_RR_Findings	AS f_BoardAdmin			ON a.request_id = f_BoardAdmin.request_id AND f_BoardAdmin.ptype = 22	-- (Board Admin (formerly Board A1))
		LEFT JOIN core_lkUpFindings		AS finding_BoardAdmin	ON f_BoardAdmin.finding = finding_BoardAdmin.Id

		LEFT JOIN Form348_RR_Findings	AS f_ApprovingAuthority			ON a.request_id = f_ApprovingAuthority.request_id AND f_ApprovingAuthority.ptype = 10	-- (Approving Authority)
		LEFT JOIN core_lkUpFindings		AS finding_ApprovingAuthority	ON f_ApprovingAuthority.finding = finding_ApprovingAuthority.Id

		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Initiated'
						AND smd.userGroup = 96
					) AS LOD_pm_sig ON LOD_pm_sig.refid = a.request_id AND LOD_pm_sig.workflowid = a.workflow
		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Initiated'
						AND smd.userGroup = 13
					) AS mpf_sig ON mpf_sig.refid = a.request_id AND mpf_sig.workflowid = a.workflow
		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Wing JA'
					) AS wing_ja_sig ON wing_ja_sig.refid = a.request_id AND wing_ja_sig.workflowid = a.workflow
		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Wing CC'
					) AS wing_cc_sig ON wing_cc_sig.refid = a.request_id AND wing_cc_sig.workflowid = a.workflow
		LEFT JOIN (
					 SELECT MAX(smd.date) AS [date], smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN dbo.core_usergroups g ON g.groupid = smd.usergroup
					 WHERE g.name = 'Board Technician'
					 GROUP BY smd.refid, smd.workflowid

					) AS board_tech_lkupsig ON board_tech_lkupsig.refid = a.request_id AND board_tech_lkupsig.workflowid = a.workflow
		LEFT JOIN dbo.core_signaturemetadata AS board_tech_sig ON board_tech_sig.date = board_tech_lkupsig.date AND board_tech_sig.refId = board_tech_lkupsig.refId AND board_tech_sig.workflowId = board_tech_lkupsig.workflowId
		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Board Medical'
					) AS board_medical_sig ON board_medical_sig.refid = a.request_id AND board_medical_sig.workflowid = a.workflow
		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Board Legal'
					) AS board_legal_sig ON board_legal_sig.refid = a.request_id AND board_legal_sig.workflowid = a.workflow
		LEFT JOIN (
					 SELECT smd.date, smd.nameAndRank, smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN vw_WorkStatus ws on ws.ws_id = smd.workStatus
					 WHERE ws.description = 'Reinvestigation Request Approving Authority'
					) AS approval_sig ON approval_sig.refid = a.request_id AND approval_sig.workflowid = a.workflow
WHERE	(w.description not like '%Cancelled')
GO

