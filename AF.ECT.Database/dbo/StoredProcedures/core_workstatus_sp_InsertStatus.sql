

CREATE PROCEDURE [dbo].[core_workstatus_sp_InsertStatus]
	@workflow int,
	@status int 

AS

IF NOT EXISTS (SELECT statusId FROM core_workStatus WHERE workflowId = @workflow AND statusId = @status)
BEGIN
	DECLARE @ct int
	SET @ct = (SELECT count(*) FROM core_WorkStatus WHERE workflowId = @workflow)
	INSERT INTO core_WorkStatus (workflowId, statusId, sortOrder) VALUES (@workflow, @status, @ct)
END
GO

