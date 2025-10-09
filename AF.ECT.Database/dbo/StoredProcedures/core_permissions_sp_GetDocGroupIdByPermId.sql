
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/23/2015
-- Description:	Returns the document group id that corresponds to the specified
--				permission id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_permissions_sp_GetDocGroupIdByPermId] 
	 @permId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	docGroupId
	FROM	core_PermissionDocGroups
	WHERE	permId = @permId
END
GO

