CREATE PROCEDURE [dbo].[imp_RWOA]
( @lod_id int) 
 
AS

BEGIN 
-----DECLARE-----------------------
 
 DECLARE @new_lod_id   int 
set @new_lod_id=(select alod_lod_id from imp_LODMAPPING  where rcpha_lodid=@lod_id)
 
 IF(  @new_lod_id is not null)
 BEGIN 	
	    BEGIN TRY --BEGIN TRY BLOCK 
	  
 
			------------RWOA -------------	
	 		  INSERT INTO RWOA 
			(
				 refId
				,workstatus
				,workflow
				,sent_to
				,reason_sent_back
				,explanation_for_sending_back
				,sender
				,date_sent
				,comments_back_to_sender
				,date_sent_back			
				,created_by
				,created_date
			
			)
			
			SELECT 
			@new_lod_id
			,1 --*******This needs to be set as per the status of lod at that time  **********/
			,1 --*******Workflow is 1 for LOD
			,SENT_TO
			,(SELECT ID FROM core_lkupRWOAReasons WHERE TYPE=REASON_SENT_BACK )
			,EXPLANATION_FOR_SENDING_BACK
			,SENDER
			,CASE WHEN DATE_SENT IS NOT NULL AND DATE_SENT <>'' THEN CAST(DATE_SENT  as datetime)
			 ELSE NULL
			 END 			
			,COMMENTS_BACK_TO_SENDER
			,CASE WHEN DATE_SENT_BACK IS NOT NULL AND DATE_SENT_BACK <>'' THEN CAST(DATE_SENT_BACK  as datetime)
			 ELSE NULL
			 END 
			,(SELECT USERID FROM CORE_USERS WHERE USERNAME=CREATED_BY)
		    ,CAST(CREATED_DATE as datetime)
		     FROM IMP_LOD_RWOA WHERE CAST(LOD_ID AS INT)=@lod_id  
		    ---End   RWOA  -----------	
		     
			END TRY 	--END TRY BLOCK 
			
			BEGIN CATCH --BEGIN CATCH BLOCK 
		 	 			DECLARE  @msg varchar(2000)
						 DECLARE @number int ,@errmsg varchar(2000)
							SELECT 
							  @number= ERROR_NUMBER()  
							 ,@errmsg= ERROR_MESSAGE()
				 		EXECUTE imp_InsertErrorRecord @number 
						,'RWOA','imp_RWOA','RWOA records error',@lod_id,@errmsg
		 				    
		  END CATCH---END CATCH BLOCK
		     
	 
	END
	ELSE  
BEGIN 
  						 
				 			EXECUTE imp_InsertErrorRecord 0 
							,'RWOA','imp_RWOA','RWOA records error',@lod_id,'MAPPINGERROR'
		
 
 END 
END
GO

