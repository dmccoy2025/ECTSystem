-- ============================================================================
-- Author: Eric Kelley
-- Create date: 23 Aug 2021
-- Description: Displays all cases except Holding step in PSID WorkFlow
-- ============================================================================

CREATE PROCEDURE [dbo].[core_lod_sp_GetPSCDSpecialCaseNotHolding]
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

SELECT DISTINCT sc_id
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
WHEN pas.priority >= 60000 THEN 'P1a'
WHEN pas.priority BETWEEN 50000 and 59999 THEN 'P1b'
WHEN pas.priority BETWEEN 40000 and 49999 THEN 'P2'
ELSE ''
END AS PriorityRank
FROM Form348_SC fsc
INNER JOIN vw_WorkStatus vws ON fsc.status = vws.ws_id
INNER JOIN core_lkupModule mods ON mods.moduleId = fsc.Module_Id
INNER JOIN core_UserRoles cur ON cur.groupId = vws.groupId
LEFT JOIN vw_users vu ON vu.userID = cur.userID
LEFT JOIN (
SELECT MAX(startDate) ReceiveDate, ws_id, refId
FROM core_WorkStatus_Tracking
GROUP BY ws_id, refId
) t ON t.refId = fsc.sc_id AND t.ws_id = fsc.status
LEFT JOIN core_WorkflowLocks l ON l.refId = fsc.SC_Id AND l.module = @moduleId
LEFT JOIN core_lkupSCSubType sub ON sub.subTypeId = fsc.sub_workflow_type
JOIN core_Workflow w ON W.workflowId = fsc.workflow
INNER JOIN Command_Struct AS CS ON CS.CS_ID = fsc.Member_Unit_Id -- added for 125
LEFT JOIN core_lkupPAS pas ON pas.pas = cs.PAS_CODE -- added for 125
Where (cur.userRoleID = @roleId)
AND (fsc.Module_Id = @moduleId)
AND (fsc.Member_Compo = @Compo)
AND (
vu.accessScope > 1
OR
dbo.fn_IsUserTheMember(@userId, fsc.member_ssn) = 0 -- Don't return cases which revolve around the user doing the search...
)
--AND (
--(
--@unitView = 1
AND (
fsc.Member_Unit_Id IN
(
SELECT child_id FROM Command_Struct_Tree WHERE parent_id = vu.unit_id AND view_type = @rptView
)
--OR
--vu.accessScope > 1
--)
)
--OR
--(
--@unitView = 0
--AND (fsc.Member_Unit_Id = vu.unit_id)
--)
--)
AND (vws.isHolding = 0)

ORDER BY pas_priority DESC


--[dbo].[core_lod_sp_GetPSIDSpecialCaseNotHolding] 30, 122
GO

