-- =============================================
-- Author:		Nandita Srivastava
-- Create date: 26 March 2008
-- Description:	Retrieves user role information for a userId
-- =============================================
CREATE PROCEDURE [dbo].[core_role_sp_GetAll] 
	-- Add the parameters for the stored procedure here
	@userId int
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
  
	 
END
GO

