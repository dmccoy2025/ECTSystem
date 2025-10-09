CREATE procedure [dbo].[core_permissions_sp_GetAll]

AS

DECLARE @allowed bit
SET @allowed=1

SELECT permId, permName, ISNULL(permDesc,'') as permDesc, @allowed, exclude 
FROM core_Permissions
GO

