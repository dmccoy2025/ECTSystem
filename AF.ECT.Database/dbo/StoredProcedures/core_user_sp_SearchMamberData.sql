--exec core_user_sp_SearchMamberData 1,'',Smith,0,7

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/4/2015
-- Work Item:		TFS User Story 120
-- Description:		Altered the stored procedure to get the last, first, and
--					middle names the member as individual parameters. Modified
--					where where clause to use the three new parameters instead
--					of combining the first and last name into a single string.  
-- ============================================================================
 CREATE PROCEDURE [dbo].[core_user_sp_SearchMamberData]
	  @userId int
	, @ssn varchar(20) = null
	, @lastName VARCHAR(50)= null
	, @firstName VARCHAR(50) = null
	, @middleName VARCHAR(60) = null	
	, @srchUnit int = null
	, @rptView int 		 
AS
BEGIN

	SET NOCOUNT ON;
	
	IF (@lastName) = '' SET @lastName = NULL
	IF (@firstName) = '' SET @firstName = NULL
	IF (@middleName) = '' SET @middleName = NULL
	IF (@ssn = '') SET @ssn = NULL
	IF (@srchUnit = 0) SET @srchUnit = NULL
 
	
	Declare @userUnit int, @groupId tinyint
	SELECT @userUnit = unit_id, @groupId = groupId from vw_Users where userId=@userId
	
	DECLARE @middleIncluded BIT = 1
	
	IF @middleName IS NULL SET @middleIncluded = 0


	SELECT 
		 RIGHT(m.SSAN, 4) LastFour
		,m.FIRST_NAME FirstName, m.LAST_NAME LastName, m.MIDDLE_NAMES MiddleName, m.gr_curr as Grade, s.LONG_NAME + ' ('+s.PAS_CODE +')' CurrentUnitName 
		,a.role RoleName,IsNull(a.userId,0) Id, a.accessStatus Status ,s.cs_id,m.svc_comp  as Component,m.pas_number as pas_code,m.SSAN
	FROM 
		memberdata   m
	LEFT JOIN 
		Command_Struct s ON s.PAS_CODE = m.PAS_NUMBER
	LEFT JOIN 
		vw_users a ON a.SSN = m.SSAN

	WHERE 
		m.LAST_NAME LIKE '%' + IsNull(@lastName, m.LAST_NAME) + '%'
		AND m.FIRST_NAME LIKE '%' + IsNull(@firstName, m.FIRST_NAME) + '%'
		--AND m.MIDDLE_NAMES LIKE '%' + IsNull(@middleName, m.MIDDLE_NAMES) + '%'
		AND
		(
			(
				@middleIncluded = 1
				AND m.MIDDLE_NAMES LIKE '%' + IsNull(@middleName, m.MIDDLE_NAMES) + '%'
			)
			OR
			(
				@middleIncluded = 0
				AND 1 = 1
			)
		)
		 
		AND
			m.SSAN LIKE '%'+ IsNull(@ssn,m.SSAN) +'%'   
		AND
			ISNUMERIC(m.SSAN) = 1  -- ALOD 1149
		AND
		(   -- ALOD 1149 as well
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
END
GO

