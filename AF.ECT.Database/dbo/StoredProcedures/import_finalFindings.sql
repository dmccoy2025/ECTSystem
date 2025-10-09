--The function uses the  import_lodmapping table created during import and
--and the imp_LOD_DISPOSITIONS table to insert the final findings .Makes use of 
--final decision field to get the value for final findings 


--EXECUTE  import_finaldecision
create PROCEDURE [dbo].[import_finalFindings]
 
AS 

BEGIN 

    
   ---Following declarations are for inserting findings record
   DECLARE @lod_modified_date VARCHAR (100  )  
		,@person_type  int
		,@lod_GRADE   VARCHAR (100  ) 
		,@lod_RANK     VARCHAR (100  ) 
		,@lod_UNIT  VARCHAR (100  )    
 
   DECLARE 
	   @lod_APPOINT_AUTH_PERS_ID	varchar(200)
	  ,@lod_APPOINT_AUTH_DECISION	varchar(600)
	  ,@lod_APPOINT_AUTH_EXPLANATION	varchar(8000)
	  ,@lod_APPOINT_AUTH_NAME	varchar(600)
	  ,@lod_APPOINT_AUTH_GRADE	varchar(600)
	  ,@lod_APPOINT_AUTH_UNIT	varchar(600)
	  ,@lod_HQAA_PERS_ID	varchar(200)
	  ,@lod_HQAA_DECISION	varchar(500)
	  ,@lod_HQAA_EXPLANATION	varchar(8000)
	  ,@lod_HQAA_NAME	varchar(600)
	  ,@lod_HQAA_RANK	varchar(600)
	  ,@lod_HQAA_SIGNATURE	varchar(600)
	  ,@lod_HQBOARD_PERS_ID	varchar(200)
	  ,@lod_HQBOARD_DECISION	varchar(500)
	  ,@lod_HQBOARD_EXPLANATION	varchar(8000)
	  ,@lod_HQBOARD_NAME	varchar(600)
	  ,@lod_HQBOARD_GRADE	varchar(600)
	  ,@inv_HQAA_PERS_ID 	varchar(200) 
	  ,@inv_HQAA_REASONS	varchar(8000)
	  ,@inv_FINAL_APPROVAL_FINDINGS	varchar(600)
	  ,@inv_HQAA_NAME	varchar(600)
	  ,@inv_HQAA_GRADE	varchar(600)
	  ,@inv_HQAA_UNIT	varchar(600)	
	  ,@inv_RA_PERS_ID	varchar(8000)
	  ,@inv_RA_REASONS	varchar(8000)
	  ,@inv_RA_NAME	varchar(8000)
	  ,@inv_RA_GRADE	varchar(8000)
	  ,@inv_RA_UNIT	varchar(8000)
	  ,@inv_RA_DATE	varchar(8000)
	 --End declarations 
	 
	 
	---Declarations for final decision
	DECLARE @FINAL_DECISION varchar(500),@person varchar(400)
    DECLARE @rcphalod_id int ,@lod_id int ,@fianlStatus int ,@invRptId int 
	DECLARE  @decision varchar(200),@finding   varchar(300),@findingsID tinyint
   ------end 
   
	 						 
 	--------LodCursor------- 
    DECLARE lod_cursor CURSOR FOR  SELECT cast(lod_id as int) ,FINAL_DECISION FROM imp_LOD_DISPOSITIONS  
    	
    OPEN lod_cursor
    	
    FETCH NEXT FROM lod_cursor  INTO @rcphalod_id ,@FINAL_DECISION
    	
    WHILE @@FETCH_STATUS = 0
    BEGIN
          
	     SET  @finding=null
	     SET @findingsID=null
         SET @lod_UNIT=NULL
	     SET @lod_RANK=NULL 
	     SET @lod_GRADE=NULL 
    	 
    SELECT @lod_id= alod_lod_id, @fianlStatus=alod_status FROM imp_LODMAPPING WHERE rcpha_lodid=	@rcphalod_id 	
    	   
     IF (@lod_id is not null and @fianlStatus=13 ) --Only if final status is complete
      BEGIN 
	    IF (@FINAL_DECISION is  null Or LEN(@FINAL_DECISION)=0)
	     BEGIN --If final  decsiion is empty
	        IF (SELECT  LIR_ID FROM imp_lod_investigation_rpts WHERE cast(lod_id as int)=	@rcphalod_id ) IS NULL   
		      BEGIN --If informal
 			    SELECT  		
		 	       @lod_HQBOARD_PERS_ID		 =lod.HQBOARD_PERS_ID
			      ,@lod_HQBOARD_DECISION		 =lod.HQBOARD_DECISION
			      ,@lod_HQBOARD_EXPLANATION		 =lod.HQBOARD_EXPLANATION
			      ,@lod_HQBOARD_NAME		 =lod.HQBOARD_NAME
			      ,@lod_HQBOARD_GRADE		 =lod.HQBOARD_GRADE					  
			      ,@lod_HQAA_PERS_ID		 =lod.HQAA_PERS_ID
			      ,@lod_HQAA_DECISION		 =lod.HQAA_DECISION
			      ,@lod_HQAA_EXPLANATION		 =lod.HQAA_EXPLANATION
			      ,@lod_HQAA_NAME		 =lod.HQAA_NAME
			      ,@lod_HQAA_RANK		 =lod.HQAA_RANK
			      ,@lod_HQAA_SIGNATURE		 =lod.HQAA_SIGNATURE							   
			      ,@lod_modified_date= lod.modified_date    FROM imp_LOD_DISPOSITIONS lod WHERE  	  cast(lod.lod_id as int)=@rcphalod_id
    					 				 
			       SET @findingsID =CASE    @lod_HQAA_DECISION  
                                      WHEN 'LINE_OF_DUTY' THEN 1
                                      WHEN 'EPTS_NOT_LOD' THEN 2
                                      WHEN 'EPTS_SERVICE' THEN 3
                                      WHEN 'NOT_LOD_MISCONDUCT' THEN 4
                                      WHEN 'NOT_LOD_NOT_MISCONDUCT' THEN 5	
                                      ELSE NULL
                                    END   
			    IF (@findingsID IS   NULL)  
			      BEGIN --Blk If no hqaa finding
    												 
                     SET @findingsID =CASE    @lod_HQBOARD_DECISION  
                                      WHEN 'LINE_OF_DUTY' THEN 1
                                      WHEN 'EPTS_NOT_LOD' THEN 2
                                      WHEN 'EPTS_SERVICE' THEN 3
                                      WHEN 'NOT_LOD_MISCONDUCT' THEN 4
                                      WHEN 'NOT_LOD_NOT_MISCONDUCT' THEN 5				
                                      ELSE NULL
                                    END   
                    
                    IF(@findingsID IS NOT NULL)
					    BEGIN --If bd finding found
                            SET @finding	=@lod_HQBOARD_DECISION
                            SET	@person=' HQBD FINDING'
                             
                            SET @person_type=6 --BOARD                           															
	 					    EXECUTE imp_InsertFindingRecord_ForFinalDecision 
                                    @lod_id,@person_type
                                    ,@lod_HQBOARD_PERS_ID,@findingsID
                                    ,@lod_HQBOARD_EXPLANATION
                                    ,@lod_HQBOARD_NAME
                                    ,@lod_RANK
                                    ,@lod_HQBOARD_GRADE
                                    ,@lod_UNIT
                                    ,@lod_modified_date
    																				
						    SET @person_type  =10 
						    EXECUTE imp_InsertFindingRecord_ForFinalDecision 
			                        @lod_id,@person_type
			                        ,@lod_HQAA_PERS_ID,@findingsID
			                        ,@lod_HQAA_EXPLANATION
			                        ,@lod_HQAA_NAME,@lod_HQAA_RANK 
			                        ,@lod_GRADE
			                        ,@lod_UNIT
	                                ,@lod_modified_date
                       END	--Blk bd finding found	
				    END --End If  no hqaa finding   
			     ELSE --Else If  hqaa finding
			      BEGIN ----  hqaa finding found
                        SET	@person=' HQAA FINDING' 
                        SET @finding	=@lod_HQAA_DECISION
                        SET @person_type  =10 --HQAA

                         EXECUTE imp_InsertFindingRecord_ForFinalDecision 
							                        @lod_id,@person_type
							                        ,@lod_HQAA_PERS_ID,@findingsID
							                        ,@lod_HQAA_EXPLANATION
							                        ,@lod_HQAA_NAME,@lod_HQAA_RANK 
							                        ,@lod_GRADE
							                        ,@lod_UNIT
	    					                        ,@lod_modified_date
				    END  --hqaa finding found				
    															
			    END --Blk End if lod is not formal
		    ELSE
		      BEGIN	--Begin if lod id formal 	 
    		  
                 SELECT
                  @inv_HQAA_PERS_ID 		 =inv.HQAA_PERS_ID 
                 ,@inv_HQAA_REASONS		 =inv.HQAA_REASONS
                 ,@inv_HQAA_NAME		 =inv.HQAA_NAME
                 ,@inv_HQAA_GRADE		 =inv.HQAA_GRADE
                 ,@inv_HQAA_UNIT		 =inv.HQAA_UNIT
                 ,@inv_FINAL_APPROVAL_FINDINGS		 =inv.FINAL_APPROVAL_FINDINGS
                 ,@inv_RA_PERS_ID		 =inv.RA_PERS_ID
                 ,@inv_RA_REASONS		 =inv.RA_REASONS
                 ,@inv_RA_NAME		 =inv.RA_NAME
                 ,@inv_RA_GRADE		 =inv.RA_GRADE
                 ,@inv_RA_UNIT		 =inv.RA_UNIT								 
                 ,@lod_modified_date= inv.modified_date FROM imp_LOD_INVESTIGATION_RPTS   inv    WHERE cast(inv.lod_id AS INT)=@rcphalod_id
 
    			  SET @findingsID =CASE    @inv_FINAL_APPROVAL_FINDINGS  
				        		      WHEN 'LINE_OF_DUTY' THEN 1
								      WHEN 'EPTS_NOT_LOD' THEN 2
								      WHEN 'EPTS_SERVICE' THEN 3
								      WHEN 'NOT_LOD_MISCONDUCT' THEN 4
								      WHEN 'NOT_LOD_NOT_MISCONDUCT' THEN 5											 						  
								      ELSE NULL
						    	    END  						
    							  
		    IF (@findingsID IS NOT NULL)
		    BEGIN 
			    SET @finding	=@inv_FINAL_APPROVAL_FINDINGS	
			    SET	@person='FORMAL HQAA  OR HQBD SET FINAL_APPROVAL_FINDINGS IN INV RPT'     						 		
			
			     SET @person_type  =18 --FORMAL HQAA
                 EXECUTE imp_InsertFindingRecord_ForFinalDecision 
						                    @lod_id,@person_type
						                    ,@inv_HQAA_PERS_ID,@findingsID
						                    ,@inv_HQAA_REASONS
						                    ,@inv_HQAA_NAME
						                    ,@lod_RANK
						                    ,@inv_HQAA_GRADE
						                    ,@inv_HQAA_UNIT
						                    ,@lod_modified_date	
            														 
		      																
    			
			    IF( @inv_RA_PERS_ID IS NOT NULL OR @inv_RA_NAME IS NOT NULL )
			     BEGIN 	
				    SET @person_type  =14 --FORMAL HQBD							
				    EXECUTE imp_InsertFindingRecord_ForFinalDecision 
                        @lod_id,@person_type
                        ,@inv_RA_PERS_ID,@findingsID
                        ,@inv_RA_REASONS
                        ,@inv_RA_NAME
                        ,@lod_RANK
                        ,@inv_RA_GRADE
                        ,@inv_RA_UNIT
                        ,@lod_modified_date											
			    END 
    											
		    END  --  End IF (@findingsID IS NOT NULL)
    		
		    END  --End if lod is formal  
		    --Update form348 table									
    		
		    UPDATE FORM348 SET FinalFindings=@findingsID WHERE lodid= @lod_id
		    print   @person +'|'+IsNull(@finding,'') +'|'+IsNull(CAST(@findingsID AS VARCHAR(100)),'')  +'|RCHPHALODID:'+cast(@rcphalod_id as varchar(200))
    					 
	    END --IF FINALDECISION is   empty 
    	 	
	    END --IF LODID IS NOT NULL 			
	    FETCH NEXT FROM lod_cursor  INTO @rcphalod_id ,@FINAL_DECISION
      END ---End of LOD_CURSOR      
         
     CLOSE lod_cursor
     DEALLOCATE  lod_cursor--WHERE LOD_ID=6633
    	
    END
GO

