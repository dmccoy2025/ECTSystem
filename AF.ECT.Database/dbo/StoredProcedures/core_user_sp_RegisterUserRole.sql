CREATE procedure [dbo].[core_user_sp_RegisterUserRole]
	@userID  int ,  
	@groupID  smallint ,  
	@status tinyint , --Default value is pending approval 
 	@userRoleID int OUTPUT

As

BEGIN
 
		set @userRoleID=(select userRoleID from  core_UserRoles   where userID=@userID   )
	 
		if( @userRoleID is null )
				Begin 
			--Set the status as 1 initial Pending
						insert into core_UserRoles(userID,groupID,status) 
							values (@userID,@groupID,@status)  
							set @userRoleID=SCOPE_IDENTITY() 
			 
				End   
		Else
			   Begin			

			update  core_UserRoles 
				set  
					 groupID=@groupID,
					 status=@status					 
			 where userRoleID=@userRoleID
			End 
	
		
			
End
GO

