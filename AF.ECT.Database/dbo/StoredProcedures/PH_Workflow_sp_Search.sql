
-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 4/29/2016
-- Description:	Selects PH cases which match the specified search criteria.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Workflow_sp_Search]
	  @caseID VARCHAR(50) = NULL
	, @status TINYINT = NULL
	, @userId  INT
	, @rptView TINYINT
	, @compo VARCHAR(10) = NULL
	, @maxCount INT = NULL
	, @moduleId TINYINT = NULL
	, @unitId INT = NULL
	, @reportingMonth INT = NULL
	, @reportingYear INT = NULL
AS

IF  @caseID ='' SET @caseID=null 
IF  @compo ='' SET @compo=null 
IF  @status =0 SET @status=null
IF  @maxCount =0 SET @maxCount=null
IF  @moduleId =0 SET @moduleId=null
IF  @unitId =0 SET @unitId=null	
IF  @reportingMonth =0 SET @reportingMonth=null	
IF  @reportingYear =0 SET @reportingYear=null	
 
DECLARE @userUnit AS INT, @scope TINYINT
SELECT @userUnit = unit_id, @scope = accessscope FROM vw_Users WHERE userId=@userId	 

SELECT
	 a.SC_Id AS RefId
	,a.case_id  AS CaseId  
	,a.member_compo AS Compo
	,cs.PAS_CODE AS Pascode
	,a.status AS WorkStatusId
	,s.description AS WorkStatus
	,s.isFinal AS IsFinal
	,a.workflow AS WorkflowId
	,w.title AS Workflow
	,w.moduleId AS ModuleId
	,convert(char(11), a.created_date, 101) DateCreated
	,cs.long_name AS UnitName
	,convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate
	,datediff(d, ISNULL(t.ReceiveDate, a.created_date), getdate()) Days
	,ISNULL(l.id,0) lockId
	,Convert(varchar, a.SC_Id) + ', ' + CONVERT(varchar, w.moduleId) AS PrintId
	,sub_workflow_type AS Sub_Type
	,DATENAME(MONTH, a.ph_reporting_period) + ' ' + DATENAME(YEAR, a.ph_reporting_period) AS [Reporting_Period]
FROM 
	Form348_sc  a
JOIN 
	core_WorkStatus ws ON ws.ws_id = a.status
JOIN 
	core_StatusCodes s ON s.statusId = ws.statusId
JOIN 
	core_workflow w ON w.workflowId = a.workflow
JOIN 
	COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
left join 
	core_workflowLocks l on l.refId = a.sc_id and (l.module = IsNull(@moduleId,l.module))
LEFT JOIN 
	(SELECT MAX(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = a.SC_Id
WHERE 
(
		a.case_id LIKE ISNULL(@caseId,a.case_id) + '%'
 	AND
		CASE 
			WHEN @status IS NULL THEN a.status
			ELSE @status 
		END
		=a.status  
	AND  
		a.member_compo  LIKE '%'+ ISNULL(@compo,a.member_compo) +'%' 
 	AND  
	(
		-- global scope
		(
			@scope = 3
			AND
			CASE 
				WHEN @unitId IS NULL THEN a.member_unit_id 
				ELSE @unitId 
			END
			=a.member_unit_id  
		)
		OR
		(
			-- component scope
			@scope = 2
			AND
			CASE 
				WHEN @unitId IS NULL THEN a.member_unit_id 
				ELSE @unitId 
			END
			=a.member_unit_id  
		)
		OR
		-- unit scope
		(
			@scope = 1  
			AND
			(
				a.member_unit_id  IN 				 
				(	
					SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView 
				) 
			)
		)
	)
	AND  	 
		a.status in 			 
		(
			SELECT ws_id FROM vw_WorkStatus  WHERE (moduleId = ISNULL(@moduleId,moduleId))
		)
	AND
		DATEPART(MONTH, a.ph_reporting_period) = ISNULL(@reportingMonth, DATEPART(MONTH, a.ph_reporting_period))
	AND
		DATEPART(YEAR, a.ph_reporting_period) = ISNULL(@reportingYear, DATEPART(YEAR, a.ph_reporting_period))
 )
ORDER BY a.case_id
GO

