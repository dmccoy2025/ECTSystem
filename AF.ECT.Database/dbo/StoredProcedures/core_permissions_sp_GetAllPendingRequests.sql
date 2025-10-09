

--core_permissions_sp_GetAllPendingRequests 1

CREATE PROCEDURE [dbo].[core_permissions_sp_GetAllPendingRequests]
	@userId int

AS

--DECLARE @userId int
--SET @userId = 1

DECLARE @scope tinyint, @groupId int, @unitId int, @viewType int

SELECT 
	@scope = g.accessScope, @groupId = g.groupId, @viewType = g.reportView
	,@unitId = ISNULL(a.ada_cs_id, a.cs_id) 
FROM core_users a
JOIN core_userroles r ON r.userRoleID = a.currentRole
JOIN core_UserGroups g ON g.groupId = r.groupId
WHERE a.userID = @userId;


SELECT 
	u.userID, u.LastName, u.FirstName, RIGHT(u.SSN,4) 'SSN', c.long_name 'Unit Name', c.PAS_CODE 'Pas Code'
	,a.requested_date 'Date Requested', a.new_role 'NewRole', g.name 'Group Name'
FROM 
	core_UserRoleRequests a
JOIN 
	vw_users u ON u.userID = a.userId
JOIN 
	Command_Struct c ON c.CS_ID = u.unit_id
JOIN 
	core_UserGroups g ON g.groupId = a.requested_group_id
WHERE 
	a.[status] = 2
AND
	a.requested_group_id IN (SELECT groupId FROM core_UserGroupsManagedBy WHERE managedBy = @groupId) 
AND
	(
		@scope IN (2,3)
		OR
		(
			@scope = 1
			AND
			u.unit_id IN (
				SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @unitId AND view_type = @viewType
			)
		)
	)
ORDER BY u.LastName
GO

