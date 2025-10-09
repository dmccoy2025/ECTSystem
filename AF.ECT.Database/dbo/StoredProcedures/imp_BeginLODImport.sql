--Execute imp_BeginALL
CREATE PROCEDURE [dbo].[imp_BeginLODImport]
 
AS
BEGIN 
	--DELETE FROM FORM348_UNIT
	--DELETE FROM FORM348_MEDICAL
	--DELETE FROM imp_LODMAPPING
	DELETE FROM  FORM348_findings 
	DELETE FROM FORM261
    DELETE FROM RWOA
	DELETE FROM core_WorkStatus_Tracking
	
	--DELETE FROM FORM348 

	
	
		 
	-----Steps to create mapping table 
	--print ('Starting lod mapping   at :' +cast(getUTCDate() as varchar(50)))
	--EXECUTE imp_CreateLODMapping --30 minutes 
	--print ('updating caseIds :' +cast(getUTCDate() as varchar(50)))
	--EXECUTE imp_updateCaseIds --2 minutes 
	----The following opens up LOD Cursor and imports all records
	--print ('Start LOD import :' +cast(getUTCDate() as varchar(50)))

	-- EXECUTE imp_ImportLods
	 --9 minutes
	---This will create all lod records and update the lod mapping table 
	
    DECLARE @lod_id int 
 	--------LodCursor------- 
	DECLARE lod_cursor CURSOR FOR  SELECT cast(lod_id as int) FROM imp_LOD_DISPOSITIONS --WHERE LOD_ID=6633
	
	OPEN lod_cursor
	FETCH NEXT FROM lod_cursor  INTO @lod_id 
	print ('LOD import :' +cast(@lod_id as varchar(50))+'|'+  cast(getUTCDate() as varchar(50)))

    WHILE @@FETCH_STATUS = 0
    BEGIN
     
		 EXECUTE imp_Importform261 @lod_id --9 minutes
		 EXECUTE imp_OtherPersonals @lod_id --9 minutes
		 EXECUTE imp_RWOA @lod_id --31 SECONDS
		 EXECUTE imp_ImportSigDates  @lod_id --25 minutes
	 	 EXECUTE imp_UpdatePersonTypesFindings @lod_id --4 minutes
				
	    FETCH NEXT FROM lod_cursor  INTO @lod_id 
	END ---End of LOD_CURSOR      
     
     
     CLOSE lod_cursor
	 DEALLOCATE  lod_cursor
	 print ('Start LOD tracking :' + cast(getUTCDate() as varchar(50)))

	 
	 
	 SET @lod_id=NULL 
	 --DELETE RECORDS INSERTED BECAUSE OF TRIGGERS FROM CORE STATUS TRACKING 
	 DELETE FROM  core_WorkStatus_Tracking  
	
	DECLARE trck_cursor CURSOR FOR  SELECT cast(lod_id as int) FROM imp_LOD_DISPOSITIONS --WHERE LOD_ID=6633
	
	OPEN trck_cursor
	FETCH NEXT FROM trck_cursor  INTO @lod_id 
	
    WHILE @@FETCH_STATUS = 0
    BEGIN
		  EXECUTE imp_updateCoreStatusTracking @lod_id --25  MINUTES
		  FETCH NEXT FROM trck_cursor  INTO @lod_id 
	END ---End of LOD_CURSOR      
     
     
     CLOSE trck_cursor
	 DEALLOCATE  trck_cursor

  --  EXECUTE imp_ReplaceTilda
		
END
GO

