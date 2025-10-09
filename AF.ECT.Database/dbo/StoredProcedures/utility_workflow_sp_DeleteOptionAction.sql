
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		10/25/2016
-- Description:		Created to delete actions
-- ============================================================================
-- Modified Author:		Evan Morrison
-- Modified date:		10/25/2016
-- Description:			Able to delete actions that do not have target or data
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_DeleteOptionAction]
	 @workflowId INT
	,@statusDescription varchar(50)
	,@optionText varchar(100)
	,@actionName varchar(50)
	,@actionTarget varchar(50)
	,@actionData varchar(50)
AS
BEGIN
	
	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Set display message
	declare @message varchar(300) = @workflow + ', ' + @statusDescription + ', ' + @optionText + ', ' + @actionName
	declare @error varchar(300) = ''

	-- Find matching work status option id

	declare @workStatusOptionId int = -1

	select @workStatusOptionId = wso.wso_id 
	from	core_WorkStatus_Options wso inner join
			core_WorkStatus ws on ws.ws_id = wso.ws_id inner join
			core_StatusCodes sc on ws.statusId = sc.statusId inner join
			core_Workflow wf on ws.workflowId = wf.workflowId
	where	wf.workflowId = @workflowId and
			sc.description = @statusDescription and
			wso.displayText = @optionText

	if (@workStatusOptionId < 0)
	begin
		set @error = '<Invalid Work Status Option>'
	end

	-- Find matching rule id
	declare @actionId tinyint = 9

	select @actionId = lka.type
	from core_lkupWorkflowAction lka
	where lka.text = @actionName

	declare @userGroup int = -1
	declare @data int = -1

	if (@actionName = 'Send Email')
	begin

		select @userGroup = UG.groupId
		from core_UserGroups UG
		Where UG.name = @actionTarget

		select @data = ET.TemplateID
		from core_EmailTemplates ET
		WHERE ET.Title = @actionData
	end
	else if (@actionName = 'Sign Memo' OR @actionName = 'Remove Signature' OR @actionName = 'Add Signature')
	begin
		select @userGroup = UG.groupId
		from core_UserGroups UG
		Where UG.name = @actionTarget

		set @data = 0
	end
	else if (@actionName IN ('Change To Formal','Save RWOA','Save Final Decision','Add Approval Authority Signature','Set SAF Upload Date','Change To Informal') )
	begin

		set @userGroup = 0
		set @data = 0
	end
	else if (@actionName = 'Send Lessons Learned Email')
	begin

		set @userGroup = 0
		select @data = ET.TemplateID
		from core_EmailTemplates ET
		WHERE ET.Title = @actionTarget
	end
	else if (@actionName = 'Recommend Cancel Formal')
	begin

		set @userGroup = 0
		set @data = @actionData
	end
	ELSE
	BEGIN
		SET @userGroup = 0
		SET @data = 0
	END

	if (@userGroup < 0)
	begin
		set @error = @error + '<Invalid Action Target>'
	end
	if (@data < 0)
	begin
		set @error = @error + '<Invalid Action Data>'
	end

	if (LEN(@error) <= 0)
	begin
		if @actionId > 0
		begin
			DELETE FROM core_WorkStatus_Actions
			where wso_id = @workStatusOptionId AND actionType = @actionId AND target = @userGroup AND data = @data
			print 'Deleted rule: ' + CONVERT(VARCHAR(4), @workStatusOptionId) + ', ' + @message
		end
		else
		begin
			print 'No rule with these parameters exists'
		end
	end -- LEN(@error <= 0)
	else
	begin
		print 'Failed to insert rule: ' + @message + '; Error: ' + @error
	end

	END
GO

