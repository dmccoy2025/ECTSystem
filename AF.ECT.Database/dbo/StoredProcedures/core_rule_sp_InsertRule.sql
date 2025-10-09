
CREATE PROCEDURE [dbo].[core_rule_sp_InsertRule]
 	@workFlow tinyint,
	@ruleType tinyint,
	@name varchar(50),
	@prompt varchar(50),
	@active bit 

AS

 Declare @ruleId as tinyInt
 Set @ruleId=(SELECT Id FROM  core_lkupRules WHERE NAME=@name) 

IF (@workFlow=0)  SET @workFlow=null

IF (@ruleId is NULL  )  
	BEGIN

		INSERT INTO core_lkupRules
		(workFlow, ruleType, name, prompt,active)
		VALUES
		(@workFlow, @ruleType, @name, @prompt, @active)

	SELECT @@IDENTITY 

	END
ELSE
	BEGIN

		UPDATE core_lkupRules
			SET 
				workFlow = @workFlow
			   ,ruleType = @ruleType
			   ,name = @name
			   ,prompt = @prompt
			   ,active=@active
			WHERE Id = @ruleId

	SELECT @ruleId

END
GO

