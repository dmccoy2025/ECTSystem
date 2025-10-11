-- =============================================
-- Author:		Nandita Srivastava
-- Create date: 26 March 2008
-- Description:	Retrieves user role information for a userId with pagination
-- =============================================
CREATE PROCEDURE [dbo].[core_role_sp_GetAll_pagination]
	-- Add the parameters for the stored procedure here
	@userId int,
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
 
	SELECT 
		a.userRoleId, a.groupId, f.name, f.abbr
		,a.status, f.accessScope,dbo.fn_GetSignatureCodesForGroup(a.groupId)
	FROM 
		dbo.core_UserRoles a 
	INNER JOIN 
		dbo.core_UserGroups f ON f.groupId = a.groupID
	 
	WHERE 
		a.userID=@userId 
	AND
		a.status!=5
	ORDER BY a.userRoleId
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
	 
END
GO