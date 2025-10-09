CREATE PROCEDURE [dbo].[core_user_sp_GetUserName] 
	@first varchar(50)
	,@last varchar(50)

AS

DECLARE @username varchar(100)
SET @username = @last;

IF (len(@first) >= 1)
BEGIN
	SET @username = @username + LEFT(@first, 1);
END

DECLARE @count int, @len int
SET @count = 1

IF EXISTS (SELECT * FROM core_users WHERE username LIKE @username)
BEGIN 
	DECLARE @checkName varchar(100)
	SET @checkName = @userName

	WHILE (@count < 10)
	BEGIN
		IF EXISTS (SELECT * FROM core_users WHERE username LIKE @username)
		BEGIN
			SET @username = @checkName + cast(@count AS varchar(10))
			SET @count = @count + 1;
		END
		ELSE
		BEGIN 
			SET @count = 10;
		END
	END
END 

SELECT upper(@username)
GO

