--exec core_user_sp_SearchMamberData 1,'',Smith,0,7

 CREATE PROCEDURE [dbo].[core_user_sp_SearchMamberData_Test]

		  
		  @userId int
		, @ssn varchar(20)=null
		, @name varchar(10)=null	
		, @srchUnit int =null
		, @rptView int 
		 
	AS
 
			IF  (@name) ='' SET @name = NULL
			IF ( @ssn  ='' )SET @ssn = NULL
			IF (@srchUnit = 0) SET @srchUnit = NULL
 
	
		Declare @userUnit int, @groupId tinyint
		SELECT @userUnit = unit_id, @groupId = groupId from vw_Users where userId=@userId

 
		SELECT 
			 RIGHT(m.SSAN, 4) LastFour
			,m.FIRST_NAME FirstName, m.LAST_NAME LastName, m.gr_curr as Grade, s.LONG_NAME + ' ('+s.PAS_CODE +')' CurrentUnitName 
			,a.role RoleName,IsNull(a.userId,0) Id, a.accessStatus Status ,s.cs_id,m.svc_comp  as Component,m.pas_number as pas_code,m.SSAN
		FROM 
			memberdata   m
		LEFT JOIN 
			Command_Struct s ON s.PAS_CODE = m.PAS_NUMBER
		LEFT JOIN 
			vw_users a ON a.SSN = m.SSAN
 
		 WHERE 
			m.FIRST_NAME +'  '+  m.LAST_NAME   LIKE '%'+IsNuLL (@Name,m.FIRST_NAME +'  '+  m.LAST_NAME)+'%' 
			 
		 AND
			m.SSAN LIKE '%'+ IsNull(@ssn,m.SSAN) +'%'   
		AND
			ISNUMERIC(m.SSAN) = 1
		AND
		(
			SUBSTRING(m.SSAN,1,1) Not LIKE '%' + CHAR(10) AND
			SUBSTRING(m.SSAN,1,1) Not LIKE '%' + CHAR(13)		
		)
		AND 
				 CASE 
				 WHEN @srchUnit IS NULL THEN s.cs_id  
				 ELSE @srchUnit 
				 END
				 =s.cs_id  
		  	 ORDER BY m.LAST_NAME
GO

