-- =============================================
-- Author:		Andy Cooper
-- Create date: 9 May 2008
-- Description:	
-- =============================================
-- Modified By:		Evan Morrison
-- Modified date:	8/10/2017
-- Description:		Status now takes in an int
-- =============================================
CREATE PROCEDURE [dbo].[core_workflow_sp_UpdateWorkflow] 
	@workflowId tinyint, 
	@title varchar(50),
	@module tinyint,
	@isFormal bit,
	@active bit,
	@status AS Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE core_Workflow
	SET title = @title, formal = @isFormal, moduleId = @module, active = @active, initialStatus = @status
	WHERE workflowId = @workflowId

END
GO

