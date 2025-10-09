
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/10/2016
-- Description:	Copies all of the memos associated with a workflow case to 
--				another workflow case (either a Reinvestigation Case or a 
--				Special Case). 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_memo_sp_copy_memos2_to_memos2]
	@originalRefId INT,
	@newRefId INT,
	@moduleId INT,
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	--------------------
	-- VALIDATE INPUT --
	--------------------
	IF (ISNULL(@originalRefId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	IF (ISNULL(@newRefId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	IF (ISNULL(@moduleId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	DECLARE @count INT = 0
	DECLARE @rrModuleId INT = 0
	DECLARE @currentMemoId INT = 0
	DECLARE @newMemoId INT = 0
	
	DECLARE MemosCursor CURSOR FOR SELECT m.memoId FROM core_Memos2 m WHERE m.refId = @originalRefId AND m.deleted = 0
	
	-- Check if any memos associated with the specified refId and module actually exist...
	SELECT	@count = COUNT(*)
	FROM	core_Memos2 m
	WHERE	m.refId = @originalRefId
	
	IF (ISNULL(@count, 0) = 0)
	BEGIN
		RETURN 1
	END
	
	-- Check to make sure the new refId actually exists...
	--SET @count = 0
	
	--SELECT	@rrModuleId = m.moduleId
	--FROM	core_lkupModule m
	--WHERE	m.moduleName = 'Reinvestigation Request'
	
	--IF (ISNULL(@rrModuleId, 0) = 0)
	--BEGIN
	--	RETURN 0
	--END
	
	--IF (@moduleId = @rrModuleId)
	--BEGIN
	--	-- Reinvestigation Request case...
	--	SELECT	@count = COUNT(*)
	--	FROM	Form348_RR r
	--	WHERE	r.request_id = @newRefId
	--END
	
	--IF (@moduleId > @rrModuleId)
	--BEGIN
	--	-- Special Case...
	--	SELECT	@count = COUNT(*)
	--	FROM	Form348_SC s
	--	WHERE	s.SC_Id = @newRefId
	--END
	
	--IF (ISNULL(@count, 0) = 0)
	--BEGIN
	--	RETURN 0
	--END
	
	
	-----------------------------
	-- INSERT COPIED MEMO DATA --
	-----------------------------
	SET @currentMemoId = 0

	OPEN MemosCursor

	FETCH NEXT FROM MemosCursor INTO @currentMemoId

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		--PRINT CONVERT(nvarchar(10), @currentMemoId)
		BEGIN TRANSACTION
			INSERT	INTO	core_Memos2 ([refId], [moduleId], [templateId], [deleted], [letterHead], [created_by], [created_date])
					SELECT	@newRefId, @moduleId, m.templateId, m.deleted, m.letterHead, m.created_by, m.created_date
					FROM	core_Memos2 m
					WHERE	m.memoId = @currentMemoId
			
			IF (@@ERROR <> 0)
			BEGIN
				ROLLBACK TRANSACTION
				BREAK
			END
			
			SET @newMemoId = SCOPE_IDENTITY()
			
			INSERT	INTO	core_MemoContents2 ([memoId], [body], [sigBlock], [suspenseDate], [memoDate], [created_date], [created_by], [attachments])
					SELECT	@newMemoId, mc.body, mc.sigBlock, mc.suspenseDate, mc.memoDate, mc.created_date, mc.created_by, mc.attachments
					FROM	core_MemoContents2 mc
					WHERE	mc.memoId = @currentMemoId
					
			IF (@@ERROR <> 0)
			BEGIN
				ROLLBACK TRANSACTION
				BREAK
			END
		COMMIT TRANSACTION
		
		FETCH NEXT FROM MemosCursor INTO @currentMemoId
	END

	CLOSE MemosCursor
	DEALLOCATE MemosCursor
	
	RETURN 1
END
GO

