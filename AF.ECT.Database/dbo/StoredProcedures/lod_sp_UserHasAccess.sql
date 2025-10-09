
-- ============================================================================
-- Author:			?
-- Create date:		?
-- Description:		Determines the user's read/write access to a case.
-- ============================================================================
-- Modified By:		?
-- Modified Date:	4/23/2010
-- Description:		The user who created LOD can not see if its not in the 
--					chain
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/18/2016
-- Description:		Added module ID for the Recruiting Services (RS) workflow.
--					Module ID = 22
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/17/2016
-- Description:		Added module ID for the PH Non-Clinical Tracking (PH)
--					workflow. Module ID = 23
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified the stored procedure to return 0 when the SSN of
--					the member the case revolves around matches the SSN of the
--					user attempting to access the case.
--					- Fixed a bug which allowed a user to have view access to a
--					SARC case even if that case did not fall under the user's
--					unit purview.
--					- Cleaned up the stored procedure and updated comments
--					because it was bothering me.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/9/2016
-- Description:		Added module ID for the Appeal Request (AP) workflow.
--					Module ID = 24
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/24/2016
-- Description:		Added module ID for the Restricted SARC workflow.
--					Module ID = 25
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	1/10/2017
-- Description:		Added module ID for the Appeal SARC workflow.
--					Module ID = 26
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/20/2017
-- Description:		Updated the CanView portion of the procedure to use the
--					member's current unit instead of the unit the case was
--					initiated under for the purview checks for final (completed
--					/canceled) cases. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/18/2017
-- Description:		* Modified the setting of the @isSpecialCase variable by
--					querying the isSpecialCase field in the core_lkupModule 
--					table instead of using a list of module IDs. 
--					* Modified to allow the those users with sysAdmin permission
--					to always have edit access...further access will be
--					controlled via Page Permissions.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/8/2017
-- Description:		* Modified to pull the member information from the 
--					Form348_RR, Form348_AP, and Form348_AP_SARC tables instead 
--					of the Form348 table for the respective workflows.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/4/2017
-- Description:		* Fixed a bug preventing the SARC Appeal values from ever
--					being set.
-- ============================================================================
CREATE PROCEDURE [dbo].[lod_sp_UserHasAccess]
	@userId INT,
	@refId INT,
	@moduleId INT
AS
BEGIN
	DECLARE	@canView INT = 0,
			@canEdit INT = 0,
			@scope INT,
			@InitLODId INT,
			@isSpecialCase INT = 0,
			@isAppealSARC INT = 0,
			@workflowId INT = 0,
			@isCaseFinal BIT = 0,
			@hasAdminPerm BIT = 0

	DECLARE @perms TABLE
	(
		permId INT,
		permName VARCHAR(50),
		permDesc VARCHAR(100),
		allowed BIT,
		exclude BIT
	)
			
	IF (@moduleId = 2)  -- LOD
	BEGIN
		SET @InitLODId = @refId
		SET @isCaseFinal = dbo.fn_IsLODFullyCompleted(@refId)
	END
	
	IF (@moduleId = 5)  -- RR
	BEGIN
		SELECT @InitLODId = r.InitialLodId, @isCaseFinal = ws.isFinal FROM Form348_RR r JOIN vw_WorkStatus ws ON r.status = ws.ws_id WHERE r.request_id = @refId
	END

	If @moduleId = 24  --AP
	BEGIN
		SELECT @InitLODId = initial_lod_id FROM	Form348_AP WHERE appeal_id = @refId
		SET @isCaseFinal = dbo.fn_IsLODAppealFullyCompleted(@refId)
	END

	If @moduleId = 25  --SARC
	BEGIN
		SET @workflowId = 28
		SET @InitLODId = @refId
		SET @isCaseFinal = dbo.fn_IsRestrictedSARCFullyCompleted(@refId)
	END

	If @moduleId = 26  --APSA
	BEGIN
		SET @isAppealSARC = 1
		SET @isCaseFinal = dbo.fn_IsRestrictedSARCAppealFullyCompleted(@refId)
		SELECT @InitLODId = initial_id FROM Form348_AP_SARC WHERE appeal_sarc_id = @refId
	END

	SELECT @isSpecialCase = m.isSpecialCase FROM core_lkupModule m WHERE m.moduleId = @moduleId

	SET @scope = (SELECT accessScope FROM vw_users WHERE userID = @userId)

		
	INSERT	INTO	@perms (permId, permName, permDesc, allowed, exclude)
			EXEC	core_permissions_sp_GetByUserId @userId
		
	IF EXISTS (SELECT permId FROM @perms WHERE permName LIKE 'sysAdmin')
	BEGIN
		SET @hasAdminPerm = 1
	END

	-- If the use is compo/global scoped, they can see everything
	IF (@scope > 1) --compo/global scope
	BEGIN
		SET @canView = 1
	END
	ELSE
	BEGIN
		-- If the user is a sysadmin let them view the case...
		IF (@hasAdminPerm = 1)
		BEGIN
			SET @canView = 1
		END
		ELSE
		BEGIN	
			-- They did not create this one, see if it within their chain
			DECLARE	@unit INT,
					@found INT,
					@sarcCase BIT,
					@restricted BIT,
					@canActOnSarcUnrestriced BIT = 0,
					@canViewSarcRestricted BIT = 0,
					@isAttached INT,
					@attachUnit INT,
					@memberSSN VARCHAR(11)
					
			-- Get member and case information from the proper data source...
			IF (@isSpecialCase = 1)
			BEGIN
				SELECT	@unit = s.member_unit_id, @sarcCase=0, @restricted = 0, @memberSSN = s.Member_ssn, @isCaseFinal = ws.isFinal
				FROM	Form348_SC s
						JOIN vw_WorkStatus ws ON s.status = ws.ws_id
				WHERE	s.SC_Id = @refId
			END
			ELSE IF (@workflowId = 28)
			BEGIN
				SELECT	@unit= s.member_unit_id, @sarcCase = 1, @restricted = 1, @memberSSN = s.member_ssn
				FROM	Form348_SARC s
				WHERE	s.sarc_id = @refId
			END
			ELSE IF (@moduleId = 5)  -- RR
			BEGIN
				SELECT	@unit = r.member_unit_id, @memberSSN = r.member_ssn, @sarcCase=0, @restricted = 0
				FROM	Form348_RR r
				WHERE	r.request_id = @refId
			END
			ELSE IF (@moduleId = 24)  --AP
			BEGIN
				SELECT	@unit = a.member_unit_id, @memberSSN = a.member_ssn, @sarcCase=0, @restricted = 0
				FROM	Form348_AP a
				WHERE	a.appeal_id = @refId
			END
			ELSE IF (@moduleId = 26)  --APSA
			BEGIN
				SELECT	@unit = a.member_unit_id, @memberSSN = a.member_ssn, @sarcCase=0, @restricted = 0
				FROM	Form348_AP_SARC a
				WHERE	a.appeal_sarc_id = @refId
			END
			ELSE
			BEGIN
				SELECT	@unit= f.member_unit_id, @isAttached = f.isAttachPAS, @attachUnit = f.member_attached_unit_id, @sarcCase = f.sarc, @restricted = f.restricted, @memberSSN = f.member_ssn
				FROM	Form348 f
				WHERE	f.lodId = @InitLODId
			END

			IF (@isCaseFinal = 1)
			BEGIN
				-- Get member's current unit. Final cases will be selected based on the member's current unit instead of the unit they were in when the case was initiated. 
				-- In-progress cases will always use the unit the case was initiated with.
				SELECT	@unit = cs.CS_ID
				FROM	MemberData m
						JOIN Command_Struct cs ON m.PAS_NUMBER = cs.PAS_CODE
				WHERE	m.SSAN = @memberSSN
			END
			
			-- Determine SARC access...
			IF (EXISTS (SELECT permId FROM @perms WHERE permName LIKE 'RSARCView'))
			BEGIN
				SET @canViewSarcRestricted = 1
			END
			
			IF (EXISTS (SELECT permId FROM @perms WHERE permName LIKE 'SARCUnrestricted'))
			BEGIN
				SET @canActOnSarcUnrestriced = 1
			END
			
			-- Check if this case falls unders the unit purview of the user...
			SELECT	@found = child_id 
			FROM	Command_Struct_Tree 
			WHERE	parent_id IN (SELECT ISNULL(ada_cs_id, cs_id) FROM core_users WHERE userID = @userid) 
					AND view_type IN (SELECT viewtype FROM vw_users WHERE userID = @userId)
					AND 
					(
						(
							child_id = @unit
						)
						OR  -- Attached PAS Code
						( 
							@isAttached = 1
							AND
							child_id = @attachUnit
						)
					)

			IF (@found IS NOT NULL)
			BEGIN
				SET @canView = 1
			END
			
			-- If this is a SARC case then verify they have the proper SARC permission in additional to the unit perview condition...
			IF (@canView = 1 AND @sarcCase = 1)
			BEGIN 
				IF (@restricted = 1)
				BEGIN 
					If (@canViewSarcRestricted <> 1)
					BEGIN
						SET @canView = 0 	
					END
				END  
				ELSE
				BEGIN
					IF (@canActOnSarcUnrestriced <> 1)
					BEGIN
						SET @canView = 0
					END 
				END
			END -- @sarcCase check
			
			-- Check if the case revolves around the user attempting to access it...
			IF (@canView = 1 AND dbo.fn_IsUserTheMember(@userId, @memberSSN) = 1)
			BEGIN
				SET @canView = 0	-- Users cannot view their own cases...
			END
		END -- @hasAdminPerm check
	END -- @scope check


	-- We know if they can view the case. Now check to see if they can edit the case as well...
	IF (@canView = 1)
	BEGIN
		DECLARE	@groupId INT,
				@status INT
				
		SET @groupId = (SELECT groupId FROM vw_users WHERE userID = @userId)
		
		-- Get the current status of the case from the proper data source...
		IF (@moduleId = 2)  -- LOD
		BEGIN
			SET @status = (SELECT f.status FROM Form348 f WHERE f.lodId = @refId)
		END
		
		IF (@moduleId = 5)  -- RR
		BEGIN
			SET @status = (SELECT r.status FROM Form348_RR r WHERE r.request_id = @refId)
		END

		IF @moduleId = 24 --AP
		BEGIN
			SET @status = (SELECT [status] FROM Form348_AP WHERE appeal_id = @refId)
		END

		IF @moduleId = 25 --SARC
		BEGIN
			SET @status = (SELECT [status] FROM Form348_SARC WHERE sarc_id = @InitLODId)
		END

		IF @moduleId = 26 --APSA
		BEGIN
			SET @status = (SELECT [status] FROM Form348_AP_SARC WHERE appeal_sarc_id = @refId)
		END
		
		IF (@isSpecialCase = 1)
		BEGIN
			SELECT @status = s.status FROM Form348_SC s WHERE s.SC_Id = @refId
		END

		IF (EXISTS (SELECT 1 WHERE @status IN (SELECT [value] FROM fn_Split(dbo.fn_GetSignatureCodesForGroup(@groupId),','))))
		BEGIN
			IF (@scope = 1)  -- Wing Scope
			BEGIN
				IF (@found = @unit And ISNULL(@isAttached, 0) = 0)  -- Normal Member Unit for Normal LOD		
					SET @canEdit = 1
				IF (@found = @unit And ISNULL(@isAttached, 0) <> 0)  -- Normal Member Unit for Attached PAS LOD		
					SET @canEdit = 0
				IF (@found = @attachUnit And ISNULL(@isAttached, 0) <> 0)  -- Attached Member Unit for Attached PAS LOD		
					SET @canEdit = 1
				IF (@found = @attachUnit And ISNULL(@isAttached, 0) = 0)  -- Attached Member Unit for Normal LOD		
					SET @canEdit = 0
			END
			ELSE
			BEGIN
				SET @canEdit = 1  -- Board Scope
			END
		END
		ELSE
		BEGIN
			IF (@hasAdminPerm = 1)
				SET @canEdit = 1
			ELSE
				SET @canEdit = 0 -- not at thier status
		END 		
	END -- @canView Check

	SELECT (@canView + @canEdit)	-- No Access = 0; View (read) Only = 1; View & Write (edit) = 2
END -- STORED PROCEDURE
GO

