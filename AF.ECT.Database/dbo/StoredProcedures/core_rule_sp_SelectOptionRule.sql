
CREATE PROCEDURE [dbo].[core_rule_sp_SelectOptionRule]
 	@wsoid int
 	

AS

BEGIN

	SELECT * FROM core_WorkStatus_Rules WHERE wso_id=@wsoid
  
END
GO

