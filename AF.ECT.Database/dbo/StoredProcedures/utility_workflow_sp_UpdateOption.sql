-- Batch submitted through debugger: SQLQuery19.sql|7|0|C:\Users\EMORRI~1\AppData\Local\Temp\~vs2C08.sql

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	2/9/2017
-- Description:		Check if the TemplateId is the same when updating
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_UpdateOption]
	 @workflowId INT
	,@statusInTitle varchar(50)
	,@statusOutTitle varchar(50)
	,@optionText varchar(100)
	,@newStatusOutTitle varchar(50)
	,@newOptionText varchar(100)
	,@newActive bit
	,@newSortOrder tinyint
	,@newTemplateTitle varchar(50)
AS
BEGIN
	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Set display message
	declare @message varchar(300) = @workflow + ', ' + @statusInTitle + ', ' + @statusOutTitle + ', ' + @optionText
	declare @newMessage varchar(300) = @workflow + ', ' + @statusInTitle + ', ' + @statusOutTitle + ', ' + @newOptionText
	declare @error varchar(300) = ''

	-- Find the work status id that matches statusInTitle

	declare @statusInId int = 0
	
	exec @statusInId = dbo.fn_GetWorkStatusId @workflowId, @statusInTitle

	if (@statusInId <= 0)
	begin
		set @error = '<Invalid Workflow Status (in)>'
	end

	-- Find the work status id that matches statusOutTitle

	declare @statusOutId int = 0
	
	exec @statusOutId = dbo.fn_GetWorkStatusId @workflowId, @statusOutTitle
	
	if (@statusOutId <= 0)
	begin
		set @error = '<Invalid Workflow Status (out)>'
	end

	-- Find the work status id that matches newStatusOutTitle

	declare @newStatusOutId int = 0
	
	exec @newStatusOutId = dbo.fn_GetWorkStatusId @workflowId, @newStatusOutTitle
	
	if (@newStatusOutId <= 0)
	begin
		set @error = '<Invalid Workflow Status (new out)>'
	end
		
	-- Find the template id that matches templateTitle

	declare	@templateId int = 0

	select	@templateID = t.t_id
	from	core_lkupDbSignTemplates t
	where	t.title = @newTemplateTitle
	
	if (@templateId <= 0)
	begin
		set @error = '<Invalid DBSign Template>'
	end

	-- Check if the option we want to update exists
	declare @result int
	declare @wsoId int

	set @result = 0
	set @wsoId = 0

	select	@result = COUNT(*)
	from	core_WorkStatus_Options wso
	where	wso.ws_id = @statusInId and
			wso.ws_id_out = @statusOutId and
			wso.displayText = @optionText

	-- Update the option in the core_WorkStatus_Options table into the workflow step specified by @statusInId
	if (LEN(@error) <= 0)
	begin
		if @result = 0
			begin
			print 'Option Does Not Exist: ' + convert(varchar(4),@wsoId) + ', ' + @message
			end
		else if @result > 1
			begin
			print 'Multiple Copies of this Option Exist: ' + convert(varchar(4),@wsoId) + ', ' + @message
			end
		else
			begin
		
			-- Check if there already exists an option that is identical to the option we are updating to
			set @result = 0
		
			select	@result = COUNT(*)
			from	core_WorkStatus_Options wso
			where	wso.ws_id = @statusInId and
					wso.ws_id_out = @newStatusOutId and
					wso.displayText = @newOptionText and
					wso.sortOrder = @newSortOrder and
					wso.template = @templateId
		
			if @result > 0
				begin
				print 'Option Already Exists: ' + convert(varchar(4),@wsoId) + ', ' + @newMessage
				end
			else
				begin
			
				select	@wsoId = wso.wso_id
				from	core_WorkStatus_Options wso
				where	wso.ws_id = @statusInId and
						wso.ws_id_out = @statusOutId and
						wso.displayText = @optionText
					
				UPDATE core_WorkStatus_Options
				SET ws_id_out = @newStatusOutId, displayText = @newOptionText, active = @newActive, sortOrder = @newSortOrder, template = @templateId
				WHERE wso_id = @wsoId
			
				print 'Updated Option: ' + convert(varchar(4),@wsoId) + ', ' + @newMessage
			
				end
		
			end
	end

	if (@wsoId <= 0)
	begin
		print 'Failed to Update option: ' + @message + '; Error: ' + @error
	end
END
GO

