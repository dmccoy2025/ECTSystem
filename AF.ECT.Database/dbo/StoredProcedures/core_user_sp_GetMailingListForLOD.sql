
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/23/2015
-- Description:	Selects user emails for a LOD workflow.  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/18/2016
-- Description:		Modified to pass in the new isFinal parameter for the call
--					to the core_user_sp_GetMailingListByGroup stored procedure.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListForLOD]
	@refId INT,
	@groupId INT,
	@status INT,
	@callingService VARCHAR(25)
AS
BEGIN
	DECLARE @workstatusUserGroupId INT = 0,
			@isFinal BIT = 1

	SELECT	@workstatusUserGroupId = sc.groupId, @isFinal = sc.isFinal
	FROM	core_WorkStatus ws
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	ws.ws_id = @status

	IF (@workstatusUserGroupId = @groupId)
	BEGIN
		EXEC core_user_sp_GetMailingListByStatus @refId, @status
	END
	ELSE
	BEGIN
		EXEC core_user_sp_GetMailingListByGroup @refId, @groupId, @callingService, @isFinal
	END
END
GO

