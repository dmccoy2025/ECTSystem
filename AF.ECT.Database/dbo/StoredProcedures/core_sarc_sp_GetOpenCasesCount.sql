

-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/1/2016
-- Description:	Returns the number of open Restricted SARC cases that can be
--				worked on by the specified user. 
-- ============================================================================
-- Modified By:		Darel Johnson
-- Modified Date:	09/01/2020
-- Description:		Add compo lookup
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetOpenCasesCount]
	@userId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		SELECT 0
	END

	DECLARE	@userUnit INT, 
			@groupId TINYINT, 
			@scope TINYINT ,
			@userSSN CHAR(9),
			@rptView TINYINT,
			@viewSubUnits INT = 1

	SELECT	@userUnit = unit_id, 
			@groupId = groupId, 
			@scope = accessscope, 
			@userSSN = SSN ,
			@rptView = viewType
	FROM	vw_Users
	WHERE	userId = @userId

	SELECT	@viewSubUnits = unitView 
	FROM	core_Users
	WHERE	userID = @userId
	
	DECLARE @Compo int
	SET @Compo = dbo.GetCompoForUser(@userId)

	SELECT	COUNT(*)
	FROM	Form348_SARC sr
			JOIN core_WorkStatus ws ON sr.status = ws.ws_id
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	sc.isFinal = 0 
	        AND sr.member_compo = @Compo
			AND
			(
				@scope > 1
				OR
				sr.member_ssn <> ISNULL(@userSSN, '---')	-- Don't return cases which revolve around the user doing the search...
			)
			AND
			(  
				(
					@viewSubUnits = 1
					AND
					(
						sr.member_unit_id IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
						)
						OR
						@scope > 1
					)
				)
				OR
				(
					@viewSubUnits = 0
					AND
					(
						sr.member_unit_id = @userUnit
					)
				)
			)
			AND  	 
			( 
				sr.status IN (SELECT ws_id FROM vw_WorkStatus WHERE moduleId = 25 AND groupId = @groupId)
				OR
				sr.status IN (SELECT status FROM core_StatusCodeSigners WHERE groupId = @groupId)
			)
END

--[dbo].[core_sarc_sp_GetOpenCasesCount] 171
GO

