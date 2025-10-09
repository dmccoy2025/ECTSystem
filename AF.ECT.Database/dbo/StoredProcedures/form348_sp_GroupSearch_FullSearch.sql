
-- ============================================================================
-- Author:			Ken Barnett
-- Created Date:	8/10/2015
-- Work Item:		TFS User Story 120
-- Description:		Used to conduct a search on LOD (form348) records using a 
--					full compliment of search parameters. This stored procedure
--					was created to replace the form348_sp_GroupSearch stored
--					procedure being used on the lod\MyLods.aspx page. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2015
-- Work Item:		TFS User Story 120
-- Description:		Altered the stored procedure to make use of the new view
--					vw_lod_Search. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @caseID from 20 to 50.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/14/2017
-- Description:		Removed the ReceivedFrom field from being selected as it 
--					was causing the query to execute slowly when thousands of
--					records were being selected.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		Workstatus now takes in an integer
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/18/2019
-- Description:		1. Added sorting by pas code priority using dbo.core_lkupPAS
--					2. Syntax cleanup
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_GroupSearch_FullSearch]
		  @caseID varchar(50)=null
		, @ssn varchar(10)=null
		, @lastName varchar(50)=null
		, @firstName varchar(50)=null
		, @middleName varchar(60)=null
		, @status INT =null
		, @userId int
		, @rptView tinyint  
		, @compo varchar(10) =null
		, @maxCount int =null
		, @moduleId tinyint=null
		, @isFormal bit =null
		, @unitId int =null
		, @sarcpermission bit =null
AS

IF  @caseID ='' SET @caseID=null 
IF  @ssn ='' SET @ssn=null 
IF  @lastName ='' SET @lastName=null
IF  @firstName ='' SET @firstName=null
IF  @middleName ='' SET @middleName=null
IF  @compo ='' SET @compo=null 
IF  @status =0 SET @status=null 
IF  @maxCount =0 SET @maxCount=null
IF  @moduleId =0 SET @moduleId=null		 
IF  @unitId ='' SET @unitId=NULL
	
Declare @userUnit int, @groupId tinyint, @scope tinyint ,@canActOnsarcUnrestriced bit, @userSSN CHAR(9)
SELECT @userUnit = unit_id, @groupId = groupId, @scope = accessscope, @userSSN = SSN from vw_Users where userId=@userId

IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
BEGIN
	SET @userSSN = NULL
END

DECLARE @middleIncluded BIT = 1
IF @middleName IS NULL SET @middleIncluded = 0

DECLARE @unitView int = 1
SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

DECLARE @filter varchar(20)
SET @filter = (SELECT filter FROM core_StatusCodes WHERE statusId = @status)

SET @Compo = dbo.GetCompoForUser(@userId)

DECLARE @DefPriority int
SET @DefPriority = 0

SELECT	 vw.RefId
		, vw.CaseId
		, vw.MemberFullName AS Name
		, RIGHT(vw.MemberSSN, 4) AS SSN
		, vw.Compo
		, vw.Pascode
		, vw.WorkStatusId
		, vw.WorkStatus
		, vw.IsFinal
		, vw.WorkflowId
		, vw.Workflow
		, vw.ModuleId
		, vw.IsFormal
		, convert(char(11), vw.CreatedDate, 101) DateCreated
		, vw.UnitName
		, convert(char(11), ISNULL(vw.ReceiveDate, vw.CreatedDate), 101) ReceiveDate
		, datediff(d, ISNULL(vw.ReceiveDate, vw.CreatedDate), getdate()) Days
		, vw.lockId
		, (
			ISNULL(pas.priority, @DefPriority) + DATEDIFF(d, ISNULL(vw.ReceiveDate, vw.CreatedDate), GETDATE())
		) as pas_priority	
FROM	vw_lod_Search vw
		LEFT JOIN core_lkupPAS pas			ON pas.pas			= vw.Pascode			-- added for 125
WHERE 
(   
	(vw.Compo = @Compo)
	AND (vw.MemberLastName LIKE '%' + IsNull(@lastName, vw.MemberLastName) + '%')
	AND (vw.MemberFirstName LIKE '%' + IsNull(@firstName, vw.MemberFirstName) + '%')
	AND (
		(
			@middleIncluded = 1
			AND 
			vw.MemberMiddleNames LIKE '%' + IsNull(@middleName, vw.MemberMiddleNames) + '%'
		)
		OR (
			@middleIncluded = 0
			AND 
			1 = 1
		)
	)
	AND (vw.CaseId LIKE '%'+IsNull (@caseId,vw.CaseId)+'%')
	AND (vw.MemberSSN LIKE '%'+ IsNull(@ssn,vw.MemberSSN) +'%')
	AND (
		@scope > 1
		OR
		vw.MemberSSN <> ISNULL(@userSSN, '---')	-- Don't return cases which revolve around the user doing the search...
	)
	AND (
				CASE 
				 WHEN @unitId IS NULL THEN vw.MemberUnitId
				 ELSE @unitId 
				 END
				 = vw.MemberUnitId
					 OR
					 (
					 vw.IsAttachPAS = 1
					 AND
					CASE 
					 WHEN @unitId IS NULL THEN vw.MemberAttachedUnitId
					 ELSE @unitId 
					 END
					 = vw.MemberAttachedUnitId
					 )
	 )
	AND 
		CASE 
		 WHEN @isFormal IS NULL THEN vw.IsFormal
		 ELSE @isFormal 
		 END
		 =vw.IsFormal
				 		 
	AND
		(vw.IsDeleted = 0)
	AND
	  	CASE 
			 WHEN (vw.IsSARC = 1 and vw.IsRestricted =1) Then 1 		
			 ELSE @sarcpermission 
			 END
			  =@sarcpermission 	
	--AND
	--  	CASE 
	--		 WHEN (a.sarc = 1) Then 1 		
	--		 ELSE @sarcpermission 	
	--		 END
	--		  =@sarcpermission 
 	AND
	(  
		(
			@unitView = 1
			AND (
				vw.MemberUnitId IN
				(
					SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
				)
				OR (
					vw.IsAttachPAS = 1
					AND
					vw.MemberAttachedUnitId  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
				)
				OR (@scope > 1)
			)
		)
		OR (
			@unitView = 0
			AND (
				vw.MemberUnitId = @userUnit
				OR (
					vw.IsAttachPAS = 1
					AND
					vw.MemberAttachedUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
				)
			)
		)
		--(@scope = 3)
		--OR
		--(@scope = 2)
		--OR
		--(	
		--	@scope = 1
		--	AND
		--	(
		--		(
		--		a.member_unit_id  IN 				 
		--			(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView ) 
		--		OR
		--			(
		--			a.isAttachPAS = 1
		--			AND
		--			a.member_attached_unit_id  IN 				 
		--				(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
		--			)
		--		)
		--	)
		--)
		/*OR
		(
			a.created_by = @userId
		)*/
	)
 	AND  	 
	( 
	    vw.WorkStatusId in  (SELECT ws_id FROM vw_WorkStatus WHERE moduleId=@moduleId AND groupId = @groupId)
	   or
		vw.WorkStatusId in  (SELECT status FROM core_StatusCodeSigners where groupId = @groupId)
     )  
	AND
		(
			@userid = CASE 
						WHEN vw.Filter = 'io'   THEN vw.IOUserId 
						--WHEN s.filter = 'aa' THEN a.appAuthUserId
						ELSE @userId
					END 
	   )
  
 )
ORDER BY pas_priority DESC
GO

