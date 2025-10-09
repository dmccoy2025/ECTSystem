

---Modified Date Oct7 2010--Remove functionality to allow  the open lod unit's to be updated

 
CREATE PROCEDURE [dbo].[memberData_Import] 
AS 
BEGIN 
  
    
    DECLARE @sysAdmin as varchar(100) ,@sysAdminId int
	   
    SET @sysAdmin=ISNULL((select top 1 username  from vw_users where groupId=1 and accessStatus=3),'SysAdmin')
    SET @sysAdminId=ISNULL ((select top 1 userId  from vw_users where groupId=1 and accessStatus=3),1)
 
 
--All general pascodes start with a g instead of an f. These units report to !!MA pascode.
-- All chaplain pascodes start with a c instead of an f. These units report to !!13 pascode.
-- All intel pascodes start with an i instead of an f. These units report to !!06 pascode.
-- All legal pascodes start with a j instead of an f. These units report to !!14 pascode.
-- All medical pascodes start with a m instead of an f. These units report to !!15 pascode.
-- All osi pascodes start with an o instead of an f. These units report to !!05 pascode.

 ---Create the PAFSC codes table 
	DECLARE @PAFSCCode TABLE 
		 (
			  PAFSC varchar(3) 
			 ,FirstLetter varchar(1)
			 ,ReportingUnit varchar(4)
		 )
  Insert into @PAFSCCode
	VALUES 
	 ('GEN'	,'G',   '!!MA')--General
	,('40' ,'M',   '!!15')--Medical
	,('41' ,'M',   '!!15')--Medical
	,('42' ,'M',   '!!15')--Medical
	,('43' ,'M',   '!!15')--Medical
	,('44' ,'M',   '!!15')--Medical
	,('45' ,'M',   '!!15')--Medical
	,('46' ,'M',   '!!15')--Medical
	,('47' ,'M',   '!!15')--Medical
	,('48' ,'M',   '!!15')--Medical
	,('71S' ,'O',   '!!05')--OSI
	,('51J' ,'J',   '!!14')--Legal
	,('52R' ,'C',   '!!13')--Chaplin
	,('14N' ,'I',   '!!06')--Intel
--------------------------------------------------------------------------------------------------------
	 
--Update the state lookups --Local state

	;WITH AllStates_Local (LOCAL_ADDR_STATE) as 
	(
		SELECT DISTINCT LOCAL_ADDR_STATE FROM  MILPDSRawData
	)
    INSERT INTO core_lkupStates (State,State_Name,Country)
		SELECT LOCAL_ADDR_STATE,'Unknown','Unknown'
		 FROM  AllStates_Local WHERE
		 ( LOCAL_ADDR_STATE not in (SELECT State from  core_lkupStates where State is not null)) 
	 
--Update the state lookups --Mailing state

	;WITH AllStates_Mail(DRS_MAIL_DOM_STATE) as 
	(
		SELECT DISTINCT DRS_MAIL_DOM_STATE FROM  MILPDSRawData
	)
	 
    INSERT INTO core_lkupStates (State,State_Name,Country)
		SELECT DRS_MAIL_DOM_STATE,'Unknown','Unknown'
		 FROM  AllStates_Mail WHERE
		 ( DRS_MAIL_DOM_STATE not in (SELECT State from  core_lkupStates where State is not null))
	
    
 -- Update MILPDS Raw data for grade code since for inserting into member data grade code can not be null
 	 UPDATE MILPDSRawData  
			SET GR_CODE=b.code		 
			FROM MILPDSRawData a, core_lkupGrade AS b
			WHERE
			( b.GRADE = CASE   CHARINDEX(')', a.GR_CURR)
                            WHEN  0 THEN  null
						    ELSE SUBSTRING(a.GR_CURR, 2, CHARINDEX(')', a.GR_CURR) - 2) 
                            END                
			 )

			 
			  
 	UPDATE MILPDSRawData  
			SET GR_CODE=99		 
		 		WHERE GR_CODE IS NULL
		 		
		 		 

DECLARE  @sysId int
DECLARE @logId as int
DECLARE @insertedMemberRows as int
DECLARE @deletedMemberRows as int
DECLARE @updatedMemberRows as int
DECLARE @rawRowsInserted as int
DECLARE @addAfterDeletion as int 
DECLARE @modifiedRows as int 
DECLARE @number int ,@errmsg varchar(2000)
DECLARE @record_mod_type  varchar(2)
DECLARE @message as varchar(300)

SET @logId=(SELECT TOP 1 id from pkgLog ORDER  BY id DESC )


--DECLAER CURSOR variables
  DECLARE @SSAN varchar(15),@FIRST_NAME varchar(50)
        ,@MIDDLE_NAMES varchar(60), @LAST_NAME  varchar(50)
        ,@SUFFIX  varchar(30), @SEX_SVC_MBR  varchar(10)
		,@PAS_GAINING varchar(15),@PAS varchar(15), @COMM_DUTY_PHONE varchar(19)
		,@OFFICE_SYMBOL varchar(18), @LOCAL_ADDR_STREET varchar(200)
		,@LOCAL_ADDR_CITY varchar(40),@LOCAL_ADDR_STATE varchar(12)
		,@HOME_PHONE varchar(20),@ADRS_MAIL_DOM_CITY varchar(40)
		,@ADRS_MAIL_DOM_STATE varchar(12), @ADRS_MAIL_ZIP varchar(10)
		,@GR_CODE int
        ,@DY_POSN_NR varchar(19),@DUTY_STATUS varchar(12),@DOB DateTime, @SVC_COMP varchar(11),@LOCAL_ADDR_ZIP varchar(10)
        ,@PAS_NUMBER varchar(14),@PAFSC  varchar(19),@INITIAL_CHARACTER varchar(11),@PAFSC_INITIAL_CODE varchar(13)
        ,@REPORTING_UNIT varchar(16), @ATTACH_PAS varchar(18)
  
---DECLAER member data variables 
  DECLARE @mb_SSAN varchar(11),@mb_FIRST_NAME varchar(50)
        ,@mb_MIDDLE_NAMES varchar(60), @mb_LAST_NAME  varchar(50)
        ,@mb_SUFFIX  varchar(30), @mb_SEX_SVC_MBR  varchar(11)
		,@mb_PAS_GAINING varchar(18),@mb_PAS varchar(18), @mb_COMM_DUTY_PHONE varchar(12)
		,@mb_OFFICE_SYMBOL varchar(18), @mb_LOCAL_ADDR_STREET varchar(200)
		,@mb_LOCAL_ADDR_CITY varchar(40),@mb_LOCAL_ADDR_STATE varchar(12)
		,@mb_HOME_PHONE varchar(20),@mb_ADRS_MAIL_DOM_CITY varchar(40)
		,@mb_ADRS_MAIL_DOM_STATE varchar(12), @mb_ADRS_MAIL_ZIP varchar(10)
		,@mb_GR_CODE int
        ,@mb_DY_POSN_NR varchar(19),@mb_DUTY_STATUS varchar(12),@mb_DOB DateTime , @mb_SVC_COMP varchar(11),@mb_LOCAL_ADDR_ZIP varchar(15)
        ,@mb_PAS_NUMBER varchar(14), @mb_ATTACH_PAS varchar(18)
        
	

	
--------Member_Cursor------- 
DECLARE member_cursor CURSOR FOR   SELECT SSAN FROM  MILPDSRawData      

---Initialize the members 
SET @rawRowsInserted=(SELECT COUNT(*) FROM  MILPDSRawData)   
SET @insertedMemberRows = 0
SET @addAfterDeletion=0
SET @modifiedRows=0



OPEN member_cursor

FETCH NEXT FROM member_cursor  INTO    @SSAN 
                                      
WHILE @@FETCH_STATUS = 0 
BEGIN ---BEGIN CURSOR     
    
    
   BEGIN TRY       
    
   SELECT  @FIRST_NAME=b.FIRST_NAME,@MIDDLE_NAMES=b.MIDDLE_NAMES,@LAST_NAME=b.LAST_NAME
          ,@SUFFIX=b.SUFFIX,@SEX_SVC_MBR= SUBSTRING(b.SEX_SVC_MBR, 1, 1)  
		  ,@PAS_GAINING=b.PAS_GAINING,@PAS=b.PAS,@COMM_DUTY_PHONE=b.COMM_DUTY_PHONE
		  ,@OFFICE_SYMBOL=b.OFFICE_SYMBOL,@LOCAL_ADDR_STREET=b.LOCAL_ADDR_STREET_1ST + '      ' + b.LOCAL_ADDR_STREET_2ND  
		  ,@LOCAL_ADDR_CITY=b.LOCAL_ADDR_CITY,@LOCAL_ADDR_STATE=b.LOCAL_ADDR_STATE 
		  ,@HOME_PHONE=b.HOME_PHONE, @ADRS_MAIL_DOM_CITY=b.ADRS_MAIL_DOM_CITY
		  ,@ADRS_MAIL_DOM_STATE=b.DRS_MAIL_DOM_STATE,@ADRS_MAIL_ZIP=b.ADRS_MAIL_ZIP,@GR_CODE=b.GR_CODE
		  ,@DY_POSN_NR=b.DY_POSN_NR
          ,@DUTY_STATUS=  CASE   CHARINDEX(')', b.DUTY_STATUS)
                             WHEN  0 THEN  null
			                 ELSE SUBSTRING(b.DUTY_STATUS, 2, CHARINDEX(')', b.DUTY_STATUS) - 2) 
                          END   
          ,@DOB=cast(b.DOB AS dateTIME) 
          ,@SVC_COMP= CASE   CHARINDEX(')', b.SVC_COMP)
			            WHEN 0 THEN '' 
			         ELSE 	
			         CASE SUBSTRING(b.SVC_COMP, 2, CHARINDEX(')', b.SVC_COMP) - 2) 
			                 WHEN 'V' THEN '6'
			                 ELSE '5'
			             END
			         END   
		 ,@LOCAL_ADDR_ZIP=b.LOCAL_ADDR_ZIP 
		 ,@PAS_NUMBER=SUBSTRING(b.PAS,5,4)   
		 ,@PAFSC=PAFSC      
		 ,@ATTACH_PAS = Case
			When LEN(ATTACH_PAS) > 4 Then SUBSTRING(b.PAS,5,4)  Else ATTACH_PAS
			End
    FROM        
         MILPDSRawData AS b  WHERE SSAN=@SSAN 

     SET @record_mod_type= NULL     
     
     
     IF ((SELECT  ssan FROM    MemberData WHERE SSAN=@SSAN and  ( Deleted =0 or Deleted IS NULL)) is null)
      BEGIN 
        SET @record_mod_type= 'A'        
      END
     
      IF ((SELECT  ssan FROM    MemberData WHERE SSAN=@SSAN and  Deleted =1) is not null)
      BEGIN 
        SET @record_mod_type= 'AD'
      END 
      
     SET @PAFSC_INITIAL_CODE=NULL
     SET @REPORTING_UNIT=NULL
     IF (@GR_CODE IN (SELECT CODE FROM core_lkupGrade WHERE CODE IN(7,8,9,10))) 
			  BEGIN  
				 SET @PAFSC_INITIAL_CODE = 'G'
				 SET @REPORTING_UNIT='!!MA'			 
			  END
      ELSE IF (LEN(@PAFSC)=4)
      BEGIN   
				SELECT @PAFSC_INITIAL_CODE=FirstLetter,@REPORTING_UNIT=ReportingUnit FROM @PAFSCCode WHERE  UPPER(SUBSTRING(@PAFSC, 1, 3))=PAFSC   OR UPPER(SUBSTRING(@PAFSC, 1, 2))=PAFSC				 
	  END
	 
      IF(@PAFSC_INITIAL_CODE IS NOT NULL)
		 BEGIN 
			SET @PAS_NUMBER=@PAFSC_INITIAL_CODE + SUBSTRING(@PAS_NUMBER,2,3)
		  END 
      
      
IF (@record_mod_type in('AD','A' ))
BEGIN 
        --Added members First Time or After Deletion so insert as added in MemberData_ChangeHistory
        SET @insertedMemberRows=@insertedMemberRows+1 
        INSERT INTO MemberData_ChangeHistory
                   ( SSAN,FIRST_NAME,MIDDLE_NAMES,LAST_NAME,SUFFIX,SEX_SVC_MBR,PAS_GAINING
                    ,PAS,COMM_DUTY_PHONE, OFFICE_SYMBOL, LOCAL_ADDR_STREET
                    ,LOCAL_ADDR_CITY, LOCAL_ADDR_STATE, HOME_PHONE,ADRS_MAIL_DOM_CITY
                    ,ADRS_MAIL_DOM_STATE, ADRS_MAIL_ZIP, GR_CURR, DY_POSN_NR
                    ,DUTY_STATUS ,DOB,SVC_COMP,ZIP,PAS_NUMBER,ChangeType,Date, ATTACH_PAS)
         
         VALUES 
         (
                     @SSAN,@FIRST_NAME,@MIDDLE_NAMES,@LAST_NAME   
                    ,@SUFFIX,@SEX_SVC_MBR,@PAS_GAINING,@PAS,@COMM_DUTY_PHONE  
		            ,@OFFICE_SYMBOL,@LOCAL_ADDR_STREET,@LOCAL_ADDR_CITY
		            ,@LOCAL_ADDR_STATE,@HOME_PHONE,@ADRS_MAIL_DOM_CITY  
		            ,@ADRS_MAIL_DOM_STATE,@ADRS_MAIL_ZIP,@GR_CODE 
                    ,@DY_POSN_NR,@DUTY_STATUS,@DOB,@SVC_COMP,@LOCAL_ADDR_ZIP  
                    ,@PAS_NUMBER,'A',GETUTCDATE(), @ATTACH_PAS
         )
  
END 

IF (@record_mod_type ='A' )
BEGIN  
 --Added members First Time so insert into MemberData
     INSERT INTO MemberData
                (SSAN, FIRST_NAME,MIDDLE_NAMES,LAST_NAME, SUFFIX, SEX_SVC_MBR, PAS_GAINING,PAS, 
                 COMM_DUTY_PHONE,OFFICE_SYMBOL,LOCAL_ADDR_STREET, LOCAL_ADDR_CITY, LOCAL_ADDR_STATE, HOME_PHONE, 
                 ADRS_MAIL_DOM_CITY, ADRS_MAIL_DOM_STATE, ADRS_MAIL_ZIP, GR_CURR, DY_POSN_NR,DUTY_STATUS,
                 DOB,SVC_COMP,ZIP,PAS_NUMBER, ATTACH_PAS)
         VALUES 
         (
                    @SSAN,@FIRST_NAME,@MIDDLE_NAMES,@LAST_NAME   
                    ,@SUFFIX,@SEX_SVC_MBR,@PAS_GAINING,@PAS,@COMM_DUTY_PHONE  
		            ,@OFFICE_SYMBOL,@LOCAL_ADDR_STREET,@LOCAL_ADDR_CITY
		            ,@LOCAL_ADDR_STATE,@HOME_PHONE,@ADRS_MAIL_DOM_CITY  
		            ,@ADRS_MAIL_DOM_STATE,@ADRS_MAIL_ZIP,@GR_CODE 
                    ,@DY_POSN_NR,@DUTY_STATUS,@DOB,@SVC_COMP,@LOCAL_ADDR_ZIP  
                    ,@PAS_NUMBER, @ATTACH_PAS
         )
         
 END 
--If added after deletion this will not be considered modified but  instead  will be added in member data change history as added record so 
--no need to track changes to these records
IF (@record_mod_type ='AD' )
BEGIN  
         UPDATE MemberData  SET               
           FIRST_NAME=@FIRST_NAME
          ,MIDDLE_NAMES=@MIDDLE_NAMES
          ,LAST_NAME=@LAST_NAME
          ,SUFFIX=@SUFFIX
          ,SEX_SVC_MBR=@SEX_SVC_MBR
          ,PAS_GAINING=@PAS_GAINING
          ,PAS=@PAS
          ,COMM_DUTY_PHONE=@COMM_DUTY_PHONE
          ,OFFICE_SYMBOL=@OFFICE_SYMBOL
          ,LOCAL_ADDR_STREET=@LOCAL_ADDR_STREET
          ,LOCAL_ADDR_CITY=@LOCAL_ADDR_CITY
          ,LOCAL_ADDR_STATE=@LOCAL_ADDR_STATE
          ,HOME_PHONE=@HOME_PHONE
          ,ADRS_MAIL_DOM_CITY=@ADRS_MAIL_DOM_CITY
          ,ADRS_MAIL_DOM_STATE=@ADRS_MAIL_DOM_STATE
          ,ADRS_MAIL_ZIP=@ADRS_MAIL_ZIP
          ,GR_CURR=@GR_CODE
          ,DY_POSN_NR=@DY_POSN_NR
          ,DUTY_STATUS=@DUTY_STATUS
          ,DOB=@DOB
          ,SVC_COMP=@SVC_COMP
          ,ZIP=@LOCAL_ADDR_ZIP
          ,PAS_NUMBER=@PAS_NUMBER
          ,Deleted =NULL 
          ,DeletedDate=null  
         , ATTACH_PAS = @ATTACH_PAS 
         WHERE SSAN=@SSAN
END 
 ---If record already exisits and is not deleted we wil consider it as as modified .So will find if the data has been modified 
 IF ((SELECT  ssan FROM    MemberData WHERE SSAN=@SSAN and  ( Deleted =0 or Deleted IS NULL)) is not null)
 BEGIN  
       SELECT      
         @mb_FIRST_NAME  =MemberData.FIRST_NAME
        ,@mb_MIDDLE_NAMES =MemberData.MIDDLE_NAMES
        ,@mb_LAST_NAME =MemberData. LAST_NAME 
        ,@mb_SUFFIX=MemberData. SUFFIX
        ,@mb_SEX_SVC_MBR=MemberData.SEX_SVC_MBR   
		,@mb_PAS_GAINING=MemberData.PAS_GAINING
		,@mb_PAS =MemberData.PAS
		,@mb_COMM_DUTY_PHONE  =MemberData.COMM_DUTY_PHONE
		,@mb_OFFICE_SYMBOL=MemberData.OFFICE_SYMBOL
		,@mb_LOCAL_ADDR_STREET=MemberData.LOCAL_ADDR_STREET   
		,@mb_LOCAL_ADDR_CITY =MemberData.LOCAL_ADDR_CITY
		,@mb_LOCAL_ADDR_STATE  =MemberData.LOCAL_ADDR_STATE
		,@mb_HOME_PHONE=MemberData.HOME_PHONE  
		,@mb_ADRS_MAIL_DOM_CITY =MemberData.ADRS_MAIL_DOM_CITY 
		,@mb_ADRS_MAIL_DOM_STATE=MemberData.ADRS_MAIL_DOM_STATE
		,@mb_ADRS_MAIL_ZIP =MemberData.ADRS_MAIL_ZIP
		,@mb_GR_CODE  =MemberData.GR_CURR
        ,@mb_DY_POSN_NR=MemberData.DY_POSN_NR
        ,@mb_DUTY_STATUS=MemberData.DUTY_STATUS 
        ,@mb_DOB=MemberData. DOB 
        ,@mb_SVC_COMP=MemberData.SVC_COMP   
        ,@mb_LOCAL_ADDR_ZIP=MemberData.ZIP  
        ,@mb_PAS_NUMBER =MemberData.PAS_NUMBER  
        ,@mb_ATTACH_PAS = MemberData.ATTACH_PAS
         FROM MemberData  WHERE MemberData.SSAN=@SSAN
        
         IF (
                    ISNULL(@mb_FIRST_NAME,'')<>ISNULL(@FIRST_NAME,'') 
                    or ISNULL(@mb_MIDDLE_NAMES ,'')<>ISNULL(@MIDDLE_NAMES,'') 
                    or ISNULL(@mb_LAST_NAME,'')<> ISNULL(@LAST_NAME,'') 
                    or ISNULL(@mb_SUFFIX,'') <> ISNULL(@SUFFIX,'')
                    or ISNULL(@mb_SEX_SVC_MBR,'') <> ISNULL(SUBSTRING(@SEX_SVC_MBR, 1, 1),'')
                    or ISNULL(@mb_PAS_GAINING ,'')<> ISNULL(@PAS_GAINING,'') 
                    or ISNULL(@mb_PAS,'')<> ISNULL(@PAS ,'') 
                    or ISNULL(@mb_COMM_DUTY_PHONE ,'')<> ISNULL(@COMM_DUTY_PHONE  ,'') 
                    or ISNULL(@mb_OFFICE_SYMBOL ,'')<> ISNULL(@OFFICE_SYMBOL ,'')  
                    or ISNULL(@mb_LOCAL_ADDR_STREET ,'')<> ISNULL(@LOCAL_ADDR_STREET,'') 
                    or ISNULL(@mb_LOCAL_ADDR_CITY ,'')<> ISNULL(@LOCAL_ADDR_CITY,'') 
                    or ISNULL(@mb_LOCAL_ADDR_STATE ,'')<> ISNULL(@LOCAL_ADDR_STATE,'') 
                    or ISNULL(@mb_HOME_PHONE ,'')<> ISNULL(@HOME_PHONE  ,'') 
                    or ISNULL(@mb_ADRS_MAIL_DOM_CITY ,'')<>ISNULL( @ADRS_MAIL_DOM_CITY,'') 
                    or ISNULL(@mb_ADRS_MAIL_DOM_STATE ,'')<>ISNULL( @ADRS_MAIL_DOM_STATE ,'')  
                    or ISNULL(@mb_ADRS_MAIL_ZIP ,'')<> ISNULL(@ADRS_MAIL_ZIP,'') 
                    or ISNULL(@mb_GR_CODE,'')  <> ISNULL(@GR_CODE ,'')
                    or ISNULL(@mb_DY_POSN_NR,'')<> ISNULL(@DY_POSN_NR ,'')   
                    or ISNULL(@mb_DOB ,'')<> ISNULL(@DOB ,'')      
                    or ISNULL(@mb_LOCAL_ADDR_ZIP ,'')<> ISNULL(@LOCAL_ADDR_ZIP ,'') 
                    or ISNULL(@mb_PAS_NUMBER ,'')<>  ISNULL(@PAS_NUMBER,'')  
                    or ISNULL(@mb_ATTACH_PAS ,'')<>  ISNULL(@ATTACH_PAS,'')  
              )
               
              BEGIN 
              SET @record_mod_type= 'M'
                INSERT INTO MemberData_ChangeHistory
                   ( SSAN,FIRST_NAME,MIDDLE_NAMES,LAST_NAME,SUFFIX,SEX_SVC_MBR,PAS_GAINING
                    ,PAS,COMM_DUTY_PHONE, OFFICE_SYMBOL, LOCAL_ADDR_STREET
                    ,LOCAL_ADDR_CITY, LOCAL_ADDR_STATE, HOME_PHONE,ADRS_MAIL_DOM_CITY
                    ,ADRS_MAIL_DOM_STATE, ADRS_MAIL_ZIP, GR_CURR, DY_POSN_NR
                    ,DUTY_STATUS ,DOB,SVC_COMP,ZIP,PAS_NUMBER,ChangeType,Date, ATTACH_PAS)
         
                     VALUES 
                     (
                                @SSAN,@mb_FIRST_NAME,@mb_MIDDLE_NAMES,@mb_LAST_NAME   
                                ,@mb_SUFFIX,@mb_SEX_SVC_MBR,@mb_PAS_GAINING,@mb_PAS,@mb_COMM_DUTY_PHONE  
		                        ,@mb_OFFICE_SYMBOL,@mb_LOCAL_ADDR_STREET,@mb_LOCAL_ADDR_CITY
		                        ,@mb_LOCAL_ADDR_STATE,@mb_HOME_PHONE,@mb_ADRS_MAIL_DOM_CITY  
		                        ,@mb_ADRS_MAIL_DOM_STATE,@mb_ADRS_MAIL_ZIP,@mb_GR_CODE 
                                ,@mb_DY_POSN_NR,@mb_DUTY_STATUS,@mb_DOB,@mb_SVC_COMP,@mb_LOCAL_ADDR_ZIP  
                                ,@mb_PAS_NUMBER,'M',GETUTCDATE(), @mb_ATTACH_PAS
                     )
                
                SET @modifiedRows=@modifiedRows+1 
             END  
              
       UPDATE MemberData  SET               
           FIRST_NAME=@FIRST_NAME
          ,MIDDLE_NAMES=@MIDDLE_NAMES
          ,LAST_NAME=@LAST_NAME
          ,SUFFIX=@SUFFIX
          ,SEX_SVC_MBR=@SEX_SVC_MBR
          ,PAS_GAINING=@PAS_GAINING
          ,PAS=@PAS
          ,COMM_DUTY_PHONE=@COMM_DUTY_PHONE
          ,OFFICE_SYMBOL=@OFFICE_SYMBOL
          ,LOCAL_ADDR_STREET=@LOCAL_ADDR_STREET
          ,LOCAL_ADDR_CITY=@LOCAL_ADDR_CITY
          ,LOCAL_ADDR_STATE=@LOCAL_ADDR_STATE
          ,HOME_PHONE=@HOME_PHONE
          ,ADRS_MAIL_DOM_CITY=@ADRS_MAIL_DOM_CITY
          ,ADRS_MAIL_DOM_STATE=@ADRS_MAIL_DOM_STATE
          ,ADRS_MAIL_ZIP=@ADRS_MAIL_ZIP
          ,GR_CURR=@GR_CODE
          ,DY_POSN_NR=@DY_POSN_NR
          ,DUTY_STATUS=@DUTY_STATUS
          ,DOB=@DOB
          ,SVC_COMP=@SVC_COMP
          ,ZIP=@LOCAL_ADDR_ZIP
          ,PAS_NUMBER=@PAS_NUMBER
          ,Deleted =NULL 
          ,DeletedDate=null  
         , ATTACH_PAS = @ATTACH_PAS 
         WHERE SSAN=@SSAN
 
END 
END TRY 	--END TRY BLOCK 
  
  	BEGIN CATCH --BEGIN CATCH BLOCK 
		 
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
			   ('memberData_Import(Add,Modify)' ,@SSAN,@logId,GETUTCDATE(),@message)			
					 				    
 	  END CATCH---END CATCH BLOCK



FETCH NEXT FROM member_cursor  INTO    @SSAN 
                                      
END ---End of LOD_CURSOR        
CLOSE member_cursor
DEALLOCATE  member_cursor  
        
--------MemberData_Cursor------- 
 
 	 DECLARE @ExistingSSNs TABLE 
		(  
			SSAN   varchar(11)  
		)
		
    ---Get the newly added pascodes	
	INSERT INTO  @ExistingSSNs     
		SELECT DISTINCT a.member_ssn FROM FORM348 a 
		JOIN vw_WorkStatus as vw on vw.ws_id =a.status
		WHERE vw.isFinal <>1

--Deleted records
--The members which have an open lod will not be deleted but if they have a user account.It will be disabled
--as part of the pkg  
	   
SET @SSAN =NULL 
DECLARE @DELETED bit
SET @deletedMemberRows=0
DECLARE memberData_cursor CURSOR FOR   SELECT SSAN ,DELETED FROM  MemberData      
        
OPEN memberData_cursor
           
FETCH NEXT FROM memberData_cursor  INTO    @SSAN,@DELETED                                    
WHILE @@FETCH_STATUS = 0 
BEGIN ---BEGIN CURSOR  


 BEGIN TRY   
IF (
     ((SELECT   ssan FROM MILPDSRawData Where SSAN=@SSAN) is Null)  	
	 AND
	 ((SELECT SSAN FROM  @ExistingSSNs Where SSAN=@SSAN) is Null) 
	 AND 
	 (@DELETED is null or @DELETED=0)
	)
	
	       BEGIN  

            Update MemberData SET Deleted =1 ,DeletedDate =GETUTCDATE() WHERE MemberData.SSAN=@SSAN  
           
	         SET @deletedMemberRows=@deletedMemberRows+1  
              INSERT INTO MemberData_ChangeHistory
					( SSAN, FIRST_NAME, MIDDLE_NAMES, LAST_NAME, SUFFIX, SEX_SVC_MBR, PAS_GAINING,    PAS, 
                      COMM_DUTY_PHONE, OFFICE_SYMBOL, LOCAL_ADDR_STREET, LOCAL_ADDR_CITY, LOCAL_ADDR_STATE, HOME_PHONE, 
                      ADRS_MAIL_DOM_CITY, ADRS_MAIL_DOM_STATE, ADRS_MAIL_ZIP, GR_CURR, DY_POSN_NR,   DUTY_STATUS,   
                      DOB,SVC_COMP,ZIP,ChangeType,Date,PAS_NUMBER, ATTACH_PAS) 
              SELECT	   SSAN, FIRST_NAME, MIDDLE_NAMES, LAST_NAME, SUFFIX, SEX_SVC_MBR, PAS_GAINING,    PAS, 
                      COMM_DUTY_PHONE, OFFICE_SYMBOL, LOCAL_ADDR_STREET, LOCAL_ADDR_CITY, LOCAL_ADDR_STATE, HOME_PHONE, 
                      ADRS_MAIL_DOM_CITY, ADRS_MAIL_DOM_STATE, ADRS_MAIL_ZIP, GR_CURR, DY_POSN_NR,  DUTY_STATUS,   
                      DOB, SVC_COMP,ZIP,'D',GETDATE() ,PAS_NUMBER , ATTACH_PAS 
              FROM MemberData  WHERE MemberData.SSAN=@SSAN  
              
               
           END 
         
 END TRY 	--END TRY BLOCK 
  
  	BEGIN CATCH --BEGIN CATCH BLOCK 
		 
        
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
			   ('memberData_Import(delete)' ,@SSAN,@logId,GETUTCDATE(),@message)			
					 				    
 	  END CATCH---END CATCH BLOCK      

FETCH NEXT FROM memberData_cursor  INTO    @SSAN,@DELETED
                                      
END ---End of LOD_CURSOR        
CLOSE memberData_cursor
DEALLOCATE  memberData_cursor 
         
             
    --Update the pascode   
	 DECLARE @AllPasCode TABLE 
	 (  
	         PASCODE  varchar(8)
			,CS_ID int
			,PARENT_PASCODE  varchar(4)
			,PARENT_CS_ID int 
	 ) 

 
    ---Get the newly added pascodes	
	INSERT INTO  @AllPasCode(PASCODE)
	SELECT    DISTINCT  PAS_NUMBER   FROM  MemberData WHERE   PAS_NUMBER NOT  IN(   select PAS_CODE from command_struct  WHERE PAS_CODE IS NOT NULL)

--Update the 
	   UPDATE @AllPasCode
		SET  PARENT_PASCODE = b.ReportingUnit  
			,PARENT_CS_ID =  c.CS_ID  
	 	FROM @AllPasCode a, @PAFSCCode b, Command_Struct  c
	 	WHERE  SUBSTRING(a.PASCODE ,1,1)  = b.FirstLetter  AND 
	 		 b.ReportingUnit   = c.PAS_CODE
	 		 
		
----Add new pas codes 
DECLARE @PASCODE varchar(8),@parent_cs_id int 
DECLARE @new_csid int 
DECLARE pascode_cursor CURSOR FOR   SELECT PASCODE,PARENT_CS_ID FROM  @AllPasCode      
SET @sysId= IsNull((select top 1 userID  FROM vw_users WHERE groupId =1),1) 


OPEN pascode_cursor
FETCH NEXT FROM pascode_cursor  INTO  @PASCODE,@parent_cs_id                      
WHILE @@FETCH_STATUS = 0 
BEGIN ---BEGIN CURSOR         

   BEGIN TRY 
   	--Add into commande struct 	
   	if (@parent_cs_id IS NULL)
   	BEGIN
   		SET @parent_cs_id=1 
   	END 
   	
	 INSERT INTO Command_Struct
	      (PAS_CODE
		  ,cs_id_parent
		  ,LONG_NAME 
          ,cs_oper_type          
          ,cs_level              
          ,component             
          ,gaining_command_cs_id
          ,CREATED_BY
          ,CREATED_DATE
          ,MODIFIED_BY 
          ,MODIFIED_DATE  
          ) 
		VALUES 
			( @PASCODE,@parent_cs_id,'UNIT -'+ @PASCODE, 'U','U','AFRC',6,@sysAdmin,GETUTCDATE(),@sysAdmin,GETUTCDATE() )
		 
	
	 SET @new_csid=(SELECT CS_ID FROM Command_Struct WHERE PAS_CODE =@PASCODE)
	
	--update the cs_ids in the @AllPasCode
		UPDATE @AllPasCode 
			SET CS_ID=@new_csid	 
		  WHERE PASCODE  = @PASCODE
		   
    --Update the command struct chain insert records for all chain types with parent unit csid as 1
 	  INSERT INTO Command_Struct_Chain (CS_ID,view_type,CHAIN_TYPE,CSC_ID_PARENT,created_by,created_date,MODIFIED_BY,MODIFIED_DATE )
 	  SELECT @new_csid, chain.id ,chain.Name,csc.CSC_ID, @sysAdmin,GETUTCDATE(),@sysAdmin,GETUTCDATE()
	  from core_lkupChainType as chain 
	  join Command_Struct_Chain csc on chain.id = csc.view_type and csc.CS_ID = @parent_cs_id
 	  where chain.active =1 

 		  
     INSERT INTO command_struct_history  
          (pkgLog_Id,pascode,cs_id,cs_id_parent,LONG_NAME,created_date,created_by,ChangeType )
     SELECT 
          @logId,@PASCODE,@new_csid ,@parent_cs_id,'UNIT -'+ @PASCODE,GETDATE(),@sysId ,'A'
      
    END TRY 	--END TRY BLOCK 
  
  	BEGIN CATCH --BEGIN CATCH BLOCK 
		 
        
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
			   ('memberData_Import(new unit insert)' ,@PASCODE,@logId,GETDATE(),@message)			
					 				    
 	  END CATCH---END CATCH BLOCK      



FETCH NEXT FROM pascode_cursor  INTO    @PASCODE,@parent_cs_id 
                                      
END ---End of LOD_CURSOR        
CLOSE pascode_cursor
DEALLOCATE  pascode_cursor 
 
/*
------Updating the open lods for new unit----------------------------------------------------------- 
        UPDATE FORM348  
           SET 
            member_unit_id =Isnull(cs.cs_id,member_unit_id) 
           ,member_unit =Isnull(cs.LONG_NAME,member_unit )
          FROM  MemberData  t2
	        INNER JOIN FORM348 t1  on t2.SSAN  = t1.member_ssn	
	        INNER JOIN vw_WorkStatus as vw on vw.ws_id =t1.status
	        LEFT JOIN Command_Struct cs on cs.PAS_CODE =t2.PAS_NUMBER   
            WHERE vw.isFinal <>1
*/       
    
    UPDATE pkgLog 
	    SET  nRowRawInserted=@rawRowsInserted,
		     nRowInserted=@insertedMemberRows+@addAfterDeletion,
		     nRowUpdated=@updatedMemberRows,
		     nDeletedMembers=@deletedMemberRows,
		     nModifiedRecords =@modifiedRows
	    WHERE id=@logId
	
END
GO

