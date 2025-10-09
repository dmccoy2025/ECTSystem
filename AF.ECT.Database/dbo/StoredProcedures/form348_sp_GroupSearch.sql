
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @caseID from 10 to 50.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/8/2016
-- Description:		Organized results acording to the days column 
--					from oldest to newest
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/23/2017
-- Description:		Removed the ReceivedFrom field from being selected.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		Workstatus now takes in an integer
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/09/2019
-- Description:		Added sorting by pas code priority using dbo.core_lkupPAS
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/15/2019
-- Description:		Modified the pas_priority formula. pas.priotity + Days = pas_priority
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/18/2019
-- Description:		1. Fixed pas_priority formula modification from 11/15/2019
--					2. Syntax cleaned up and consistent
--					3. Removed redundant where clause for compo
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/20/2019
-- Description:		Added column 'Priority' to MyLODs.aspx
-- ============================================================================
-- Modified By:		Michael van Diest
-- Modified Date:	08/11/2020
-- Description:		Added column 'State' to MyLODs.aspx
-- ============================================================================

CREATE PROCEDURE [dbo].[form348_sp_GroupSearch]
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
	
	-- Values for testing
	-- DECLARE @caseID varchar(10)
	--		, @ssn varchar(10)
	--		, @name varchar(10)
	--		, @status tinyint
	--		, @userId  int
	--		, @rptView tinyint  
	--		, @compo varchar(10)
	--		, @maxCount int
	--		, @moduleId tinyint
	--		, @isFormal bit
	--		, @unitId int
	--		, @sarcpermission  bit
	--
	--SET @userId = 3
	--SET @moduleId = 2
	--SET @rptView = 2
	--SET @isFormal = 0
	--SET @compo = '6'
	--SET @status = 0
	--SET @sarcpermission = 0
	
	Declare @userUnit int, @groupId tinyint, @scope tinyint ,@canActOnsarcUnrestriced bit, @userSSN CHAR(9)
	SELECT @userUnit = unit_id, @groupId = groupId, @scope = accessscope, @userSSN = SSN from vw_Users where userId=@userId

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
				WHEN pas.priority >= 60000 THEN 'P1'
				WHEN pas.priority BETWEEN 50000 and 59999 THEN 'P2'
				WHEN pas.priority BETWEEN 40000 and 49999 THEN 'P3'
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
 		(a.member_name LIKE '%'+ISNULL (@Name, a.member_name)+'%')
		AND (a.member_compo = @Compo)
		AND (a.case_id LIKE '%'+ISNULL (@caseId, a.case_id)+'%')
		AND (a.member_ssn LIKE '%'+ ISNULL(@ssn, a.member_ssn) +'%') 
		AND (
			@scope > 1
			OR
			dbo.fn_IsUserTheMember(@userId, a.member_ssn) = 0	-- Don't return cases which revolve around the user doing the search...
		)
		AND (
					CASE 
					 WHEN @unitId IS NULL THEN a.member_unit_id 
					 ELSE @unitId 
					 END
					 = a.member_unit_id  
						 OR
						 (
						 a.isAttachPAS = 1
						 AND
						CASE 
						 WHEN @unitId IS NULL THEN a.member_attached_unit_id
						 ELSE @unitId 
						 END
						 = a.member_attached_unit_id
						 )
		 )
		AND 
			CASE 
			 WHEN @isFormal IS NULL THEN a.formal_inv
			 ELSE @isFormal 
			 END
			 = a.formal_inv		 
				 		 
		AND (a.deleted = 0)
		--AND (a.member_compo  LIKE '%'+ ISNULL(@compo,a.member_compo) +'%')		-- removed redundant clause
		AND
	  		CASE 
				 WHEN (a.sarc = 1 and a.restricted =1) Then 1 		
				 ELSE @sarcpermission 
				 END
				  =@sarcpermission 	
		--AND
		--  	CASE 
		--		 WHEN (a.sarc = 1) Then 1 		
		--		 ELSE @sarcpermission 	
		--		 END
		--		  =@sarcpermission 
 		AND (  
			(
				@unitView = 1
				AND
				(
					a.member_unit_id IN
					(
						SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
					)
					OR
					(
						a.isAttachPAS = 1
						AND
						a.member_attached_unit_id  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
					)
					OR
					@scope > 1
				)
			)
			OR
			(
				@unitView = 0
				AND
				(
					a.member_unit_id = @userUnit
					OR
					(
						a.isAttachPAS = 1
						AND
						a.member_attached_unit_id  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
					)
				)
			)
			--(@scope = 3)
			--OR
			--(@scope = 2)
			--OR
			--(	
			--	@scope = 1
			--	AND
			--	(
			--		(
			--		a.member_unit_id  IN 				 
			--			(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView ) 
			--		OR
			--			(
			--			a.isAttachPAS = 1
			--			AND
			--			a.member_attached_unit_id  IN 				 
			--				(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
			--			)
			--		)
			--	)
			--)
			/*OR
			(
				a.created_by = @userId
			)*/
		)
 		AND  	 
		( 
			a.status IN  (SELECT ws_id FROM vw_WorkStatus WHERE moduleId=@moduleId AND groupId = @groupId)
		   OR
			a.status IN  (SELECT status FROM core_StatusCodeSigners where groupId = @groupId)
		 )  
		AND
			(
				@userid = CASE 
							WHEN s.filter = 'io'   THEN b.IoUserId 
							--WHEN s.filter = 'aa' THEN a.appAuthUserId
							ELSE @userId
						END 
		   )
  
	 )
	ORDER BY pas_priority DESC
END
GO

