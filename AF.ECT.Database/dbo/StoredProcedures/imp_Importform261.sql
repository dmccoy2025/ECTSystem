--delete from form261
CREATE PROCEDURE [dbo].[imp_Importform261]
( @lod_id int) 
 
AS

BEGIN 
	--DECLARE-----------------------
	---OTHER PERSONNELS------------- 
    DECLARE @LIR_id int  
	DECLARE @otherPerson varchar(4000)
	 ,@gradeString varchar(20)		
	 ,@oName  varchar(100)
	 ,@oSSN  varchar(100)
	 ,@oGrade  varchar(100)
	 ,@lodInvY_N  varchar(10) 
	,@investigationMade varchar(20)   
  ---------------------------------
   -------IO AND AA SIG INFO---------    
     DECLARE
	  @aaUserId int
	  , @aaPersId int
	 ,@ioUserId int
	 , @ioPersId int
	 ,@aaName  varchar(100)
	,@aaSSN  varchar(50)
	,@aaUnitDescription  varchar(350)
	,@aaGrade varchar(20)
	,@ioName  varchar(100)
 	,@ioSSN  varchar(50)
 	,@ioUnitDescription  varchar(350)
 	,@ioGrade varchar(20)
	,@aaSigInfo varchar(4000)
 	,@ioSigInfo varchar(4000)
 ---------------------------------
  
 -----------
 DECLARE @new_lod_id   int 
 set @new_lod_id=(select alod_lod_id from imp_LODMAPPING  where rcpha_lodid=@lod_id)
 

 IF(  @new_lod_id is not null)
 BEGIN 
		
		 /*Not fetched the following columns
  		sig_date_io 
		sig_info_io
	 	sig_info_appointing		
		Assumed 
		findings date  ==circumstances time (LOD_INVESTIGATION_RPTS) 
  		*/
	     BEGIN TRY --BEGIN TRY BLOCK 
	        --BEGIN TRANSACTION BLOCK 
		UPDATE FORM348 SET formal_inv=1 WHERE lodId =@new_lod_id
	   
	     INSERT INTO Form261
	     (
	     lodId
	    ,reportDate
	    ,investigationOf 
		,status
		,inactiveDutyTraining
		,durationStartDate
		,durationFinishDate
		,IoUserId
	    ,final_approval_findings
		,findingsDate
		,place
		,howSustained
		,medicalDiagnosis
		,presentForDuty
		,absentWithAuthority
		,intentionalMisconduct
		,mentallySound
		,remarks
		,modified_date 
		,modified_by )
		SELECT 
		 @new_lod_id
		,CASE 
			WHEN  REPORT_DATE  is null or REPORT_DATE ='' THEN  null
			ELSE CAST( SUBSTRING( REPORT_DATE ,1,19)  AS DATETIME)
			END
		,(SELECT ID FROM core_lkupIncidentType where incidentType=INVESTIGATION_OF)
		,CASE STATUS
		 WHEN 'Regular or EAD' Then 1
		 WHEN 'More than 30' Then 2
		 WHEN '30 Days or Less' Then 3 
		 WHEN 'Inactive Duty Training' Then 4
		 WHEN 'Short Tour of Active Duty For Training' Then 5
		 ELSE NULL 
		 END 
		 ,STATUS_INACTIVE_TYPE
		 ,CASE 
			WHEN  STATUS_DURATION_START  is null or STATUS_DURATION_START ='' THEN  null
			ELSE CAST( SUBSTRING( STATUS_DURATION_START ,1,19)  AS DATETIME)
			END
		 
		 ,CASE 
			WHEN  STATUS_DURATION_FINISH  is null or STATUS_DURATION_FINISH ='' THEN  null
			ELSE CAST( SUBSTRING( STATUS_DURATION_FINISH ,1,19)  AS DATETIME)
			END
 
		 ,(SELECT USERID FROM imp_USERMAPPING WHERE cast( PERSON_ID as int)=  cast(INV_OFFICER_PERS_ID as int) )
		 ,(SELECT ID FROM core_lkUpFindings WHERE findingType=final_approval_findings)
		 ,CASE 
			WHEN  CIRCUMSTANCES_TIME  is null or CIRCUMSTANCES_TIME ='' THEN  null
			ELSE CAST( SUBSTRING( CIRCUMSTANCES_TIME ,1,19)  AS DATETIME)
			END
		 	,CIRCUMSTANCES_PLACE
			,SUSTAINED_DESC		
			,DIAGNOSIS			
			,CASE PRESENT_FOR_DUTY_YN
			WHEN 'Y' THEN 1
			WHEN 'N' THEN 0
			ELSE NULL
			END
			,CASE ABSENT_WITH_AUTH_YN
			WHEN 'Y' THEN 1
			WHEN 'N' THEN 0
			ELSE NULL
			END
			,CASE INTENTIONAL_MISCONDUCT_YN
			WHEN 'Y' THEN 1
			WHEN 'N' THEN 0
			ELSE NULL
			END
			,CASE MENTALLY_SOUND_YN
			WHEN 'Y' THEN 1
			WHEN 'N' THEN 0
			ELSE NULL
			END
			,REMARKS			 
			 ,CAST( CREATED_DATE as datetime)
		    ,(SELECT USERID FROM imp_USERMAPPING WHERE USERNAME=CREATED_BY)
			 FROM imp_lod_investigation_rpts where LOD_ID=@lod_id  	 
	  
	
			------Update form261 SignatureInfo 
			SET  @aaUserId =NULL
			SET @ioUserId =NULL
			SET @aaName  =NULL
			SET  @aaSSN  =NULL
			SET  @aaUnitDescription=NULL
			SET  @aaGrade=NULL
			SET  @ioName  =NULL
 			SET  @ioSSN  =NULL
 			SET  @ioUnitDescription  =NULL
 			SET  @ioGrade =NULL
			SET  @aaSigInfo =NULL
 			SET  @ioSigInfo =NULL
		 	 
		 	SET @aaPersId=(SELECT CAST( WING_AA_PERS_ID AS INT)    FROM imp_lod_investigation_rpts where LOD_ID=@lod_id  )
		 	SET @ioPersId=(SELECT CAST(INV_OFFICER_PERS_ID AS INT)     FROM imp_lod_investigation_rpts where LOD_ID=@lod_id  )
		 	
		 
				SET @aaUserId=( SELECT USERID FROM imp_USERMAPPING  WHERE cast( PERSON_ID as int)= @aaPersId ) 
			 
				SET @ioUserId=( SELECT USERID FROM imp_USERMAPPING WHERE cast( PERSON_ID as int)= @ioPersId  ) 
		 
			SELECT 
			 @aaName=CASE
						WHEN WING_AA_NAME  is null or WING_AA_NAME='' THEN ( SELECT  ISNULL(LastName+'  ' +FirstName,'')  FROM vw_users where USERID=@aaUserId)
						ELSE WING_AA_NAME
					 END
			 ,@aaSSN=(SELECT  ISNULL(SSN,'')FROM vw_users where USERID=@aaUserId)
			,@aaUnitDescription=CASE
								WHEN WING_AA_UNIT  is null or WING_AA_UNIT='' THEN ( SELECT  ISNULL(unit_description,'') FROM vw_users where USERID=@aaUserId)
								ELSE WING_AA_UNIT
								END
			 ,@aaGrade=		CASE
								WHEN WING_AA_GRADE  is null or WING_AA_GRADE='' THEN ( SELECT  ISNULL(RANK,'') FROM vw_users where USERID=@aaUserId)
								ELSE (SELECT RANK FROM core_lkupGrade WHERE CODE=(SELECT TOP 1 GRADECODE FROM imp_gradeLookUp WHERE GRADE=WING_AA_GRADE ))  
 		  						END
			 
				 FROM imp_lod_investigation_rpts    WHERE CAST(LOD_ID AS INT)= @lod_id      
		 	 
 		 
 				SET @aaSigInfo= '<PersonnelData>'
				SET @aaSigInfo=@aaSigInfo+'<Name>'+IsNull(@aaName,'')+'</Name>'
				SET @aaSigInfo=@aaSigInfo+'<SSN>'+IsNull(@aaSSN,'')+'</SSN>'
				SET @aaSigInfo=@aaSigInfo+'<Grade>'+IsNull(@aaGrade,'')+'</Grade>'
				SET @aaSigInfo=@aaSigInfo+'<InvestigationMade>false</InvestigationMade>'
				SET @aaSigInfo=@aaSigInfo+'<Compo></Compo>'
				SET @aaSigInfo=@aaSigInfo+'<PasCode></PasCode>'
				SET @aaSigInfo=@aaSigInfo+'<PasCodeDescription>'+IsNull(@aaUnitDescription,'')+'</PasCodeDescription>'
				SET @aaSigInfo=@aaSigInfo+'<Branch></Branch>'
				SET @aaSigInfo=@aaSigInfo+'</PersonnelData>'

			SELECT 
			 @ioName=( SELECT  ISNULL(LastName+'  ' +FirstName,'')  FROM vw_users where USERID=@ioUserId)
			,@ioSSN=	(SELECT  ISNULL(SSN,'') FROM vw_users where USERID=@ioUserId)
			,@ioUnitDescription=( SELECT  ISNULL(unit_description,'') from vw_users where USERID=@ioUserId)
 			,@ioGrade=(SELECT RANK FROM vw_users where USERID=@ioUserId)
 			FROM imp_lod_investigation_rpts WHERE CAST(LOD_ID AS INT)= @lod_id      

 				SET @ioSigInfo= '<PersonnelData>'
				SET @ioSigInfo=@ioSigInfo+'<Name>'+IsNull(@ioName,'')+'</Name>'
				SET @ioSigInfo=@ioSigInfo+'<SSN>'+IsNull(@ioSSN,'')+'</SSN>'
				SET @ioSigInfo=@ioSigInfo+'<Grade>'+IsNull(@ioGrade,'')+'</Grade>'
				SET @ioSigInfo=@ioSigInfo+'<InvestigationMade>false</InvestigationMade>'
				SET @ioSigInfo=@ioSigInfo+'<Compo></Compo>'
				SET @ioSigInfo=@ioSigInfo+'<PasCode></PasCode>'
				SET @ioSigInfo=@ioSigInfo+'<PasCodeDescription>'+IsNull(@ioUnitDescription,'')+'</PasCodeDescription>'
				SET @ioSigInfo=@ioSigInfo+'<Branch></Branch>'
				SET @ioSigInfo=@ioSigInfo+'</PersonnelData>'
			
			  
				UPDATE Form261
				SET 
			 
				 sig_info_io=cast(@ioSigInfo as xml)
			 
				,sig_info_appointing=cast(@aaSigInfo as xml)
				WHERE lodid=@new_lod_id 
				 
			  
		END TRY 
		
			--END TRY BLOCK  	
			BEGIN CATCH --BEGIN CATCH BLOCK 
		 			 
							DECLARE  @msg varchar(2000)
							 DECLARE @number int ,@errmsg varchar(2000)
							SELECT 
							  @number= ERROR_NUMBER()  
							 ,@errmsg= ERROR_MESSAGE() 
				 			EXECUTE imp_InsertErrorRecord @number 
							,'FORM261_DATA','imp_Importform261 ','error in FORM261 record',@lod_id,@errmsg
			 				    
			 END CATCH---END CATCH BLOCK                        
				 
	
END 	
 
 ELSE 
 
 BEGIN 
  						 
				 			EXECUTE imp_InsertErrorRecord 0 
							,'FORM261_DATA','imp_Importform261 ','error in FORM261 record',@lod_id,'MAPPINGERROR'
		
 
 END 


END
GO

