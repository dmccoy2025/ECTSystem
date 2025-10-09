
-- ============================================================================
-- Author:		Kamal Singh
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Cleaned up the procedure to increase readability.
-- ============================================================================
CREATE PROCEDURE [dbo].[ReminderEmailGetSettingsByStatus] 
	@pworkflowId BIGINT,
	@pstatusId BIGINT,
	@pcompo BIGINT
AS
BEGIN
	SELECT	eSetting.id As id, eSetting.groupId AS groupId, userGroup.name AS groupName, eSetting.templateId AS templateID,template.Title AS templateName, eSetting.intervalTime as intervalTime
	FROM	ReminderEmailSettings eSetting 
			INNER JOIN [ALOD].[dbo].[core_UserGroups] As userGroup ON eSetting.groupId = userGroup.groupId 
			INNER JOIN [ALOD].[dbo].[Core_EmailTemplates] AS template ON eSetting.templateId = template.TemplateID 
			INNER JOIN [dbo].[core_WorkStatus] AS workStatus ON eSetting.wsId = workStatus.ws_Id 
	WHERE	eSetting.workflowId = @pworkflowId 
			AND workStatus.StatusId = @pstatusID 
			AND eSetting.compo = @pcompo
END
GO

