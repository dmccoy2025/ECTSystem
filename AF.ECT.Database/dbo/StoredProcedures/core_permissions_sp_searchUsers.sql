--EXEC core_permissions_sp_searchUsers 1 

-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/22/2015
-- Work Item:		TFS Bug 362
-- Description:		Altered the stored procedure to use the
--					core_GroupPermissions table instead of the 
--					core_UserPermissions table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_permissions_sp_searchUsers]
	@permissionId INT
AS

SELECT	a.userId AS Id,
		a.AccessStatusDescr,
		RIGHT(a.SSN, 4) AS LastFour,
		a.LastName + '  ' + a.FirstName AS Name,
		a.RANK, 
		a.role AS RoleName,
		a.unit_Description + ' ('+a.PAS_CODE +')' AS CurrentUnitName,
		a.SSN
FROM	vw_users a
WHERE	a.groupId IN (SELECT gp.groupId FROM core_GroupPermissions gp WHERE gp.permId = @permissionId)
GO

