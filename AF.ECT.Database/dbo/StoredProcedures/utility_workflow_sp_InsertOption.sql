
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
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertOption]
	 @workflowId INT
	,@statusInTitle varchar(50)
	,@statusOutTitle varchar(50)
	,@optionText varchar(100)
	,@active bit
	,@sortOrder tinyint
	,@templateTitle varchar(50)
AS
BEGIN
	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Set display message
	declare @message varchar(300) = @workflow + ', ' + @statusInTitle + ', ' + @statusOutTitle + ', ' + @optionText
	declare @error varchar(300) = ''

	-- Find the work status id that matches statusInTitle

	declare @statusInId int = 0

	select	@statusInId = ws.ws_id
	from	dbo.core_WorkStatus AS ws inner join
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId inner join
			core_Workflow wf on ws.workflowId = wf.workflowId
	where	wf.workflowId = @workflowId
			AND sc.description = @statusInTitle

	if (@statusInId <= 0)
	begin
		set @error = '<Invalid Workflow Status (in)>'
	end

	-- Find the work status id that matches statusInTitle

	declare @statusOutId int = 0

	select	@statusOutId = ws.ws_id
	from	dbo.core_WorkStatus AS ws inner join
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId inner join
			core_Workflow wf on ws.workflowId = wf.workflowId
	where	wf.workflowId = @workflowId
			AND sc.description = @statusOutTitle
	
	if (@statusOutId <= 0)
	begin
		set @error = '<Invalid Workflow Status (out)>'
	end
		
	-- Find the template id that matches templateTitle

	declare	@templateId int = 0

	select	@templateID = t.t_id
	from	core_lkupDbSignTemplates t
	where	t.title = @templateTitle
	
	if (@templateId <= 0)
	begin
		set @error = '<Invalid DBSign Template>'
	end

	-- Check if there already exists an option identical to this new one
	declare @result int
	declare @wsoId int

	set @result = 0

	select	@result = COUNT(*)
	from	core_WorkStatus_Options wso
	where	wso.ws_id = @statusInId and
			wso.ws_id_out = @statusOutId and
			wso.displayText = @optionText and
			wso.template = @templateId

	-- Insert a new option into the core_WorkStatus_Options table into the workflow step specified by @statusInId
	if (LEN(@error) <= 0)
	begin
		if @result = 0
			begin

			insert into core_WorkStatus_Options (ws_id, ws_id_out, displayText, active, sortOrder, template)
			values (@statusInId, @statusOutId, @optionText, @active, @sortOrder, @templateId)

			select @wsoId = @@IDENTITY 

			print 'Added Option: ' + convert(varchar(4),@wsoId) + ', ' + @message
			end
		else
			begin
		
			select	@wsoId = wso.wso_id
			from	core_WorkStatus_Options wso
			where	wso.ws_id = @statusInId and
					wso.ws_id_out = @statusOutId and
					wso.displayText = @optionText and
					wso.template = @templateId
				
			print 'Option Already Exists: ' + convert(varchar(4),@wsoId) + ', ' + @message
		
			end
	end

	if (@wsoId <= 0)
	begin
		print 'Failed to insert option: ' + @message + '; Error: ' + @error
	end
END
GO

