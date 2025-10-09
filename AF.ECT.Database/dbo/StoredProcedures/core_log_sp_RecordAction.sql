-- =============================================
-- Author:		Nick McQuillen
-- Create date: 4/10/2008
-- Description:	Insert record into Action Log
-- =============================================
-- Modified By:		Evan Morrison
-- Modified date:	7/24/2017
-- Description:		Workstatus are now integers
-- =============================================
CREATE PROCEDURE [dbo].[core_log_sp_RecordAction]
	@moduleId as tinyint,
	@actionId as tinyint,
	@userId as int,
	@referenceId as int,
	@notes as varchar(1000),
	@status INT,
	@logId int OUT,
	@newStatus INT=NULL,
	@address varchar(20)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO
		core_LogAction
		(moduleId,actionId,actionDate,userId,referenceId,notes, status,newStatus, address)
	VALUES
		(@moduleId,@actionId,getdate(),@userId,@referenceId,@notes, @status,@newStatus, @address)

	SET @logId = SCOPE_IDENTITY() 
	SELECT @logId

END
GO

