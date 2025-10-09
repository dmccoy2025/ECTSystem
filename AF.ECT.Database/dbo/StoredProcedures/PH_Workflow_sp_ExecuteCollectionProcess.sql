
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/18/2016
-- Description:	Marks all of the open cases created last month as delinquent.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to be able to handle multiple delinquent work 
--					statuses associated with different workflows.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/4/2017
-- Description:		o Modified to update the tracking information for all of the
--					cases moved to the delinquent step. 
--					o Modifed to only move PH cases currently in the Unit PH
--					step. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/13/2017
-- Description:		Pass in the workflow id when updating tracking
-- ============================================================================

CREATE PROCEDURE [dbo].[PH_Workflow_sp_ExecuteCollectionProcess]
	@year INT,
	@previousMonth INT,
	@systemIPAddress VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @currentRefId INT,
			@currentOldWorkStatusId INT,
			@currentWorkflowId TINYINT,
			@phModuleId INT = 23,
			@workflow INT = 25,
			@overrideActionId INT = 6,
			@systemAdminUserId INT = 1,
			@newWorkStatus INT = NULL,
			@updatedCasesCount INT = 0,
			@actionMessage VARCHAR(100) = 'Automatic System Override Status Action'
	
	DECLARE @ReferenceIds TABLE
	(
		Id INT,
		OldWorkStatusId INT,
		WorkflowId TINYINT
	)

	DECLARE @ExemptStatuses TABLE
	(
		WorkStatusId INT,
		WorkflowId INT
	)

	DECLARE @DelinquentWorkStatus TABLE
	(
		WorkStatusId INT,
		WorkflowId INT
	)

	DECLARE RefIdsCursor CURSOR FOR SELECT	r.Id, r.OldWorkStatusId, r.WorkflowId
									FROM	@ReferenceIds r


	INSERT	INTO	@DelinquentWorkStatus ([WorkStatusId], [WorkflowId])
			SELECT	ws.ws_id, ws.workflowId
			FROM	core_StatusCodes sc
					JOIN core_WorkStatus ws ON sc.statusId = ws.statusId
			WHERE	sc.description IN ('PH Delinquent')

	INSERT	INTO	@ExemptStatuses ([WorkStatusId], [WorkflowId])
			SELECT	ws.ws_id, ws.workflowId
			FROM	core_StatusCodes sc
					JOIN core_WorkStatus ws ON sc.statusId = ws.statusId
			WHERE	sc.description IN ('PH AFRC Review')
	
	IF ((SELECT COUNT(*) FROM @DelinquentWorkStatus) = 0)
	BEGIN
		RETURN
	END

	-- FIND ALL OF THE IDS FOR CASES CREATED LAST MONTH WHICH HAVE NOT BEEN MOVED TO THE HQ AFRC DPH STEP --
	INSERT	INTO	@ReferenceIds ([Id], [OldWorkStatusId], [WorkflowId])
			SELECT	s.SC_Id, s.status, s.workflow
			FROM	Form348_SC s
					JOIN core_WorkStatus ws ON s.status = ws.ws_id
					JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
			WHERE	YEAR(s.ph_reporting_period) = @year					-- Was the reporting period in the same year as the year for the previous month...
					AND MONTH(s.ph_reporting_period) = @previousMonth	-- Was the reporting period in the same month as the previous month...
					AND sc.isFinal <> 1									-- Case has not been closed out...
					AND ISNULL(s.is_delinquent, 0) = 0					-- Case isn't already marked as delinquent...
					AND s.status NOT IN (SELECT e.WorkStatusId FROM @ExemptStatuses e)	-- Case isn't in one of the exempt statuses


	-- MARK ALL OF THE CASES FOUND AS DELIQUENT & SEND THEM TO THE DELIQUENT STATUS --
	UPDATE	Form348_SC
	SET		status = (SELECT TOP 1 ds.WorkStatusId FROM @DelinquentWorkStatus ds WHERE ds.WorkflowId = workflow),
			is_delinquent = 1
	WHERE	SC_Id IN (SELECT Id FROM @ReferenceIds)
	

	SET @updatedCasesCount = (SELECT COUNT(*) FROM @ReferenceIds)

	-- SELECT ALL OF THE CASES WHICH WERE UPDATED --
	SELECT	Id
	FROM	@ReferenceIds

	OPEN RefIdsCursor

	FETCH NEXT FROM RefIdsCursor INTO @currentRefId, @currentOldWorkStatusId, @currentWorkflowId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @newWorkStatus = (SELECT TOP 1 ds.WorkStatusId FROM @DelinquentWorkStatus ds WHERE ds.WorkflowId = @currentWorkflowId)

		-- If more than one case was updated, then we need to update the tracking for each case
		-- because the Form348_SC update trigger does not handle BULK updates of its records
		IF (@updatedCasesCount > 1)
		BEGIN
			EXEC workstatus_sp_UpdateTracking @currentRefId, @newWorkStatus, @phModuleId, @systemAdminUserId, @workflow
		END

		EXEC core_log_sp_RecordAction @phModuleId, @overrideActionId, @systemAdminUserId, @currentRefId, @actionMessage, @currentOldWorkStatusId, 0, @newWorkStatus, @systemIPAddress

		FETCH NEXT FROM RefIdsCursor INTO @currentRefId, @currentOldWorkStatusId, @currentWorkflowId
	END

	CLOSE RefIdsCursor
	DEALLOCATE RefIdsCursor
END
GO

