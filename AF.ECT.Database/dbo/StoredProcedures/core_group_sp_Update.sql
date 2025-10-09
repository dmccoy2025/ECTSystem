-- =============================================
-- Author:		Andy Cooper
-- Create date: 27 March 2008
-- Description:	Updates a user group
-- =============================================
CREATE PROCEDURE [dbo].[core_group_sp_Update] 
	-- Add the parameters for the stored procedure here
	@groupId int, 
	@name varchar(100),
	@abbr varchar(10),
	@compo char(1),
	@accessScope tinyint,
	@active bit
   ,@partialMatch as bit
   ,@showInfo as bit
   ,@hipaaRequired as bit
   ,@canRegister as bit
   ,@sortOrder as tinyInt 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
GO

