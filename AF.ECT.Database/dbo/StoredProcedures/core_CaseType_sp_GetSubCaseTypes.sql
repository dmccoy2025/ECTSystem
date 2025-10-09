
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Returns all of the Sub Case Types associated with the specified
--				Case Type Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_GetSubCaseTypes]
	@caseTypeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@caseTypeId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	sct.Id, sct.Name
	FROM	core_CaseType_SubCaseType_Map map
			JOIN core_SubCaseType sct ON map.SubCaseTypeId = sct.Id
	WHERE	map.CaseTypeId = @caseTypeId
END
GO

