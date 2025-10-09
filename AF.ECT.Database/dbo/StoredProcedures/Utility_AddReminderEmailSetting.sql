-- ============================================================================
-- Author:		?
-- Create date:	?
-- Description:	?
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
CREATE PROCEDURE [dbo].[Utility_AddReminderEmailSetting] 
	@pworkflowId INT,
	@pstatus varchar(256),
	@pcompo varchar(128),
	@pgroup varchar(128),
	@ptemplate varchar(128),
	@pinterval int
AS

BEGIN
	DECLARE @statusId bigint
	DECLARE @compoId bigint
	DECLARE @groupId bigint
	DECLARE @templateId bigint
	DECLARE @settingId bigint

	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @pworkflowId)
	
	SELECT @statusId = statusId FROM [dbo].[core_StatusCodes] WHERE description = @pstatus
			
	SELECT @compoId = compo FROM [dbo].[core_lkupCompo] WHERE compo_descr = @pcompo
	
	SELECT @groupId = groupId FROM [dbo].[core_UserGroups] WHERE name = @pgroup
	
	SELECT @templateId = TemplateId FROM [dbo].[core_EmailTemplates] WHERE title = @ptemplate
	
	SELECT @settingId = setting.id FROM [dbo].[ReminderEmailSettings] AS setting INNER JOIN [dbo].[core_Workstatus] AS workStatus
	ON workstatus.ws_id = setting.wsId
	WHERE setting.workflowId = @pworkflowId AND workStatus.statusId = @statusID
	
	IF(@pworkflowId = 0 OR @pworkflowId IS NULL)
	BEGIN
		PRINT ''
		PRINT @workflow + ' Workflow Not Found for Status ' + @pstatus
		PRINT ''
	
	END
	
	ELSE IF(@statusId = 0 OR @statusId IS NULL)
	BEGIN
		PRINT ''
		PRINT @pstatus + ' Status Not Found for Workflow ' + @workflow
		PRINT ''
	END
	
	ELSE IF(@compoId = 0 OR @compoId IS NULL)
	BEGIN
		PRINT ''
		PRINT @pcompo + ' Compo Not Found'
		PRINT ''
	END
	
	ELSE IF(@groupId = 0 OR @groupId IS NULL)
	BEGIN
		PRINT ''
		PRINT @pgroup + ' Group Not Found'
		PRINT ''
	END
	
	ELSE IF(@templateId = 0 OR @templateId IS NULL)
	BEGIN
		PRINT ''
		PRINT @ptemplate + ' Template Not Found'
		PRINT ''
	END
	
	ELSE IF (@pinterval < 1 OR @pinterval > 999)
	BEGIN
		PRINT 'interval must be between 1 and 999,' + @pinterval + ' is not valid'
	END
	
	ELSE IF (@settingId > 0)
	BEGIN
		PRINT''
		PRINT @workflow + ' Workflow and ' + @pstatus + ' Already Exist'
		PRINT''
	END

		
	ELSE
	BEGIN
		EXEC ReminderEmailAddSettingsByStatus @pworkflowId, @statusId, @compoId, @groupId, @templateId, @pinterval
		PRINT 'Workflow ' + @workflow + ' and Status ' + @pstatus + ' was Added Succesfully'
	END

END
GO

