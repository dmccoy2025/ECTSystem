

--DECLARE @refId int, @module tinyint, @workstatus int, @userId int, @transDate datetime = Null
--
--SET @refId = 13
--SET @workStatus = 1
--SET @userId = 3
--SET @module = 2
--SET @transDate = '1/1/2012'


CREATE PROCEDURE [dbo].[workstatus_sp_UpdateTracking2]
	@refId int,
	@workStatus int,
	@module tinyint,
	@userId int,
	@updateType int,
	@transDate datetime = Null

AS

DECLARE @trackId int, @curStatus int,  @newTrackId int 
--get current status
SELECT TOP 1 @trackId = wst_id, @curStatus = ws_id 
FROM core_workstatus_tracking 
WHERE refId = @refId AND module = @module
ORDER BY wst_id DESC 

--SELECT @trackId, @curStatus

--if the refId/module does not exist, add a new tracking entry
--if it does exist, check the status
--if the status is the same, do nothing
--if the status is different, close the current status and start a new entry

IF (@trackId IS NULL)
		BEGIN
				--there is no record for this refId/module, enter a new one
				INSERT INTO core_workstatus_tracking 
				(ws_id, refId, module, startDate)
				VALUES
				(@workstatus, @refId, @module, Case When @updateType = 1 Then ISNULL(@transDate, GetDate()) Else GETDATE() End)
				
			 
		END
ELSE
IF (@curStatus <> @workStatus)
		BEGIN
			--this refId module exists and has a new status
			UPDATE core_workstatus_tracking SET endDate = 
			Case
				When @updateType = 2 Then
					IsNull(@transDate, GETDATE())
				Else
					getDate()
			END, completedBy = @userId WHERE wst_id = @trackId

			INSERT INTO core_workstatus_tracking 
			(ws_id, refId, module, startDate)
			VALUES
			(@workstatus, @refId, @module, Case When @updateType = 1 Then ISNULL(@transDate, GetDate()) Else GETDATE() End)
			 
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
				UPDATE core_workstatus_tracking SET endDate = 
				Case
					When @updateType = 2 Then
						IsNull(@transDate, GETDATE())
					Else
						getDate()
				END, completedBy = @userId WHERE wst_id = @newTrackId
			END
			
	END
GO

