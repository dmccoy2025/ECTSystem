-- =============================================
-- Author:		Andy Cooper
-- Create date: 13 May 2008
-- Description:	Inserts a new workflow step
-- =============================================
CREATE PROCEDURE [dbo].[core_workflow_sp_InsertStep] 
	-- Add the parameters for the stored procedure here
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

	INSERT INTO core_WorkflowSteps
	(workflowId, statusIn, statusOut, displayText, stepType
	,active, displayOrder, dbSignTemplate, deathStatus, memoTemplate)
	VALUES
	(@workflow, @statusIn, @statusOut, @text, @stepType
	,@active, @displayOrder, @dbSign, @deathStatus, @memoTemplate)

	SELECT SCOPE_IDENTITY()

END
GO

