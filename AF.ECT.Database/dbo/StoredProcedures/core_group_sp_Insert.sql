-- =============================================
-- Author:		Nick McQuillen
-- Create date: 3/27/2008
-- Description:	Insert a group
-- =============================================
CREATE PROCEDURE [dbo].[core_group_sp_Insert]
	@name as nvarchar(100) 
   ,@abbr as nvarchar(10) 
   ,@compo as char(1) 
   ,@accessScope tinyint 
   ,@active as bit
   ,@partialMatch as bit
   ,@showInfo as bit
   ,@hipaaRequired as bit
   ,@canRegister as bit
   ,@sortOrder as tinyInt 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @groupId smallint 

	SET @groupId=(SELECT  groupId FROM dbo.core_UserGroups  WHERE 
				  abbr=@abbr AND compo=@compo) 
	
	IF (@groupId IS NULL )   
		BEGIN 
				INSERT INTO dbo.core_UserGroups 
				(name, abbr, accessScope, compo, active,partialMatch  
				,showInfo,hipaaRequired,canRegister,sortOrder)
				VALUES 
				(@name, @abbr, @accessScope, @compo, @active ,@partialMatch  
				,@showInfo,@hipaaRequired,@canRegister,@sortOrder)

				 
				SET @groupId = scope_identity()
			    --all groups are managed by System Admin, so add that record now
				INSERT INTO core_UserGroupsManagedBy (groupId, managedBy)	
				VALUES (@groupId, 2)
		END	
	ELSE
		BEGIN
			UPDATE 
					dbo.core_UserGroups
				SET
					name = @name
					,abbr = @abbr
					,compo = @compo
					,accessScope = @accessScope
					,active = @active
					,partialMatch=@partialMatch  
					,showInfo=@showInfo 
					,hipaaRequired=@hipaaRequired
					,canRegister=@canRegister
					,sortOrder=@sortOrder
				WHERE
					groupId = @groupId
		END 
		
 
		

	 

END
GO

