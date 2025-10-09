


CREATE VIEW [dbo].[vw_sc]
AS
SELECT	a.SC_Id, 
		a.case_id AS [Case Id], 
		w.description AS Status, 
		CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS Closed, 
		CONVERT(char(11), ISNULL(track.ReceiveDate, a.created_date), 101) AS [Date Of Status], 
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, a.created_date), GETDATE()) ELSE 0 END AS Days, 
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(a.created_date, GETDATE()), GETDATE()) ELSE 0 END AS [Days Open],
		CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(a.created_date, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days],
		a.member_name AS [Member Name], 
		RIGHT(a.member_ssn, 4) AS [Member SSN], 
		dbo.InitCap(g.RANK) AS [Member Rank], 
		g.GRADE AS [Member Grade], 
		a.member_unit AS [Member Unit], 
		CONVERT(char(11), a.member_DOB, 101) AS [Member DOB], 
		CONVERT(char(11), a.created_date, 101)AS [Date Created], 
		CONVERT(char(11), a.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(a.created_by) AS [Created By], 
		dbo.FormatName(a.modified_by) AS [Modified By], 
		CONVERT(char(11), comp.CompletedDate, 101) AS [Date Completed], 
		a.icd9_Id AS [ICD-9],
		i.value AS [ICD Code],
		'[' + i.value + '] ' + i.text AS [ICD Text], 
		CONVERT(char(11), med_tech_sig.date, 101) AS [Med Tech Sign Date], 
		med_tech_sig.nameAndRank AS [Med Tech],
		CONVERT(char(11), hqt_sig.date, 101) AS [HQ Tech Sign Date], 
		hqt_sig.nameAndRank AS [HQ Tech],
		CONVERT(char(11), med_off_sig.date, 101) AS [Med Officer Sign Date], 
		med_off_sig.nameAndRank AS [Med Officer],
		CONVERT(char(11), hqt_ph_sig.date, 101) AS [HQ DPH Sign Date], 
		hqt_ph_sig.nameAndRank AS [HQ DPH],
		CONVERT(char(11), unit_ph_sig.date, 101) AS [Unit PH Sign Date], 
		unit_ph_sig.nameAndRank AS [Unit PH],
		w.ws_id, 
		wf.workflowId,
		w.isFinal, 
		a.created_by, 
		a.member_unit, 
		a.member_unit_id,
		a.Code37_Init_Date AS [Code 37 Date],
		a.sub_workflow_type,
		a.Expiration_Date AS [Expiration Date],
		a.certification_stamp AS [CertStampId],
		cs.Name AS [Certification Stamp],
		a.case_type AS [CaseTypeId],
		ct.Name AS [Case Type],
		a.sub_case_type AS [SubCaseTypeId],
		sct.Name AS [Sub Case Type],
		a.HQTech_Disposition AS [DispositionId],
		d.Name AS [Disposition],
		a.rating AS [RatingId],
		r.ratingName AS [Rating],
		a.completed_by_unit AS [cbgId],
		cbg.Name AS [Completed By],
		a.is_delinquent AS [Is Delinquent],
		CONVERT(char(11), a.ph_reporting_period, 101) AS [Reporting Period],
		phUnit.LONG_NAME AS [PH Wing RMU],
		phUnit.CS_ID AS [PH Wing RMU ID],
		RTRIM(phUser.LastName + ' ' + phUser.FirstName + ' ' + ISNULL(phUser.MiddleName, '')) AS [DPH User],
		pr.title AS [Process],
		a.renewal_date AS [Renewal Date]
FROM	dbo.Form348_SC AS a 
		LEFT JOIN dbo.core_lkupGrade AS g ON g.CODE = a.member_grade 
		INNER JOIN dbo.vw_WorkStatus AS w ON w.ws_id = a.status 
		LEFT OUTER JOIN dbo.core_lkupICD9 AS i ON i.ICD9_ID = a.icd9_Id 
		LEFT JOIN dbo.core_CaseType AS ct ON a.case_type = ct.Id 
		LEFT JOIN dbo.core_SubCaseType AS sct ON a.sub_case_type = sct.Id 
		LEFT JOIN dbo.core_lkupDisposition AS d ON a.HQTech_Disposition = d.Id 
		LEFT JOIN dbo.core_lkupPEPPRating AS r ON a.rating = r.ratingId 
		LEFT JOIN dbo.core_CompletedByGroup AS cbg ON a.completed_by_unit = cbg.Id 
		LEFT JOIN dbo.core_Users AS phUser ON a.ph_user_id = phUser.userID 
		LEFT JOIN dbo.Command_Struct AS phUnit ON a.ph_wing_rmu_id = phUnit.CS_ID 
		LEFT JOIN dbo.core_CertificationStamp AS cs ON a.certification_stamp = cs.Id
		LEFT JOIN (
					 SELECT MAX(smd.date) AS [date], smd.refid, smd.workflowid
					 FROM dbo.core_signaturemetadata smd
					 JOIN dbo.core_usergroups g ON g.groupid = smd.usergroup
					 WHERE g.name = 'medical technician'
					 GROUP BY smd.refid, smd.workflowid

					) AS med_tech_lkupsig ON med_tech_lkupsig.refid = a.sc_id AND med_tech_lkupsig.workflowid = a.workflow
		LEFT JOIN dbo.core_signaturemetadata AS med_tech_sig ON med_tech_sig.date = med_tech_lkupsig.date AND med_tech_sig.refId = med_tech_lkupsig.refId AND med_tech_sig.workflowId = med_tech_lkupsig.workflowId
		LEFT JOIN (
					SELECT MAX(smd.date) AS [date], smd.refId, smd.workflowId
					 FROM dbo.core_SignatureMetaData smd
					 JOIN dbo.core_UserGroups g on g.groupId = smd.userGroup
					 WHERE g.name = 'Board Medical'
					 GROUP BY smd.refId, smd.workflowId

					) AS med_off_lkupsig ON med_off_lkupsig.refId = a.SC_Id AND med_off_lkupsig.workflowId = a.workflow
		LEFT JOIN dbo.core_SignatureMetaData AS med_off_sig ON med_off_sig.date = med_off_lkupsig.date AND med_off_sig.refId = med_off_lkupsig.refId AND med_off_sig.workflowId = med_off_lkupsig.workflowId
		LEFT JOIN (
					 SELECT MAX(smd.date) AS [date], smd.refId, smd.workflowId
					 FROM dbo.core_SignatureMetaData smd
					 JOIN dbo.core_UserGroups g on g.groupId = smd.userGroup
					 WHERE g.name = 'HQ AFRC Technician'
					 GROUP BY smd.refId, smd.workflowId

					) AS hqt_lkupsig on hqt_lkupsig.refId = a.SC_Id AND hqt_lkupsig.workflowId = a.workflow
		LEFT JOIN dbo.core_SignatureMetaData AS hqt_sig ON hqt_sig.date = hqt_lkupsig.date AND hqt_sig.refId = hqt_lkupsig.refId AND hqt_sig.workflowId = hqt_lkupsig.workflowId
		LEFT JOIN (
					 SELECT MAX(smd.date) AS [date], smd.refId, smd.workflowId
					 FROM dbo.core_SignatureMetaData smd
					 JOIN dbo.core_UserGroups g on g.groupId = smd.userGroup
					 where g.name = 'Unit PH'
					 GROUP BY smd.refId, smd.workflowId

					) AS unit_ph_lkupsig on unit_ph_lkupsig.refId = a.SC_Id and unit_ph_lkupsig.workflowId = a.workflow
		LEFT JOIN dbo.core_SignatureMetaData AS unit_ph_sig ON unit_ph_sig.date = unit_ph_lkupsig.date and unit_ph_sig.refId = unit_ph_lkupsig.refId and unit_ph_sig.workflowId = unit_ph_lkupsig.workflowId
		LEFT JOIN (
					 SELECT MAX(smd.date) AS [date], smd.refId, smd.workflowId
					 FROM dbo.core_SignatureMetaData smd
					 JOIN dbo.core_UserGroups g ON g.groupId = smd.userGroup
					 WHERE g.name = 'HQ AFRC DPH'
					 GROUP BY smd.refId, smd.workflowId

					) AS hqt_ph_lkupsig ON hqt_ph_lkupsig.refId = a.SC_Id AND hqt_ph_lkupsig.workflowId = a.workflow
		LEFT JOIN dbo.core_SignatureMetaData AS hqt_ph_sig ON hqt_ph_sig.date = hqt_ph_lkupsig.date AND hqt_ph_sig.refId = hqt_ph_lkupsig.refId AND hqt_ph_sig.workflowId = hqt_ph_lkupsig.workflowId
		INNER JOIN dbo.core_Workflow AS wf ON wf.workflowId = w.workflowId 
		INNER JOIN dbo.core_Users AS created ON created.userID = a.created_by 
		INNER JOIN dbo.core_Users AS updated ON updated.userID = a.modified_by 
		LEFT OUTER JOIN (
							SELECT	MAX(wst.startDate) AS ReceiveDate, wst.ws_id, wst.refId
							FROM	dbo.core_WorkStatus_Tracking wst
									JOIN core_lkupModule m ON wst.module = m.moduleId
							WHERE	m.isSpecialCase = 1 -- wst.module > 5
							GROUP BY wst.ws_id, wst.refId
						) AS track ON track.refId = a.SC_Id AND track.ws_id = a.status 
		LEFT OUTER JOIN (
							SELECT	refId, MAX(endDate) AS CompletedDate
							FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 INNER JOIN
									dbo.vw_WorkStatus ws on core_WorkStatus_Tracking_1.ws_id = ws.ws_id
									JOIN core_lkupModule m ON module = m.moduleId
							WHERE	m.isSpecialCase = 1 AND ws.isFinal = 1 AND ws.isCancel = 0 -- ws.description NOT LIKE '%Cancel%'
							GROUP BY refId
						) AS comp ON comp.refId = a.SC_Id 
		LEFT JOIN core_lkupProcess pr on pr.id = a.process
WHERE	w.isCancel = 0 -- (w.description NOT LIKE '%Cancel%')
GO

