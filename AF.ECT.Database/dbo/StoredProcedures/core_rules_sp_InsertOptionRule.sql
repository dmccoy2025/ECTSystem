CREATE PROCEDURE [dbo].[core_rules_sp_InsertOptionRule]
	 @optionId int
	,@ruleId tinyInt	 
	,@data varchar(500)
	,@chkAll bit =null 

AS

INSERT INTO core_WorkStatus_Rules
	(wso_id, ruleId, ruleData,checkAll)
VALUES
 (@optionId, @ruleId, @data,@chkAll)
GO

