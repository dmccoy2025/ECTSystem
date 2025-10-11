--core_group_sp_GetAll_pagination '2'

CREATE procedure [dbo].[core_group_sp_GetAll_pagination]
	@compo As INT,
	@PageNumber INT = 1,
	@PageSize INT = 10
AS

SELECT	*
FROM 
	core_UserGroups a 
WHERE
	a.compo = @compo
ORDER BY 
	a.groupId
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO