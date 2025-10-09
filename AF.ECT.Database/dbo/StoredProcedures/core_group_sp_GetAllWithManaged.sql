-- =====================================================
-- Author:		Andy Cooper
-- Create date: 28 March 2008
-- Update by: Eric Kelley
-- Updated date: 11 Jan 2021
-- Description: Given a user groups returns a list of all user groups from that compo 
-- and a flag indicating if the specified group manages that group
-- Update Description: Adds 'ViewBy' column to procedure
-- ======================================================
-- Modified By:     Eric Kelley
-- Modified Date:   04/2/2021
-- Description:     Includes the member viewBy results.
-- ======================================================

CREATE PROCEDURE [dbo].[core_group_sp_GetAllWithManaged] 
	@groupId smallint 
AS

SET NOCOUNT ON;

DECLARE @compo char(1)
SELECT @compo = compo FROM core_UserGroups WHERE groupId = @groupId





SELECT a.GroupId, a.Name
	,CAST (CASE b.managedBy
		WHEN @groupId THEN 1
		ELSE 0
	END AS BIT) AS Manages
	,ISNULL(b.notify,0) Notify
	
	,CAST (CASE 
		WHEN c.viewerId = @groupId AND c.memberId = a.groupId Then 1
		ELSE 0
	END AS BIT) AS ViewBy

FROM core_UserGroups a
LEFT JOIN core_UserGroupsManagedBy b ON b.managedby = @groupId AND b.groupId = a.groupId
left JOIN core_UserGroupsViewBy c ON c.viewerId = @groupId  AND c.memberId = a.groupid
WHERE (a.compo = @compo OR @compo = '0')

ORDER BY a.name


--[core_group_sp_GetAllWithManaged] 96
GO

