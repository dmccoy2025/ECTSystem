CREATE PROCEDURE [dbo].[imp_InsertFindingRecord] 
(
	-- Add the parameters for the function here
	  
	   @new_lod_id int 
	  ,@pType int
	  ,@lod_PERS_ID	int 	 
	  ,@lod_CreatedBy int
	  ,@lod_DECISION	varchar(2000)
	  ,@lod_EXPLANATION varchar(max)
	  ,@lod_NAME varchar(200)		 
	  ,@lod_RANK	 varchar(200)	 
	  ,@lod_GRADE varchar(200)
	  ,@lod_UNIT	 varchar(200)	 	 
	  ,@lod_modified_date datetime
	  ,@lod_APPROVED_YN  varchar(5)= NULL
)
 
AS
BEGIN
  DECLARE 
		  @userId int
		 ,@SSN VARCHAR (100  )
		 ,@NAME VARCHAR (200  )
		 ,@UNIT VARCHAR (500  )
		 ,@PAS_CODE VARCHAR (100  )
		 ,@RANK VARCHAR (100  )
		 ,@GRADE VARCHAR (100  )
		 ,@concury_n  VARCHAR (100  )     
		 ,@findings int 
		 ,@comments   VARCHAR (4000  )    

	 		SET @userId =(SELECT USERID FROM  imp_USERMAPPING WHERE CAST(PERSON_ID AS INT) =@lod_PERS_ID)--@lod_PERS_ID)
			SELECT @NAME=isnull(FIRSTNAME,'')+ ' '+ isnull(LASTNAME,'') , @UNIT =unit_description,@SSN=  SSN,@RANK=  RANK,@GRADE=GRADE, @PAS_CODE=PAS_CODE FROM VW_USERS  WHERE USERID=@userId --@lod_PERS_ID)

			if (@userId is null)
				set @userId = @lod_CreatedBy;
	 	
			SET @concury_n = CASE    @lod_DECISION  
						  WHEN 'CONCUR' THEN 'Y'
						  WHEN 'NONCONCUR' THEN 'N'
						  ELSE NULL 
			END   
			SET @findings =CASE    @lod_DECISION  
									  WHEN 'LINE_OF_DUTY' THEN 1
									  WHEN 'EPTS_NOT_LOD' THEN 2
									  WHEN 'EPTS_SERVICE' THEN 3
									  WHEN 'NOT_LOD_MISCONDUCT' THEN 4
									  WHEN 'NOT_LOD_NOT_MISCONDUCT' THEN 5
									  WHEN 'FORMAL_INV' THEN 6
									  ELSE NULL
									END   
			  SET @comments =@lod_EXPLANATION  
				
			 SET @NAME= CASE 
							 WHEN @lod_NAME IS NULL OR  @lod_NAME=''  THEN @NAME
							 ELSE @lod_NAME
						  END  
			
			  SET @UNIT= CASE 
							 WHEN @lod_UNIT IS NULL OR  @lod_UNIT=''  THEN @UNIT
							 ELSE @lod_UNIT
						  END  				
						  
			  SET @GRADE= CASE 
							 WHEN @lod_GRADE IS NULL OR  @lod_GRADE=''  THEN @GRADE
							 ELSE @lod_GRADE
						  END  
									  
			  SET @RANK= CASE 
							 WHEN @lod_RANK IS NULL OR  @lod_RANK='' THEN @RANK
							 ELSE @lod_RANK
						  END 
				
				IF @pType in(13,12) 
				--Formal wing aa and ja there is extra field  WING_JA_APPROVED_YN and WING_AA_APPROVED_YN
					BEGIN  
						SET @concury_n = @lod_APPROVED_YN   
					END
			 		
						  
		---Insert Values --------------------------------	
		BEGIN	TRY 		  
						  INSERT INTO FORM348_findings
							(
								   LODID,ptype,name,ssn
								  ,grade,compo,rank,pascode
								  ,finding ,decision_yn,explanation
								  ,created_by,created_date
								  ,modified_by,modified_date 
							)
							Values
							(
							  @new_lod_id,@pType,@NAME,@ssn
							 ,@GRADE,'6',@RANK,@PAS_CODE
							 ,@findings, @concury_n,@comments
							 ,@userId,@lod_modified_date,@userId,
							  @lod_modified_date
							)
			END TRY 	
			BEGIN CATCH 
			
			 DECLARE @rcpha_lod_id   int 
			 DECLARE @msg varchar(20)
			  DECLARE @number int ,@errmsg varchar(2000)
							SELECT 
							  @number= ERROR_NUMBER()  
							 ,@errmsg= ERROR_MESSAGE()
			 SET @msg='|PersonType:' + cast(@pType as varchar(10))
			SET @rcpha_lod_id=(select rcpha_lodid from imp_LODMAPPING  where alod_lod_id=@new_lod_id)
 
			
	 		EXECUTE imp_InsertErrorRecord @number 
			,'FORM348_FINDINGS'
			,'imp_UpdatePersonTypesFindings/imp_InsertFindingRecord'
			,@msg,@rcpha_lod_id,@errmsg
	 	 	   
					
		END CATCH 
							
  
END
GO

