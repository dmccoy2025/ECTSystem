--EXECUTE imp_CreateLODMapping
--EXECUTE imp_update_LODStatus
--EXECUTE imp_updateCaseIds
--SELECT * FROM imp_LODMAPPING
--SELECT COUNT(*) FROM imp_LODMAPPING
--select  COUNT(*)  FROM imp_lod_dispositions
CREATE PROCEDURE [dbo].[imp_CreateLODMapping]
 
AS

BEGIN
DECLARE    @lod_id    INT 
DECLARE    @orig_lod_id    INT 
DECLARE    @created_date    DATETIME 

 
DECLARE lod_cursor CURSOR FOR  
	SELECT lod_id, LOD_ID_ORIGINAL, CREATED_DATE FROM imp_lod_dispositions
  
DELETE FROM imp_LODMAPPING 
OPEN lod_cursor

FETCH NEXT FROM lod_cursor INTO @lod_id ,@orig_lod_id,@created_date
    	
WHILE @@FETCH_STATUS = 0
    	 
BEGIN
	BEGIN TRY 
    
		IF (@orig_lod_id=0)
			SET @orig_lod_id=null
					
		INSERT INTO imp_LODMAPPING(rcpha_lodid ,rc_pha_org_lodid ,createdDate)   
		values (@lod_id,@orig_lod_id,@created_date) 
		
		--Update Status
		EXECUTE imp_update_LODStatus @lod_id
		
	END TRY 	

	BEGIN CATCH
		EXECUTE imp_InsertErrorRecord @@error 
		,'CREATE LODMAPPING TABLE','imp_CreateLODMapping ','error creating mapping record ',@lod_id,ERROR_MESSAGE	
	END CATCH
		
	FETCH NEXT FROM lod_cursor  INTO @lod_id ,@orig_lod_id,@created_date
		 	
END   --END LOD_CURSOR   
       
CLOSE lod_cursor
DEALLOCATE  lod_cursor     
    
END
GO

