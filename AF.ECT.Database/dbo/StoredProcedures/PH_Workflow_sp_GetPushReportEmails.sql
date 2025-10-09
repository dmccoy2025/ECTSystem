
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/7/2016
-- Description:	Returns emails for the HQ AFRC DPH and Unit PH users which 
--				belong to units which have not either started or completed the
--				current month's PH form.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/29/2017
-- Description:		Removed the RMU filter.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Workflow_sp_GetPushReportEmails]
	@executionYear INT,
	@executionMonth INT,
	@userGroupId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Units TABLE
	(
		unitId INT
	)

	DECLARE @MemberParentUnits TABLE 
	(
		cs_id INT
	)

	DECLARE @EmailAddresses TABLE
	(
		emailAddress VARCHAR(200)
	)

	DECLARE @count INT = 0,
			@viewType AS TINYINT = 0,
			@currentUnitId INT = 0

	DECLARE UnitsWithPHUnitUsers CURSOR FOR SELECT	DISTINCT u.cs_id
											FROM	core_Users u
													JOIN core_UserRoles ur ON u.userID = ur.userID
													JOIN core_UserGroups ug ON ur.groupId = ug.groupId
													JOIN core_lkupAccessStatus ac ON u.accessStatus = ac.statusId
											WHERE	ug.name IN ('Unit PH')
													AND ac.description = 'Approved'
													AND u.cs_id IS NOT NULL

	DECLARE UnitCursor CURSOR FOR SELECT unitId FROM @Units

	SELECT	@viewType = reportView
	FROM	core_userGroups 
	WHERE	groupId = @userGroupId	-- The Unit PH & HQ AFRC DPH share the same report view...

	IF (ISNULL(@viewType, 0) = 0)
	BEGIN
		RETURN
	END

	-- FIGURE OUT WHICH UNITS REQUIRE THE PUSH REPORT EMAIL TO BE SENT --
	SET @currentUnitId = 0

	OPEN UnitsWithPHUnitUsers

	FETCH NEXT FROM UnitsWithPHUnitUsers INTO @currentUnitId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT	@count = COUNT(s.SC_Id)
		FROM	Form348_SC s
				JOIN core_WorkStatus ws ON s.status = ws.ws_id
				JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
		WHERE	s.ph_wing_rmu_id = @currentUnitId					-- Does a case exist for this unit...
				AND YEAR(s.ph_reporting_period) = @executionYear	-- Was the reporting period in the same month as the exection year...
				AND MONTH(s.ph_reporting_period) = @executionMonth	-- Was the reporting period in the same month as the execution month...
				AND sc.isFinal = 1									-- Has the case been closed...
				AND sc.isCancel = 0									-- Has the case NOT been cancelled...
		
		IF (ISNULL(@count, 0) = 0)
		BEGIN
			-- This PH case for this unit has either not been started (no record exists) OR the case has not been completed...
			INSERT	INTO	@Units ([unitId])
					VALUES	(@currentUnitId)
		END
		
		SET @count = 0
		
		FETCH NEXT FROM UnitsWithPHUnitUsers INTO @currentUnitId
	END

	CLOSE UnitsWithPHUnitUsers
	DEALLOCATE UnitsWithPHUnitUsers


	-- GET A LIST OF EMAILS TO THE UNIT PH USERS ASSOCIATED WITH THE SELECTED UNITS --
	SET @currentUnitId = 0

	OPEN UnitCursor

	FETCH NEXT FROM UnitCursor INTO @currentUnitId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT	INTO	@MemberParentUnits (cs_id)
				SELECT	DISTINCT parent_id  FROM Command_Struct_Tree WHERE child_id = @currentUnitId AND view_type = @viewType;

		WITH AllEmails(Email, Email2, Email3) AS
		( 
			SELECT	a.Email ,a.Email2,a.Email3 
			FROM	vw_users a 
					JOIN core_UserRoles ur ON a.userID = ur.userID 
			WHERE
				a.accessStatus =  3
			AND
				a.receiveEmail = 1
			AND 
				ur.groupId = @userGroupId
			AND 
			(
				a.unit_id in (SELECT cs_id FROM @MemberParentUnits)
			)
		)
		INSERT	INTO	@EmailAddresses ([emailAddress])
				SELECT	Email	FROM AllEmails	WHERE Email is not NULL and LEN(email) > 0
					UNION
				SELECT	Email2	FROM AllEmails	WHERE Email2 is not NULL and LEN(email2) > 0
					UNION
				SELECT	Email3	FROM AllEmails	WHERE Email3 is not NULL and LEN(email3) > 0
		
		DELETE FROM @MemberParentUnits
		
		FETCH NEXT FROM UnitCursor INTO @currentUnitId
	END

	CLOSE UnitCursor
	DEALLOCATE UnitCursor

	-- SELECT EMAIL ADDRESSES -- 
	SELECT	DISTINCT ea.emailAddress
	FROM	@EmailAddresses ea
END
GO

