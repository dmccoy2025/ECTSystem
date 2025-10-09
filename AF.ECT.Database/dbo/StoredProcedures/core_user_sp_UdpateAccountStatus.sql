-- =============================================
-- Author:		Nandita Srivastava
-- Create date: 28 March 2008
-- Description:	Updates the user account if the current status is pending or disabled then only expiration date is changed
-- =============================================
CREATE PROCEDURE [dbo].[core_user_sp_UdpateAccountStatus]
	@userID int,
	@accountStatus tinyInt,
	@expirationDate dateTime,
	@comment varchar(200)
    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from

SET NOCOUNT ON;
Declare @currentStatus as tinyInt 
set @currentStatus =(select accessStatus from core_Users where userID=@userID)

 

   if (@currentStatus is not null )
		Begin 
		 -- if (((@currentStatus=1 ) or (@currentStatus=4)) and ( @accountStatus=2))--If the current status is pending or disabled and account needs to be approved
		  --Begin 
				 update core_Users set accessStatus=@accountStatus ,expirationDate=@expirationDate, comment =@comment where userID=@userID
			--End 
			--else
			--Begin 
				--update core_Users set accessStatus=@accountStatus,comment =@comment where userID=@userID
		--	End 
		End  

   
END
GO

