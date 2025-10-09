-- =============================================
-- Author:		Andy Cooper
-- Create date: 26 March 2008
-- Description:	Retrieves user role information
-- =============================================
CREATE PROCEDURE [dbo].[core_role_sp_GetById] 
	-- Add the parameters for the stored procedure here
	@roleId int = 0 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		a.userRoleId, a.groupId,f.name, f.abbr
		,a.status, f.accessScope ,dbo.fn_GetSignatureCodesForGroup(a.groupId)
	FROM 
		dbo.core_UserRoles a 
	INNER JOIN 
		dbo.core_UserGroups f ON f.groupId = a.groupID
 	WHERE
		a.userRoleID = @roleId
END
GO

