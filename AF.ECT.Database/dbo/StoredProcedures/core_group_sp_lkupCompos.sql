CREATE PROCEDURE [dbo].[core_group_sp_lkupCompos]
	@compo char(1)
	
As

	SET NOCOUNT ON;

SELECT groupId as Value, name as Name FROM core_UserGroups WHERE compo = @compo AND groupId > 1
GO

