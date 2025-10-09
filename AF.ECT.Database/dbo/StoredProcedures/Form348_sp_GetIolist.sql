
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/15/2015	
-- Description:		Cleaned up and formatted the stored procedure. Ordered the
--					final selection statement by Name. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/11/2015	
-- Description:		Changed the check against the PAS field to be against the 
--					PAS_NUMBER field.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/28/2016
-- Description:		Removed the criteria where the selected users have to have
--					the IO role already assigned to them.
--					Added conditions based on the grade/rank of the selected
--					users and members.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/10/2017
-- Description:		Modified to no longer select MemberData records that have
--					been marked as deleted.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/10/2017
-- Description:		o Modified to allow users/members of the same rank as the
--					case member's rank to be selected for the IO pick list.
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_sp_GetIolist]
	@userId INT,
	@rptView TINYINT,
	@caseMemberGradeCode INT
AS
BEGIN
	DECLARE @userCsid INT,
			@generalGradeCode INT = 0

	SET @userCsid = (SELECT unit_id FROM vw_Users WHERE userId=@userId)  

	DECLARE @Names TABLE 
	(
		Value		VARCHAR(100),
		FullName	VARCHAR(100),
		Rank		NVARCHAR(10)
	)

	-- Find the code for the General rank. It is the highest rank
	SELECT	@generalGradeCode = g.CODE
	FROM	core_lkupGrade g
	WHERE	g.TITLE = 'General'

	-- Check if the member's grade is within the enlisted ranks which have a code greater than the code for the officers
	-- in the core_lkupGrade table...
	IF (@caseMemberGradeCode > @generalGradeCode)
	BEGIN
		SET @caseMemberGradeCode = 0
	END

	-- Find Users...
	INSERT	INTO	@Names(Value,FullName,Rank)
			SELECT	
					DISTINCT('uid:' + cast(c.userId as VARCHAR(20))) AS Value, 
					c.lastname + ', ' + c.firstname AS FullName, 
					g.RANK AS Rank
			FROM	
					core_Users c
					INNER JOIN dbo.core_lkupGrade AS g ON g.CODE = c.rank_code                   
					LEFT OUTER JOIN dbo.core_UserRoles AS b ON b.userId = c.userId  
			WHERE	
					ISNULL(c.ada_cs_id, c.cs_id) IN (
														SELECT child_id FROM Command_Struct_Tree WHERE  parent_id=@userCsid and view_type=@rptView
													)
					AND c.accessStatus = 3
					AND g.Grade IN ('03','04','05','06','07','08','09','010') -- Captain through General
					AND g.CODE >= @caseMemberGradeCode -- User's rank is greater than or equal to the member of the case


	-- Find Members...
	INSERT	INTO	@Names(Value,FullName,Rank)
			SELECT	
					DISTINCT('ssn:'+ m.SSAN) AS Value, 
					m.last_name + ', ' + m.first_name AS FullName, 
					g.RANK AS Rank
			FROM	
					MemberData m
					LEFT JOIN core_lkupGrade AS g ON g.code=m.GR_CURR
			WHERE	
					ISNULL(m.Deleted, 0) = 0
					AND PAS_NUMBER IN (
											SELECT child_pas  FROM Command_Struct_Tree WHERE  parent_id=@userCsid and view_type=@rptView
									  )
					AND g.Grade IN ('03','04','05','06','07','08','09','010') -- Captain through General
					AND g.CODE >= @caseMemberGradeCode					-- Member's rank is greater than or equal to the rank of the member of the case
					AND (m.SSAN IS NOT NULL)
					AND (LEN(m.SSAN) <> 0) 
					AND (ISNUMERIC(m.SSAN) = 1)  
					AND m.last_name + ', ' + m.first_name NOT IN(SELECT FullName FROM @Names)


	-- Select results...
	SELECT	
			Value, 
			n.Rank + ' ' + n.FullName AS Name 
	FROM	
			@Names n
	ORDER BY 
			n.FullName ASC
END
GO

