
CREATE PROCEDURE [dbo].[core_user_sp_UpdateManagedSettings]
	@userId int,
	@compo char(1),
	@roleId int,
	@groupId tinyint,
	@comment varchar(200),
	@receiveEmail bit,
	@expirationDate dateTime 

AS

UPDATE core_users
SET workCompo = @compo, receiveEmail = @receiveEmail, comment = @comment,
expirationDate=@expirationDate
WHERE userID = @userId

UPDATE core_UserRoles
SET groupId = @groupId
WHERE userRoleID = @roleID
GO

