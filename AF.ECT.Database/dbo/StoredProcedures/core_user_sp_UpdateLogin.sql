
CREATE PROCEDURE [dbo].[core_user_sp_UpdateLogin] 
	@userId int,
	@sessionId nvarchar(50),
	@remoteAddr nvarchar(50)

AS

SET NOCOUNT ON

declare @isValid bit

--insert/update this user
IF EXISTS (SELECT userId FROM core_usersOnline WHERE userId = @userId)
begin

	--this user is already logged in, is it the same session?
	declare @curSession nvarchar(50)
	set @curSession = (select sessionId from core_UsersOnline where userId = @userId)
	
	if (@curSession = @sessionId) --same session
	begin
		UPDATE core_usersOnline 
		SET lastAccess = Getdate()
		WHERE userId = @userId
	
		set @isValid = 1
	end
	else
	begin
		--the sessions don't match
		set @isValid = 0
	end
end 
ELSE
BEGIN
	--this user/account has not logged in 
	--delete any other entries this user might have, deteremined by edipin
	declare @edipin nvarchar(10)
	set @edipin = (select edipin from core_Users where userID = @userId);
	
	delete from core_UsersOnline where userId in (
		select userId from core_Users where EDIPIN like @edipin);
	
	DECLARE @groupId tinyint
	
	SET @groupId = (SELECT TOP 1 groupId FROM vw_users WHERE userID = @userId)
	INSERT INTO core_UsersOnline (userId, groupId, sessionId, remoteAddress) VALUES (@userId, @groupId, @sessionId, @remoteAddr)
	UPDATE core_users SET lastAccessDate = getdate() WHERE userID = @userId
	
	set @isValid = 1
END

--delete any expired users
declare @expired table
(
	userId int
)

insert into @expired 
select userId FROM core_UsersOnline WHERE lastAccess < dateadd(minute,-30,getdate())

--clear any case locks for expired sessions
delete from core_WorkflowLocks where userId in (select userId from @expired)

--delete from online
delete from core_UsersOnline where userId in (select userId from @expired)

--return our access
select @isValid
GO

