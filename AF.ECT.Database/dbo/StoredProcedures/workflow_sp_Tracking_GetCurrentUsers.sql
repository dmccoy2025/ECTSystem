
-- ============================================================================
-- Author:		Evan Morrison
-- Create date:	5/8/2017
-- Description:	Gets the users that can work 
--				on a case
-- ============================================================================
CREATE  PROCEDURE [dbo].[workflow_sp_Tracking_GetCurrentUsers]
	@refId int,
	@moduleId int,
	@wsstatus int,
	@member_unit_id int
	
AS

	DECLARE @reportView tinyint, 
			@pascode varchar(4), 
			@groupId int,
			@show bit, 
			@scope tinyint,
			@isAttach INT = 0,
			@attachPas VARCHAR(8)

	DECLARE @StatusUserGroups TABLE
	(
		GroupID INT
	)

	SELECT	@pascode = cs.pas_code
	FROM	Command_Struct cs
	WHERE	cs.CS_ID = @member_unit_id

	
	SELECT @groupId  = groupId 
	FROM  vw_WorkStatus 
	WHERE ws_id = @wsstatus

	INSERT	INTO @StatusUserGroups ([GroupID]) VALUES (@groupId)

	INSERT	INTO @StatusUserGroups ([GroupID])
			SELECT	scs.groupId
			FROM	core_StatusCodeSigners scs
			WHERE	scs.status = @wsstatus

	SELECT	@show =showInfo,@reportView=reportView, @scope = accessScope 
	FROM	core_UserGroups 
	WHERE	groupId = @groupId 

	DECLARE @tempTable TABLE (userId INT )
	
	-- Find user's which meet the user group requirement for the case's status...
	INSERT	INTO	@tempTable (userId)
			SELECT	DISTINCT c.userID 
			FROM	core_UserRoles AS c 
					JOIN @StatusUserGroups g ON c.groupId = g.GroupID
		 
	SELECT a.UserId , Rank, FirstName, LastName 
		,(CASE @show WHEN   0    THEN '' ELSE  work_street END ) address 
		,(CASE @show WHEN   0    THEN '' ELSE   work_city  END ) city 
		,(CASE @show WHEN   0    THEN '' ELSE   work_state  END ) state 
		,(CASE @show WHEN   0    THEN '' ELSE   work_zip  END ) zip 
		,(CASE @show WHEN   0    THEN '' ELSE   Phone  END ) phone
		,(CASE @show WHEN   0    THEN '' ELSE   DSN   END ) dsn 
		,(CASE @show WHEN   0    THEN '' ELSE  Email END ) email  
		, a.role AS 'role' ,'', a.pas_code
	FROM vw_users a	
	WHERE 
		(
			(@scope = 3)
			OR
			(@scope = 2)
			OR
			(	
				@scope = 1
				AND
				a.pas_code IN  (SELECT parent_pas FROM command_struct_tree where child_pas=@pascode and view_type=@reportView) 
			) 
		)
	AND a.accessStatus = 3
	AND a.userID IN (SELECT * FROM @tempTable)
GO

