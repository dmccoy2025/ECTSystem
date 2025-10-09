
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 2/23/2017
-- Description:	Selects user emails for workflow  
-- ============================================================================

CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByWorkflow]
	@refId INT,
	@status SMALLINT,
	@workflowId INT,
	@groupId INT
AS

IF @workflowId In (1, 27)
BEGIN			
	
	DECLARE @workstatusUserGroupId INT = 0,
			@isFinal BIT = 1

	SELECT	@workstatusUserGroupId = sc.groupId, @isFinal = sc.isFinal
	FROM	core_WorkStatus ws
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	ws.ws_id = @status

	IF (@workstatusUserGroupId = @groupId)
	BEGIN
		EXEC core_user_sp_GetMailingListByStatus_LOD @refId, @status
	END
	ELSE
	BEGIN
		EXEC core_user_sp_GetMailingListByGroup @refId, @groupId, 'LOD', @isFinal
	END


END
ELSE IF @workflowId = 5
BEGIN
	
	EXEC core_user_sp_GetMailingListByStatus_RR @refId, @status

END		
ELSE IF @workflowId = 26
BEGIN
	
	EXEC core_user_sp_GetMailingListByStatus_AP @refId, @status

END	
ELSE IF @workflowId = 28
BEGIN
	
	EXEC core_user_sp_GetMailingListByStatus_SARC @refId, @status

END
ELSE IF @workflowId = 29
BEGIN
	
	EXEC core_user_sp_GetMailingListByStatus_APSA @refId, @status

END
ELSE
BEGIN

	EXEC core_user_sp_GetMailingListByStatus_SC @refId, @status

END
GO

