
CREATE PROCEDURE [dbo].[core_workflow_sp_CopyActions]
 	@dest_wsoid int,
	@src_wsoid int 
 	

AS

BEGIN

BEGIN TRANSACTION 

--Delete existing 
Delete from core_WorkStatus_Actions where wso_id=@dest_wsoid 
--Copy New 
Insert into core_WorkStatus_Actions (wso_id,actionType,target,data) 
  Select @dest_wsoid,actionType,target,data  from core_WorkStatus_Actions 
where  wso_id=@src_wsoid

COMMIT TRANSACTION 

END
GO

