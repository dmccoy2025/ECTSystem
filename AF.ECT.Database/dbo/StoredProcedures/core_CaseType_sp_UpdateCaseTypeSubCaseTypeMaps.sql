
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Updates the Case Type to Sub Case Type Maps for the specified
--				Sub Type ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_UpdateCaseTypeSubCaseTypeMaps]
	 @caseTypeId INT,
	 @subCaseTypeIds tblIntegerList READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@caseTypeId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	
	-- Delete all existing records associated with the Case Type Id...
	DELETE	FROM	core_CaseType_SubCaseType_Map
			WHERE	CaseTypeId = @caseTypeId
	
	-- Insert all sub case types associated with the Case Type Id...
	INSERT	INTO	core_CaseType_SubCaseType_Map ([SubCaseTypeId], [CaseTypeId])
			SELECT	s.n, @caseTypeId
			FROM	@subCaseTypeIds s
END
GO

