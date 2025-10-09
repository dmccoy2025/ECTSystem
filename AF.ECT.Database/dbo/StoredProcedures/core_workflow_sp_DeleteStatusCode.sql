-- =============================================
-- Author:		Andy Cooper
-- Create date: 9 May 2008
-- Description:	Deletes a StatusCode
-- =============================================
CREATE PROCEDURE [dbo].[core_workflow_sp_DeleteStatusCode] 
	@statusId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM core_LogAction
	WHERE status = @statusId

	DELETE FROM core_StatusCodes 
	WHERE statusId = @statusId
	
	

END
GO

