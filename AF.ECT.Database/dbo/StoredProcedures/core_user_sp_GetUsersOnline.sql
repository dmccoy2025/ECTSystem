

CREATE PROCEDURE [dbo].[core_user_sp_GetUsersOnline]

AS

SET NOCOUNT ON

SELECT 
	a.userId, b.rank + ' ' + b.lastname + ', ' + b.firstName AS UserName
	,b.role,b.unit_description + ' ('+ b.pas_code +')' as unitName , a.loginTime
FROM 
	core_UsersOnline a
JOIN 
	vw_users b ON b.userID = a.userId
GO

