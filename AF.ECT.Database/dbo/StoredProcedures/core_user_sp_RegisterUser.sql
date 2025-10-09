
-- =============================================
-- Author:		Nandita Srivastava
-- Create date: 28 March 2008
-- Description:	Creates or adds a new user 
-- =============================================
/* This procedure will insert/update  a new user */
CREATE procedure [dbo].[core_user_sp_RegisterUser]
		@userID int, 
        @workCompo Char(1),
        @receiveEmail bit,
        @groupID int,
		@accountStatus tinyInt,
		@expirationDate DateTime 
	 
AS

DECLARE @userRoleID AS int

IF NOT EXISTS (SELECT userID FROM core_Users WHERE userID=@userID)
BEGIN  	  

	--If the user does not exist insert set the initial userrole ID to 1 (something)
	INSERT INTO core_users(userID,workCompo,accessStatus,receiveEmail,created_date,expirationDate) 
	VALUES (@userID,@workCompo,@accountStatus,@receiveEmail,getUTCDate(),@expirationDate) 

	--Insert new default user role in the user role table
--	IF NOT EXISTS (SELECT userRoleID FROM core_UserRoles WHERE userID=@userID and groupID=1)
--	BEGIN
		--Set the status as 1 initial Pending and the initial group as default user
---		INSERT INTO core_UserRoles(userID,groupID,status) VALUES (@userID,1,2)  
--		SET @userRoleID=SCOPE_IDENTITY() 
--	END 
	 
--	UPDATE core_users SET currentRoleId=@userRoleID WHERE userID=@userID

END 
 
ELSE --Update user
BEGIN   
	UPDATE core_Users 
	SET workCompo=@workCompo, receiveEmail=@receiveEmail,expirationDate=@expirationDate,accessstatus=@accountStatus
	WHERE userID=@userID
END
GO

