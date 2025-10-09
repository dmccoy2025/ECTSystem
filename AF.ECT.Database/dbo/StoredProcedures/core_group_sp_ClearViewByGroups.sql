-- =========================================================
-- Author:		Eric Kelley
-- Create date: 03/15/2021
-- Description:	Resets viewBy option by groupID
-- =========================================================
Create Procedure [dbo].[core_group_sp_ClearViewByGroups]
	@groupId smallint
AS

DELETE FROM [dbo].[core_UserGroupsViewBy]
      WHERE viewerId = @groupId

--usp.core_group_sp_ClearViewByGroups 110
GO

