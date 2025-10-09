
CREATE PROCEDURE [dbo].[core_actions_sp_DeleteOptionAction]
 	@wsaid int
 	

AS

BEGIN

	DELETE FROM core_WorkStatus_Actions WHERE wsa_id=@wsaid
  
END
GO

