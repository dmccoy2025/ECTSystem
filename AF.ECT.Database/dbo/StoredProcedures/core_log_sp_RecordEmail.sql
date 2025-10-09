-- =============================================
-- Author:		Nandita Srivastava
-- Create date: 4/10/2008
-- Description:	Insert record into emil Log
-- =============================================
CREATE  PROCEDURE [dbo].[core_log_sp_RecordEmail]
	@userId as int,
	@eTo varchar(100),	 
	@eCC varchar(100),
	@eBCC varchar(max),
 	@subject varchar(200),
 	@body varchar(max)  ,
	 @failed varchar(max),
	 @templateId int

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @eId INT
	INSERT INTO
		core_logEmail 
		(user_id ,date_sent, e_to  ,e_cc ,e_bcc ,subject , body,failed,templateId )
	VALUES
		(@userId,getdate(),@eTo,@eCC,@eBCC, @subject,@body,@failed, @templateId )

	SET @eId = SCOPE_IDENTITY() 
	SELECT @eId

END
GO

