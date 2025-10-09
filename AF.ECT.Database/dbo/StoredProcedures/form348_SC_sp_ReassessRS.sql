
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/9/2016
-- Description:	Creates a new RS case for an existing RS case. If the RS case 
--				already exists then the case is updated. Data from the original
--				RS case is used to populate some of the fields for the new LOD 
--				case. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to have the workflow ID passed in as a parameter
--					which is used to select the correct initial workstatus Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_SC_sp_ReassessRS]
(
	@userId INT,
	@originalRefId INT,
	@newCaseId VARCHAR(50),
	@workflowId INT
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @count INT = 0
	DECLARE @initialStatusId INT = 0
	DECLARE @newRefId INT = 0
	DECLARE @memoResults INT = 0
	DECLARE @rsModuleId INT = 0
	
	-- Validate input parameters...
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	IF (ISNULL(@originalRefId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	IF (ISNULL(@newCaseId, '') = '')
	BEGIN
		RETURN 0
	END

	IF (ISNULL(@workflowId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	-- Check if the original case actually exists...
	SELECT	@count = COUNT(*)
	FROM	Form348_SC sc
	WHERE	sc.SC_Id = @originalRefId
	
	IF (ISNULL(@count, 1) <> 1)
	BEGIN
		RETURN 0
	END
	
	-- Check if there already exists a case with the new case id...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	Form348_SC sc
	WHERE	sc.case_id = @newCaseId
	
	IF (ISNULL(@count, 0) <> 0)
	BEGIN
		RETURN 0
	END
	
	-- Find the initial status Id...
	SELECT	@initialStatusId = ws.ws_id, @rsModuleId = sc.moduleId
	FROM	core_StatusCodes sc
			JOIN core_WorkStatus ws ON sc.statusId = ws.statusId
	WHERE	sc.description = 'RS Initiate'
			AND ws.workflowId = @workflowId
	
	IF (ISNULL(@initialStatusId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	BEGIN TRANSACTION
		-- Insert new reassessment RS case...
		INSERT	INTO	Form348_SC (
							[case_id],
							[Module_Id],
							[workflow],
							[Doc_Group_Id],
							[status],
							[Member_ssn],
							[Member_name],
							[Member_Compo],
							[Member_Unit_Id],
							[Member_Unit],
							[Member_DOB],
							[Member_Grade],
							[sub_workflow_type],
							[created_by],
							[created_date],
							[modified_by],
							[modified_date]
						)
				SELECT	@newCaseId,
						sc.Module_Id,
						@workflowId,
						0,
						@initialStatusId,
						sc.Member_ssn,
						sc.Member_name,
						sc.Member_Compo,
						sc.Member_Unit_Id,
						sc.Member_Unit,
						sc.Member_DOB,
						sc.Member_Grade,
						sc.sub_workflow_type,
						@userId,
						GETDATE(),
						@userId,
						GETDATE()
				FROM	Form348_SC sc
				WHERE	sc.SC_Id = @originalRefId
				
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 0
		END
		
		SET @newRefId = SCOPE_IDENTITY()
		
		IF (ISNULL(@newRefId, 0) = 0)
		BEGIN
			RETURN 0
		END
		
		-- Insert mapping record between the original reference Id and the new reference Id...
		INSERT	INTO	Form348_SC_Reassessment ([OriginalRefId], [ReassessmentRefId])
				VALUES	(@originalRefId, @newRefId)
				
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 0
		END
		
		EXEC @memoResults = core_memo_sp_copy_memos2_to_memos2 @originalRefId, @newRefId, @rsModuleId, @userId
		
		IF (ISNULL(@memoResults, 0) = 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 0
		END
		
	COMMIT TRANSACTION
	
	SELECT @newRefId
END
GO

