
CREATE PROCEDURE [dbo].[core_workflow_sp_CopyRules]
 	@dest_wsoid int,
	@src_wsoid int 
 	

AS

BEGIN

BEGIN TRANSACTION 

--Delete existing 
Delete from core_WorkStatus_Rules where wso_id=@dest_wsoid
--Copy New 
Insert into core_WorkStatus_Rules (wso_id,ruleId,ruleData,checkAll) 
  Select @dest_wsoid,ruleId,ruleData,checkAll  from core_WorkStatus_Rules
where  wso_id=@src_wsoid

COMMIT TRANSACTION 

END
GO

