-- =============================================
-- Author:		Andy Cooper
-- Create date: 14 May 2008
-- Description:	Inserts a new workflow step
-- =============================================
CREATE PROCEDURE [dbo].[core_workflow_sp_UpdateStep] 
	@stepId smallint,
	@workflow tinyint, 
	@statusIn tinyint,
	@statusOut tinyint,
	@text varchar(100),
	@stepType tinyint,
	@active bit,
	@displayOrder tinyint,
	@dbSign TINYINT,
	@deathStatus CHAR(1),
	@memoTemplate tinyint
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE core_WorkflowSteps SET
		workflowId = @workflow
		,statusIn = @statusIn
		,statusOut = @statusOut
		,displayText = @text
		,stepType = @stepType
		,active = @active
		,displayOrder = @displayOrder
		,dbsignTemplate = @dbSign
		,deathStatus = @deathStatus
		,memoTemplate = @memoTemplate
	WHERE
		stepId = @stepId

END
GO

