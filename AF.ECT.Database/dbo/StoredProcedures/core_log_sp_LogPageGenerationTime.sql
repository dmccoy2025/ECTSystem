
-- =============================================
-- Author:		Darel Johnson
-- Create date: 9/6/2019
-- Description:	Insert record into generation time Log
-- =============================================
CREATE  PROCEDURE [dbo].[core_log_sp_LogPageGenerationTime]
     @action_date smalldatetime,
     @measuredTime  varchar(255),
	 @currentPage varchar(255),
	 @referringPage varchar(255),
	 @username varchar(200),
	 @role varchar(200)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @gId INT
	INSERT INTO
		[dbo].core_LogPageGenerationTime 
		(action_date, measuredTime, currentPage, referringPage, username, role)
	VALUES
		(@action_date, @measuredTime, @currentPage, @referringPage, @username, @role)

	SET @gId = SCOPE_IDENTITY() 
	SELECT @gId

END
GO

