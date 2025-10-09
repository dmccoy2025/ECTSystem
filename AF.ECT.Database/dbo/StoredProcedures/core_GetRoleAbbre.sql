CREATE PROCEDURE [dbo].[core_GetRoleAbbre]

	@groups VARCHAR(100)

AS

SET NOCOUNT ON;

SELECT     name + ','
FROM         dbo.core_UserGroups
WHERE     (groupId IN (select value from dbo.split(@groups, ',')))
GO

