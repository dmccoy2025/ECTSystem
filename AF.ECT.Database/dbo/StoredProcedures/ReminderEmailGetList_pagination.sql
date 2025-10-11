-- ============================================================================
-- Author:		Kamal Singh
-- Create date: ?
-- Description:	Gets the list of emails to send reminder emails to.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addresses.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/1/2016
-- Description:		Updated to no longer use a hard coded values for the LOD
--					statuses.
-- ============================================================================
-- Modified By:		GitHub Copilot
-- Modified Date:	2025-10-11
-- Description:		Added pagination parameters and logic.
-- ============================================================================
CREATE PROCEDURE [dbo].[ReminderEmailGetList_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN

	SELECT	DISTINCT emails.id,
			emails.caseId,
			users.email,
			DATEDIFF(day, emails.lastModifiedDate, getdate()) AS daysPending,
			module.moduleName,
			setting.templateId
	FROM	ReminderEmailSettings			AS setting
			INNER JOIN ReminderEmails		AS emails	ON setting.id = emails.settingId
			INNER JOIN core_Workflow		AS workflow	ON setting.workflowId = workflow.workflowId
			INNER JOIN vw_WorkStatus		AS ws		ON setting.wsId = ws.ws_id
			INNER JOIN core_UserRoles		AS roles	ON setting.groupId = roles.groupId
			INNER JOIN vw_users				AS viewUser ON roles.userID = viewUser.userID
			INNER JOIN core_lkupModule		AS module	ON workflow.moduleId = module.moduleId
			INNER JOIN core_UserGroups		AS groups	ON setting.groupId = groups.groupId
			INNER JOIN core_Users			AS users	ON  viewUser.userId = users.userId
			INNER JOIN Command_Struct_Tree	AS CST		ON viewUser.unit_id = CST.parent_id AND CST.child_id = emails.member_unit_id AND CST.view_type = groups.reportView
	WHERE	users.ReceiveReminderEmail = 1
			AND users.accessStatus = 3
			AND DATEDIFF(day, emails.lastSentDate, getdate()) >= setting.intervalTime
			AND ws.description <> 'Formal Investigation'

	UNION

	SELECT	emails.id,
			emails.caseId,
			users.email,
			DATEDIFF(day, emails.lastModifiedDate, getdate()) AS daysPending,
			module.moduleName,
			setting.templateId
	FROM	ReminderEmailSettings		AS setting
			INNER JOIN ReminderEmails	AS emails	ON setting.id = emails.settingId
			INNER JOIN core_Workflow	AS workflow ON setting.workflowId = workflow.workflowId
			INNER JOIN vw_WorkStatus	AS ws		ON setting.wsId = ws.ws_id
			INNER JOIN Form348			AS LOD		ON emails.caseID = LOD.case_Id
			INNER JOIN core_Users		AS users	ON LOD.io_uid = users.userId
			INNER JOIN core_lkupModule	AS module	ON workflow.moduleId = module.moduleId
	WHERE	users.ReceiveReminderEmail = 1
			AND users.accessStatus = 3
			AND DATEDIFF(day, emails.lastSentDate, getdate()) >= setting.intervalTime
			AND module.moduleId = 2
			AND setting.groupId = 14
			AND ws.description = 'Formal Investigation'
	ORDER BY id
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY

END
GO