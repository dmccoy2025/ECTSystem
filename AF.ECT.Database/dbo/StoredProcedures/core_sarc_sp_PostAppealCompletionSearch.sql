
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		8/1/2016
-- Description:		Retreive all completed SARC Appeal cases
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/17/2017
-- Description:		- Fixed the DaysComplete fields to use the correct refId on
--					the join. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the APSA cases member info instead of
--					the LODs/SARCs member info.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/27/2017
-- Description:		- Modified to use the is_post_processing_complete flag
--					instead of the member_notified field. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_PostAppealCompletionSearch]
	 @caseID VARCHAR(50) = null
	,@ssn VARCHAR(10) = null
	,@name VARCHAR(10) = null
	,@userId INT
	,@rptView TINYINT
	,@compo VARCHAR(10) = null
	,@maxCount INT = null
	,@moduleId TINYINT = null
	,@unitId INT = null
	,@sarcpermission BIT = null
AS

IF @caseID = '' SET @caseID = null 
IF @ssn = '' SET @ssn = null 
IF @name = '' SET @name = null 	
IF @compo = '' SET @compo = null 	 
IF @maxCount = 0 SET @maxCount = null
IF @moduleId = 0 SET @moduleId = null		 
IF @unitId = 0 SET @unitId = NULL

DECLARE @userUnit INT, @groupId TINYINT, @scope TINYINT
SELECT @userUnit = unit_id, @groupId = groupId, @scope = accessscope FROM vw_Users WHERE userId = @userId

DECLARE @unitView INT = 1
SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

SELECT	 a.appeal_sarc_id As appealId
		,a.initial_id AS RefId
		,a.case_id AS CaseId  
		,a.member_name AS Name
		,RIGHT(a.member_ssn, 4) AS SSN 	 
		,cs.PAS_CODE AS Pascode
		,a.status AS WorkStatusId
		,ws.description AS StatusDescription
		,ws.isFinal AS IsFinal
		,a.workflow AS WorkflowId
		,w.title AS Workflow
		,w.moduleId AS ModuleId
		,convert(char(11), a.created_date, 101) DateCreated
		,cs.long_name + ' ('+ cs.PAS_CODE  +')' AS UnitName
		,convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate
		,datediff(d,  t.ReceiveDate , getdate()) DaysCompleted 
		,a.member_notified
		,ISNULL(l.id,0) lockId
FROM	Form348_AP_SARC a
		JOIN Form348 f ON f.lodId = a.initial_id AND f.workflow = a.initial_workflow
		JOIN vw_workstatus ws ON ws.ws_id = a.status 
		JOIN Form348_Medical med ON med.lodid = a.initial_id 	
		JOIN core_workflow w ON w.workflowId = a.workflow
		JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id
		left join core_workflowLocks l on l.refId = a.initial_id and l.module = @moduleId
		LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM	core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.ws_id = a.status AND t.refId = a.appeal_sarc_id 
WHERE	(   
			a.member_name LIKE '%' + IsNuLL (@Name,a.member_name)+'%' 
			AND
			a.case_id LIKE '%' + IsNull (@caseId,a.case_id)+'%'
			AND  
			a.member_ssn LIKE '%'+ IsNull(@ssn,a.member_ssn) +'%' 
			AND
			(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, a.member_ssn) = 0	-- Don't return cases which revolve around the user doing the search...
			)
			AND
			(
				CASE 
					WHEN @unitId IS NULL THEN a.member_unit_id 
					ELSE @unitId 
				END
				= a.member_unit_id  
			 )
			AND 
				ws.isFinal =1 
		 	AND 
		    (	
				a.is_post_processing_complete = 0
		 	)		
		 	AND 
		    (
				ws.isCancel = 0
		    )		 
			AND   
				a.member_compo LIKE '%'+ IsNull(@compo,a.member_compo) +'%' 
			AND
			(
				(
					@unitView = 1
					AND
					(
						a.member_unit_id IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
						)
					)
				)
				OR
				(
					@unitView = 0
					AND
					(
						a.member_unit_id = @userUnit
					)
				)
			) 
		)
UNION
SELECT	 a.appeal_sarc_id As appealId
		,a.initial_id AS RefId
		,a.case_id AS CaseId  
		,a.member_name AS Name
		,RIGHT(a.member_ssn, 4) AS SSN 	 
		,cs.PAS_CODE AS Pascode
		,a.status AS WorkStatusId
		,ws.description AS StatusDescription
		,ws.isFinal AS IsFinal
		,a.workflow AS WorkflowId
		,w.title AS Workflow
		,w.moduleId AS ModuleId
		,convert(char(11), a.created_date, 101) DateCreated
		,cs.long_name + ' ('+ cs.PAS_CODE  +')' AS UnitName
		,convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate
		,datediff(d,  t.ReceiveDate , getdate()) DaysCompleted 
		,a.member_notified
		,ISNULL(l.id,0) lockId
FROM	Form348_AP_SARC a
		JOIN Form348_SARC sarc ON sarc.sarc_id = a.initial_id AND sarc.workflow = a.initial_workflow
		JOIN vw_workstatus ws ON ws.ws_id = a.status 
		JOIN Form348_Medical med ON med.lodid = a.initial_id 	
		JOIN core_workflow w ON w.workflowId = a.workflow
		JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id
		left join core_workflowLocks l on l.refId = a.initial_id and l.module = @moduleId
		LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM	core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.ws_id = a.status AND t.refId = a.appeal_sarc_id 
WHERE	(   
			a.member_name LIKE '%' + IsNuLL (@Name,a.member_name)+'%' 
			AND
			a.case_id LIKE '%' + IsNull (@caseId,a.case_id)+'%'
			AND  
			a.member_ssn LIKE '%'+ IsNull(@ssn,a.member_ssn) +'%' 
			AND
			(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, a.member_ssn) = 0	-- Don't return cases which revolve around the user doing the search...
			)
			AND
			(
				CASE 
					WHEN @unitId IS NULL THEN a.member_unit_id 
					ELSE @unitId 
				END
				= a.member_unit_id  
			 )
			AND 
				ws.isFinal = 1 
		 	AND 
		    (	
				a.is_post_processing_complete = 0
		 	)		
		 	AND 
		    (
				ws.isCancel = 0
		    )		 
			AND   
				a.member_compo LIKE '%'+ IsNull(@compo,a.member_compo) +'%' 
			AND
			(
				(
					@unitView = 1
					AND
					(
						a.member_unit_id IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
						)
					)
				)
				OR
				(
					@unitView = 0
					AND
					(
						a.member_unit_id = @userUnit
					)
				)
			) 
		)
ORDER	BY ReceiveDate
OPTION (RECOMPILE)
GO

