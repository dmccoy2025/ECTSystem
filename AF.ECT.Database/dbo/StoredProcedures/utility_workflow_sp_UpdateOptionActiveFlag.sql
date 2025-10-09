
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/8/2016
-- Description:	Sets the active flag for the specified workstatus option to the
--				specified value [0 OR 1].
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_UpdateOptionActiveFlag]
	 @workflowId INT
	,@statusInTitle varchar(50)
	,@statusOutTitle varchar(50)
	,@optionText varchar(100)
	,@active bit
AS
BEGIN
	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Set display message
	DECLARE @message VARCHAR(300) = @workflow + ', ' + @statusInTitle + ', ' + @statusOutTitle + ', ' + @optionText
	DECLARE @error VARCHAR(300) = ''

	DECLARE @statusInId int = 0
	DECLARE @statusOutId int = 0
	DECLARE @result int
	DECLARE @wsoId int


	-- Find the work status id that matches statusInTitle
	SELECT	@statusInId = ws.ws_id
	FROM	dbo.core_WorkStatus AS ws INNER JOIN
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId INNER JOIN
			core_Workflow wf ON ws.workflowId = wf.workflowId
	WHERE	wf.workflowId = @workflowId AND
			sc.description = @statusInTitle

	IF (@statusInId <= 0)
	BEGIN
		SET @error = '<Invalid Workflow Status (in)>'
	END


	-- Find the work status id that matches statusInTitle
	SELECT	@statusOutId = ws.ws_id
	FROM	dbo.core_WorkStatus AS ws INNER JOIN
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId INNER JOIN
			core_Workflow wf on ws.workflowId = wf.workflowId
	WHERE	wf.workflowId = @workflowId AND
			sc.description = @statusOutTitle
	
	IF (@statusOutId <= 0)
	BEGIN
		SET @error = '<Invalid Workflow Status (out)>'
	END


	-- Check if this option exists
	SET @result = 0

	SELECT	@result = COUNT(*)
	FROM	core_WorkStatus_Options wso
	WHERE	wso.ws_id = @statusInId AND
			wso.ws_id_out = @statusOutId AND
			wso.displayText = @optionText

	IF (LEN(@error) <= 0)
	BEGIN
		-- If only one option was found with the specified status values then set the active flag...
		IF (@result = 1)
		BEGIN

			SELECT	@wsoId = wso.wso_id
			FROM	core_WorkStatus_Options wso
			WHERE	wso.ws_id = @statusInId AND
					wso.ws_id_out = @statusOutId AND
					wso.displayText = @optionText
				
			UPDATE	core_WorkStatus_Options
			SET		active = @active
			WHERE	wso_id = @wsoId

			PRINT 'Update Option: ' + convert(VARCHAR(4),@wsoId) + ', ' + @message
		END
		ELSE
		BEGIN
			IF (@result = 0)
			BEGIN
				PRINT 'Option Does Not Exists: ' + @message
				SET @wsoId = 0
			END
			ELSE
			BEGIN
				PRINT 'Multiple Options Exists: ' + @message
				SET @wsoId = 0
			END
		
		END
	END

	IF (@wsoId <= 0)
	BEGIN
		PRINT 'Failed to update option: ' + @message + '; Error: ' + @error
	END
END
GO

