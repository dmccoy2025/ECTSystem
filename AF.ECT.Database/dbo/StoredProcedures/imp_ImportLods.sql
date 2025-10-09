--SELECT * FROM form348
--SELECT * from form348_MEDICAL
--EXEC imp_ImportLods
--SELECT * FROM imp_LODMAPPING
--SELECT COUNT(*) FROM imp_LODMAPPING
--select  COUNT(*)  FROM imp_lod_dispositions
--DELETE FROM import_Error_LOG
--SELECT * FROM import_Error_LOG

CREATE PROCEDURE [dbo].[imp_ImportLods]
 
AS
BEGIN 
 
  ----------DECLARE  ----------------------------
  ---LOD--------- 
DECLARE @lod_id int 
DECLARE @new_lod_id int
--------LodCursor------- 
DECLARE lod_cursor CURSOR FOR  
SELECT lod_id FROM imp_LOD_DISPOSITIONS 

DECLARE  @msg varchar(2000)
DECLARE @number int ,@errmsg varchar(2000)

declare @createdBy as int, @modifiedBy as int, @tmp int
 
  --------------------------------- 
 ---------Clean tables------------------
 DELETE FROM FORM348_UNIT
 DELETE FROM FORM348_MEDICAL
 DELETE FROM FORM348
 DELETE FROM FORM261
 DELETE FROM RWOA
 DELETE FROM  FORM348_findings 
 DELETE FROM core_WorkStatus_Tracking
 UPDATE imp_LODMAPPING SET alod_lod_id=NULL
 
 
 dbcc checkident(form348, reseed, 0)
 dbcc checkident(core_workstatus_tracking,reseed, 0)
 dbcc checkident(rwoa, reseed, 0)
 dbcc checkident(form348_findings, reseed, 0)
 
 
 create table #added_users
 (
	userid int,
	username nvarchar(200)
);
 
 ----------------------------------------
 -----TRANSACTION------------------------
--DECLARE @TransactionName varchar(20) 
--SET @TransactionName= 'lodRecordInsert' 
----------------------------------------
		 
OPEN lod_cursor

FETCH NEXT FROM lod_cursor INTO @lod_id 


WHILE @@FETCH_STATUS = 0

BEGIN ---BEGIN lod_cursor CURSOR 
	    
	    BEGIN TRY --BEGIN TRY BLOCK 
	    --BEGIN TRANSACTION @TransactionName  --BEGIN TRANSACTION BLOCK 
	   /***Form348*******
	    Member grade in lod_dispositions is in the form SRAT SGT,TSGT   
	    Not fetched columns are following
	    doc_group_id
	    deleted--How are we getting this info 
	    sig_date_unit_commander,sig_name_unit_commander
	    sig_date_med_officer	    
	    sig_date_legal	  
	    sig_date_appointing	   
	    sig_date_board_legal	   
	    sig_date_board_medical 
	    sig_date_board_admin	     
	    sig_date_approval    
	    sig_title_approval
	    sig_date_formal_approval	    
	    sig_title_formal_approval
	    to_unit
	    from_unit
	    FinalFindings
	    sarc
	    restricted
	   */	 
	  	/***Form348 Medical ****** 
	   	Not fetched columns are following
	   	mva_involved_yn  	diagnosis_text */
	   	
	   	--make sure the users exist for these
	   	--start with created by
	   	declare @checkUser as nvarchar(200)
	   	set @checkUser = (select ltrim(rtrim(created_by)) from imp_LOD_DISPOSITIONS where LOD_ID = @lod_id)
	   	set @createdBy = (select top 1 userId from imp_USERMAPPING where USERNAME like @checkUser)
	   	
	   	if (@createdBy is null)
	   	begin
	   		--we didn't find them in the mapping table, try our temp table, see if we already created them
	   		if exists (select userid from #added_users where username like @checkUser)
	   			set @createdBy = (select userid from #added_users where username like @checkUser)
	   		else
	   		begin
	   			exec imp_create_user @checkUser, @createdBy out;
				insert into #added_users (userid, username) values (@createdBy, @checkUser);
			end
		end
		
				
	   	--now check modified by
	   	set @checkUser = (select ltrim(rtrim(MODIFIED_BY)) from imp_LOD_DISPOSITIONS where LOD_ID = @lod_id)
	   	set @modifiedBy = (select top 1 userId from imp_USERMAPPING where USERNAME like @checkUser)
	   	
	   	if (@modifiedBy is null)
	   	begin
	   		if exists (select userId from #added_users where username like @checkUser)
	   			set @modifiedBy = (select userid from #added_users where username like @checkUser)
	   		else
	   		begin
	   			exec imp_create_user @checkUser, @modifiedBy out;
				insert into #added_users (userid, username) values (@modifiedBy, @checkUser);
			end
		end
		
	   	
	  	INSERT INTO form348
	    (
	     workflow  
	     ,status
	     ,case_id
	     ,member_name
	    ,member_ssn
	    ,member_grade
	    ,member_unit
	    ,member_unit_id
	    ,member_DOB
	    ,member_compo
	    ,created_by
	    ,created_date
	    ,modified_by
	    ,modified_date
	    ,med_tech_comments
	    ,rwoa_reason
	    ,rwoa_explantion
	    ,rwoa_date
	    ,aa_ptype
	    ,deleted
	    ,formal_inv	
	    ,appAuthUserId
	    ,FinalDecision 
	    ,Board_For_General_YN
	    ,io_poc_info
		,io_instructions
		,io_completion_date
		,io_ssn
		--,sig_date_unit_commander
		,sig_name_unit_commander
		--,sig_date_med_officer
		,sig_name_med_officer
		--,sig_date_legal
		,sig_name_legal
		--,sig_date_appointing
		,sig_name_appointing
		--,sig_date_board_legal
		,sig_name_board_legal
		--,sig_date_board_medical
		,sig_name_board_medical
		--,sig_date_board_admin
		,sig_name_board_admin
		--,sig_date_approval
		,sig_name_approval
		,sig_title_approval
		)
	   
	    SELECT 
			1--WorkflowId	  
	 	    ,(SELECT alod_status  FROM imp_LODMAPPING WHERE rcpha_lodid=@lod_id) ---Since the grade is rank here we are doing one to one mapping
	 	    ,(SELECT case_id  FROM imp_LODMAPPING WHERE rcpha_lodid=@lod_id) ---Since the grade is rank here we are doing one to one mapping
 		, MEMBER_NAME 
	     ,MEMBER_SSN
	     ,(SELECT TOP 1 GRADECODE  FROM imp_gradeLookUp WHERE GRADE=member_grade)---Since the grade is rank here we are doing one to one mapping
		 ,MEMBER_UNIT 
		 ,cast(MEMBER_CS_ID  as int) 
		 ,CASE 
			WHEN  MEMBER_DOB  is null or MEMBER_DOB ='' THEN  null
			ELSE CAST( SUBSTRING( MEMBER_DOB ,1,19)  AS DATETIME)
			END 
		 ,'6'
		 ,@createdBy --CREATED_BY)
		 ,CREATED_DATE
		 ,@modifiedBy --MODIFIED_BY)
		 ,MODIFIED_DATE 		 
		 ,RWOA_MEDTECH_COMMENTS 
		 ,(SELECT  ID FROM core_lkupRWOAReasons WHERE TYPE=RWOA_REASON )
		 ,RWOA_EXPLANATION 
		 ,CASE 
			WHEN RWOA_DATE is null or RWOA_DATE='' THEN  null
			ELSE cast( SUBSTRing(RWOA_DATE,1,19)  AS DATETIME)
			END     
		  ,5----aaptype
		 ,0--Deleted
		 ,CASE FORMAL_INV_YN
			WHEN 'Y' THEN 1
			WHEN 'N' THEN 0
			ELSE 0 
			END
		 ,(SELECT USERID FROM imp_USERMAPPING where  CAST(PERSON_ID AS INT)=CAST(APPOINT_AUTH_PERS_ID AS INT))
		 ,FINAL_DECISION
		 ,BOARD_FOR_GENERAL_YN
		 ,APPOINT_AUTH_POC
		 ,ASSIGN_INV_INSTRUCTIONS
		 ,CASE 
			WHEN ASSIGN_INV_COMPLETION is null or ASSIGN_INV_COMPLETION='' THEN  null
			ELSE cast( SUBSTRing(ASSIGN_INV_COMPLETION,1,19)  AS DATETIME)
			END		 
		 ,(SELECT SSN FROM core_users where USERID=( SELECT USERID FROM imp_USERMAPPING where  CAST(PERSON_ID AS INT)=CAST(ASSIGN_INV_OFFICER_PERS_ID AS INT)   ))--))--To get the ssn
		--,getUTCDate()
		,CMDR_NAME
		--,getUTCDate()
		,PHYSICIAN_NAME
		--,getUTCDate()
		,(SELECT LASTNAME+' '+FIRSTNAME FROM CORE_USERS WHERE  USERID=( SELECT USERID FROM imp_USERMAPPING where CAST(PERSON_ID AS INT)=CAST(WING_JA_PERS_ID AS INT) ))--))--Wing JA Name is not there 
		--,getUTCDate()
		,APPOINT_AUTH_NAME
		--,getUTCDate()
		,HQJA_NAME
		---,getUTCDate()
		,HQSG_NAME
		--,getUTCDate()
		 ,HQBOARD_NAME
		 --,getUTCDate()
		 ,HQAA_NAME
		 ,HQAA_SIGNATURE 		
		 
	   FROM imp_lod_dispositions WHERE LOD_ID = @lod_id
	  
   
	 -------------Get Identity------------------------------------------------------- 
	   SET @new_lod_id=SCOPE_IDENTITY()
	   
	 -------------Update LOD_MAPPING TABLE------------------------------------------- 
		UPDATE 
			imp_LODMAPPING
		SET 
			alod_lod_id=@new_lod_id
		WHERE 
			rcpha_lodid=@lod_id
 		
 	end try
	BEGIN CATCH
		SELECT  @number= ERROR_NUMBER(),@errmsg= ERROR_MESSAGE() 
 		EXECUTE imp_InsertErrorRecord @number ,'FORM_348, approval','imp_ImportLods ','Error creating form348 record',@lod_id, @errmsg
	END CATCH
	
 		
	 ----------------update formal approval from investigation rpts--------------------  		
	 
	 begin try
	 		    
	   UPDATE form348
	   SET 
		 sig_name_formal_approval=t2.HQAA_NAME
	   --,sig_title_formal_approval=t2.
	   FROM  
	   imp_lod_investigation_rpts t2
	   WHERE 
		CAST(t2.LOD_ID AS INT)=@lod_id  
		
	end try
	BEGIN CATCH
		SELECT  @number= ERROR_NUMBER(),@errmsg= ERROR_MESSAGE() 
 		EXECUTE imp_InsertErrorRecord @number ,'FORM_348, approval','imp_ImportLods ','Error creating form348 record',@lod_id, @errmsg
	END CATCH
	
	----------------form348_Medical----------------------   	
	begin try
		
	  UPDATE form348_Medical
	  SET 
	    member_status=  t2.MEMBER_STATUS
	   ,event_nature_type= t2.EVENT_NATURE_TYPE
	   ,event_nature_details=t2.EVENT_NATURE_DETAILS
	   ,medical_facility=t2.MEDICAL_FACILITY
	   ,medical_facility_type=t2.MEDICAL_FACILITY_TYPE
	   ,treatment_date =CASE 
							WHEN t2.TREATMENT_DATE  is null or t2.TREATMENT_DATE ='' THEN  null
							ELSE CAST( SUBSTRING(t2.TREATMENT_DATE ,1,19)  AS DATETIME)
						END 
	   ,death_involved_yn=t2.DEATH_INVOLVED_YN
	   ,epts_yn=CASE t2.EPTS_YN --Extra value X is considered as Null
					WHEN 'Y' THEN 1
					WHEN 'N' THEN 0
					ELSE NULL
				END 
	   ,icd9Id=(SELECT ICD9_ID FROM core_lkupICD9 where value=t2.ICD9_ID)
	   ,physician_approval_comments=t2.PHYSICIAN_APPROVAL_REASON	  
	   ,physician_cancel_explanation=t2.PHYSICIAN_CANCEL_EXPLANATION
	   ,physician_cancel_reason=(SELECT ID FROM core_lkupPhyCancelReasons WHERE DESCRIPTION=t2.PHYSICIAN_CANCEL_REASON)
	   ,modified_by=@modifiedBy--t2.MODIFIED_BY)
	   ,modified_date=cast(t2.MODIFIED_DATE as datetime)
	   
	   FROM form348_Medical,imp_lod_dispositions t2   
	   WHERE form348_Medical.lodid=@new_lod_id
	   AND CAST(t2.LOD_ID AS INT)=@lod_id
	   
	end try
	BEGIN CATCH
		SELECT  @number= ERROR_NUMBER(),@errmsg= ERROR_MESSAGE() 
 		EXECUTE imp_InsertErrorRecord @number ,'FORM_348, Medical','imp_ImportLods ','Error creating form348 record',@lod_id, @errmsg
	END CATCH
	   
	----------------form348_unit----------------------   
	begin try
	
    UPDATE form348_unit
	  SET 
	     form348_unit.cmdr_circ_details=t2.CMDR_CIRC_DETAILS
	   , form348_unit.cmdr_duty_determination=t2.CMDR_DUTY_DETERMINATION
	   , form348_unit.cmdr_duty_from=CASE 
									WHEN t2.CMDR_DUTY_FROM  is null or t2.CMDR_DUTY_FROM ='' THEN  null
									ELSE CAST( SUBSTRING(t2.CMDR_DUTY_FROM ,1,19)  AS DATETIME)
			END  
	   , form348_unit.cmdr_duty_others= t2.CMDR_DUTY_OTHERS
	   , form348_unit.cmdr_duty_to= CASE 
									WHEN t2.CMDR_DUTY_TO  is null or t2.CMDR_DUTY_TO ='' THEN  null
									ELSE CAST( SUBSTRING(t2.CMDR_DUTY_TO ,1,19)  AS DATETIME)
									END	 
	   , form348_unit.cmdr_activated_yn= t2.CMDR_ACTIVATED_YN
	   , form348_unit.modified_date=CAST( t2.MODIFIED_DATE as datetime)
	   , form348_unit.modified_by= @modifiedBy--MODIFIED_BY)
 	  FROM form348_unit	,   imp_lod_dispositions t2   
	  WHERE form348_unit.lodid=@new_lod_id
	  AND CAST(t2.LOD_ID AS INT)=@lod_id 
	        

		END TRY 	--END TRY BLOCK 
		BEGIN CATCH --BEGIN CATCH BLOCK 

		SELECT 
		  @number= ERROR_NUMBER()  
		 ,@errmsg= ERROR_MESSAGE() 
		 

		EXECUTE imp_InsertErrorRecord @number
		,'FORM_348, UNIT','imp_ImportLods ','Error creating form348 record',@lod_id, @errmsg
		 		 						    
	END CATCH---END CATCH BLOCK   
		
FETCH NEXT FROM lod_cursor  INTO @lod_id 
END ---End of LOD_CURSOR        
CLOSE lod_cursor
DEALLOCATE  lod_cursor


drop table #added_users;
         
END    ---End of STORED PROCEDURE
GO

