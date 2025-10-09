

CREATE PROCEDURE [dbo].[core_rule_sp_DeleteRule]
	@ruleId tinyInt

AS

SET XACT_ABORT ON


--DELETE FROM core_WorkStatus_Rules WHERE ruleId = @ruleId
DELETE FROM core_lkupRules WHERE id = @ruleId
GO

