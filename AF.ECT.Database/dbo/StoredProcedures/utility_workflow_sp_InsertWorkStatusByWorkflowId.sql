-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 7/28/2016
-- Description:	Insert a new record into the core_WorkStatus table.
-- ============================================================================
-- Modified By:		Kenneth Barnett
-- Modified Date:	1/6/2017
-- Description:		Altered to now have the module ID also passed in so that it
--					can be used in the selection criteria of the status code id
--					selection
-- ============================================================================
-- Modified By:		Kenneth Barnett
-- Modified Date:	7/13/2017
-- Description:		Added the @isBoardStatus and @isHolding optional 
--					parameters.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/3/2017
-- Description:		Workstatus id is now an int
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertWorkStatusByWorkflowId]
	 @ws_id int
	,@moduleId INT
	,@workflowId INT
	,@statusTitle VARCHAR(50)
	,@sortOrder tinyint
	,@isBoardStatus BIT = 0
	,@isHolding BIT = 0
AS
BEGIN
	DECLARE @statusId INT

	SELECT	@statusId = sc.statusId
	FROM	core_StatusCodes sc
	WHERE	sc.description = @statusTitle
			AND sc.moduleId = @moduleId

	IF NOT EXISTS(SELECT * FROM dbo.core_WorkStatus WHERE statusId = @statusId AND workflowId = @workflowId)
		BEGIN
			SET IDENTITY_INSERT dbo.core_WorkStatus ON

			INSERT	INTO	[dbo].[core_WorkStatus] ([ws_id], [workflowId], [statusId], [sortOrder], [isBoardStatus], [isHolding])
					VALUES	(@ws_id, @workflowId, @statusId, @sortOrder, @isBoardStatus, @isHolding)
			
			SET IDENTITY_INSERT dbo.core_WorkStatus OFF
		
			PRINT 'Inserted new values (' + CONVERT(VARCHAR(3), @ws_id) + ', ' + 
											CONVERT(VARCHAR(3), @workflowId) + ', ' +
											CONVERT(VARCHAR(3), @statusId) + ', ' +
											CONVERT(VARCHAR(2), @sortOrder) + ', ' +
											CONVERT(VARCHAR(1), @isBoardStatus) + ', ' +
											CONVERT(VARCHAR(1), @isHolding) + ') into core_WorkStatus table.'
		
			-- VERIFY NEW STATUS ID
			SELECT	WS.* ,WF.title AS Workflow ,S.description AS WorkStatus
			FROM	core_WorkStatus WS
					INNER JOIN core_Workflow WF ON WF.workflowId = WS.workflowId
					INNER JOIN core_StatusCodes S ON S.statusId = WS.statusId
			WHERE	WS.workflowId = @workflowId
		END
	ELSE
		BEGIN
			PRINT 'Values already exist in core_WorkStatus table (' + CONVERT(VARCHAR(3), @ws_id) + ', ' + 
											CONVERT(VARCHAR(3), @workflowId) + ', ' +
											CONVERT(VARCHAR(3), @statusId) + ', ' +
											CONVERT(VARCHAR(2), @sortOrder) + ', ' +
											CONVERT(VARCHAR(1), @isBoardStatus) + ', ' +
											CONVERT(VARCHAR(1), @isHolding) + ') into core_WorkStatus table.'
		END
END
GO

