
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		2/23/2017
-- Description:		Selects user emails for a APSA case by work status Id.  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the APSA cases member info instead of
--					the LODs/SARCs member info.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByStatus_APSA]
	@refId INT,
 	@status SMALLINT
AS
BEGIN
	DECLARE @MemberParentUnits TABLE 
	(
		cs_id INT
	)
	
	DECLARE @AssociatedGroups TABLE
	(
		GroupId INT
	) 

	--Member unit falls in the user's hirerchy 
	DECLARE @member_unit_id INT, @compo CHAR(1), @groupId INT, @view_type SMALLINT

	--get the viewtype from the status
	--this is the group responsible for the status code
	SELECT	@groupId = a.groupId, @view_type = g.reportView
	FROM	vw_WorkStatus a 
			JOIN core_UserGroups g on g.groupId = a.groupId
	WHERE	a.ws_id = @status
	
	
	IF (@groupId > 0)
	BEGIN
		INSERT INTO @AssociatedGroups ([GroupId])
		VALUES (@groupId)
	END
	
	-- Find other user groups associated with the passed in work status...
	INSERT	INTO	@AssociatedGroups ([GroupId])
			SELECT	scs.groupId
			FROM	core_StatusCodeSigners scs
			WHERE	scs.status = @status

	--get the member's unit and info
	SELECT	@member_unit_id = apsa.member_unit_id, 
			@compo = apsa.member_compo
	FROM	Form348_AP_SARC apsa
	WHERE	apsa.appeal_sarc_id = @refId
	
	--get the parent units for the member's unit
	INSERT	INTO	@MemberParentUnits (cs_id)
			SELECT	DISTINCT parent_id  
			FROM	Command_Struct_Tree 
			WHERE	child_id=@member_unit_id 
					AND view_type = @view_type;

	WITH AllEmails(Email, Email2, Email3) AS
	( 
		SELECT	a.Email ,a.Email2,a.Email3  
		FROM	vw_users a 
				JOIN core_UserRoles ur ON a.userID = ur.userID
		WHERE
			ur.groupId IN (SELECT GroupId FROM @AssociatedGroups)
		AND
			a.accessStatus =  3
		AND
			a.receiveEmail = 1
		AND  
			a.compo = @compo 
		--Scope filtering
		AND 
		(
			a.unit_id in (SELECT cs_id FROM @MemberParentUnits)
		)
	)
	SELECT  Email FROM AllEmails WHERE Email IS NOT NULL AND LEN(email) > 0 
	UNION
	SELECT	 Email2 FROM AllEmails WHERE Email2 IS NOT NULL AND LEN(email2) > 0 
	UNION
	SELECT	 Email3 FROM AllEmails WHERE Email3 IS NOT NULL AND LEN(email3) > 0
END
GO

