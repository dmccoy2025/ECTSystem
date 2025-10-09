
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/18/2016
-- Description:	Returns the units which have either not completed nor started
--				the current month's PH form.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/29/2017
-- Description:		Removed the RMU filter.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Workflow_sp_GetPushReportUnits]
	@executionYear INT,
	@executionMonth INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Units TABLE
	(
		unitId INT
	)

	DECLARE @count INT = 0,
			@unitPHGroupId INT = 0,
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

	SELECT	@unitPHGroupId = ug.groupId
	FROM	core_UserGroups ug
	WHERE	ug.name = 'Unit PH'

	IF (ISNULL(@unitPHGroupId, 0) = 0)
	BEGIN
		RETURN
	END

	SELECT	@viewType = reportView
	FROM	core_userGroups 
	WHERE	groupId = @unitPHGroupId

	IF (ISNULL(@viewType, 0) = 0)
	BEGIN
		RETURN
	END

	-- FIGURE OUT WHICH UNITS HAVE NOT COMPLETED OR STARTED THE PH FORM --
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

	-- SELECT UNITS --
	SELECT	cs.LONG_NAME + ' (' + cs.PAS_CODE + ')', u.unitId
	FROM	@Units u
			JOIN Command_Struct cs ON u.unitId = cs.CS_ID
	ORDER	BY cs.LONG_NAME
END
GO

