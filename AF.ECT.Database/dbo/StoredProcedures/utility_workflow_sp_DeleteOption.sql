
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
CREATE PROCEDURE [dbo].[utility_workflow_sp_DeleteOption]
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

	-- Find the work status id that matches statusInTitle

	declare @statusInId int

	select	@statusInId = ws.ws_id
	from	dbo.core_WorkStatus AS ws inner join
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId inner join
			core_Workflow wf on ws.workflowId = wf.workflowId
	where	wf.workflowId = @workflowId and
			sc.description = @statusInTitle


	-- Find the work status id that matches statusOutTitle

	declare @statusOutId int

	select	@statusOutId = ws.ws_id
	from	dbo.core_WorkStatus AS ws inner join
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId inner join
			core_Workflow wf on ws.workflowId = wf.workflowId
	where	wf.workflowId = @workflowId and
			sc.description = @statusOutTitle
	
		
	-- Find the template id that matches templateTitle

	declare	@templateId int

	select	@templateID = t.t_id
	from	core_lkupDbSignTemplates t
	where	t.title = @templateTitle


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

	-- Check if option exists
	if @result = 0
		begin
		print 'Option does not exist.'
		end
	else
		begin
		-- Call procedure to delete option and any related rules and actions
		select	@wsoId = wso.wso_id
		from	core_WorkStatus_Options wso
		where	wso.ws_id = @statusInId and
			wso.ws_id_out = @statusOutId and
			wso.displayText = @optionText and
			wso.template = @templateId
		
			EXEC core_workstatus_sp_DeleteOption @wsoId		
			
		print 'The following option was deleted: ' + convert(varchar(4),@wsoId) + ', ' + convert(varchar(50),@workflow) + ', ' + convert(varchar(50),@statusInTitle) + ', ' + convert(varchar(50),@statusOutTitle) + ', ' + convert(varchar(100),@optionText) + ', ' + convert(varchar(1),@active) + ', ' + convert(varchar(4),@sortOrder) + ', ' + convert(varchar(50),@templateTitle)
	
		end
END
GO

