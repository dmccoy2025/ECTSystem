
--EXEC cmdStruct_sp_UpdateAffected
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/23/2015
-- Description:		Changed the unit count check from 20 to 200. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/2/2015
-- Description:		Changed the unit count check from 200 to 2500. 
-- ============================================================================
CREATE PROCEDURE [dbo].[cmdStruct_sp_UpdateAffected]
	@cs_id int,
	@userId int
AS

set nocount on;

BEGIN  

    DECLARE @allUnits Table
      (
        cs_id int
      ) 
 
    DECLARE @long_name varchar(200)
    DECLARE @userRoleAndName varchar(200)
    
    SELECT @userRoleAndName = ( SELECT  Role + '--' + FirstName + '  ' +LastName   from vw_Users where userId=@userId)
 
    SET  @long_name=(SELECT LONG_NAME FROM Command_Struct WHERE CS_ID =@cs_id)
 
    DECLARE @unitCount int 

    
    INSERT INTO @allUnits (cs_id )  SELECT child_id  from Command_Struct_Tree where parent_id =@cs_id  
    INSERT INTO @allUnits (cs_id )  SELECT parent_id from Command_Struct_Tree where child_id  =@cs_id  
    
    SET @unitCount=(SELECT count(DISTINCT cs_id) FROM @allUnits )    
    
   
    IF  ( @unitCount ) > 2500
     BEGIN    
      INSERT INTO 
		core_Messages
			(message,title,name,createdBy,popup,startTime,endTime)
		Values
			('Deferred command chain update','Command Chain Update',@userRoleAndName,1,1, DATEADD(hour,1, getDate()), DATEADD(hour,1, getDate()) )
 
         SELECT  0
     END 
     ELSE
     BEGIN 
             DECLARE @unitId   int 
  	         DECLARE  unit_cursor CURSOR FOR   SELECT DISTINCT cs_id FROM @allUnits  
    
              OPEN unit_cursor
        	
        	        FETCH NEXT FROM unit_cursor  INTO @unitId
		 	        WHILE @@FETCH_STATUS = 0
    		        BEGIN --BEGIN   CURSOR 
                        EXECUTE cmdStruct_sp_RebuildAllTrees_Single @unitId
       		  	        FETCH NEXT FROM unit_cursor  INTO @unitId
           	        END --END   CURSOR  
            
              CLOSE unit_cursor
	          DEALLOCATE  unit_cursor	
	                  	  
        	  INSERT INTO 
		      core_Messages
			      (message,title,name,createdBy,popup,startTime,endTime)
		      Values
			      ('Command  tree updated for  ' + @long_name + '. Number of Units affected was ' + cast(@unitCount as varchar(100)),  'Command Chain Update',@userRoleAndName,1,1,DATEADD(hour,1, getDate()),DATEADD(hour,1, getDate()))
            
             SELECT   @unitCount
	           
	   END 
	  
			
 END--End Stored Proc
GO

