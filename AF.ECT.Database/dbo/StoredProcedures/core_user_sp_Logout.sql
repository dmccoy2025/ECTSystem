

CREATE procedure [dbo].[core_user_sp_Logout]
	@userId int
	
AS

--clear any case locks
delete from core_WorkflowLocks where userId = @userId

--remove from online table
delete from core_UsersOnline where userId = @userId
GO

