-- =============================================
-- Author:		Andy Cooper
-- Create date: 26 March 2008
-- Description:	Retrieves the Passcodes for a user role
-- =============================================
CREATE PROCEDURE [dbo].[core_role_sp_GetPasCodesByRoleId] 
	@roleId int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT 
		t1.PasCode,t1.Status, cast(1 as bit) as canEdit
    FROM 
		core_UserRolePasCodes  t1 		 
     WHERE 
		t1.userRoleId = @roleId 
	AND
		t1.status != 5
    
END
GO

