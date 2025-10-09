
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
-- Modified Date:	7/20/2017
-- Description:		Remove signature action now takes in the workstatus
--					id as a data point
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertOptionAction]
	 @workflowId INT
	,@statusDescription varchar(50)
	,@optionDisplayText varchar(100)
	,@actionTypeText varchar(50)
	,@targetName varchar(100)
	,@dataTitle varchar(50)
AS
BEGIN
	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Set display message
	declare @message varchar(300) = @workflow + ', ' + @statusDescription + ', ' + @optionDisplayText + ', ' + @actionTypeText + ', ' + @targetName + ', ' + @dataTitle
	declare @error varchar(300) = ''

	-- Find matching work status option id

	declare @workStatusOptionId int = 0

	select	@workStatusOptionId = wso.wso_id 
	from	core_WorkStatus_Options wso inner join
			core_WorkStatus ws on ws.ws_id = wso.ws_id inner join
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId inner join
			core_Workflow wf on ws.workflowId = wf.workflowId
	where	wf.workflowId = @workflowId AND
			sc.description = @statusDescription and
			wso.displayText = @optionDisplayText

	if (@workStatusOptionId <= 0)
	begin
		set @error = '<Invalid Work Status Option>'
	end

	-- Declare id variables

	declare @typeId int = 0
	declare @targetId int = -1
	declare @dataId int = -1

	-- Find the action type id

	select	@typeId = wfa.type
	from	core_lkupWorkflowAction wfa
	where	wfa.text = @actionTypeText

	if (@typeId < 0)
	begin
		set @error = @error + '<Invalid Action Type>'
	end

	-- Find the target id if the action type requires one

	if @actionTypeText = 'Send Email' OR @actionTypeText = 'Add Signature' OR @actionTypeText = 'Remove Signature'
		-- Set target id to a value from UserGroups
		select @targetId = ug.groupId
		from core_UserGroups ug
		where ug.name = @targetName
	
	else if @actionTypeText = 'Sign Memo' 
		-- Set target id to a value from MemoTemplates
		select @targetId = mt.templateId
		from core_MemoTemplates mt
		where mt.title = @targetName
	else
		set @targetId = 0

	if (@targetId < 0)
	begin
		set @error = @error + '<Invalid Target>'
	end

	-- Find the data id if the action type requires one

	if @actionTypeText = 'Send Email' OR @actionTypeText = 'Send Lessons Learned Email'
		-- Set data id to value from EmailTemlpates
		select @dataId = et.TemplateID
		from core_EmailTemplates et
		where et.Title = @dataTitle
	else if @actionTypeText = 'Recommend Cancel Formal'
		set @dataId = CONVERT(INT, @dataTitle)
	else if @actionTypeText = 'Remove Signature'
		SELECT @dataId = ws.ws_id
		FROM core_WorkStatus ws
		JOIN core_StatusCodes sc on sc.statusId = ws.statusId
		where sc.description = @dataTitle
	else
		set @dataId = 0

	if (@dataId < 0)
	begin
		set @error = @error + '<Invalid Data>'
	end

	-- Check if there already exists an action identical to this new one in this option
	declare @result int
	declare @wsaId int = 0

	set @result = 0

	select	@result = COUNT(*)
	from	core_WorkStatus_Actions wsa
	where	wsa.wso_id = @workStatusOptionId and
			wsa.actionType = @typeId and
			wsa.target = @targetId and
			wsa.data = @dataId

	-- Insert a new action into the option

	if (LEN(@error) <= 0)
	begin
		if @result = 0
			begin

			insert into core_WorkStatus_Actions (actionType, wso_id, target, data)
			values (@typeId, @workStatusOptionId , @targetId, @dataId)

			select @wsaId = SCOPE_IDENTITY()

			print 'Added Action: ' + convert(varchar(4),@wsaId) + ', ' + @message
		
			end

		else
			begin
		
			select	@wsaId = wsa.wsa_id
			from	core_WorkStatus_Actions wsa
			where	wsa.wso_id = @workStatusOptionId and
					wsa.actionType = @typeId and
					wsa.target = @targetId and
					wsa.data = @dataId
		
			print 'Action Already Exists: ' + convert(varchar(4),@wsaId) + ', ' + @message
		
			end
	end
	if (@wsaId <= 0)
	begin
		print 'Failed to insert action: ' + @message + '; Error: ' + @error
	end
END
GO

