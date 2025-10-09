
-- ============================================================================
-- Author:		Kamal Singh
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/1/2016
-- Description:		Updated to also select the workstatus ID by its associated
--					workflow Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[ReminderEmailAddSettingsByStatus] 
	@pworkflowId bigint,
	@pstatusId bigint,
	@pcompo bigint,
	@pgroupId bigint,
	@ptemplateId bigint,
	@pinterval int
AS
BEGIN
	DECLARE @wsId bigint

	SELECT	@wsId = ws.ws_Id 
	FROM	[dbo].[core_WorkStatus] ws
	WHERE	ws.statusId = @pstatusId
			AND ws.workflowId = @pworkflowId

	INSERT Into ReminderEmailSettings 
	VALUES( @pworkflowId, @wsId, @pcompo, @pgroupId, @ptemplateId, @pinterval )

	DECLARE @settingId bigint
	SELECT @settingId = SCOPE_IDENTITY()
	EXEC ReminderEmailsAdd @settingId, @wsId, @pworkflowId
END
GO

