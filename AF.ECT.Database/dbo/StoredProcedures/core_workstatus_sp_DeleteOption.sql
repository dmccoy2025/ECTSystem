

CREATE PROCEDURE [dbo].[core_workstatus_sp_DeleteOption]
	@optionId int

AS

SET XACT_ABORT ON
BEGIN TRANSACTION

DELETE FROM core_WorkStatus_Rules WHERE wso_id = @optionId
DELETE FROM core_WorkStatus_Actions WHERE wso_id = @optionId
DELETE FROM core_WorkStatus_Options WHERE wso_id = @optionId

COMMIT TRANSACTION
GO

