CREATE procedure [dbo].[core_permissions_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS

DECLARE @allowed bit
SET @allowed=1

SELECT permId, permName, ISNULL(permDesc,'') as permDesc, @allowed, exclude 
FROM core_Permissions
ORDER BY permId
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO