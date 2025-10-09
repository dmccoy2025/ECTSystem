CREATE PROCEDURE [dbo].[imp_InsertErrorRecord] 
(
	-- Add the parameters for the function here
	  
	   
	  @err int 
	  ,@updateType varchar(200) 
	  ,@storedproc varchar(200)
	  ,@msg varchar(200) 
	   ,@lod_id int 
	   ,@errMsg  varchar(4000)
)
 
AS
BEGIN
	DECLARE @message as varchar(300)
  		    	  
  		    	   IF @err <> 0 
					   BEGIN 
						SET @message= 'Error:'+ cast(@err as varchar(10) ) + '|'+ @msg 
	 		 		   END 
				   ELSE
				   BEGIN 
						SET @message= 'UnknownError'+ '|'+ @msg 
				    END 
				   
				   INSERT INTO import_Error_LOG				   
					 (UpdatingData ,StoredProc,Message, RCPHALODID,Time,ErrorMessage)
				   VALUES 
					(@updateType ,@storedproc, @message,@lod_id,GETUTCDATE(),@errMsg)
				   
					
	 
  
END
GO

