-- =========================================================
-- Author:		Eric Kelley
-- Create date: 03/15/2021
-- Description:	Returns which member can view another member
-- =========================================================

Create Procedure [dbo].[core_group_sp_GetViewBy] 
	@groupId smallInt
As
	SET NOCOUNT ON

SET NOCOUNT ON;

DECLARE @compo char(1)
SELECT @compo = compo FROM core_UserGroups WHERE groupId = @groupId



SELECT 
a.GroupId
, a.Name
--	,CAST (CASE b.managedBy
--		WHEN @groupId THEN 1
--		ELSE 0
--	END AS BIT) AS Manages
--	,ISNULL(b.notify,0) Notify
	,CAST (CASE 
		WHEN b.viewerId = @groupId and a.groupId = b.memberId THEN 1
		ELSE 0
	END AS BIT) AS ViewBy

FROM core_UserGroups a
LEFT JOIN core_UserGroupsViewBy b ON b.viewerId = @groupId AND b.memberId = a.groupId
--LEFT JOIN core_UserGroupsViewBy c ON c.viewerId = @groupId AND b.groupId = c.memberId
WHERE @groupId = b.viewerId
ORDER BY a.name


--[dbo].[core_group_sp_GetViewBy] 96
GO

