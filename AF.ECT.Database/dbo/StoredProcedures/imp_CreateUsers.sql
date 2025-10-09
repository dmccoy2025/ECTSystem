
CREATE procedure [dbo].[imp_CreateUsers]
	 
AS

 BEGIN 
 
 DELETE FROM imp_USERMAPPING
 
 update core_Users set currentRole = null where userID > 2;
  
 DELETE FROM core_Userroles WHERE USERID > 2;
 DELETE FROM core_Users WHERE USERID > 2;
 
 --reset some identity columns
 dbcc checkident(core_users, reseed, 2);
 dbcc checkident(core_userRoles, reseed, 2);
  
 create table #PHA_USERS
 (
    UserName varchar(100) primary key
 )
 
 INSERT INTO #PHA_USERS SELECT DISTINCT LTRIM(RTRIM(grantee)) FROM imp_dba_role_privs WHERE GRANTED_ROLE ='PHA_USER' 
 
 
 declare @total as int
 set @total = 1;
 
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
		isnull(np.pers_id,p.pers_id) as PERS_ID
		,users.username as USERNAME
		,CASE 
			WHEN p.PERS_ID Is not null then ISNULL(pf.first_name, '') 
			ELSE ISNULL(np.first_NAME,'')
			END FirstName
		,CASE 
			WHEN p.PERS_ID Is not null then ISNULL(pf.last_name, '') 
			ELSE ISNULL(np.LAST_NAME,'')
			END LastName
		,CASE 
			WHEN p.PERS_ID Is not null then ISNULL(pf.MI, '') 
			ELSE ISNULL(np.MI,'')
			END MiddleName
		,pf.name_suffix AS Title
		,p.ssn AS SSN 
		,CASE 
			WHEN pf.DOB IS NOT NULL AND pf.DOB <>'' THEN cast(pf.DOB  as Datetime) 
		    ELSE NULL
			END AS DateOfBirth 
		,pf.Home_Phone AS PHONE 
		,NULL AS DSN 
		,CASE 
			WHEN isnull(p.E_MAIL_HOME, np.e_mail_home) IS NOT NULL AND len(isnull(p.E_MAIL_HOME, np.e_mail_home)) > 0 THEN   p.E_MAIL_HOME
		    ELSE ''
			END AS Email 
		,ISNULL(p.E_MAIL_WORK, np.e_mail_work) AS Email2
		,isnull(p.E_MAIL_UNIT, np.e_mail_unit) AS Email3
		,'6' AS workCompo
		,CASE 
			WHEN u.account_status IS NOT NULL AND  u.account_status <>''  
			THEN (select statusId from IMP_lkupAccessStatus where accountstatus=u.account_status ) 
			ELSE 4--If no record is found in the dba_users then its considered disabled
			END AccessStatus
		,1 AS receiveEmail   
		,CASE 
			WHEN u.EXPIRY_DATE IS NOT NULL AND u.EXPIRY_DATE <>'' THEN   cast(u.EXPIRY_DATE as Datetime) 
		    ELSE  DATEADD (year,1, getUTCDate())
			END AS ExpirationDate 	
		,null  AS COMMENT 
		,getUTCDate() AS lastAccessDate   
		,CASE WHEN  pf.rank is null or pf.rank ='' THEN 99 ---Unknown
			ELSE (SELECT TOP 1 GRADE code FROM imp_gradeLookUp  WHERE  GRADE=pf.rank)
			END rank_code
		,pf.MAIL_ADDRESS1 + '  ' +  pf.MAIL_ADDRESS2 AS work_street
		,pf.MAIL_CITY AS work_city
		,pf.MAIL_STATE  AS work_state
		,pf.MAIL_POSTAL_CODE AS work_zip
		,NULL AS work_country
		,CASE 
			WHEN u.CREATED IS NOT NULL AND u.CREATED <>'' THEN   cast(u.CREATED as Datetime)   
		    ELSE GETutcdate()
			END AS created_Date  
		,isnull(cmdstr.cs_id,11) AS cs_id --if not then UNKNOWN
		,null AS ada_cs_id
		,null  AS currentRole 
		,CASE 
			WHEN u.LOCK_DATE IS NOT NULL AND u.LOCK_DATE <>'' THEN   cast(u.LOCK_DATE as Datetime)   
		    ELSE NULL
			END AS DisabledDate  
		FROM 
			#pha_users users
		left join 
			imp_personnel p on p.username like users.username
		left join
			imp_non_unit_personnel np on np.username like users.username
		LEFT JOIN   
			imp_personnel_feed pf ON pf.pers_id = isnull(np.pers_id,p.pers_id)
		LEFT JOIN 
			imp_manpower mp  on pf.pos_nbr = mp.pos_nbr
		LEFT JOIN 
			command_struct cmdstr on mp.pas_code = cmdstr.pas_code 
		LEFT JOIN  
			imp_dba_users u on u.USERNAME like p.username
		where 
			isnull(np.pers_id,p.pers_id) is not null
 ) t
       

       
  DECLARE
	 @PERS_ID INT 	
	,@USERNAME			nvarchar(20)
 	,@FirstName			nvarchar(100)
	,@LastName			nvarchar(100)
	,@MiddleName			nvarchar(50)
	,@Title			nvarchar(50)
	,@SSN			nvarchar(50)
	,@DateOfBirth			datetime
	,@Phone			nvarchar(50)
	,@DSN 			nvarchar(50)
	,@Email			nvarchar(100)
	,@Email2			nvarchar(100)
	,@Email3			nvarchar(100)
	,@workCompo			nvarchar(50)
	,@accessStatus			nvarchar(50)
	,@receiveEmail			bit
	,@expirationDate			datetime
	,@COMMENT			nvarchar(4000)
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
							
	INSERT INTO core_Users
		(USERNAME,FirstName,LastName,MiddleName,Title,SSN,DateOfBirth,Phone
		,DSN,Email,Email2,Email3,workCompo,accessStatus,receiveEmail,expirationDate
		,COMMENT,lastAccessDate,rank_code,work_street,work_city,work_state
		,work_zip,work_country,created_date,modified_date,cs_id,ada_cs_id
		,currentRole,DisabledDate) 
	VALUES 
	(
		 @USERNAME
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
		,ISNULL(@accessStatus, 4)
		,@receiveEmail
		,@expirationDate
		,@COMMENT
		,@lastAccessDate
		,isnull(@rank_code,99)
		,@work_street
		,@work_city
		,@work_state
		,@work_zip		
		,@work_country
		,@created_date
		,GETUTCDATE()
		,@cs_id
		,@ada_cs_id
		,@currentRole
		,@DisabledDate
	)
	 
	set @userId = scope_identity()
			
	INSERT INTO imp_USERMAPPING 
		(USERID,PERSON_ID,USERNAME )
	VALUES 
		(@userid,@PERS_ID,@USERNAME )
					
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
		
FETCH NEXT FROM userCursor INTO
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
       
drop table #pha_users;
       
END
GO

