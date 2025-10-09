
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/25/2016
-- Description:	Returns the number of AP cases associated with the specified
--				initial ID and workflow.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetAppealCount]
	@initialId INT,
	@initialWorkflow INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@initialId, 0) = 0)
	BEGIN
		SELECT 0
	END
	
	SELECT	COUNT(a.appeal_sarc_id)
	FROM	Form348_AP_SARC a
	WHERE	a.initial_id = @initialId
			AND a.initial_workflow = @initialWorkflow

END
GO

