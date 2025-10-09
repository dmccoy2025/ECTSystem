-- =============================================
-- Author:		Andy Cooper
-- Create date: 20 March 2008
-- Description:	Returns permissions for a User
-- =============================================
CREATE PROCEDURE [dbo].[core_permissions_sp_GetByUserName] 
	@userName varchar(100)
AS

SET NOCOUNT ON;

DECLARE @userId int
SET @userId = (SELECT TOP 1 userId FROM vw_users WHERE userId = @userName ORDER BY created_date ASC )

EXEC core_permissions_sp_GetByUserId @userId
GO

