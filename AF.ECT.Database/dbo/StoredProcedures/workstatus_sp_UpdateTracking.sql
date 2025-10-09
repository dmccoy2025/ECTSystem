--DECLARE @refId int, @module tinyint, @workstatus int, @userId int
--
--SET @refId = 13
--SET @workStatus = 1
--SET @userId = 3
--SET @module = 2


-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/13/2017
-- Description:		Pass in the workflow id when updating tracking, and
--					update tracking with user rank and name
-- ============================================================================


CREATE PROCEDURE [dbo].[workstatus_sp_UpdateTracking]
	@refId int,
	@workStatus int,
	@module tinyint,
	@userId int,
	@workflow INT

AS

DECLARE @trackId int, @curStatus int,  @newTrackId int 
--get current status
SELECT TOP 1 @trackId = wst_id, @curStatus = ws_id 
FROM core_workstatus_tracking 
WHERE refId = @refId AND module = @module
ORDER BY wst_id DESC 



DECLARE @rank INT, @nameAndRank VARCHAR(100)
SELECT TOP 1 @rank = rank_code, @nameAndRank = u.FirstName + ' ' + (CASE WHEN u.MiddleName is NULL THEN '' WHEN u.MiddleName = '' THEN '' ELSE LEFT(u.MiddleName, 1) + ' ' END) +  u.LastName
FROM core_Users u
where u.userID = @userId

--SELECT @trackId, @curStatus

--if the refId/module does not exist, add a new tracking entry
--if it does exist, check the status
--if the status is the same, do nothing
--if the status is different, close the current status and start a new entry

IF (@trackId IS NULL)
		BEGIN
				--there is no record for this refId/module, enter a new one
				INSERT INTO core_workstatus_tracking 
				(ws_id, refId, module, workflowId)
				VALUES
				(@workstatus, @refId, @module, @workflow)
				
			 
		END
ELSE
IF (@curStatus <> @workStatus)
		BEGIN
			--this refId module exists and has a new status
			UPDATE core_workstatus_tracking SET endDate = getDate(), completedBy = @userId, rank = @rank, name = @nameAndRank WHERE wst_id = @trackId

			INSERT INTO core_workstatus_tracking 
			(ws_id, refId, module, workflowId)
			VALUES
			(@workstatus, @refId, @module, @workflow)
			 
		END 
		
	SELECT TOP 1 @newTrackId = wst_id 
	FROM core_workstatus_tracking 
	WHERE refId = @refId AND module = @module
	ORDER BY wst_id DESC 
--If a new reocrd was inserted (complete or final record) 
	IF (@newTrackId IS NOT NULL)
	BEGIN 
		DECLARE @isFinal bit
		SET @isFinal= (SELECT isFinal from vw_WorkStatus where ws_id =@workstatus)
		IF  ( @isFinal=1 )
			BEGIN 
				UPDATE core_workstatus_tracking SET endDate = getDate(), completedBy = @userId, rank = @rank, name = @nameAndRank WHERE wst_id = @newTrackId
			END
			
	END
GO

