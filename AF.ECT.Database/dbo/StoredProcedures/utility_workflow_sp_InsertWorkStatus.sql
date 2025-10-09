-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 4/11/2014
-- Description:	Insert a new record into the core_WorkStatus table.
-- =============================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertWorkStatus]
	 @ws_id int
	,@workflowTitle VARCHAR(50)
	,@statusTitle VARCHAR(50)
	,@sortOrder tinyint
AS

DECLARE @workflowId INT
DECLARE @statusId INT


SELECT @workflowId = workflowId FROM core_Workflow
WHERE title = @workflowTitle

SELECT @statusId = statusId FROM core_StatusCodes
WHERE description = @statusTitle



IF NOT EXISTS(SELECT * FROM dbo.core_WorkStatus WHERE statusId = @statusId AND workflowId = @workflowId)
	BEGIN
		SET IDENTITY_INSERT dbo.core_WorkStatus ON
		INSERT INTO [dbo].[core_WorkStatus] ([ws_id], [workflowId], [statusId], [sortOrder])
			VALUES (@ws_id, @workflowId, @statusId, @sortOrder)
		SET IDENTITY_INSERT dbo.core_WorkStatus OFF
		
		PRINT 'Inserted new values (' + CONVERT(VARCHAR(3), @ws_id) + ', ' + 
										CONVERT(VARCHAR(3), @workflowId) + ', ' +
										CONVERT(VARCHAR(3), @statusId) + ', ' +
										CONVERT(VARCHAR(2), @sortOrder) +  ') into core_WorkStatus table.'
		
		-- VERIFY NEW STATUS ID
		SELECT WS.* ,WF.title AS Workflow ,S.description AS WorkStatus
		FROM core_WorkStatus WS
		INNER JOIN core_Workflow WF
		ON WF.workflowId = WS.workflowId
		INNER JOIN core_StatusCodes S
		ON S.statusId = WS.statusId
		WHERE WS.workflowId = @workflowId
	END
ELSE
	BEGIN
		PRINT 'Values already exist in core_WorkStatus table.'
	END
GO

