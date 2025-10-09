

--DELETE FROM  CORE_USERROLES
--SELECT * FROM imp_USERMAPPING  
--SELECT * FROM CORE_USERROLES  

--SELECT * FROM CORE_USERS  

--DELETE FROM  CORE_USERS WHERE USERID<>1
--EXECUTE imp_ImportUsers
--SELECT * FROM IMPORT_ERROR_LOG WHERE STOREDPROC='imp_ImportUsers'

create procedure [dbo].[imp_CreateUsers_print]
	 
AS


 BEGIN 
 
 print ('Selecting RCPHA users...')
 
 DECLARE @PHA_USERS TABLE 
 (

    UserName varchar(100)
 )
 
 INSERT INTO @PHA_USERS SELECT DISTINCT LTRIM(RTRIM(grantee)) FROM imp_dba_role_privs WHERE GRANTED_ROLE ='PHA_USER' 
 
 print 'Populating cursor...'
 
DECLARE  userCursor CURSOR FOR   
  SELECT 
	 t.PERS_ID 
	,t.USERNAME
 	,t.FirstName
	,t.LastName
	,t.MiddleName
	,t.Title
	,t.SSN
	,t.DateOfBirth
	,t.Phone
	,t.DSN 
	,t.Email
	,t.Email2
	,t.Email3
	,t.workCompo
	,t.accessStatus
	,t.receiveEmail
	,t.expirationDate
	,t.COMMENT
	,t.lastAccessDate
	,t.rank_code
	,t.work_street
	,t.work_city
	,t.work_state
	,t.work_zip
	,t.work_country
	,t.created_date
	,t.cs_id
	,t.ada_cs_id
	,t.currentRole
	,t.DisabledDate

FROM 
(
	SELECT 
	p.pers_id as PERS_ID
	,p.username as  USERNAME
  ,CASE WHEN pf.first_name IS NULL THEN '' 
	ELSE pf.first_name  
	END   FirstName
  ,CASE WHEN pf.last_name IS NULL THEN '' 
  ELSE pf.last_name
    END LastName
  ,pf.mi AS MiddleName
  ,pf.name_suffix AS Title
  ,p.ssn AS SSN 
    ,CASE 
			WHEN pf.DOB  IS NOT NULL AND pf.DOB  <>'' THEN   cast(pf.DOB  as Datetime) 
		    ELSE NULL
		   END AS DateOfBirth 
   ,pf.Home_Phone AS PHONE 
   ,NULL AS DSN 
      ,CASE 
			WHEN p.E_MAIL_HOME IS NOT NULL AND p.E_MAIL_HOME  <>'' THEN   p.E_MAIL_HOME
		    ELSE ''
		   END AS Email 
    
   ,p.E_MAIL_WORK AS Email2
   ,p.E_MAIL_UNIT AS Email3
   ,'6' AS workCompo
   ,CASE WHEN u.account_status IS NOT NULL AND  u.account_status <>''  
	THEN (select statusId from IMP_lkupAccessStatus where accountstatus=u.account_status ) 
    ELSE 
     4--If no record is found in the dba_users then its considered disabled
    END AccessStatus
    
    ,1 AS receiveEmail   
   ,CASE 
			WHEN u.EXPIRY_DATE IS NOT NULL AND u.EXPIRY_DATE <>'' THEN   cast(u.EXPIRY_DATE as Datetime) 
		    ELSE  DATEADD (year,1, getUTCDate())
		   END AS ExpirationDate 	
    ,null  AS COMMENT 
    ,getUTCDate() AS lastAccessDate   
    ,  CASE WHEN  pf.rank is null or pf.rank ='' THEN 99 ---Unknown
		ELSE  (SELECT TOP 1 GRADE code FROM imp_gradeLookUp  WHERE  GRADE=pf.rank)
	    END     rank_code
	  ,pf.MAIL_ADDRESS1 +'  '+  pf.MAIL_ADDRESS2 AS work_street
      ,pf.MAIL_CITY AS work_city
     ,pf.MAIL_STATE  AS work_state
     ,pf.MAIL_POSTAL_CODE AS work_zip
      ,NULL  AS work_country
      ,CASE 
			WHEN u.CREATED IS NOT NULL AND u.CREATED <>'' THEN   cast(u.CREATED as Datetime)   
		    ELSE  GETutcdate()
		   END 
			AS created_Date  
        
    
     ,isnull(cmdstr.cs_id,11) AS cs_id --if not then UNKNOWN
      ,null AS ada_cs_id
	  ,null  AS currentRole 
	 ,CASE 
			WHEN u.LOCK_DATE IS NOT NULL AND u.LOCK_DATE <>'' THEN   cast(u.LOCK_DATE as Datetime)   
		    ELSE NULL
		   END 
			AS DisabledDate  
       FROM imp_personnel p  
       LEFT JOIN   imp_personnel_feed   pf  ON  pf.pers_id=p.pers_id 
       LEFT JOIN imp_manpower mp  on pf.pos_nbr = mp.pos_nbr
	   LEFT JOIN command_struct cmdstr on mp.pas_code = cmdstr.pas_code 
       LEFT JOIN  imp_dba_users u on LTRIM(RTRIM(u.USERNAME))=LTRIM(RTRIM(p.username))  
	   WHERE p.USERNAME IS NOT NULL 
	  AND LTRIM(RTRIM(u.USERNAME)) in 
	   (
			SELECT UserName  from @PHA_USERS 
	   ) 
	   
	   
	   
	   
       UNION ALL 
      
      
      
       SELECT 
       p.pers_id as PERS_ID
        ,p.username  AS  USERNAME
        ,CASE WHEN p.first_name IS NULL THEN '' 
			ELSE p.first_name  
			END   FirstName
		  ,CASE WHEN p.LAST_NAME IS NULL THEN '' 
		  ELSE p.LAST_NAME
		  END 
		    LastName
     
       ,p.mi AS MiddleName
       ,p.name_suffix  AS Title
       ,'' AS SSN 
		,NULL AS DateOfBirth
		,NULL AS PHONE 
		 ,NULL AS DSN 
		 ,CASE 
			WHEN p.E_MAIL_HOME IS NOT NULL AND p.E_MAIL_HOME  <>'' THEN   p.E_MAIL_HOME
		    ELSE ''
		   END AS Email 
		,p.E_MAIL_WORK AS Email2
		,p.E_MAIL_UNIT AS Email3
        ,'6' AS workCompo
		,CASE WHEN u.account_status IS NOT NULL AND  u.account_status <>''  
		THEN (select statusId from IMP_lkupAccessStatus where accountstatus=u.account_status ) 
		ELSE 
			 4--If no record is found in the dba_users then its considered disabled
		END AccessStatus
		 ,1 AS recvemail   
		  ,CASE 
			WHEN u.EXPIRY_DATE IS NOT NULL AND u.EXPIRY_DATE <>'' THEN   cast(u.EXPIRY_DATE as Datetime) 
		    ELSE  DATEADD (year,1, getUTCDate())
		   END AS ExpirationDate 	 
		 ,p.CONTACT_INFO AS comment      
		 ,getUTCDate() AS lastAccessDate  
		 ,99 AS rank_code --Unknown
		,p.RES_ADDRESS1 +' '+ p.RES_ADDRESS2 AS work_street
		 ,p.RES_CITY AS  work_city
		 ,p.RES_STATE   AS work_state 
		 ,p.RES_POSTAL_CODE AS work_zip 
		 ,p.RES_COUNTRY AS work_country 
		 ,CASE 
			WHEN u.CREATED IS NOT NULL AND u.CREATED <>'' THEN   cast(u.CREATED as Datetime)   
		    ELSE  GETutcdate()
		   END 
			AS created_Date  
		 
		 ,CASE WHEN p.CS_ID=0 or p.CS_ID is null THEN 11--UNKNOWN 
		  ELSE p.CS_ID  
		  END  cs_id		  
		 ,null AS ada_cs_id
		  ,null  AS currentRole 
	      ,CASE 
			WHEN u.LOCK_DATE IS NOT NULL AND u.LOCK_DATE <>'' THEN   cast(u.LOCK_DATE as Datetime)   
		    ELSE NULL
		   END 
			AS DisabledDate 
	      
	      	 FROM imp_non_unit_personnel p  
		left join 
		imp_dba_users u on 
		 LTRIM(RTRIM(u.USERNAME))=p.username
		  WHERE p.USERNAME IS NOT NULL 
		        AND LTRIM(RTRIM(u.USERNAME)) in 
	   (
			SELECT UserName  from @PHA_USERS 
	   )   
		      
  
  )     t
       
       
       
  DECLARE
	 @PERS_ID INT 	
	,@USERNAME			varchar(50)
 	,@FirstName			varchar(50)
	,@LastName			varchar(50)
	,@MiddleName			varchar(50)
	,@Title			varchar(50)
	,@SSN			varchar(50)
	,@DateOfBirth			datetime
	,@Phone			varchar(50)
	,@DSN 			varchar(50)
	,@Email			varchar(50)
	,@Email2			varchar(50)
	,@Email3			varchar(50)
	,@workCompo			varchar(50)
	,@accessStatus			varchar(50)
	,@receiveEmail			bit
	,@expirationDate			datetime
	,@COMMENT			varchar(2000)
	,@lastAccessDate			datetime
	,@rank_code			int
	,@work_street			varchar(2000)
	,@work_city			varchar(50)
	,@work_state			varchar(50)
	,@work_zip			varchar(50)
	,@work_country			varchar(50)
	,@created_date			datetime
	,@cs_id			int
	,@ada_cs_id		int
	,@currentRole	int
	,@DisabledDate			datetime
	
 
DECLARE @userid as int 

print 'Creating Users...'
       
       OPEN userCursor
				
			 	FETCH NEXT FROM userCursor  INTO
			 	 @PERS_ID
			 	 ,@USERNAME
				,@FirstName
				,@LastName
				,@MiddleName
				,@Title
				,@SSN
				,@DateOfBirth
				,@Phone
				,@DSN 
				,@Email
				,@Email2	
				,@Email3
				,@workCompo
				,@accessStatus
				,@receiveEmail
				,@expirationDate
				,@COMMENT
				,@lastAccessDate
				,@rank_code
				,@work_street
				,@work_city
				,@work_state
				,@work_zip		
				,@work_country
				,@created_date
				,@cs_id
				,@ada_cs_id
				,@currentRole
				,@DisabledDate
				
			
  
				WHILE @@FETCH_STATUS = 0
				BEGIN 
				BEGIN TRY 
						
							print @username + ' status: ' + @accessStatus;


				
				END TRY 	--END TRY BLOCK 
			BEGIN CATCH --BEGIN CATCH BLOCK 
		 	
		 
							DECLARE  @msg varchar(2000)
							DECLARE @number int ,@errmsg varchar(2000)
							SELECT 
							  @number= ERROR_NUMBER()  
							 ,@errmsg= ERROR_MESSAGE() 
							 
							 print '** Error creating user ' + @username

				 			EXECUTE imp_InsertErrorRecord @number
							,'IMPORTING USERS','imp_ImportUsers ',@USERNAME,null, @errmsg
 
      

		
		
		 		 						    
		 END CATCH---END CATCH BLOCK   
		
       			 FETCH NEXT FROM userCursor  INTO
			 	 @PERS_ID
			 	 ,@USERNAME
				,@FirstName
				,@LastName
				,@MiddleName
				,@Title
				,@SSN
				,@DateOfBirth
				,@Phone
				,@DSN 
				,@Email
				,@Email2	
				,@Email3
				,@workCompo
				,@accessStatus
				,@receiveEmail
				,@expirationDate
				,@COMMENT
				,@lastAccessDate
				,@rank_code
				,@work_street
				,@work_city
				,@work_state
				,@work_zip		
				,@work_country
				,@created_date
				,@cs_id
				,@ada_cs_id
				,@currentRole
				,@DisabledDate
						
				END ---End of OTHER_PERSONS_CURSOR 
				
				CLOSE userCursor
				DEALLOCATE  userCursor
		 
 
       
       END
GO

