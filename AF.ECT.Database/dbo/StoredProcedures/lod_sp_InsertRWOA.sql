

CREATE PROCEDURE [dbo].[lod_sp_InsertRWOA]
	 @refId int
	,@wfId   tinyInt
	,@statusIn    tinyInt
	,@statusOut    tinyInt 
	,@sentTo varchar(100)
	,@userId int
	

AS

SET NOCOUNT ON

	 DECLARE @rwoa_reason tinyInt
		 ,@rwoa_explantion varchar(4000)
		 ,@medtech_comments varchar(4000) 
		 ,@rwoa_date datetime
	 
		
		SELECT
			 @rwoa_reason=rwoa_reason,@rwoa_explantion=rwoa_explantion
			,@medtech_comments=med_tech_comments,@rwoa_date=rwoa_date
			 FROM Form348 WHERE lodid = @refId AND status = @statusOut 

			IF @rwoa_explantion IS NULL OR  @rwoa_explantion='' 
				BEGIN
					SET @rwoa_explantion=null 
				End
			
			IF @medtech_comments IS NULL OR  @medtech_comments='' 
				BEGIN
					SET @medtech_comments=null 
				End

			IF @rwoa_date IS NULL  
				BEGIN
					SET @rwoa_date=getUTCDate() 
				End
 
		INSERT INTO 
			Rwoa (refId,workstatus,workflow,sent_to,reason_sent_back
				  ,explanation_for_sending_back,sender,date_sent
				  ,comments_back_to_sender,date_sent_back
				  ,created_by,created_date )
		VALUES
				  (@refId,@statusIn,@wfId,@sentTo,@rwoa_reason
				   ,@rwoa_explantion, 'RLB',@rwoa_date
				   ,@medtech_comments,getUTCDate(),@userId,getUTCDate())
 
SELECT @@ROWCOUNT
GO

