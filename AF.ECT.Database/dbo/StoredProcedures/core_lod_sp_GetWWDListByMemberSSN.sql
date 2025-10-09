--exec [core_lod_sp_GetWWDListByMemberSSN] '010000004',1,430

CREATE PROCEDURE [dbo].[core_lod_sp_GetWWDListByMemberSSN]
		@memberSSN varchar(9)=null
		, @searchType int
		, @userId  int

AS


Declare @scope tinyint, @userUnit int
SELECT @scope = accessscope, @userUnit = unit_id from vw_Users where userId=@userId


SELECT
	 a.SC_Id as Value,
	 a.case_id  as Name  
FROM 
	Form348_SC  a
WHERE 
(   
	a.member_ssn LIKE '%' + IsNull(@memberSSN, a.member_ssn)
	AND
	a.workflow = 11  -- WWD
 	AND  
	(
		(@scope = 3)
		OR
		(@scope = 2)
		OR
		(	
			@scope = 1  
			AND
			a.member_unit_id  IN 				 
				(	SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit 
				) 
		)		 
	)
	AND
	 (@searchType = 1 
	 OR
		(
		Initial_SC Is Null
		AND
		Expiration_Date Is Not Null -- complete
		)
	)
 )
ORDER BY a.case_id
GO

