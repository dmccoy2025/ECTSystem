
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/16/2015
-- Description:	Performs a full seach of the Reinvestigation Request cases.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		Workstatus now takes in integers
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_RR_sp_FullSearch]
		  @caseID varchar(50)= null
		, @ssn varchar(9)= null
		, @lastName varchar(50)= null
		, @firstName varchar(50)= null
		, @middleName varchar(60)= null
		, @status INT = null
		, @userId int
		, @rptView tinyint  
		, @compo varchar(10) = null
		, @maxCount int = null
		, @moduleId tinyint= null
		, @unitId int = null
		, @overridescope bit = null
AS

IF  @caseID = '' SET @caseID=null 
IF  @ssn = '' SET @ssn=null 
IF  @lastName = '' SET @lastName=null 	
IF  @firstName = '' SET @firstName=null 	
IF  @middleName = '' SET @middleName=null 	
IF  @compo = '' SET @compo=null 
IF  @status = 0 SET @status=null
IF  @maxCount = 0 SET @maxCount=null
IF  @moduleId = 0 SET @moduleId=null
IF  @unitId = 0 SET @unitId=null	
 
DECLARE @userUnit AS INT, @scope TINYINT, @userSSN CHAR(9)
SELECT @userUnit = unit_id, @scope = accessscope, @userSSN = SSN FROM vw_Users WHERE userId=@userId	 

DECLARE @middleIncluded BIT = 1
IF @middleName IS NULL SET @middleIncluded = 0

IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
BEGIN
    SET @userSSN = NULL
END

IF @overridescope IS NOT NULL
BEGIN 
	SET @scope=3 
	SET @unitId=NULL
	SET @userSSN = NULL
END 

SELECT	 vw.RefId
		,vw.CaseId  
		,vw.MemberFullName AS Name
		,RIGHT(vw.MemberSSN, 4) AS SSN 
		,vw.Compo
		,vw.Pascode
		,vw.WorkStatusId
		,vw.WorkStatus
		,vw.IsFinal
		,vw.WorkflowId
		,vw.Workflow
		,vw.ModuleId
		,convert(char(11), vw.CreatedDate, 101) DateCreated
		,vw.UnitName
		,convert(char(11), ISNULL(vw.ReceiveDate, vw.CreatedDate), 101) ReceiveDate
		,datediff(d, ISNULL(vw.ReceiveDate, vw.CreatedDate), getdate()) Days
		,vw.lockId
		,Convert(varchar, vw.RefId) + ', ' + CONVERT(varchar, vw.ModuleId) As PrintId
FROM	vw_rr_Search vw
WHERE 
(   
	vw.MemberLastName LIKE '%' + IsNull(@lastName, vw.MemberLastName) + '%'
	AND vw.MemberFirstName LIKE '%' + IsNull(@firstName, vw.MemberFirstName) + '%'
	AND
	(
		(
			@middleIncluded = 1
			AND vw.MemberMiddleNames LIKE '%' + IsNull(@middleName, vw.MemberMiddleNames) + '%'
		)
		OR
		(
			@middleIncluded = 0
			AND 1 = 1
		)
	)
	AND
		vw.CaseId LIKE IsNull (@caseId,vw.CaseId) + '%'
	AND  
		vw.MemberSSN LIKE '%' + IsNull(@ssn,vw.MemberSSN) 
 	AND
		CASE 
			WHEN @status IS NULL THEN vw.WorkStatusId
			ELSE @status 
		END
		= vw.WorkStatusId
	AND  
		vw.Compo LIKE '%'+ IsNull(@compo,vw.Compo) +'%' 
 	AND  
	(
		-- global scope
		(
			@scope = 3
			AND
			CASE 
				WHEN @unitId IS NULL THEN vw.MemberUnitId
				ELSE @unitId 
			END
			= vw.MemberUnitId
		)
		OR
		(
			-- component scope
			@scope = 2
			AND
			CASE 
				WHEN @unitId IS NULL THEN vw.MemberUnitId
				ELSE @unitId 
			END
			= vw.MemberUnitId
		)
		OR
		-- unit scope
		(
			@scope = 1  
			AND
			(
				vw.MemberUnitId  IN 				 
				(	
					SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView 
				) 
			)
			AND
				vw.MemberSSN <> ISNULL(@userSSN, '---')	-- Don't return cases which revolve around the user doing the search...
		)
	)
	AND  	 
		vw.WorkStatusId in 			 
		(
			SELECT ws_id FROM vw_WorkStatus  WHERE (moduleId = IsNull(@moduleId,moduleId) )
		)
)
ORDER BY vw.CaseId
GO

