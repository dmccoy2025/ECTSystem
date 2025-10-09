
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/28/2016
-- Description:	Performs a full seach of the Appeal Request cases.
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/24/2017
-- Description:	Workstatus now takes in an integer
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_AP_sp_FullSearch]
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
 
DECLARE @userUnit AS INT, @scope TINYINT
SELECT @userUnit = unit_id, @scope = accessscope FROM vw_Users WHERE userId=@userId	 

DECLARE @middleIncluded BIT = 1
IF @middleName IS NULL SET @middleIncluded = 0
 
IF @overridescope IS NOT NULL
BEGIN 
	SET @scope=3 
	SET @unitId=NULL
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
FROM	vw_ap_Search vw
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

