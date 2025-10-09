-- ============================================================================
-- Author:			?
-- Create date:		??/??/????
-- Description:		Selects all of a user's special cases
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/11/2015	
-- Description:		Added the rptView statements. Now takes into
--					account the user's report view when selecting
--					cases. 		
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetSpecialCases]
	@userId int
 

AS

--Declare @userId int

--SET @userId = 295  -- 469 is system admin, 295 is MPF  

DECLARE @roleId int = 0
Select @roleId = currentRole from core_Users where userID = @userId

DECLARE @unitView int = 1
SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

DECLARE @rptView int, @userSSN CHAR(9)
SELECT @rptView = ViewType, @userSSN = SSN FROM vw_users WHERE userID = @userId

IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
BEGIN
    SET @userSSN = NULL
END

-- user id not found, must have been passed in a role id
If @roleId = 0
	Set @roleId = @userId

	DECLARE @Compo int
	SET @Compo = dbo.GetCompoForUser(@userId)

Select distinct sc_id
	, SubString(fsc.member_ssn, 6, 4) As Protected_SSN
	, fsc.member_name As Member_Name
	, fsc.member_unit As Unit_Name
	, fsc.Case_Id
	, vws.description As Status
	, mods.moduleName As Module
	, Convert(char(11), ISNULL(t.ReceiveDate, fsc.created_date), 101) As Receive_Date
	, DateDiff(d, ISNULL(t.ReceiveDate, fsc.created_date), GetDate()) As Days
From Form348_SC fsc
--	Inner Join Form348 lod On lod.lodId = fsc.InitialLodId
	Inner Join vw_WorkStatus vws On fsc.status = vws.ws_id
	Inner Join core_lkupModule mods On mods.moduleId = fsc.Module_Id
	Inner Join core_UserRoles cur On 
		cur.groupId = vws.groupId
	Left Join vw_users vu On vu.userID = cur.userID
	LEFT JOIN (
		SELECT Max(startDate) ReceiveDate, ws_id, refId 
		FROM core_WorkStatus_Tracking 
		GROUP BY ws_id, refId) t 
			ON t.refId = fsc.sc_id AND t.ws_id = fsc.status
Where 
		cur.userRoleID = @roleId
		AND fsc.Member_Compo = @Compo
		AND
		(
			vu.accessScope > 1
			OR
			dbo.fn_IsUserTheMember(@userId, fsc.member_ssn) = 0	-- Don't return cases which revolve around the user doing the search...
		)
		AND
		(
			(
				@unitView = 1
				AND (
						fsc.Member_Unit_Id In 
						(
							Select child_id From Command_Struct_Tree where parent_id = vu.unit_id and view_type = @rptView
						)
						Or
						vu.accessScope > 1
					)
			)
			
			OR
			(
				@unitView = 0
				AND (fsc.Member_Unit_Id = vu.unit_id)
			)
		)
GO

