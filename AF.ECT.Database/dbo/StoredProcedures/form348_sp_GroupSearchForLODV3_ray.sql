

-- ============================================================================
-- Created By:		Ray Hall
-- Modified Date:	04/23/2022
-- Description:		Sorted Wing JA V2 from V3
-- ============================================================================

CREATE PROCEDURE [dbo].[form348_sp_GroupSearchForLODV3_ray] 
	  @caseID			varchar(50) = NULL
	, @ssn				varchar(10) = NULL
	, @name				varchar(10) = NULL
	, @status			INT			= NULL
	, @userId			int
	, @rptView			tinyint  
	, @compo			varchar(10) = NULL
	, @maxCount			int			= NULL
	, @moduleId			tinyint		= NULL
	, @isFormal			bit			= NULL
	, @unitId			int			= NULL
	, @sarcpermission	bit			= NULL
AS
BEGIN
	IF  @caseID =''		SET @caseID = NULL 
	IF  @ssn =''		SET @ssn = NULL 
	IF  @name =''		SET @name = NULL 	
	IF  @compo =''		SET @compo = NULL 
	IF  @status =0		SET @status = NULL 
	IF  @maxCount =0	SET @maxCount = NULL
	IF  @moduleId =0	SET @moduleId = NULL		 
	IF  @unitId =''		SET @unitId = NULL
	

	Declare @userUnit int, @groupId tinyint, @scope tinyint ,@canActOnsarcUnrestriced bit, @userSSN CHAR(9)
	
	SELECT @userUnit = unit_id ,@groupId = groupId
	, @scope = accessscope, @userSSN = SSN from vw_Users where userId=@userId

	Set @groupId = 
				Case @groupId
					When 3 then 118 --med tech
					when 4 then 119 --med off
					when 2 then 120 --unit cc
					when 6 then 6 --wing ja
					when 5 then 5 --wing cc
				End


	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	DECLARE @unitView int = 1
	SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

	--SET @groupId= (SELECT groupId FROM vw_users WHERE(SELECT TOP 1 groupId from core_UserRoles where userId=@userId and active=1 and status=3) 
	DECLARE @filter varchar(20)
	SET @filter = (SELECT filter FROM core_StatusCodes WHERE statusId = @status)
 
	SET @Compo = dbo.GetCompoForUser(@userId)

	DECLARE @DefPriority int
	SET @DefPriority = 0

	SELECT	  a.lodId as RefId,a.case_id  as CaseId  
			, a.member_name  as Name
			, CS.STATE as State
			, RIGHT( a.member_ssn, 4) as SSN 
			, a.member_compo as Compo
			, cs.PAS_CODE as Pascode
			, a.status as WorkStatusId
			, s.description as WorkStatus
			, s.isFinal as IsFinal
			, a.workflow  as WorkflowId
			, w.title as Workflow
			, w.moduleId as ModuleId
			, a.formal_inv  as IsFormal
			, CONVERT(char(11), a.created_date, 101) DateCreated
			, cs.long_name as UnitName
			, CONVERT(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate
			, DATEDIFF(d, ISNULL(t.ReceiveDate, a.created_date), GETDATE()) as Days
			, ISNULL(l.id,0) lockId
			, (
				ISNULL(pas.priority, @DefPriority) + DATEDIFF(d, ISNULL(t.ReceiveDate, a.created_date), GETDATE())
			  ) as pas_priority													-- added for 125
			, CASE 
				WHEN fm.death_involved_yn = 'Yes' Then 'P-Death'
				WHEN pas.priority >= 60000 THEN 'P1a'
				WHEN pas.priority BETWEEN 50000 and 59999 THEN 'P1b'
				WHEN pas.priority BETWEEN 40000 and 49999 THEN 'P2'
				ELSE ''
			  END AS PriorityRank	
	FROM	Form348  a
			LEFT JOIN FORM261 b				ON b.lodid		= a.lodid
			JOIN core_WorkStatus ws			ON ws.ws_id		= a.status
			JOIN core_StatusCodes s			ON s.statusId	= ws.statusId
			JOIN core_workflow w			ON w.workflowId = a.workflow
			JOIN COMMAND_STRUCT CS			ON cs.cs_id		= a.member_unit_id 
			JOIN Form348_Medical fm			ON fm.lodid		= a.lodid
			left join core_workflowLocks l	ON l.refId		= a.lodId and l.module = @moduleId
			LEFT JOIN (
				SELECT	max(startDate) ReceiveDate, 
						ws_id, 
						refId 
				FROM core_WorkStatus_Tracking 
				GROUP BY ws_id, refId
			) t								ON t.refId = a.lodId AND t.ws_id = a.status
			LEFT JOIN core_lkupPAS pas		ON pas.pas = cs.PAS_CODE	-- added for 125
	WHERE 
	(   
		a.status IN  (SELECT ws_id FROM vw_WorkStatus WHERE moduleId=@moduleId AND groupId = @groupId)
	   OR
		a.status IN  (SELECT status FROM core_StatusCodeSigners where groupId = @groupId)
	 )
	ORDER BY PriorityRank DESC, pas_priority DESC
	--Select @caseID as 'id', @ssn as 'ssn', @name as 'name', @status as 'status', @userId as 'userId', @rptView as 'view', @compo as 'compo', @maxCount as 'count', @moduleId as 'mId', @isFormal as 'formal', @unitId as 'uId', @sarcpermission as 'sarc'
	--Select @groupId, @moduleId
END
-- [dbo].[form348_sp_GroupSearchForLODV3_ray] NULL,NULL, NULL, 0, 140, 4, NULL, NULL, 2, NULL, NULL, 0 

 --@caseID			varchar(50) = NULL
	--, @ssn				varchar(10) = NULL
	--, @name				varchar(10) = NULL
	--, @status			INT			= NULL
	--, @userId			int
	--, @rptView			tinyint  
	--, @compo			varchar(10) = NULL
	--, @maxCount			int			= NULL
	--, @moduleId			tinyint		= NULL
	--, @isFormal			bit			= NULL
	--, @unitId			int			= NULL
	--, @sarcpermission	bit			= NULL
GO

