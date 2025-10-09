-- ============================================================================
-- Author:		Steve Kennedy
-- Create date: 5/15/2014
-- Description:	Displays Disposition step in My Cases page
--				Modified version of core_lod_sp_GetSpecialCasesByModuleId				
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/11/2015	
-- Description:		Added the rptView and unitView statements. Now takes into
--					account the user's report view and unit view when selecting
--					cases. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/6/2015	
-- Description:		Modified the stored procedure to use the new isDisposition
--					field in the core_StatusCode table. Removed the @status
--					parameter.
--					Replaced this old WHERE clause expression:
--						And fsc.status = @status
--					with:
--						And vws.isDisposition = 1
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/18/2016
-- Description:		Added the selection of the case lock lockId. 
--					Cleaned up the stored procedure.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/8/2016
-- Description:		Ordred the results by the days column from
--					oldest to newest.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	4/3/2017
-- Description:		return the workflow title
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/18/2019
-- Description:		1. Added sorting by pas code priority using dbo.core_lkupPAS
--					2. Syntax cleanup
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/25/2019
-- Description:		Added column 'Priority' to Special Cases
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetSpecialCaseDisposition]
	@moduleId INT,	
	@userId INT
AS

DECLARE @roleId INT = 0
SELECT @roleId = currentRole FROM core_Users WHERE userID = @userId

DECLARE @unitView INT = 1
SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

DECLARE @rptView INT, @userSSN CHAR(9)
SELECT @rptView = ViewType, @userSSN = SSN FROM vw_users WHERE userID = @userId

IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
BEGIN
    SET @userSSN = NULL
END

DECLARE @DefPriority int
SET @DefPriority = 0

DECLARE @Compo int
SET @Compo = dbo.GetCompoForUser(@userId)

-- user id not found, must have been passed in a role id
IF(@roleId = 0)
BEGIN
	SET @roleId = @userId
END

SELECT	DISTINCT sc_id
		, SUBSTRING(fsc.member_ssn, 6, 4) AS Protected_SSN
		, fsc.member_name AS Member_Name
		, fsc.member_unit AS Unit_Name
		, fsc.Case_Id
		, vws.description AS Status
		, mods.moduleName AS Module
		, CONVERT(char(11), ISNULL(t.ReceiveDate, fsc.created_date), 101) AS Receive_Date
		, DATEDIFF(d, ISNULL(t.ReceiveDate, fsc.created_date), GETDATE()) AS Days
		, Fast_Track_Type AS Sub_Type
		, ISNULL(l.id, 0) lockId
		, (CASE fsc.sub_workflow_type WHEN 0 Then w.title ELSE sub.subTypeTitle END) AS workflow_title
		, (
			ISNULL(pas.priority, @DefPriority) + DATEDIFF(d, ISNULL(t.ReceiveDate, fsc.created_date), GETDATE())
		) as pas_priority	
		, CASE 
			WHEN pas.priority >= 60000 THEN 'P1'
			WHEN pas.priority BETWEEN 50000 and 59999 THEN 'P2'
			WHEN pas.priority BETWEEN 40000 and 49999 THEN 'P3'
			ELSE ''
		END AS PriorityRank
FROM	Form348_SC fsc
		INNER JOIN vw_WorkStatus vws		ON fsc.status		= vws.ws_id
		INNER JOIN core_lkupModule mods		ON mods.moduleId	= fsc.Module_Id
		INNER JOIN core_UserRoles cur		ON cur.groupId		= vws.groupId
		LEFT JOIN vw_users vu				ON vu.userID		= cur.userID
		LEFT JOIN (
			SELECT	MAX(startDate) ReceiveDate, 
					ws_id, 
					refId 
			FROM core_WorkStatus_Tracking 
			GROUP BY ws_id, refId
		) t									ON t.refId			= fsc.sc_id AND t.ws_id = fsc.status
		LEFT JOIN core_WorkflowLocks l		ON l.refId			= fsc.SC_Id AND l.module = @moduleId
		LEFT JOIN core_lkupSCSubType sub	ON sub.subTypeId	= fsc.sub_workflow_type
		JOIN core_Workflow w				ON W.workflowId		= fsc.workflow
		INNER JOIN Command_Struct AS CS		ON CS.CS_ID			= fsc.Member_Unit_Id	-- added for 125
		LEFT JOIN core_lkupPAS pas			ON pas.pas			= cs.PAS_CODE			-- added for 125
WHERE	(cur.userRoleID = @roleId)
		AND (fsc.Member_Compo = @Compo)
		AND (fsc.Module_Id = @moduleId)
		AND (vws.isDisposition = 1)
		AND (
			vu.accessScope > 1
			OR
			dbo.fn_IsUserTheMember(@userId, fsc.member_ssn) = 0				-- Don't return cases which revolve around the user doing the search...
		)
		AND (
			(
				@unitView = 1
				AND (
					fsc.Member_Unit_Id IN
					(
						SELECT child_id FROM Command_Struct_Tree WHERE parent_id = vu.unit_id AND view_type = @rptView
					)
					OR
					vu.accessScope > 1
				)
			)
			OR (
				@unitView = 0
				AND (fsc.Member_Unit_Id = vu.unit_id)
			)
		)
ORDER BY pas_priority DESC
GO

