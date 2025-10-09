
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 11/10/16
-- Description:	Change case type of RS cases
-- ============================================================================

CREATE PROCEDURE [dbo].[RS_ChangeCaseType]
	@caseid As VARCHAR(50),
	@caseType As VARCHAR(50),
	@caseTypeOther As VARCHAR(50),
	@subCaseType As VARCHAR(50),
	@subCaseTypeOther As VARCHAR(50)
AS
BEGIN
	
	DECLARE @caseTypeId INT = NULL
	DECLARE @subCaseTypeId INT = NULL
	DECLARE @otherCaseType VARCHAR(50) = NULL
	DECLARE @otherSubCaseType VARCHAR(50) = NULL


	--see if the case exists
	If NOT EXISTS(SELECT 1 FROM Form348_SC WHERE case_id = @caseid AND Module_Id = 22)
	BEGIN
		return 'Cannot find case: ' + @caseid
	END

	--check to see if the case type exists
	If NOT EXISTS(SELECT 1 From core_CaseType WHERE Name = @caseType)
	BEGIN
		return 'Cannot find case type: ' + @caseType
	END

	--get the case type id
	SELECT @caseTypeId = (SELECT TOP 1 Id FROM core_CaseType WHERE Name = @caseType)

	--check if the case type is associated with the RS worklfow
	If NOT EXISTS(SELECT 1 FROM core_Workflow_CaseType_Map WHERE WorkflowId = 24 AND CaseTypeId = @caseTypeId)
	BEGIN
		return 'Case finding is not part of the workflow: ' + @caseType
	END

	--check if the case type has a subcase type association
	If EXISTS(SELECT 1 From core_CaseType_SubCaseType_Map WHERE CaseTypeId = @caseTypeId)
	BEGIN
		
		--check to see if the sub case type exists
		If NOT EXISTS(SELECT 1 From core_SubCaseType WHERE Name = @subCaseType)
		BEGIN
			return 'Cannot find sub case type: ' + @subcaseType
		END
		ELSE
		BEGIN
			SELECT @subCaseTypeId = (SELECT TOP 1 Id FROM core_SubCaseType WHERE Name = @subCaseType)

			--check to see if the subcase type is associated with the case type
			If NOT EXISTS(SELECT 1 From core_CaseType_SubCaseType_Map WHERE CaseTypeId = @caseTypeId AND SubCaseTypeId=@subCaseTypeId)
			BEGIN
				return 'This Sub case does not match a case type: ' + @subcaseType
			END
		END
	END
	ELSE
	BEGIN
		SELECT @subCaseType = NULL
	END

	IF(@caseType = 'Other')
	BEGIN
		SELECT @otherCaseType = @caseTypeOther
	END

	IF(@subCaseType = 'Other')
	BEGIN
		SELECT @otherSubCaseType = @subCaseTypeOther
	END


	UPDATE Form348_SC
	SET case_type = @subCaseTypeId, case_type_name = @otherCaseType, sub_case_type = @subCaseTypeId, sub_case_type_name = @subCaseTypeOther
	WHERE case_id = @caseid  
END
GO

