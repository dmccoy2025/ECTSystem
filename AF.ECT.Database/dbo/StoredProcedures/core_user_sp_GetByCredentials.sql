--EXEC core_user_sp_GetByCredentials 'Kerrie','Duncan','DUNKERRIE','010000095'
CREATE PROCEDURE [dbo].[core_user_sp_GetByCredentials] 
	 @first varchar(50)
	,@last varchar(50)
	,@username varchar(50)
	,@ssn varchar(50)

AS
BEGIN 

	DECLARE @userId int , @extssn varchar(9) 
	SELECT @userId =  USERID, @extssn=SSN FROM core_Users WHERE username=@username AND FirstName=@first AND LastName=@last 
	IF( @userId is not null)
		BEGIN 
		--SSN does not exist and is a non unit personnel so there is a match			 
				IF(@extssn is null or @extssn='')
					BEGIN
						UPDATE core_Users 
							SET SSN=@ssn 
							WHERE userID=@userId AND (SSN is null or SSN='')
					END
				ELSE
				 BEGIN
				 	--User name exist but with a different ssn so   unsuccessful match
					 SET @userId=0 
				 END  
		END
	ELSE  
		BEGIN 
		 	--Unsuccessful match
				
			SET @userId=0 
		END
		
		 
		SELECT @userId
	
	
  
END
GO

