
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Selects user emails for a LOD related cases by group Id.  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015	
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addressed.
--					Cleaned up the stored procedure.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/18/2016
-- Description:		Modified to pass in an additional parameter: isFinal. This
--					was done to prevent emails for Unit Commanders from being
--					included in the results when grabbing emails for cases which
--					are NOT in a final status. It defaults to 1 because this
--					stored procedure was originally meant to be called when a
--					case is completed or cancelled (i.e. pushed to a final 
--					status).
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/30/2016
-- Description:		Added appeal workflow
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	12/9/2016
-- Description:		Added SARC workflow
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	1/12/2017
-- Description:		Added Appeal SARC workflow
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/6/2017
-- Description:		- Removed the Unit Commander check from the final select
--					statement. 
--					- SARC Appeal now pulls the unit information from the 
--					appropriate source.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR/Appeal/SARC Appeal cases member
--					unit info instead of the LODs/SARCs member unit info.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByGroup]
	@refId INT,
	@groupId INT,
	@callingService VARCHAR(25),
	@isFinal BIT = 1
AS
BEGIN
	DECLARE @member_unit_id INT, @viewType as TINYINT, @isAttached INT;  

	SET @viewType = (SELECT reportView FROM core_userGroups WHERE groupId = @groupId);

	DECLARE @MemberParentUnits TABLE 
	(
		cs_id INT
	) 

	SET @isAttached = 0

	IF (@callingService = 'LOD')
	BEGIN			
		SELECT	@member_unit_id = member_unit_id, @isAttached = isAttachPas 
		FROM	Form348 
		WHERE	lodid = @refId;
	
		IF (@isAttached = 1)
		BEGIN
			SELECT	@member_unit_id = member_attached_unit_id 
			FROM	Form348 
			WHERE	lodid = @refId;
		END
	END
	
	IF (@callingService = 'reinvestigationRequest')
	BEGIN
		SELECT	@member_unit_id = rr.member_unit_id
		FROM	Form348_RR rr
		WHERE	request_id = @refId;
	END		

	IF (@callingService = 'appealRequest')
	BEGIN
		SELECT	@member_unit_id = ap.member_unit_id
		FROM	Form348_AP ap
		WHERE	appeal_id = @refId;
	END	

	IF (@callingService = 'SARC')
	BEGIN
		SELECT	@member_unit_id = sr.member_unit_id 
		FROM	Form348_SARC sr 
		WHERE	sr.sarc_id = @refId
	END

	IF (@callingService = 'SARCAppeal')
	BEGIN
		SELECT	@member_unit_id = ap.member_unit_id 
		FROM	Form348_AP_SARC ap
		WHERE	appeal_sarc_id = @refId
	END	
	
	INSERT	INTO	@MemberParentUnits (cs_id)
			SELECT	DISTINCT parent_id  
			FROM	Command_Struct_Tree 
			WHERE	child_id = @member_unit_id 
					AND view_type = @viewType;

	WITH AllEmails(Email, Email2, Email3) AS
	( 
		SELECT	a.Email ,a.Email2,a.Email3 
		FROM	vw_users a
				JOIN core_UserRoles ur ON a.userID = ur.userID
		WHERE	a.accessStatus =  3
				AND a.receiveEmail = 1
				AND ur.groupId = @groupId
				AND a.unit_id IN (SELECT cs_id FROM @MemberParentUnits)
	)    
	SELECT  Email FROM AllEmails WHERE Email IS NOT NULL AND LEN(email) > 0
		UNION
	SELECT	 Email2 FROM AllEmails WHERE Email2 IS NOT NULL AND LEN(email2) > 0
		UNION
	SELECT	 Email3 FROM AllEmails WHERE Email3 IS NOT NULL AND LEN(email3) > 0
END
GO

