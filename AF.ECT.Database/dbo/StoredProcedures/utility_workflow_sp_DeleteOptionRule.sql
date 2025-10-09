
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		1) Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
--					2) Altered the memo select to include a WHERE
--					clause which prevents a memo ID value of 0 from being
--					selected and included in the @processedRuleData variable.
--					3) Altered the Previous status select to include a WHERE
--					clause which prevents a status ID value of 0 from being 
--					selected and included in the @processedRuleData variable.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_DeleteOptionRule]
	 @workflowId INT
	,@statusDescription varchar(50)
	,@optionText varchar(100) 	 
	,@ruleName varchar(50)
	,@ruleType varchar(50)
	,@ruleData varchar(1000)
	,@checkAll bit 
AS
BEGIN
	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Set display message
	declare @message varchar(300) = @workflow + ', ' + @statusDescription + ', ' + @optionText + ', ' + @ruleName
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
	declare @ruleId tinyint = 0

	select @ruleId = lkr.Id
	from core_lkupRules lkr
	where lkr.name = @ruleName

	if (@ruleId < 0)
	begin
		set @error = @error + '<Invalid Rule Name>'
	end

	declare @workStatusRuleId int = 0

	declare @existingRuleData varchar(100) = ''
	select @workStatusRuleId = wsr.wsr_id, @existingRuleData = wsr.ruleData
	from core_WorkStatus_Rules wsr
	where wso_id = @workStatusOptionId and ruleId = @ruleId

	-- Process rule data.  Some rule data exists as comma separated lists that 
	-- need to be pre-processed prior to inserting into the option rule.   
	declare @processedRuleData varchar(100)

	-- Previous status - Convert comma separated status values into integer status codes
	if	@ruleName = 'validatepreviousstatus' OR 
		@ruleName = 'validatepoststatus' OR 
		@ruleName = 'laststatuswas' OR 
		@ruleName = 'laststatuswasnot' OR 
		@ruleName = 'prevstatuswas' OR 
		@ruleName = 'prevstatuswasnot'
	begin
		select @processedRuleData = coalesce(@processedRuleData + ',', '') + CONVERT(VARCHAR(4), isnull(newData.statusId, ''))
		from	
		(
			(
				-- New data is csv of strings, so need to look up values
				select  seNew.statusId as statusId
				from	fn_Split(@ruleData, ',') rdata inner join 
						fn_GetStatusEnums() seNew on seNew.value = rdata.value and seNew.statusId > 0
			)
			UNION
			(
				-- Existing data is csv of ids
				select erdata.value as statusId
				from	fn_Split(@existingRuleData, ',') erdata
				where LEN(ISNULL(erdata.value,'')) > 0 AND erdata.value != '0' 
			)
		) as newData
		WHERE newData.statusId <> 0
	
		if ((LEN(ISNULL(@processedRuleData,'')) = 0) OR @processedRuleData = '0')
		begin
			set @error = @error + '<Invalid Rule Data>'
			set @processedRuleData = ''
		end
	end

	-- Document
	else if @ruleName = 'document'
	begin
		select @processedRuleData = coalesce(@processedRuleData + ',', '') + CONVERT(VARCHAR(100), newData.document)
		from	
		(
			(
				select rdata.value as document
				from	fn_Split(@ruleData, ',') rdata 
			)
			UNION
			(
				select erdata.value as document
				from	fn_Split(@existingRuleData, ',') erdata
				where LEN(ISNULL(erdata.value,'')) > 0 AND erdata.value != '0'
 			)
		) as newData
	
		if ((LEN(ISNULL(@processedRuleData,'')) = 0) OR @processedRuleData = '0')
		begin
			set @error = @error + '<Invalid Rule Data>'
			set @processedRuleData = ''
		end
	end

	-- Memo
	else if @ruleName = 'memo'
	begin
		select @processedRuleData = coalesce(@processedRuleData + ',', '') + CONVERT(VARCHAR(4), newData.memoId)
		from	
		(
			(
				-- New data is csv of strings, so need to look up ids
				select mt.templateId as memoId
				from	fn_Split(@ruleData, ',') rdata inner join
						core_MemoTemplates mt on rdata.value = mt.title and mt.templateId > 0
			)
			UNION
			(
				-- Existing data is csv of ids
				select erdata.value as memoId
				from	fn_Split(@existingRuleData, ',') erdata 
				where LEN(ISNULL(erdata.value,'')) > 0 AND erdata.value != '0'
			)
		) as newData
		WHERE newdata.memoId <> 0
	
		if ((LEN(ISNULL(@processedRuleData,'')) = 0) OR @processedRuleData = '0')
		begin
			set @error = @error + '<Invalid Rule Data>'
			set @processedRuleData = ''
		end
	end

	-- Default case - no pre-processing
	else
	begin
		select @processedRuleData = @ruleData
	end

	if (LEN(@error) <= 0)
	begin
		if @workStatusRuleId > 0
		begin
			DELETE FROM core_WorkStatus_Rules
			where wsr_id = @workStatusRuleId
			print 'Deleted rule: ' + CONVERT(VARCHAR(4), @workStatusRuleId) + ', ' + @message
		end
		else
		begin
			print 'No rule with these parameters exists'
		end
	end -- LEN(@error <= 0)

	if (@workStatusRuleId <= 0)
	begin
		print 'Failed to insert rule: ' + @message + '; Error: ' + @error
	end
END
GO

