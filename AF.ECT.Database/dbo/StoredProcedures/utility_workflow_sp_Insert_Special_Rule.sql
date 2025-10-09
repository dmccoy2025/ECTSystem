-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	?
-- -- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_Insert_Special_Rule] 
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
declare @workStatusOptionId int = -1


select @workStatusOptionId = wso.wso_id 
from	core_WorkStatus_Options wso inner join
		core_WorkStatus ws on ws.ws_id = wso.ws_id inner join
		core_StatusCodes sc on ws.statusId = sc.statusId inner join
		core_Workflow wf on ws.workflowId = wf.workflowId
where	wf.workflowId = @workflowId and
		sc.description = @statusDescription and
		wso.displayText = @optionText
		
		
PRINT '@workStatusOptionId = ' + CONVERT(VARCHAR(4), @workStatusOptionId)
		
if (@workStatusOptionId < 0)
begin
	set @error = '<Invalid Work Status Option>'
end
	
-- Find matching rule id
declare @ruleId tinyint = 0

select @ruleId = lkr.Id
from core_lkupRules lkr
where lkr.name = @ruleName

PRINT '@ruleId = ' + CONVERT(VARCHAR(4), @ruleId)

if (@ruleId < 0)
begin
	set @error = @error + '<Invalid Rule Name>'
end

declare @workStatusRuleId int = 0

--declare @existingRuleData varchar(100) = ''
--select @workStatusRuleId = wsr.wsr_id, @existingRuleData = wsr.ruleData
--from core_WorkStatus_Rules wsr
--where wso_id = @workStatusOptionId and ruleId = @ruleId		

PRINT '@workStatusRuleId = ' + CONVERT(VARCHAR(4), @workStatusRuleId)


begin
	if @workStatusRuleId > 0
	begin
		update core_WorkStatus_Rules 
		set		wso_id = @workStatusOptionId,
				ruleId = @ruleId,
				ruleData = @ruleData,
				checkAll = @checkAll
		where wsr_id = @workStatusRuleId
		print 'Updated rule: ' + CONVERT(VARCHAR(4), @workStatusRuleId) + ', ' + @message
	end
	else
	begin
		insert into core_WorkStatus_Rules (wso_id,ruleId, ruleData, checkAll)
		values (@workStatusOptionId, @ruleId, @ruleData, @checkAll)
		select @workStatusRuleId = SCOPE_IDENTITY()
		print 'Added new rule: ' + CONVERT(VARCHAR(4), @workStatusRuleId) + ', ' + @message
	end
end -- LEN(@error <= 0)

if (@workStatusRuleId <= 0)
begin
	print 'Failed to insert rule: ' + @message + '; Error: ' + @error
end
		
END
GO

