-- =========================================================
-- Author:		Eric Kelley
-- Create date: 03/15/2021
-- Description:	Updates which member can view another member
-- =========================================================
Create Procedure [dbo].[core_group_sp_UpdateViewByGroups]
	@viewerId smallInt,
	@memberId smallint
	
AS 

INSERT INTO [dbo].[core_UserGroupsViewBy]
								   (viewerId
								   ,memberId)

							 VALUES
								   (@viewerId
								   , @memberId)
--usp.core_group_sp_UpdateViewByGroups 110, 96
GO

