
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 7/26/2016
-- Description:	Selects the findings associated with the passed in workflow
--				ID.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	1/4/2017
-- Description:		Selects the findings associated with the passed in workflow
--					ID.
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	09/27/2021
-- Description:		Return findings by workFlow and groupId
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetFindings]
	@workflowId TINYINT,
	@groupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@workflowId, 0) = 0)
	BEGIN
		RETURN
	END
	
	DECLARE @Findings As Table
	(
		Id TinyInt,
		FindingType varchar(50),
		Description varchar(100)
	)

	DECLARE @specialText As Table
	(
		id Int,
		Description varchar(100)
	)

	INSERT Into @Findings(Id, FindingType, Description)
		SELECT	f.Id, f.findingType, f.Description
		FROM	core_Workflow_Findings wf
				JOIN core_lkUpFindings f ON wf.FindingId = f.Id
		WHERE	wf.WorkflowId = @workflowId
		order by wf.sort_order

	INSERT INTO @specialText (id, Description)
		SELECT findingId, Description
		From core_lkupFindingsText
		WHERE workflow = @workflowId AND
			  groupId = @groupId

	UPDATE @Findings
	SET Description = ST.Description
	From @specialText ST INNER JOIN @Findings F ON F.Id = ST.id

	IF (@workflowId = 32 and (@groupId <> 88 and @groupId <> 8))
	SELECT TOP(2)* From @Findings
	ELSE
	SELECT * From @Findings
END

--[dbo].[core_workflow_sp_GetFindings] 32, 8
GO

