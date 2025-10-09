CREATE procedure [dbo].[core_permissions_sp_GetUserAssignable]

AS

DECLARE @allowed bit
SET @allowed=1

SELECT permId, permName, ISNULL(permDesc,'') as permDesc, @allowed, exclude 
FROM core_Permissions
WHERE exclude = 0
GO

