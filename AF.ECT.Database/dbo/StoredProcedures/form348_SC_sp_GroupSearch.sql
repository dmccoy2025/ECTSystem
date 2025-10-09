--exec form348_sc_sp_GroupSearch  '',null,null,0,430,2,'6',0,7,0,false

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @caseID from 10 to 50.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_SC_sp_GroupSearch]

		  @caseID varchar(50)=null
		, @ssn varchar(10)=null
		, @name varchar(10)=null
		, @status tinyint =null
		, @userId  int
		, @rptView tinyint  
		, @compo varchar(10) =null
		, @maxCount int =null
		, @moduleId tinyint=null
		, @unitId int =null
		, @sarcpermission  bit =null

	AS

		IF  @caseID ='' SET @caseID=null 
		IF  @ssn ='' SET @ssn=null 
		IF  @name ='' SET @name=null 	
		IF  @compo ='' SET @compo=null 
		IF  @status =0 SET @status=null 
		IF  @maxCount =0 SET @maxCount=null
		IF  @moduleId =0 SET @moduleId=null		 
		IF  @unitId ='' SET @unitId=NULL
		 

-- DECLARE @caseID varchar(10)
--		, @ssn varchar(10)
--		, @name varchar(10)
--		, @status tinyint
--		, @userId  int
--		, @rptView tinyint  
--		, @compo varchar(10)
--		, @maxCount int
--		, @moduleId tinyint
--		, @unitId int
--		, @sarcpermission  bit

--SET @userId = 430  -- HQ AFRC Technician
--SET @moduleId = 7
--SET @rptView = 2
--SET @compo = '6'
--SET @status = 0
--SET @sarcpermission = 0
	
Declare @userUnit int, @groupId tinyint, @scope tinyint ,@canActOnsarcUnrestriced bit
SELECT @userUnit = unit_id, @groupId = groupId, @scope = accessscope  from vw_Users where userId=@userId

EXEC core_permissions_sp_UserHasPermission @userId, 'SARCUnrestricted', @canActOnsarcUnrestriced out
 
--SET @groupId= (SELECT groupId FROM vw_users WHERE(SELECT TOP 1 groupId from core_UserRoles where userId=@userId and active=1 and status=3) 
DECLARE @filter varchar(20)
SET @filter = (SELECT filter FROM core_StatusCodes WHERE statusId = @status)
 
DECLARE @DefPriority int
SET @DefPriority = 0


SET @Compo = dbo.GetCompoForUser(@userId)

SELECT
	
	 a.SC_Id as RefId,
	 a.case_id  as CaseId  
	,a.member_name  as Name
	,RIGHT( a.member_ssn, 4) as SSN 
	, a.Member_Compo as Compo
	,cs.PAS_CODE as Pascode
	,a.status as WorkStatusId
	,s.description as WorkStatus
	,s.isFinal as IsFinal
	,a.workflow  as WorkflowId
	,w.title as Workflow
	,w.moduleId as ModuleId
	,convert(char(11), a.created_date, 101) DateCreated
	,cs.long_name as UnitName
	,convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate
	,datediff(d, ISNULL(t.ReceiveDate, a.created_date), getdate()) Days
	,ISNULL(l.id,0) lockId
	, Convert(varchar, a.SC_Id) + ', ' + CONVERT(varchar, w.moduleId) As PrintId
	, Fast_Track_Type as Sub_Type
	, (
		ISNULL(pas.priority, @DefPriority) + DATEDIFF(d, ISNULL(t.ReceiveDate, a.created_date), GETDATE())
	) as pas_priority
FROM Form348_SC  a
JOIN core_WorkStatus ws ON ws.ws_id = a.status
JOIN core_StatusCodes s ON s.statusId = ws.statusId
JOIN core_workflow w ON w.workflowId = a.workflow
JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
left join core_workflowLocks l on l.refId = a.SC_Id and l.module = @moduleId
LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = a.SC_Id AND t.ws_id = a.status
LEFT JOIN core_lkupPAS pas			ON pas.pas			= cs.PAS_CODE			-- added for 125
WHERE 
(   
	(a.Member_Compo = @Compo)
	AND
 	(a.member_name LIKE '%'+IsNuLL (@Name,a.member_name)+'%') 
	AND
	(a.case_id LIKE '%'+IsNull (@caseId,a.case_id)+'%')
	AND  
	(a.member_ssn LIKE '%'+ IsNull(@ssn,a.member_ssn) +'%' )
	AND
		CASE 
		WHEN @unitId IS NULL THEN a.member_unit_id 
		ELSE @unitId 
		END
		= a.member_unit_id  
	AND
	(  
		(@scope = 3)
		OR
		(@scope = 2)
		OR
		(	
			@scope = 1
			AND
			a.member_unit_id  IN 				 
				(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView 
				) 
		)
		/*OR
		(
			a.created_by = @userId
		)*/
	)	
 	AND  	 
	( 
	    a.status in  (SELECT ws_id FROM vw_WorkStatus WHERE moduleId=@moduleId AND groupId = @groupId)
	   or
		a.status in  (SELECT status FROM core_StatusCodeSigners where groupId = @groupId)
     )  
  
 )
ORDER BY pas_priority DESC
GO

