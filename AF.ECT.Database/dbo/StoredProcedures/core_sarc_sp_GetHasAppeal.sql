
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 1/16/2017
-- Description:	Returns whether or not the case specified by the passed in 
--				refId and workflow is being or has been appealed. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetHasAppeal]
	@refId INT,
	@workflowId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	Form348_AP_SARC a
			JOIN core_WorkStatus ws ON a.status = ws.ws_id
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	a.initial_id = @refId
			AND a.initial_workflow = @workflowId
			AND sc.isCancel = 0
	
	IF (@count > 0)
	BEGIN
		SELECT 1	-- TRUE
	END
	ELSE
	BEGIN
		SELECT 0	-- FALSE
	END
END
GO

