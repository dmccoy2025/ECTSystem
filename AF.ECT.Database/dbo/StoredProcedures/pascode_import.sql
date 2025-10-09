
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Imports unit information.  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/23/2015	
-- Description:		Modified the third run portion of the script to reset
--					the local variables to default values during each iteration
--					of the cursor. This was to fix a bug which could cause a 
--					unit to be updated when it wasn't suppose to because the 
--					ID of the unit was still be assigned to the local
--					variable when it should not have been.
-- ============================================================================
CREATE PROCEDURE [dbo].[pascode_import]
   @logId  as int
AS
 
 
DECLARE @insertedMemberRows as int 
DECLARE @rawRowsInserted as int
DECLARE @modifiedRows as int 
DECLARE @parentModified as bit

    DECLARE @addedPascodes Table
	(
		pas_code varchar(10), 
	    parent_pas_code varchar(10),
	    parentModified bit 
	)  
    
    DECLARE @sysAdmin as varchar(100) ,@sysAdminId int
		   
    SET @sysAdmin=ISNULL((select top 1 username  from vw_users where groupId=1 and accessStatus=3),'SysAdmin')
    SET @sysAdminId=ISNULL ((select top 1 userId  from vw_users where groupId=1 and accessStatus=3),1)
 
    DECLARE @new_PAS_CODE VARCHAR(4)
    
    DECLARE @new_cs_id  int ,@new_parent_cs_id int 
    DECLARE @cs_cs_id int 
    
    DECLARE  @ADDRESS1 VARCHAR(100),@ADDRESS2 VARCHAR(100),@BASE_CODE VARCHAR(2)
             ,@CITY  VARCHAR(30),@COMMAND_CODE  VARCHAR(2),@COMMAND_STRUCT_UTC  VARCHAR(5)
            ,@COMPONENT VARCHAR(4),@COUNTRY VARCHAR(40),@CREATED_BY  VARCHAR(30)
			,@CREATED_DATE DATETIME ,@CS_LEVEL  VARCHAR(10)
			,@CS_OPER_TYPE  VARCHAR(2),@GEO_LOC  VARCHAR(4)
			,@LONG_NAME   VARCHAR(100),@PAS_CODE  VARCHAR(4)
			,@POSTAL_CODE  VARCHAR(10),@STATE  VARCHAR(2)
			,@UIC  VARCHAR(6),@UNIT_DET  VARCHAR(4)
			,@UNIT_KIND   VARCHAR(5),@UNIT_NBR  VARCHAR(4)
			,@UNIT_TYPE   VARCHAR(2),
			@PARENT_PAS_CODE VARCHAR(4) 
			
   DECLARE  @cs_ADDRESS1 VARCHAR(100),@cs_ADDRESS2 VARCHAR(100),@cs_BASE_CODE VARCHAR(2)
             ,@cs_CITY  VARCHAR(30),@cs_COMMAND_CODE  VARCHAR(2),@cs_COMMAND_STRUCT_UTC  VARCHAR(5)
            ,@cs_COMPONENT VARCHAR(4),@cs_COUNTRY VARCHAR(40),@cs_CREATED_BY  VARCHAR(30)
			,@cs_CREATED_DATE DATETIME ,@cs_CS_LEVEL  VARCHAR(10)
			,@cs_CS_OPER_TYPE  VARCHAR(2),@cs_GEO_LOC  VARCHAR(4)
			,@cs_LONG_NAME   VARCHAR(100),@cs_PAS_CODE  VARCHAR(4)
			,@cs_POSTAL_CODE  VARCHAR(10),@cs_STATE  VARCHAR(2)
			,@cs_UIC  VARCHAR(6),@cs_UNIT_DET  VARCHAR(4)
			,@cs_UNIT_KIND   VARCHAR(5),@cs_UNIT_NBR  VARCHAR(4)
			,@cs_UNIT_TYPE   VARCHAR(2)
			,@cs_PARENT_PASCODE VARCHAR(4)  
			,@cs_ID_PARENT int 
			,@cs_userModified bit
			,@new_parent_csc_id int 
			
			
						
--------Initialize members------- 
SET @rawRowsInserted=(SELECT COUNT(*) FROM  ALOD_PAS_RAW)    
SET @insertedMemberRows =0
SET @modifiedRows =0

----------------------------First Run to create all non existing units-----------------------
--------command cursor-------  
DECLARE @rowErrorNumber int = 666, @rowErrorMessage nvarchar(300)
DECLARE command_cursor CURSOR FOR   SELECT LTRIM(RTRIM(ANU)) FROM  ALOD_PAS_RAW   

OPEN command_cursor

FETCH NEXT FROM command_cursor  INTO  @new_PAS_CODE 
                                      
WHILE @@FETCH_STATUS = 0 
BEGIN ---BEGIN (1) CURSOR
	BEGIN TRY -- (2) 
		
	IF ISNULL(@new_PAS_CODE, '') <> ''  -- only import non-null pas codes
		BEGIN  -- BEGIN (3) NON NULL PAS CODE
		SELECT 	
			@ADDRESS1=  RTRIM(LTRIM(ABA7_1))--address1
			,@ADDRESS2=  RTRIM(LTRIM(ABA7_2))--address2
			,@BASE_CODE=CAP2--base code
			,@CITY=ADY7	--city
			,@COMMAND_CODE=AKU19--command_code
			,@COMMAND_STRUCT_UTC=CRZ--unit type code
			,@COMPONENT='AFRC'--component
			,@COUNTRY=CASE  --AEW IS THE COUNTRY /STAE ABBREVIATION
						WHEN LEN(IsNull(AEW, '')) > 2 THEN    AEW 
						WHEN IsNull(AEW, '') = '' THEN NULL  -- added 01/10/2012
						ELSE 'USA'  --state 'USA'--country
					 END  	   
			 ,@CREATED_BY=@sysAdmin--sysadmin (created by)		
			 ,@CREATED_DATE=GETUTCDATE()
			 ,@CS_LEVEL='U' --cs -level  U-Unknown
			 ,@CS_OPER_TYPE='U'--cs oper type  U-Unknown
			 ,@GEO_LOC=AJH8 --geo loc
			 ,@PAS_CODE=ANU--pascode
			 ,@POSTAL_CODE= RTRIM(LTRIM(AMF303)) + RTRIM(LTRIM(ABC2))  --postal code
			 ,@STATE= CASE WHEN LEN(AEW)= 2  THEN AEW --AEW IS THE COUNTRY /STAE ABBREVIATION
						 ELSE NULL 
					   END 
			 ,@UIC=UIC--uic
			 ,@UNIT_DET=AFP1--det number
			 ,@UNIT_KIND=AMX5--unit kind
			 ,@UNIT_NBR=ANA --unit nbr
			 ,@UNIT_TYPE=ANC --unit type 
			 ,@PARENT_PAS_CODE=ANU30 
			 ,@LONG_NAME=IsNull(RTRIM(LTRIM(ANA)),' ') + ' ' + IsNull(RTRIM(LTRIM(AMY)),' ') + ' ' + IsNull(RTRIM(LTRIM(ANC)),' ')
--			 ,@LONG_NAME=IsNull(RTRIM(LTRIM(UNIT)),' ') 
--			 ,@LONG_NAME=IsNull(RTRIM(LTRIM(AMY)),' ')+ '  '+ IsNull(RTRIM(LTRIM(@UNIT_TYPE)),' ') + ' Det ' +  IsNull(RTRIM(LTRIM(@UNIT_DET)),' ') --long_name
		FROM ALOD_PAS_RAW WHERE ANU=@new_PAS_CODE

		IF((SELECT  PAS_CODE  FROM    Command_Struct where  LTRIM(RTRIM(PAS_CODE)) =@new_PAS_CODE ) is null)
			BEGIN -- BEGIN (4) INSERT

			INSERT INTO @addedPascodes (pas_code,parent_pas_code,parentModified) Values (@new_PAS_CODE,@PARENT_PAS_CODE,0)

			SET @insertedMemberRows=@insertedMemberRows+1
			--Insert into command struct 		
			INSERT INTO Command_Struct
			(
				ADDRESS1,ADDRESS2,BASE_CODE,CITY,COMMAND_CODE,COMMAND_STRUCT_UTC,COMPONENT
				,COUNTRY,CREATED_BY,CREATED_DATE,CS_LEVEL ,CS_OPER_TYPE ,GEO_LOC,LONG_NAME 
				,PAS_CODE,POSTAL_CODE,STATE,UIC,UNIT_DET,UNIT_KIND,UNIT_NBR,UNIT_TYPE 
			)
			VALUES 
			(
				@ADDRESS1,@ADDRESS2 ,@BASE_CODE,@CITY,@COMMAND_CODE,@COMMAND_STRUCT_UTC  
				,@COMPONENT,@COUNTRY,@CREATED_BY,@CREATED_DATE,@CS_LEVEL ,@CS_OPER_TYPE
				,@GEO_LOC,@LONG_NAME,@PAS_CODE,@POSTAL_CODE,@STATE,@UIC,@UNIT_DET  
				,@UNIT_KIND,@UNIT_NBR,@UNIT_TYPE    
			) 

			SET @new_cs_id=(SELECT CS_ID FROM Command_Struct  WHERE PAS_CODE =@PAS_CODE)
 	 	 
			INSERT INTO Command_Struct_history 
			(
				pkgLog_Id ,pascode,cs_id,ADDRESS1,ADDRESS2,BASE_CODE,CITY,COMMAND_CODE,COMMAND_STRUCT_UTC
				,COMPONENT ,COUNTRY,created_by,CREATED_DATE,CS_LEVEL ,CS_OPER_TYPE ,GEO_LOC,LONG_NAME 
				,POSTAL_CODE,STATE,UIC,UNIT_DET,UNIT_KIND,UNIT_NBR,UNIT_TYPE ,PARENT_PAS_CODE,ChangeType 
			)
			VALUES 
			(
				@logId,@PAS_CODE,@new_cs_id,@ADDRESS1,@ADDRESS2 ,@BASE_CODE,@CITY,@COMMAND_CODE,@COMMAND_STRUCT_UTC  
				,@COMPONENT,@COUNTRY,@sysAdminId,@CREATED_DATE,@CS_LEVEL ,@CS_OPER_TYPE
				,@GEO_LOC,@LONG_NAME,@POSTAL_CODE,@STATE,@UIC,@UNIT_DET  
				,@UNIT_KIND,@UNIT_NBR,@UNIT_TYPE,@PARENT_PAS_CODE,'A'
			)     

			END  -- (4) INSERT
  
		ELSE IF @new_PAS_CODE   IN (SELECT  PAS_CODE  FROM    Command_Struct )
			BEGIN  -- BEGIN (4) UPDATE

			SELECT 	
				@cs_cs_id=Command_Struct.CS_ID 
				,@cs_ADDRESS1=  Command_Struct.ADDRESS1--address1
				,@cs_ADDRESS2=   Command_Struct.ADDRESS2--address2
				,@cs_BASE_CODE= Command_Struct.BASE_CODE--base code
				,@cs_CITY= Command_Struct.CITY	--city
				,@cs_COMMAND_CODE= Command_Struct.COMMAND_CODE--command_code
				,@cs_COMMAND_STRUCT_UTC= Command_Struct.COMMAND_STRUCT_UTC--unit type code
				,@cs_COMPONENT= Command_Struct.COMPONENT--component
				,@cs_COUNTRY= Command_Struct.COUNTRY
				,@cs_CS_LEVEL= Command_Struct.CS_LEVEL --cs -level  U-Unknown
				,@cs_CS_OPER_TYPE= Command_Struct.CS_OPER_TYPE--cs oper type  U-Unknown
				,@cs_GEO_LOC= Command_Struct.GEO_LOC--geo loc
				,@cs_LONG_NAME= Command_Struct.LONG_NAME 
				,@cs_POSTAL_CODE=  Command_Struct.POSTAL_CODE --postal code
				,@cs_STATE=  Command_Struct.STATE
				,@cs_UIC= Command_Struct.UIC--uic
				,@cs_UNIT_DET= Command_Struct.UNIT_DET--det number
				,@cs_UNIT_KIND= Command_Struct.UNIT_KIND--unit kind
				,@cs_UNIT_NBR= Command_Struct.UNIT_NBR --unit nbr
				,@cs_UNIT_TYPE= Command_Struct.UNIT_TYPE --unit type 
				,@cs_ID_PARENT = Command_Struct.CS_ID_PARENT --Parent Id
				,@cs_userModified=Command_Struct.UserModified
				,@cs_CREATED_DATE = isnull(Command_Struct.CREATED_DATE, GETUTCDATE())
			FROM  Command_Struct 
			WHERE Command_Struct.PAS_CODE=@new_PAS_CODE

			SET @cs_PARENT_PASCODE =(SELECT PAS_CODE FROM Command_Struct WHERE CS_ID =@cs_ID_PARENT) 

			--Following line will check if the existing unit is a pseudo if the condition is true that will mean its a pseudo unit .If not then any modification will be applied
			IF NOT(@cs_PARENT_PASCODE IN ('!!MA','!!15','!!05','!!13','!!06','!!14') AND SUBSTRING(@new_PAS_CODE,1,1)  IN ('g','m','o','j' ,'c' ,'i')) 
				BEGIN -- (5) if psuedo pass code exists
				IF 
				   (
					 ISNULL(@cs_ADDRESS1 ,'')<> ISNULL(@ADDRESS1,'') 
					 Or ISNULL(@cs_ADDRESS2 ,'')<> ISNULL(@ADDRESS2,'') 
					 Or ISNULL(@cs_BASE_CODE ,'')<> ISNULL(@BASE_CODE,'') 
					 Or ISNULL(@cs_CITY ,'')<> ISNULL(@CITY,'') 
					 Or ISNULL(@cs_COMMAND_CODE ,'')<> ISNULL(@COMMAND_CODE,'') 
					 Or ISNULL(@cs_COMMAND_STRUCT_UTC ,'')<> ISNULL(@COMMAND_STRUCT_UTC,'') 
					 Or ISNULL(@cs_COUNTRY ,'')<>  ISNULL(@COUNTRY,'') 
					 Or ISNULL(@cs_GEO_LOC ,'')<> ISNULL(@GEO_LOC,'') 
					 Or ISNULL(@cs_LONG_NAME ,' ')<> ISNULL(@LONG_NAME,' ') 
					 Or ISNULL(@cs_POSTAL_CODE ,'')<> ISNULL(@POSTAL_CODE,'') 
					 Or ISNULL(@cs_STATE ,'')<>  ISNULL(@STATE,'') 
					 Or ISNULL(@cs_UIC ,'')<> ISNULL(@UIC,'') 
					 Or ISNULL(@cs_UNIT_DET ,'')<> ISNULL(@UNIT_DET,'') 
					 Or ISNULL(@cs_UNIT_KIND ,'')<> ISNULL(@UNIT_KIND,'') 
					 Or ISNULL(@cs_UNIT_NBR ,'')<> ISNULL(@UNIT_NBR,'') 
					 Or ISNULL(@cs_UNIT_TYPE ,'')<> ISNULL(@UNIT_TYPE,'')
					 OR ISNULL(@cs_PARENT_PASCODE,'') <> ISNULL(@PARENT_PAS_CODE, '')
				   )
   
					BEGIN  -- (6) if any changes found
		
					IF (ISNULL(@cs_PARENT_PASCODE,'') <> ISNULL(@PARENT_PAS_CODE, '') )
						BEGIN -- (7)
							--notice any parent pascode changes
							INSERT INTO @addedPascodes (pas_code,parent_pas_code,parentModified) Values (@new_PAS_CODE,@PARENT_PAS_CODE,1 ) 
						END -- (7)
			 
					SET @modifiedRows=@modifiedRows+1 
			
					UPDATE COMMAND_STRUCT SET 
						 ADDRESS1=  @ADDRESS1--address1
						,ADDRESS2= @ADDRESS2--address2
						,BASE_CODE=@BASE_CODE--base code
						,CITY=@CITY	--city
						,COMMAND_CODE=@COMMAND_CODE--command_code
						,COMMAND_STRUCT_UTC=@COMMAND_STRUCT_UTC--unit type code
						,COMPONENT=@COMPONENT--component
						,COUNTRY=@COUNTRY 
						,MODIFIED_BY=@sysAdmin--sysadmin (created by)		
						,MODIFIED_DATE=GETUTCDATE()
						,CS_LEVEL=@CS_LEVEL
						,CS_OPER_TYPE=@CS_OPER_TYPE 
						,GEO_LOC=@GEO_LOC
						,LONG_NAME=CASE 
							WHEN ISNULL(@LONG_NAME, ' ') = ' ' THEN LONG_NAME
							ELSE @LONG_NAME  -- only change if new unit name is not null
							END
						,POSTAL_CODE= @POSTAL_CODE
						,STATE= @STATE
						,UIC=@UIC--uic
						,UNIT_DET=@UNIT_DET--det number
						,UNIT_KIND=@UNIT_KIND--unit kind
						,UNIT_NBR=@UNIT_NBR --unit nbr
						,UNIT_TYPE=@UNIT_TYPE --unit type 
					WHERE PAS_CODE=@new_PAS_CODE 
						AND 
						(
							UserModified <> 1  ---- only update records that have not been modified by the appplication
							OR 
							(
								IsNull(LONG_NAME, '') = ''
								AND
								ISNULL(@LONG_NAME, '') <> ''
							)
						)
	        
	        
					---Add a commmand history record 
					INSERT INTO Command_Struct_history 
					(
						pkgLog_Id ,pascode,cs_id,ADDRESS1,ADDRESS2,BASE_CODE,CITY,COMMAND_CODE,COMMAND_STRUCT_UTC,COMPONENT
						,COUNTRY,CREATED_BY,CREATED_DATE,CS_LEVEL ,CS_OPER_TYPE ,GEO_LOC,LONG_NAME 
						,POSTAL_CODE,STATE,UIC,UNIT_DET,UNIT_KIND,UNIT_NBR,UNIT_TYPE ,PARENT_PAS_CODE,ChangeType
					)
					VALUES 
					(
						 @logId,@PAS_CODE,@cs_cs_id,@cs_ADDRESS1,@cs_ADDRESS2 ,@cs_BASE_CODE,@cs_CITY,@cs_COMMAND_CODE,@cs_COMMAND_STRUCT_UTC  
						,@cs_COMPONENT,@cs_COUNTRY,@sysAdminId,@cs_CREATED_DATE,@cs_CS_LEVEL ,@cs_CS_OPER_TYPE
						,@cs_GEO_LOC,@cs_LONG_NAME,@cs_POSTAL_CODE,@cs_STATE,@cs_UIC,@cs_UNIT_DET  
						,@cs_UNIT_KIND,@cs_UNIT_NBR,@cs_UNIT_TYPE,@cs_PARENT_PASCODE,'M'
					) 
	     
					END -- (6) End if any changes found

				END  -- (5)End if psuedo pas code exists		

			END    --End (4) Update

			-- REQUIRED FIELDS (@LONG_NAME, @UNIT_NBR, @COMPO,) CHECK
			-- Import still succeeds, we just track the error
		IF (
			IsNull(@LONG_NAME, ' ') = ' ' OR 
			ISNULL(@COMPONENT, ' ') = ' ' OR
			ISNULL(@UNIT_NBR, ' ') = ' '
			)
			BEGIN  -- (4)
				Set @rowErrorMessage = 'Required Field Missing: '
				If ISNULL(@Long_Name, ' ') = ' '
					BEGIN -- (5) 
						Set @rowErrorMessage = @rowErrorMessage + ' LONG_NAME;'
					END -- (5) 
				If ISNULL(@COMPONENT, ' ') = ' '
					BEGIN -- (5) 
						Set @rowErrorMessage = @rowErrorMessage + ' COMPONENT;'
					END -- (5) 
				If ISNULL(@UNIT_NBR, ' ') = ' '
					BEGIN -- (5) 
						Set @rowErrorMessage = @rowErrorMessage + ' UNIT_NBR;'
					END -- (5) 

			INSERT INTO  	core_pkgImport_Errors			   
			(storedProcName ,keyValue, pkgLog_Id,  Time,ErrorMessage)
			VALUES 
			('pascode_import (command_struct_insert)' ,@new_PAS_CODE,@logId,GETUTCDATE(),@rowErrorMessage)			
			
			END -- (4) 

		END -- (3) NON NULL PAS CODE
	ELSE
		BEGIN -- (3) 
			Set @rowErrorMessage = 'PAS CODE Is Null: '
			INSERT INTO  	core_pkgImport_Errors			   
			(storedProcName ,keyValue, pkgLog_Id,  Time,ErrorMessage)
			VALUES 
			('pascode_import (command_struct_insert)' ,@new_PAS_CODE,@logId,GETUTCDATE(),@rowErrorMessage)	
		END -- (3) 
        
	END TRY 	-- END TRY BLOCK 
		
	BEGIN CATCH --BEGIN CATCH BLOCK 
		 
		DECLARE @number int ,@errmsg varchar(2000)
		SELECT 	  @number= ERROR_NUMBER(),@errmsg= ERROR_MESSAGE() 
		DECLARE @message as varchar(300)
  		    	  
		IF @number <> 0 
			BEGIN 
				SET @message= 'Error:'+ cast(@number as varchar(10) ) + '|'+ @errmsg 
			END 
		ELSE
			BEGIN 
				SET @message= 'UnknownError'+ '|'+ @errmsg 
			END  	
		   
		INSERT INTO  	core_pkgImport_Errors			   
		(storedProcName ,keyValue,pkgLog_Id,  Time,ErrorMessage)
		VALUES 
		('pascode_import (command_struct_insert)' ,@new_PAS_CODE,@logId,GETUTCDATE(),@message)			
				 				    
	END CATCH---END CATCH BLOCK


	SET @ADDRESS1=  NULL 
	SET @ADDRESS2=  NULL
	SET @BASE_CODE=NULL
	SET @CITY=NULL
	SET @COMMAND_CODE=NULL
	SET @COMMAND_STRUCT_UTC=NULL
	SET @COMPONENT=NULL
	SET @COUNTRY=NULL	   
	SET @CREATED_BY=NULL 	
	SET @CREATED_DATE=NULL
	SET @CS_LEVEL=NULL
	SET @CS_OPER_TYPE=NULL
	SET @GEO_LOC=NULL
	SET @LONG_NAME=NULL
	SET @PAS_CODE=NULL
	SET @POSTAL_CODE= NULL
	SET @STATE= NULL
	SET @UIC=NULL
	SET @UNIT_DET=NULL
	SET @UNIT_KIND=NULL
	SET @UNIT_NBR=NULL
	SET @UNIT_TYPE=NULL
	SET @PARENT_PAS_CODE=NULL
	SET @cs_PARENT_PASCODE=NULL
	SET @rowErrorMessage = NULL
     
FETCH NEXT FROM command_cursor  INTO    @new_PAS_CODE 
                                      
END ---End of LOD_CURSOR        
CLOSE command_cursor
DEALLOCATE  command_cursor
 
----------------------------End Run to create all non existing units-----------------------

----------------------------Second  Run to insert all command_struct_chain records for all non existing units(This is to make sure that if the parent unit csc_ids are always available 
 
 DECLARE added_cursor CURSOR FOR  SELECT pas_code, PARENT_PAS_CODE,parentModified FROM @addedPascodes  
 
 OPEN added_cursor
 
 FETCH NEXT FROM added_cursor  INTO  @new_PAS_CODE,@PARENT_PAS_CODE,@parentModified
 
 WHILE @@FETCH_STATUS = 0 
 
 BEGIN ---BEGIN CURSOR     
     
     BEGIN TRY   
	            
	    SET @new_cs_id=(SELECT CS_ID FROM Command_Struct WHERE   PAS_CODE=@new_PAS_CODE)
	    SET @new_parent_cs_id =CASE 
                               WHEN @PARENT_PAS_CODE is null THEN 1
                               WHEN (( select COUNT(*)  from Command_Struct WHERE PAS_CODE =@PARENT_PAS_CODE )>1) THEN 1
                               WHEN ((SELECT CS_ID FROM Command_Struct WHERE PAS_CODE =@PARENT_PAS_CODE )  is null )THEN 1
                               ELSE   (SELECT CS_ID FROM Command_Struct WHERE PAS_CODE =@PARENT_PAS_CODE )
                               END  
	          
	          --Make sure that the parent pascode is updated only when the unit has not been modified by the application 
	            UPDATE Command_Struct 
                     SET  CS_ID_PARENT= @new_parent_cs_id                
                WHERE PAS_CODE =@new_PAS_CODE  
                
   
			IF(@parentModified=0)--insert into command chain only if its a fresh new pascode 
				BEGIN 
				   --insert the command struct chain   records for all chain types  with parentid null
 				   INSERT INTO Command_Struct_Chain (CS_ID,view_type,CHAIN_TYPE,created_by,created_date,MODIFIED_BY,MODIFIED_DATE )
 				   SELECT @new_cs_id, chain.id ,chain.name,@sysAdmin,GETUTCDATE(),@sysAdmin,GETUTCDATE()  from  
 					 core_lkupChainType  chain where chain.active =1 	      
			   END 
	      


     END TRY 
     BEGIN CATCH --BEGIN CATCH BLOCK 
    		 
		     SET @number=NULL 
             SET @errmsg='' 
             SET @message=''
    			
			    SELECT 	  @number= ERROR_NUMBER(),@errmsg= ERROR_MESSAGE() 
    			  
  		    	       IF @number <> 0 
					       BEGIN 
						    SET @message= 'Error:'+ cast(@number as varchar(10) ) + '|'+ @errmsg 
	 		 		       END 
				       ELSE
				       BEGIN 
						    SET @message= 'UnknownError'+ '|'+ @errmsg 
				        END  	
    		   
		         INSERT INTO  	core_pkgImport_Errors			   
			       (storedProcName ,keyValue,pkgLog_Id,  Time,ErrorMessage)
			     VALUES 
			       ('pascode_import  (command_struct_chain_insert)' ,@new_PAS_CODE,@logId,GETUTCDATE(),@message)			
    					 				    
     END CATCH---END CATCH BLOCK
     
     SET @PAS_CODE=NULL   
     SET @PARENT_PAS_CODE=NULL   
     
 FETCH NEXT FROM added_cursor  INTO    @new_PAS_CODE,@PARENT_PAS_CODE,@parentModified
                                      
END ---End of LOD_CURSOR        
CLOSE added_cursor 
DEALLOCATE  added_cursor 
--**************************************End Second run at this point we should have csc_ids for all the non existing units ************************
 DECLARE @VIEW_TYPE  AS tinyInt 

 SET @new_PAS_CODE=null
 SET @PARENT_PAS_CODE =null 
--**************************************Third run this will update all the parent csc ids ************************
DECLARE added_cursor_2 CURSOR FOR  SELECT pas_code, PARENT_PAS_CODE,parentModified FROM @addedPascodes  
 
  OPEN added_cursor_2
 
 FETCH NEXT FROM added_cursor_2  INTO  @new_PAS_CODE,@PARENT_PAS_CODE,@parentModified
 
 WHILE @@FETCH_STATUS = 0 
 
 BEGIN ---BEGIN CURSOR     
     
     BEGIN TRY   
		SET @new_cs_id = -1
		SET @new_parent_cs_id = -1

		SELECT @new_cs_id=CS_ID,@new_parent_cs_id =CS_ID_PARENT FROM Command_Struct WHERE   PAS_CODE=@new_PAS_CODE and @parentModified=0 

		--Update the command struct chain insert records for all chain types with parent unit need to record all errors
		--so we need to get each view type individually 
		IF (@new_cs_id <> -1) AND (@new_parent_cs_id <> -1)
		BEGIN
			
			SET @VIEW_TYPE=1
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE  and UserModified <> 1
			END 

			SET @VIEW_TYPE=2
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1
			END 

			SET @VIEW_TYPE=3
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1
			END 

			SET @VIEW_TYPE=4
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1
			END 

			SET @VIEW_TYPE=5
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1
			END  

			SET @VIEW_TYPE=6
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1
			END  

			SET @VIEW_TYPE=7
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1
			END

			SET @VIEW_TYPE=8
			IF ((SELECT active from core_lkupChainType WHERE id=@VIEW_TYPE) =1 )
			BEGIN 
				SET @new_parent_csc_id=(SELECT csc_id from command_struct_chain where CS_ID=@new_parent_cs_id and view_type =@VIEW_TYPE)
				
				UPDATE	Command_Struct_chain		
				SET		CSC_ID_PARENT = @new_parent_csc_id
				WHERE	CS_ID=@new_cs_id AND view_type =@VIEW_TYPE and UserModified <> 1	
			END    
		END	
     END TRY 
     BEGIN CATCH --BEGIN CATCH BLOCK 
    		 
		     SET @number=NULL 
             SET @errmsg='' 
             SET @message=''
    			
			    SELECT 	  @number= ERROR_NUMBER(),@errmsg= ERROR_MESSAGE() 
    			  
  		    	       IF @number <> 0 
					       BEGIN 
						    SET @message= 'Error:'+ cast(@number as varchar(10) ) + '|'+ @errmsg 
	 		 		       END 
				       ELSE
				       BEGIN 
						    SET @message= 'UnknownError'+ '|'+ @errmsg 
				        END  	
    		   
		         INSERT INTO  	core_pkgImport_Errors			   
			       (storedProcName ,keyValue,pkgLog_Id,  Time,ErrorMessage)
			     VALUES 
			       ('pascode_import  (command_struct_chain_update)' ,@new_PAS_CODE,@logId,GETUTCDATE(),@message)			
    					 				    
     END CATCH---END CATCH BLOCK
     
     SET @PAS_CODE=NULL   
     SET @PARENT_PAS_CODE=NULL   
     
 FETCH NEXT FROM added_cursor_2  INTO     @new_PAS_CODE,@PARENT_PAS_CODE ,@parentModified
                                      
END ---End of LOD_CURSOR        
CLOSE added_cursor_2 
DEALLOCATE  added_cursor_2  
--**************************************End Third run  ----------------------------------------------------------
  
--Update any Command_Struct_Chain Records that are currently assigned to our unknown unit.
--We grab the real parent id from and join back to the chain for a real csc_id 
Update csc set CSC_ID_PARENT = case when csc2.CSC_ID = 0 then 11 else csc2.CSC_ID end
from Command_Struct_Chain csc  
 join Command_Struct cs on csc.CS_ID = cs.CS_ID
 join Command_Struct_Chain csc2 on cs.CS_ID_PARENT = csc2.CS_ID and csc.view_type = csc2.view_type
where csc.CSC_ID_PARENT = 11

  
  
	UPDATE pkgLog 
	SET  nRowRawInserted=@rawRowsInserted,
		 nRowInserted=@insertedMemberRows,
		 nModifiedRecords =@modifiedRows	 
	WHERE id=@logId
	 
	 
-- Catch any PAS Codes that were re-added and the name was previously blank
Update Form348
Set member_unit = cs.Long_Name
From Form348 f Inner Join Command_Struct cs On cs.CS_ID = f.member_unit_id
Where IsNull(f.member_unit, '') = '' And IsNull(cs.LONG_NAME, '') <> ''
GO

