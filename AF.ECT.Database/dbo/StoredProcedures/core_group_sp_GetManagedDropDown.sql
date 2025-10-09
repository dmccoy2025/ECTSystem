-- =============================================
-- Author: Eric Kelley
-- Create date: 04/03/2021
-- Description: Retrieves a list of managed groups
-- for drop down list
-- =============================================
CREATE PROCEDURE [dbo].[core_group_sp_GetManagedDropDown]
-- Add the parameters for the stored procedure here
@groupId smallint
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;


SELECT Distinct g.groupId, g.name
--,a.notify
, CAST (CASE
WHEN v.viewerId = @groupId and g.groupId = v.memberId THEN 1
ELSE 0
END AS BIT) AS ViewBy
FROM core_UserGroupsManagedBy a
left JOIN core_UserGroups g ON g.groupId = a.groupId
Full JOIN core_UserGroupsViewBy v on v.memberId = g.groupId and v.viewerId = @groupId
WHERE a.managedBy = @groupId or v.viewerId = @groupId
ORDER BY g.name
   
--Execute [dbo].[core_group_sp_GetViewBy] @groupId

END

--[core_group_sp_GetManagedDropDown] 96
GO

