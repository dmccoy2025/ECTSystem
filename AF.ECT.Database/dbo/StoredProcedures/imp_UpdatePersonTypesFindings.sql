--select *from imp_LOD_DISPOSITIONS
--execute imp_UpdatePersonTypesFindings

CREATE PROCEDURE [dbo].[imp_UpdatePersonTypesFindings]
	 (  @lod_id int   )
AS
BEGIN 
--DECLARE VARIABLES SET 1 --------

DECLARE 
   @lod_APPOINT_AUTH_PERS_ID	varchar(4000)
  ,@lod_APPOINT_AUTH_DECISION	varchar(4000)
  ,@lod_APPOINT_AUTH_EXPLANATION	varchar(4000)
  ,@lod_APPOINT_AUTH_NAME	varchar(4000)
  ,@lod_APPOINT_AUTH_GRADE	varchar(4000)
  ,@lod_APPOINT_AUTH_UNIT	varchar(4000)
  ,@lod_WING_JA_PERS_ID	varchar(4000)
  ,@lod_WING_JA_DECISION	varchar(4000)
  ,@lod_WING_JA_EXPLANATION	varchar(4000)
  ,@lod_CMDR_PERS_ID	varchar(4000)
  ,@lod_CMDR_REC_FINDINGS	varchar(4000)
  ,@lod_CMDR_EXPLANATION	varchar(4000)
  ,@lod_CMDR_NAME	varchar(4000)
  ,@lod_CMDR_GRADE	varchar(4000)
  ,@lod_CMDR_UNIT	varchar(4000)
  ,@lod_HQSG_PERS_ID	varchar(4000)
  ,@lod_HQSG_DECISION	varchar(4000)
  ,@lod_HQSG_EXPLANATION	varchar(4000)
  ,@lod_HQSG_NAME	varchar(4000)
  ,@lod_HQSG_GRADE	varchar(4000)
  ,@lod_HQJA_PERS_ID	varchar(4000)
  ,@lod_HQJA_DECISION	varchar(4000)
  ,@lod_HQJA_EXPLANATION	varchar(4000)
  ,@lod_HQJA_NAME	varchar(4000)
  ,@lod_HQJA_GRADE	varchar(4000)
  ,@lod_HQBOARD_PERS_ID	varchar(4000)
  ,@lod_HQBOARD_DECISION	varchar(4000)
  ,@lod_HQBOARD_EXPLANATION	varchar(4000)
  ,@lod_HQBOARD_NAME	varchar(4000)
  ,@lod_HQBOARD_GRADE	varchar(4000)
  ,@lod_HQAA_PERS_ID	varchar(4000)
  ,@lod_HQAA_DECISION	varchar(4000)
  ,@lod_HQAA_EXPLANATION	varchar(4000)
  ,@lod_HQAA_NAME	varchar(4000)
  ,@lod_HQAA_RANK	varchar(4000)
  ,@lod_HQAA_SIGNATURE	varchar(4000)
  ,@lod_HQSR_PERS_ID	varchar(4000)
  ,@lod_HQSR_DECISION	varchar(4000)
  ,@lod_HQSR_EXPLANATION	varchar(4000)
  ,@lod_HQSR_NAME	varchar(4000)
  ,@lod_HQSR_GRADE	varchar(4000)
  ,@inv_INV_OFFICER_PERS_ID	varchar(4000)
  ,@lod_INV_OFFICER_BRANCH	varchar(4000)
  ,@lod_INV_OFFICER_SSN	varchar(4000)
  ,@lod_INV_OFFICER_NAME	varchar(4000)
  ,@lod_INV_OFFICER_GRADE	varchar(4000)
  ,@lod_INV_OFFICER_UNIT	varchar(4000)
  ,@lod_MED_TECH_PERS_ID	varchar(4000)
  ,@lod_MEDTECH_NAME	varchar(4000)
  ,@lod_MEDTECH_GRADE	varchar(4000)
  ,@lod_MEDTECH_UNIT	varchar(4000)
  ,@lod_MPF_PERS_ID	varchar(4000)
  ,@lod_MPF_NAME	varchar(4000)
  ,@lod_MPF_GRADE	varchar(4000)
  ,@lod_MPF_UNIT	varchar(4000)
  ,@lod_PHYSICIAN_PERS_ID	varchar(4000)
  ,@lod_PHYSICIAN_NAME	varchar(4000)
  ,@lod_PHYSICIAN_GRADE	varchar(4000)
  ,@lod_PHYSICIAN_DECISION	varchar(4000)
  ,@inv_io_remarks varchar(4000)
  ,@inv_io_findings varchar(4000)
 ,@inv_HQAA_PERS_ID 	varchar(4000)
 ,@inv_FINAL_APPROVAL_FINDINGS	varchar(4000)
 ,@inv_HQAA_REASONS	varchar(4000)
 ,@inv_HQAA_NAME	varchar(4000)
 ,@inv_HQAA_GRADE	varchar(4000)
 ,@inv_HQAA_UNIT	varchar(4000)
 ,@inv_HQJA_PERS_ID	varchar(4000)
 ,@inv_HQJA_FINDINGS	varchar(4000)
 ,@inv_HQJA_REASONS	varchar(4000)
 ,@inv_HQJA_NAME	varchar(4000)
 ,@inv_HQJA_GRADE	varchar(4000)
 ,@inv_HQJA_UNIT	varchar(4000)
 ,@inv_HQSG_PERS_ID  	varchar(4000)
 ,@inv_HQSG_FINDINGS	varchar(4000)
 ,@inv_HQSG_REASONS	varchar(4000)
 ,@inv_HQSG_NAME	varchar(4000)
 ,@inv_HQSG_GRADE	varchar(4000)
 ,@inv_HQSG_UNIT	varchar(4000)
 ,@inv_HQSR_PERS_ID	varchar(4000)
 ,@inv_HQSR_FINDINGS	varchar(4000)
 ,@inv_HQSR_REASONS	varchar(4000)
 ,@inv_HQSR_NAME	varchar(4000)
 ,@inv_HQSR_GRADE	varchar(4000)
 ,@inv_HQSR_UNIT	varchar(4000)
 ,@inv_RA_PERS_ID	varchar(4000)
 ,@inv_RA_REASONS	varchar(4000)
 ,@inv_RA_NAME	varchar(4000)
 ,@inv_RA_GRADE	varchar(4000)
 ,@inv_RA_UNIT	varchar(4000)
 ,@inv_RA_DATE	varchar(4000)
 ,@inv_WING_AA_PERS_ID	varchar(4000)
 ,@inv_WING_AA_APPROVED_YN	varchar(4000)
 ,@inv_WING_AA_FINDINGS	varchar(4000)
 ,@inv_WING_AA_REASONS	varchar(4000)
 ,@inv_WING_AA_NAME	varchar(4000)
 ,@inv_WING_AA_GRADE	varchar(4000)
 ,@inv_WING_AA_UNIT	varchar(4000)
 ,@inv_WING_JA_PERS_ID	varchar(4000)
 ,@inv_WING_JA_APPROVED_YN	varchar(4000)
 ,@inv_WING_JA_REASONS	varchar(4000)
 ,@inv_WING_JA_NAME	varchar(4000)
 ,@inv_WING_JA_GRADE	varchar(4000)
 ,@inv_WING_JA_UNIT	varchar(4000)


 -----SECOND SET OF VRIABLES
 ,@lod_PERS_ID  int  
,@lod_DECISION  VARCHAR (100  )    
,@lod_EXPLANATION  VARCHAR (4000  )    
,@lod_NAME  VARCHAR (100  )    
,@lod_GRADE   VARCHAR (100  ) 
,@lod_RANK     VARCHAR (100  ) 
,@lod_UNIT  VARCHAR (100  )    
,@lod_FINDINGS   VARCHAR (100  )    
,@lod_SIGNATURE  VARCHAR (100  )    
,@lod_BRANCH  VARCHAR (100  )    
,@inv_PERS_ID  int  
,@inv_FINDINGS VARCHAR (100  )    
,@inv_REASONS VARCHAR (4000  )    
,@inv_NAME VARCHAR (100  )    
,@inv_GRADE VARCHAR (100  )    
,@inv_UNIT VARCHAR (100  )    
,@person_type  int   
,@concury_n  VARCHAR (100  )     
,@findings int 
,@comments   VARCHAR (4000  )    
,@lod_modified_date VARCHAR (100  )     
,@inv_modified_date VARCHAR (100  )     
 ,@userId int
 ,@SSN VARCHAR (100  )
 ,@PAS_CODE VARCHAR (100  )
,@RANK VARCHAR (100  )
,@GRADE VARCHAR (100  ) 
 , @lod_APPROVED_YN VARCHAR (100  )
 ,@new_lod_id int 
 ,@lod_created_by int

  /* NOT USED IF NOT FOLLOWING CURSOR		      

	 
DELETE FROM  FORM348_findings 
,@lod_id int   

OPEN lodid_cursor
			FETCH NEXT FROM lodid_cursor  INTO @lod_id
      		WHILE @@FETCH_STATUS = 0
      		
BEGIN --BEGIN CURSOR
    */ 		 
   SELECT 
   @lod_APPOINT_AUTH_PERS_ID		 =lod.APPOINT_AUTH_PERS_ID
  ,@lod_APPOINT_AUTH_DECISION		 =lod.APPOINT_AUTH_DECISION
  ,@lod_APPOINT_AUTH_EXPLANATION		 =lod.APPOINT_AUTH_EXPLANATION
  ,@lod_APPOINT_AUTH_NAME		 =lod.APPOINT_AUTH_NAME
  ,@lod_APPOINT_AUTH_GRADE		 =lod.APPOINT_AUTH_GRADE
  ,@lod_APPOINT_AUTH_UNIT		 =lod.APPOINT_AUTH_UNIT
  ,@lod_WING_JA_PERS_ID		 =lod.WING_JA_PERS_ID
  ,@lod_WING_JA_DECISION		 =lod.WING_JA_DECISION
  ,@lod_WING_JA_EXPLANATION		 =lod.WING_JA_EXPLANATION
  ,@lod_CMDR_PERS_ID		 =lod.CMDR_PERS_ID
  ,@lod_CMDR_REC_FINDINGS		 =lod.CMDR_REC_FINDINGS
  ,@lod_CMDR_EXPLANATION		 =lod.CMDR_EXPLANATION
  ,@lod_CMDR_NAME		 =lod.CMDR_NAME
  ,@lod_CMDR_GRADE		 =lod.CMDR_GRADE
  ,@lod_CMDR_UNIT		 =lod.CMDR_UNIT
  ,@lod_HQSG_PERS_ID		 =lod.HQSG_PERS_ID
  ,@lod_HQSG_DECISION		 =lod.HQSG_DECISION
  ,@lod_HQSG_EXPLANATION		 =lod.HQSG_EXPLANATION
  ,@lod_HQSG_NAME		 =lod.HQSG_NAME
  ,@lod_HQSG_GRADE		 =lod.HQSG_GRADE
  ,@lod_HQJA_PERS_ID		 =lod.HQJA_PERS_ID
  ,@lod_HQJA_DECISION		 =lod.HQJA_DECISION
  ,@lod_HQJA_EXPLANATION		 =lod.HQJA_EXPLANATION
  ,@lod_HQJA_NAME		 =lod.HQJA_NAME
  ,@lod_HQJA_GRADE		 =lod.HQJA_GRADE
  ,@lod_HQBOARD_PERS_ID		 =lod.HQBOARD_PERS_ID
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
  ,@lod_HQSR_PERS_ID		 =lod.HQSR_PERS_ID
  ,@lod_HQSR_DECISION		 =lod.HQSR_DECISION
  ,@lod_HQSR_EXPLANATION		 =lod.HQSR_EXPLANATION
  ,@lod_HQSR_NAME		 =lod.HQSR_NAME
  ,@lod_HQSR_GRADE		 =lod.HQSR_GRADE
  ,@inv_INV_OFFICER_PERS_ID		 =inv.INV_OFFICER_PERS_ID
  ,@lod_INV_OFFICER_BRANCH		 =lod.INV_OFFICER_BRANCH
  ,@lod_INV_OFFICER_SSN		 =lod.INV_OFFICER_SSN
  ,@lod_INV_OFFICER_NAME		 =lod.INV_OFFICER_NAME
  ,@lod_INV_OFFICER_GRADE		 =lod.INV_OFFICER_GRADE
  ,@lod_INV_OFFICER_UNIT		 =lod.INV_OFFICER_UNIT
  ,@lod_MED_TECH_PERS_ID		 =lod.MED_TECH_PERS_ID
  ,@lod_MEDTECH_NAME		 =lod.MEDTECH_NAME
  ,@lod_MEDTECH_GRADE		 =lod.MEDTECH_GRADE
  ,@lod_MEDTECH_UNIT		 =lod.MEDTECH_UNIT 
  ,@lod_MPF_PERS_ID		 =lod.MPF_PERS_ID
  ,@lod_MPF_NAME		 =lod.MPF_NAME
  ,@lod_MPF_GRADE		 =lod.MPF_GRADE
  ,@lod_MPF_UNIT		 =lod.MPF_UNIT
  ,@lod_PHYSICIAN_PERS_ID		 =lod.PHYSICIAN_PERS_ID
  ,@lod_PHYSICIAN_NAME		 =lod.PHYSICIAN_NAME
  ,@lod_PHYSICIAN_GRADE		 =lod.PHYSICIAN_GRADE
  ,@lod_PHYSICIAN_DECISION		 =lod.PHYSICIAN_DECISION  
  ,@inv_io_remarks 			 =inv.REMARKS 
   ,@inv_io_findings  	 =inv. FINDINGS 
 ,@inv_HQAA_PERS_ID 		 =inv.HQAA_PERS_ID 
 ,@inv_FINAL_APPROVAL_FINDINGS		 =inv.FINAL_APPROVAL_FINDINGS
 ,@inv_HQAA_REASONS		 =inv.HQAA_REASONS
 ,@inv_HQAA_NAME		 =inv.HQAA_NAME
 ,@inv_HQAA_GRADE		 =inv.HQAA_GRADE
 ,@inv_HQAA_UNIT		 =inv.HQAA_UNIT
 ,@inv_HQJA_PERS_ID		 =inv.HQJA_PERS_ID
 ,@inv_HQJA_FINDINGS		 =inv.HQJA_FINDINGS
 ,@inv_HQJA_REASONS		 =inv.HQJA_REASONS
 ,@inv_HQJA_NAME		 =inv.HQJA_NAME
 ,@inv_HQJA_GRADE		 =inv.HQJA_GRADE
 ,@inv_HQJA_UNIT		 =inv.HQJA_UNIT
 ,@inv_HQSG_PERS_ID  		 =inv.HQSG_PERS_ID  
 ,@inv_HQSG_FINDINGS		 =inv.HQSG_FINDINGS
 ,@inv_HQSG_REASONS		 =inv.HQSG_REASONS
 ,@inv_HQSG_NAME		 =inv.HQSG_NAME
 ,@inv_HQSG_GRADE		 =inv.HQSG_GRADE
 ,@inv_HQSG_UNIT		 =inv.HQSG_UNIT
 ,@inv_HQSR_PERS_ID		 =inv.HQSR_PERS_ID
 ,@inv_HQSR_FINDINGS		 =inv.HQSR_FINDINGS
 ,@inv_HQSR_REASONS		 =inv.HQSR_REASONS
 ,@inv_HQSR_NAME		 =inv.HQSR_NAME
 ,@inv_HQSR_GRADE		 =inv.HQSR_GRADE
 ,@inv_HQSR_UNIT		 =inv.HQSR_UNIT
 ,@inv_RA_PERS_ID		 =inv.RA_PERS_ID
 ,@inv_RA_REASONS		 =inv.RA_REASONS
 ,@inv_RA_NAME		 =inv.RA_NAME
 ,@inv_RA_GRADE		 =inv.RA_GRADE
 ,@inv_RA_UNIT		 =inv.RA_UNIT
 ,@inv_RA_DATE		 =inv.RA_DATE
 ,@inv_WING_AA_PERS_ID		 =inv.WING_AA_PERS_ID
 ,@inv_WING_AA_APPROVED_YN		 =inv.WING_AA_APPROVED_YN
 ,@inv_WING_AA_FINDINGS		 =inv.WING_AA_FINDINGS
 ,@inv_WING_AA_REASONS		 =inv.WING_AA_REASONS
 ,@inv_WING_AA_NAME		 =inv.WING_AA_NAME
 ,@inv_WING_AA_GRADE		 =inv.WING_AA_GRADE
 ,@inv_WING_AA_UNIT		 =inv.WING_AA_UNIT
 ,@inv_WING_JA_PERS_ID		 =inv.WING_JA_PERS_ID
 ,@inv_WING_JA_APPROVED_YN		 =inv.WING_JA_APPROVED_YN
 ,@inv_WING_JA_REASONS		 =inv.WING_JA_REASONS
 ,@inv_WING_JA_NAME		 =inv.WING_JA_NAME
 ,@inv_WING_JA_GRADE		 =inv.WING_JA_GRADE
 ,@inv_WING_JA_UNIT		 =inv.WING_JA_UNIT
 ,@lod_modified_date= lod.modified_date 
 ,@inv_modified_date= inv.modified_date  
 
 
 FROM imp_lod_dispositions   lod
 left join  imp_LOD_INVESTIGATION_RPTS   inv 
 on cast(inv.lod_id AS INT)=CAST(lod.lod_id   AS int) 
 where cast(lod.lod_id as INT) = @lod_id  
			
 
 set @new_lod_id=(select alod_lod_id from imp_LODMAPPING  where rcpha_lodid=@lod_id)
 
 IF(  @new_lod_id is not null)
 
 BEGIN  
	set @lod_created_by = (select created_by from Form348 where lodId = @new_lod_id)
	
 --Appointing Authority 
			SET @lod_PERS_ID		=cast(@lod_APPOINT_AUTH_PERS_ID as int)
			SET @lod_DECISION	=@lod_APPOINT_AUTH_DECISION 
			SET @lod_EXPLANATION	=@lod_APPOINT_AUTH_EXPLANATION 
			SET @lod_NAME		=@lod_APPOINT_AUTH_NAME 
			SET @lod_RANK		=@lod_APPOINT_AUTH_GRADE 
			SET @lod_UNIT		=@lod_APPOINT_AUTH_UNIT 
			
			  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN APPOINT AUTHORITY
			  	
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @person_type  =5
					
					
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						  
								 
				END -- END  APPOINT AUTHORITY
			
		
			SET @lod_PERS_ID		=cast(@lod_WING_JA_PERS_ID  as int)
			SET @lod_DECISION	=@lod_WING_JA_DECISION 
			SET @lod_EXPLANATION	=@lod_WING_JA_EXPLANATION 
			 
			
			  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN WING JA
			  	
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @lod_RANK=NULL
						SET @lod_UNIT=NULL
						SET @lod_NAME=NULL
						SET @person_type  =4
					
					
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						  
								 
				END -- END  WING JA

			SET @lod_PERS_ID		=cast(@lod_CMDR_PERS_ID as int)
			SET @lod_DECISION	=@lod_CMDR_REC_FINDINGS 
			SET @lod_EXPLANATION	=@lod_CMDR_EXPLANATION
			SET @lod_NAME		=@lod_CMDR_NAME
			SET @lod_RANK		=@lod_CMDR_GRADE
			SET @lod_UNIT		=@lod_CMDR_UNIT 
			
	 
			
			  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN UNIT COMMANDER  
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
					 	SET @person_type  =3 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
							SET @lod_SIGNATURE =null
							SET @lod_BRANCH =null
						   
				END -- END UNIT COMMANDER
				   
				    SET @lod_PERS_ID		=cast(@lod_HQSG_PERS_ID as int)
					SET @lod_DECISION	=@lod_HQSG_DECISION
					SET @lod_EXPLANATION	=@lod_HQSG_EXPLANATION
					SET @lod_NAME		=@lod_HQSG_NAME
					SET @lod_RANK		=@lod_HQSG_GRADE
				 
			
	 
			
			   IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN BORAD MED  
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @lod_UNIT=NULL
					 	SET @person_type  =8 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END BORAD MED  
				   
				   	SET @lod_PERS_ID		=cast(@lod_HQJA_PERS_ID as int)
					SET @lod_DECISION	=@lod_HQJA_DECISION
					SET @lod_EXPLANATION	=@lod_HQJA_EXPLANATION
					SET @lod_NAME		=@lod_HQJA_NAME
					SET @lod_RANK		=@lod_HQJA_GRADE
				  
			   IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN BORAD LEGAL  
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @lod_UNIT=NULL
					 	SET @person_type  =7 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END BORAD LEGAL  				   
			 SET @lod_PERS_ID		=cast(@lod_HQBOARD_PERS_ID as int)
					SET @lod_DECISION	=@lod_HQBOARD_DECISION
					SET @lod_EXPLANATION	=@lod_HQBOARD_EXPLANATION
					SET @lod_NAME		=@lod_HQBOARD_NAME
					SET @lod_RANK		=@lod_HQBOARD_GRADE
				  
			   IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN BORAD TECH  
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @lod_UNIT=NULL
					 	SET @person_type  =6
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END BORAD TECH  				   
				   
	 	         SET @lod_PERS_ID		=cast(@lod_HQAA_PERS_ID as int)
					SET @lod_DECISION	=@lod_HQAA_DECISION
					SET @lod_EXPLANATION	=@lod_HQAA_EXPLANATION
					SET @lod_NAME		=@lod_HQAA_NAME
					SET @lod_RANK		=@lod_HQAA_RANK
				 
					
				  
			   IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN BORAD AA
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @lod_UNIT=NULL
					 	SET @person_type  =10
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END BORAD AA
	 
	 
	   SET @lod_PERS_ID		=cast(@lod_MED_TECH_PERS_ID as int)
					SET @lod_DECISION	=NULL
					SET @lod_EXPLANATION	=NULL
					SET @lod_NAME		=@lod_MEDTECH_NAME
					SET @lod_RANK		=@lod_MEDTECH_GRADE
					SET @lod_UNIT =@lod_MEDTECH_UNIT  
				   
				   
			  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN MEDTECH
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @person_type  =1 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END MEDTECH
			 SET @lod_PERS_ID		=cast(@lod_PHYSICIAN_PERS_ID as int)
					SET @lod_DECISION	=@lod_PHYSICIAN_DECISION
					SET @lod_EXPLANATION	=NULL
					SET @lod_NAME		=@lod_PHYSICIAN_NAME
					SET @lod_RANK		=@lod_PHYSICIAN_GRADE
			 	   
				   
			  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN MED OFF
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @lod_UNIT =NULL
						SET @person_type  =2 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END OFF
			    SET @lod_PERS_ID		=cast(@lod_MPF_PERS_ID as int)
					SET @lod_DECISION	=NULL
					SET @lod_EXPLANATION	=NULL
					SET @lod_NAME		=@lod_MPF_NAME
					SET @lod_RANK		=@lod_MPF_GRADE
					SET @lod_UNIT =@lod_MPF_UNIT  
				   
				   
			  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN MPF
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @person_type  =11 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END MPF
	   
				    SET @lod_PERS_ID		=cast(@inv_INV_OFFICER_PERS_ID as int)
					SET @lod_DECISION	=@inv_io_findings 
					SET @lod_EXPLANATION	=@inv_io_remarks
					SET @lod_NAME		=@lod_INV_OFFICER_NAME
					SET @lod_RANK		=@lod_INV_OFFICER_GRADE
				    SET @lod_UNIT=@lod_INV_OFFICER_UNIT
 
		  --,@lod_INV_OFFICER_BRANCH		 =lod.INV_OFFICER_BRANCH
		 -- ,@lod_INV_OFFICER_SSN		 =lod.INV_OFFICER_SSN
		 
   
			   IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN IO 
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @person_type  =19
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@inv_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END IO 
				
				
				
					SET @lod_PERS_ID		=cast(@inv_HQAA_PERS_ID as int)
					SET @lod_DECISION	=@inv_FINAL_APPROVAL_FINDINGS
					SET @lod_EXPLANATION	=@inv_HQAA_REASONS
					SET @lod_NAME		=@inv_HQAA_NAME
					SET @lod_RANK		=@inv_HQAA_GRADE
					SET @lod_UNIT =@inv_HQAA_UNIT
				  
				  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN FORMAL HQAA
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL					 
						SET @person_type  =18 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END HQAA
				
				SET @lod_PERS_ID		=cast(@inv_HQJA_PERS_ID as int)
					SET @lod_DECISION	=@inv_HQJA_FINDINGS
					SET @lod_EXPLANATION	=@inv_HQJA_REASONS
					SET @lod_NAME		=@inv_HQJA_NAME
					SET @lod_RANK		=@inv_HQJA_GRADE
					SET @lod_UNIT =@inv_HQJA_UNIT
				  
				  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN FORMAL HQJA
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						 
						SET @person_type  =15 
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END HQJA	
			SET @lod_PERS_ID		=cast(@inv_HQSG_PERS_ID as int)
					SET @lod_DECISION	=@inv_HQSG_FINDINGS
					SET @lod_EXPLANATION	=@inv_HQSG_REASONS
					SET @lod_NAME		=@inv_HQSG_NAME
					SET @lod_RANK		=@inv_HQSG_GRADE
					SET @lod_UNIT =@inv_HQSG_UNIT
				  
				  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN FORMAL HQSG
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						SET @person_type  =16
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END HQSG	
				
				SET @lod_PERS_ID		=cast(@inv_RA_PERS_ID as int)
					SET @lod_DECISION	=NULL
					SET @lod_EXPLANATION	=@inv_RA_REASONS
					SET @lod_NAME		=@inv_RA_NAME
					SET @lod_RANK		=@inv_RA_GRADE
					SET @lod_UNIT =@inv_RA_UNIT
					--SET @inv_DATE=@inv_RA_DATE
				  --
				  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN FORMAL HRA
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
				 
						SET @person_type  =14
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END RA
				
				SET @lod_APPROVED_YN=NULL
				
				    SET @lod_PERS_ID		=cast(@inv_WING_AA_PERS_ID as int)
			 		SET @lod_DECISION	=@inv_WING_AA_FINDINGS
					SET @lod_EXPLANATION	=@inv_WING_AA_REASONS
					SET @lod_NAME		=@inv_WING_AA_NAME
					SET @lod_RANK		=@inv_WING_AA_GRADE
					SET @lod_UNIT =@inv_WING_AA_UNIT
					SET @lod_APPROVED_YN	=@inv_WING_AA_APPROVED_YN
					--SET @inv_DATE=@inv_RA_DATE
				  --
				  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN FORMAL APPAUTH
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
					 	SET @person_type  =13
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END FORMAL APP AUTH
				
				
					SET @lod_APPROVED_YN=NULL
				   SET @lod_PERS_ID		=cast(@inv_WING_JA_PERS_ID as int)
					SET @lod_DECISION	=NULL
	 				SET @lod_EXPLANATION	=@inv_WING_JA_REASONS
					SET @lod_NAME		=@inv_WING_JA_NAME
					SET @lod_RANK		=@inv_WING_JA_GRADE
					SET @lod_UNIT =@inv_WING_JA_UNIT
					SET @lod_APPROVED_YN	=@inv_WING_JA_APPROVED_YN
					--SET @inv_DATE=@inv_RA_DATE
				  
				  IF @lod_PERS_ID is not null and  @lod_PERS_ID<>0
			  
				BEGIN -- BEGIN FORMAL WING JA 
						---SET PERSON TYPES 
						SET @lod_GRADE=NULL
						 
						SET @person_type  =12
						EXECUTE imp_InsertFindingRecord @new_lod_id,@person_type,@lod_PERS_ID,@lod_created_by,@lod_DECISION
											,@lod_EXPLANATION,@lod_NAME,@lod_RANK,
											@lod_GRADE,@lod_UNIT,@lod_modified_date
			 
			  				--SET NULL FOR NEXT PERSON 
							SET @lod_PERS_ID=null
							SET @lod_DECISION=null
							SET @lod_EXPLANATION =null     
							SET @lod_NAME  =null
							SET @lod_GRADE  =null
							SET @lod_RANK  =null						 
							SET @lod_UNIT =null     
						 
						   
				END -- END FORMAL WING JA
		END 		
	ELSE
	BEGIN 
  						 
				 			EXECUTE imp_InsertErrorRecord 0 
							,'FORM348_RECORDS','imp_UpdatePersonTypesFindings ','error creating form248_findings record',@lod_id,'MAPPINGERROR'
		
 
 END 	
	
  /* NOT USED IF NOT FOLLOWING CURSOR		      
  FETCH NEXT FROM lodid_cursor  INTO @lod_id
		
	END ---End Cursor
 	  CLOSE lodid_cursor
	  DEALLOCATE  lodid_cursor	 */ 
 
 END --END STORED PROC
GO

