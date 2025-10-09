

-- ============================================================================
-- Author:		Steve Kennedy
-- Create date: 2016-11-17
-- Description:	Gets all SARCS by user id
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/7/2016
-- Description:		Cleaned up procedure and fixed it to properly select cases
--					for the RSL user group.
--					Renamed the stored procedure core_lod_sp_GetRestrictedSARCS
--					to core_sarc_sp_GetOpenCases. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/21/2017
-- Description:		Modified to find user related data in separate select
--					statement instead of the case select statement.
-- ============================================================================
-- Modified By:		Darel Johnson
-- Modified Date:	09/01/2020
-- Description:		Add compo lookup
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetOpenCases]
	@userId INT
AS
BEGIN
	DECLARE @scope TINYINT = 0,
			@roleId INT = 0,
			@groupId INT = 0,
			@unitView INT = 1,
			@rptView INT = 0,
			@userSSN CHAR(9) = '',
			@moduleId TINYINT = 0,
			@userUnitId INT = 0

	SELECT	
			@scope = u.accessScope,
			@roleId = u.currentRole,
			@groupId = u.groupId,
			@rptView = u.ViewType,
			@userSSN = u.SSN,
			@userUnitId = u.unit_id
	FROM	
			vw_users u
	WHERE	
			u.userID = @userId

	SELECT	@unitView = unitView 
	FROM	core_Users 
	WHERE	userID = @userId

	SELECT	@moduleId = m.moduleId 
	FROM	core_lkupModule m 
	WHERE	m.moduleName = 'Restricted SARC'

	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	DECLARE @Compo int
	SET @Compo = dbo.GetCompoForUser(@userId)

	-- user id not found, must have been passed in a role id
	IF @roleId = 0
		SET @roleId = @userId

	SELECT	DISTINCT sarc.sarc_id,
			SUBSTRING(sarc.member_ssn, 6, 4) As Protected_SSN,
			sarc.member_name As Member_Name,
			sarc.member_unit As Unit_Name,
			sarc.Case_Id,
			vws.description As Status,
			CONVERT(char(11), ISNULL(t.ReceiveDate, sarc.created_date), 101) As Receive_Date,
			DATEDIFF(d, ISNULL(t.ReceiveDate, sarc.created_date), GETDATE()) As Days,
			ISNULL(l.id,0) AS lockId
	FROM	Form348_SARC sarc
			JOIN vw_WorkStatus vws On sarc.status = vws.ws_id
			LEFT JOIN core_workflowLocks l ON l.refId = sarc.sarc_id and l.module = @moduleId
			LEFT JOIN (
				SELECT	Max(startDate) ReceiveDate, ws_id, refId 
				FROM	core_WorkStatus_Tracking 
				GROUP	BY ws_id, refId
			) t ON t.refId = sarc.sarc_id AND t.ws_id = sarc.status
	WHERE	vws.isFinal = 0
			AND vws.isCancel = 0
			AND sarc.member_compo = @Compo
			AND
			(
				@scope > 1
				OR
				sarc.Member_ssn <> ISNULL(@userSSN, '---')	-- Don't return cases which revolve around the user doing the search...
			)
			AND
			(  
				(
					@unitView = 1
					AND
					(
						sarc.member_unit_id IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnitId AND view_type = @rptView
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
						sarc.member_unit_id = @userUnitId
					)
				)
			)
			AND  	 
			( 
				sarc.status IN (SELECT ws_id FROM vw_WorkStatus WHERE moduleId = 25 AND groupId = @groupId)
				OR
				sarc.status IN (SELECT status FROM core_StatusCodeSigners WHERE groupId = @groupId)
			)

END
GO

