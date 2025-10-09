


CREATE VIEW [dbo].[vw_sc_ph_reporting]
AS
SELECT	a.SC_Id, 
		a.case_id AS [Case Id], 
		w.description AS Status, 
		CASE w.isFinal WHEN 1 THEN 'Yes' ELSE 'No' END AS Closed, 
		CONVERT(char(11), ISNULL(track.ReceiveDate, a.created_date), 101) AS [Date Of Status], 
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, a.created_date), GETDATE()) ELSE 0 END AS Days, 
		CASE w.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(a.created_date, GETDATE()), GETDATE()) ELSE 0 END AS [Days Open],
		CASE w.isFinal WHEN 1 THEN DATEDIFF(D, ISNULL(a.created_date, GETDATE()), ISNULL(comp.CompletedDate, GETDATE())) ELSE NULL END AS [Total Days],
		CONVERT(char(11), a.created_date, 101)AS [Date Created], 
		CONVERT(char(11), a.modified_date, 101) AS [Date Modified], 
		dbo.FormatName(a.created_by) AS [Created By], 
		dbo.FormatName(a.modified_by) AS [Modified By], 
		CONVERT(char(11), comp.CompletedDate, 101) AS [Date Completed], 
		CONVERT(char(11), hqt_ph_sig.date, 101) AS [HQ DPH Sign Date], 
		hqt_ph_sig.nameAndRank AS [HQ DPH],
		CONVERT(char(11), unit_ph_sig.date, 101) AS [Unit PH Sign Date], 
		unit_ph_sig.nameAndRank AS [Unit PH],
		w.ws_id, 
		wf.workflowId,
		w.isFinal, 
		a.created_by, 
		a.sub_workflow_type,
		a.Expiration_Date AS [Expiration Date],
		a.completed_by_unit AS [cbgId],
		cbg.Name AS [Completed By],
		a.is_delinquent AS [Is Delinquent],
		CONVERT(char(11), a.ph_reporting_period, 101) AS [Reporting Period],
		phUnit.LONG_NAME AS [PH Wing RMU],
		phUnit.CS_ID AS [PH Wing RMU ID],
		RTRIM(phUser.LastName + ' ' + phUser.FirstName + ' ' + ISNULL(phUser.MiddleName, '')) AS [DPH User],
		phUnit.IsCollocated
FROM	dbo.Form348_SC AS a 
		INNER JOIN dbo.vw_WorkStatus AS w ON w.ws_id = a.status 
		LEFT JOIN dbo.core_CompletedByGroup AS cbg ON a.completed_by_unit = cbg.Id 
		LEFT JOIN dbo.core_Users AS phUser ON a.ph_user_id = phUser.userID 
		LEFT JOIN dbo.Command_Struct AS phUnit ON a.ph_wing_rmu_id = phUnit.CS_ID 
		LEFT JOIN dbo.core_Workflow AS wf ON wf.workflowId = w.workflowId 
		INNER JOIN dbo.core_lkupModule AS m ON wf.moduleId = m.moduleId 
		INNER JOIN dbo.core_Users AS created ON created.userID = a.created_by 
		INNER JOIN dbo.core_Users AS updated ON updated.userID = a.modified_by 
		LEFT OUTER JOIN
						(
							SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
							FROM	dbo.core_WorkStatus_Tracking
									JOIN core_lkupModule m ON module = m.moduleId
							WHERE	m.isSpecialCase = 1 -- module > 5
							GROUP BY ws_id, refId
						) AS track ON track.refId = a.SC_Id AND track.ws_id = a.status 
		LEFT OUTER JOIN
						(
							SELECT	refId, MAX(endDate) AS CompletedDate
							FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 INNER JOIN
									dbo.vw_WorkStatus ws on core_WorkStatus_Tracking_1.ws_id = ws.ws_id
									JOIN core_lkupModule m ON module = m.moduleId
							WHERE	m.isSpecialCase = 1 AND ws.isFinal = 1 AND ws.isCancel = 0 --ws.description NOT LIKE '%Cancel%'
							GROUP BY refId
						) AS comp ON comp.refId = a.SC_Id
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
WHERE	m.moduleName = 'PH Non-Clinical Tracking' --wf.title = 'PH Non-Clinical Tracking (PH)'
		AND w.isFinal = 1
		AND w.isCancel = 0
GO

