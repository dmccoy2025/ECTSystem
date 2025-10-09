
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 3/22/2017
-- Description:	Gets all of the LOD PM users which have purview over the
--				specified unit.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_GetLODPM]
	@Member_unit_id INT
AS
BEGIN

	SELECT distinct us.FirstName, us.LastName, us.userID
	FROM core_Users us 
		JOIN core_UserRoles ur ON ur.userID = us.userID
		JOIN core_UserGroups ug ON ug.groupId = ur.groupId
	WHERE ug.name = 'LOD Program Manager'
		AND @Member_unit_id IN
			(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=us.cs_id and view_type=ug.reportView ) 

END
GO

