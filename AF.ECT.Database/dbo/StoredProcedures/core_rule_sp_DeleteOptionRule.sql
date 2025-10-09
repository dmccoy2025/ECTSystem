
CREATE PROCEDURE [dbo].[core_rule_sp_DeleteOptionRule]
 	@wsrid int
 	

AS

BEGIN

	DELETE FROM core_WorkStatus_Rules WHERE wsr_id=@wsrid
  
END
GO

