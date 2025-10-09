--core_group_sp_GetAll '2'

CREATE procedure [dbo].[core_group_sp_GetAll]
	@compo As INT
AS

SELECT	*
FROM 
	core_UserGroups a 
WHERE
	a.compo = @compo
ORDER BY 
	a.groupId
GO

