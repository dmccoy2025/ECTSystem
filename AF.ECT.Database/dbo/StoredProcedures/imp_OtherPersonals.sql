--delete from form261
CREATE PROCEDURE [dbo].[imp_OtherPersonals]
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
	  	   /*Not fetched the following columns
  		sig_date_io 
		sig_info_io
	 	sig_info_appointing		
		Assumed 
		findings date  ==circumstances time (LOD_INVESTIGATION_RPTS) 
  		*/
	   
	  	   	-----***Form 261 Other Personals *******
  			SET @LIR_id=(SELECT CAST(LIR_id AS INT) FROM imp_lod_investigation_rpts where CAST(LOD_ID AS INT)=@lod_id ) 
			 
		 	--Grade in lod_pers_involved are examples O-6, SSgtwe need to be to get E1 --O1 kind of information 

		
			DECLARE otherPersons_cursor CURSOR FOR  SELECT NAME ,SSN ,GRADE ,LOD_INVESTIGATION_YN
			 FROM imp_LOD_PERS_INVOLVED where CAST(lir_id AS INT)=@LIR_id
			
			OPEN otherPersons_cursor
				
			 	FETCH NEXT FROM otherPersons_cursor  INTO   @oName,@oSSN,@oGrade,@lodInvY_N    
						WHILE @@FETCH_STATUS = 0
						BEGIN 
			 			SET @otherPerson=''
			 			SET @otherPerson=@otherPerson+'<PersonnelData>'
						SET @otherPerson=@otherPerson+'<Name>'+IsNull(@oName,'')+'</Name>'
						SET @otherPerson=@otherPerson+'<SSN>'+IsNull(@oSSN,'')+'</SSN>'
						SET @gradeString=(SELECT TOP 1 GRADE FROM core_lkupGrade WHERE CODE=(SELECT TOP 1 GRADECODE FROM imp_gradeLookUp WHERE GRADE=@oGrade ))
						SET @otherPerson=@otherPerson+'<Grade>'+IsNull(@gradeString,'')+'</Grade>'
						SET @investigationMade=CASE @lodInvY_N 
												   WHEN 'Y' THEN 'true'
												   WHEN 'N' THEN 'false'
												   ELSE NULL
													END
						 
						SET @otherPerson=@otherPerson+'<InvestigationMade>'+IsNull(@investigationMade,'')+'</InvestigationMade>'
					  	 
						SET @otherPerson=@otherPerson+'<Compo></Compo>'
						SET @otherPerson=@otherPerson+'<PasCode></PasCode>'
						SET @otherPerson=@otherPerson+'<PasCodeDescription></PasCodeDescription>'
						SET @otherPerson=@otherPerson+'<Branch></Branch>'
						SET @otherPerson=@otherPerson+'</PersonnelData>'
						
						SET @oName  =NULL
						SET @oSSN  =NULL
						SET @oGrade  =NULL
						SET @lodInvY_N  =NULL
						SET @investigationMade=NULL
						SET @gradeString=NULL
						 
						FETCH NEXT FROM otherPersons_cursor  INTO   @oName,@oSSN,@oGrade,@lodInvY_N    
			 
						
				END ---End of OTHER_PERSONS_CURSOR 
				
				CLOSE otherPersons_cursor
				DEALLOCATE  otherPersons_cursor
		 
			 SET @otherPerson=	 '<ArrayOfPersonnelData>' + @otherPerson+ 	'</ArrayOfPersonnelData>'
			 
	 
		 	UPDATE Form261 SET otherPersonnels=cast(@otherPerson as xml) where lodid=@new_lod_id 
			 
	 	    
		
		END TRY 
		
			--END TRY BLOCK  	
			BEGIN CATCH --BEGIN CATCH BLOCK 
		 	 
							DECLARE @msg varchar(2000)
						  DECLARE @number int ,@errmsg varchar(2000)
							SELECT 
							  @number= ERROR_NUMBER()  
							 ,@errmsg= ERROR_MESSAGE()
				 			EXECUTE imp_InsertErrorRecord @number 
							,'FORM261OTHERPERSON_DATA','imp_OtherPersonals ','error in other personal records',@lod_id,@errmsg
			 				    
			 END CATCH---END CATCH BLOCK                        
				 
	
	
 
 END 
ELSE 
		BEGIN 
  						
  						EXECUTE imp_InsertErrorRecord 0 
							,'FORM261OTHERPERSON_DATA','imp_OtherPersonals ','error in other personal records',@lod_id,'MAPPINGERROR'
			 			
  							 
				 		 
 
		END 
END
GO

