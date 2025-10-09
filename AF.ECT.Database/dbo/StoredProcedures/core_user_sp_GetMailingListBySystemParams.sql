
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/23/2015
-- Description:	Selects distinct user email addresses based on different pieces
--				of criteria. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015	
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addressed.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListBySystemParams]
	@includeWork BIT = 0,
	@includePersonal BIT = 0,
	@includeUnit BIT = 0,
	@userGroupList tblIntegerList READONLY
AS

DECLARE @includeAllUserGroups BIT = 0

DECLARE @Results TABLE
(
	EmailAddress VARCHAR(200)
)

IF (SELECT COUNT(*) FROM @userGroupList) = 0
	BEGIN
	SET @includeAllUserGroups = 1
	END
	
IF @includeWork = 1
	BEGIN
	
	INSERT	INTO @Results (EmailAddress)
	SELECT	u.Email
	FROM	vw_users u
			JOIN core_UserRoles ur ON u.userID = ur.userID
	WHERE	ISNULL(u.Email, '') <> ''
			AND u.AccessStatusDescr = 'Approved'
			AND (@includeAllUserGroups = 1 OR ur.groupId IN (SELECT n FROM @userGroupList))
	
	END
	
IF @includePersonal = 1
	BEGIN
	
	INSERT	INTO @Results
	SELECT	u.Email2
	FROM	vw_users u
			JOIN core_UserRoles ur ON u.userID = ur.userID
	WHERE	ISNULL(u.Email2, '') <> ''
			AND u.AccessStatusDescr = 'Approved'
			AND (@includeAllUserGroups = 1 OR ur.groupId IN (SELECT n FROM @userGroupList))
	
	END
	
IF @includeUnit = 1
	BEGIN
	
	INSERT	INTO @Results
	SELECT	u.Email3
	FROM	vw_users u
			JOIN core_UserRoles ur ON u.userID = ur.userID
	WHERE	ISNULL(u.Email3, '') <> ''
			AND u.AccessStatusDescr = 'Approved'
			AND (@includeAllUserGroups = 1 OR ur.groupId IN (SELECT n FROM @userGroupList))
	
	END
	
SELECT	DISTINCT EmailAddress
FROM	@Results
GO

