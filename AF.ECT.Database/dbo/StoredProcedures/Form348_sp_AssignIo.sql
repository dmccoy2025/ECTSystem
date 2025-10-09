
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/15/2015	
-- Description:		Cleaned up and formatted the stored procedure. Fixed a few
--					bugs and inconsistencies. Removed the Form261 update
--					statement. The update is being done in code now with 
--					NHibernate. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/14/2015	
-- Description:		Added the new @isFormal input parameter which is used to
--					determine the correct appointing authority personnel type 
--					Id when getting the findings ID. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/16/2017	
-- Description:		Update name format findings table
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_sp_AssignIo]
	@refId INT,
	@ioUserId INT,
	@aaUserId INT,
	@isFormal BIT
AS

IF (ISNULL(@refId, 0) <= 0)
	BEGIN
	RAISERROR('Invalid Parameter: @refId cannot be NULL, zero, or negative.', 18, 0)
	RETURN 0
	END
	
IF (ISNULL(@ioUserId, 0) <= 0)
	BEGIN
	RAISERROR('Invalid Parameter: @ioUserId cannot be NULL, zero, or negative.', 18, 0)
	RETURN 0
	END
	
IF (ISNULL(@aaUserId, 0) <= 0)
	BEGIN
	RAISERROR('Invalid Parameter: @aaUserId cannot be NULL, zero, or negative.', 18, 0)
	RETURN 0
	END
	
IF (@isFormal IS NULL)
	BEGIN
	RAISERROR('Invalid Parameter: @isFormal cannot be NULL.', 18, 0)
	RETURN 0
	END

SET XACT_ABORT ON

BEGIN TRANSACTION
		
	DECLARE	@oldIOName		VARCHAR(100),
			@oldAAName		VARCHAR(100),
			@newIOName		VARCHAR(100),
			@newAAName		VARCHAR(100),
			@oldAAId		INT,
			@oldIOId		INT,
			@aaRecId		INT,
			@ioRecId		INT,
			@ssn			CHAR(9),
			@name			VARCHAR(256),
			@rank			VARCHAR(100),
			@grade			VARCHAR(6),
			@compo			CHAR(1),
			@pascode		VARCHAR(8),
			@status			TINYINT,
			@notes			VARCHAR(100),
			@logId			INT,
			@appAuthPType	INT
			
	-- Determine Appointing Authority personnel type (informal or formal)
	IF (@isFormal = 0)
	BEGIN
		SELECT	@appAuthPType = pt.Id
		FROM	core_lkupPersonnelTypes pt
		WHERE	pt.Description = 'APPOINT_AUTH'
	END
	ELSE
	BEGIN
		SELECT	@appAuthPType = pt.Id
		FROM	core_lkupPersonnelTypes pt
		WHERE	pt.Description = 'FORMAL_APP_AUTH'
	END
			
	-- Get the old AA data
	SELECT @oldAAName = name, @aaRecId = Id FROM FORM348_findings WHERE ptype = @appAuthPType AND LODID = @refId
	SELECT @oldIOName = name, @ioRecId = Id FROM FORM348_findings WHERE ptype = 19 AND LODID = @refId	-- 19 is IO

	-- Fetch new name, SSN, etc info for AA
	SELECT	@ssn = ssn,
			@name = FirstName + ' ' + LastName,
			@rank = RANK,
			@grade = GRADE,
			@compo = compo,
			@pascode = PAS_CODE
	FROM	vw_users
	WHERE	userID = @aaUserId
	
	IF @aaRecId IS NULL
		BEGIN
		
		INSERT INTO FORM348_findings ([LODID], [ptype], [ssn], [name], [grade], [rank], [compo], [pascode], [modified_by], [modified_date], [created_by], [created_date])
		VALUES (@refId, @appAuthPType, @ssn, @name, @grade, @rank, @compo, @pascode, @aaUserId, GETUTCDATE(), @aaUserId, GETUTCDATE())
		
		END
	ELSE
		BEGIN
		
		UPDATE	FORM348_findings
		SET		ssn = @ssn,
				name = @name,
				rank = @rank,
				grade = @grade,
				compo = @compo,
				pascode = @pascode,
				modified_by = @aaUserId,
				modified_date = GETUTCDATE()
		WHERE	ID = @aaRecId
		
		END
	
	SET @newAAName = @name
	
	-- Fetch new name, ssn, etc info for IO
	SELECT	@ssn = ssn,
			@name = FirstName + ' ' + LastName,
			@rank = RANK,
			@grade = GRADE,
			@compo = compo,
			@pascode = PAS_CODE
	FROM	vw_users
	WHERE	userID = @ioUserId
	
	IF @ioRecId IS NULL
		BEGIN
		
		INSERT INTO FORM348_findings ([LODID], [ptype], [ssn], [name], [grade], [rank], [compo], [pascode], [modified_by], [modified_date], [created_by], [created_date])
		VALUES (@refId, 19, @ssn, @name, @grade, @rank, @compo, @pascode, @aaUserId, GETUTCDATE(), @aaUserId, GETUTCDATE())
		
		END
	ELSE
		BEGIN
		
		UPDATE	FORM348_findings
		SET		ssn = @ssn,
				name = @name,
				rank = @rank,
				grade = @grade,
				compo = @compo,
				pascode = @pascode,
				modified_by = @aaUserId,
				modified_date = GETUTCDATE()
		WHERE	ID = @ioRecId
		
		END
		
	SET @newIOName = @name
	
	-- Check if the Form261 record already exists. If not then insert the record; otherwise, update the record...
	IF NOT EXISTS (SELECT lodId FROM Form261 WHERE lodId = @refId)
		BEGIN
		
		INSERT INTO Form261 ([lodId], [IoUserId], [modified_by], [modified_date])
		VALUES (@refId, @ioUserId, @aaUserId, GETUTCDATE())
		
		END
	--ELSE
	--	BEGIN
		
	--	UPDATE	Form261
	--	SET		IoUserId = @ioUserId,
	--			modified_by = @aaUserId,
	--			modified_date = GETUTCDATE()
	--	WHERE	lodId = @refId
		
	--	END
		
	-- Update the Form 348 record...
	UPDATE	Form348
	SET		appAuthUserId = @aaUserId,
			modified_by = @aaUserId,
			modified_date = GETUTCDATE()
	WHERE	lodId = @refId
	
	-- Lod the action...
	SET @status = (SELECT status FROM Form348 WHERE lodId = @refId)
	SET @notes = (
					SELECT	'Appointed IO: ' + RANK + ' ' + LastName + ', ' + FirstName
					FROM	vw_users
					WHERE	userID = @ioUserId
				 )
	
	EXEC core_log_sp_RecordAction 2, 19, @aaUserId, @refId, @notes, @status, @logId OUT, NULL, NULL
	
	INSERT INTO core_LogChangeSet ([logId], [section], [field], [old], [new])
	VALUES (@logId, 'LOD (Investigation)', 'Appointing Authority', ISNULL(@oldAAName, ''), @newAAName)
	
	INSERT INTO core_LogChangeSet ([logId], [section], [field], [old], [new])
	VALUES (@logId, 'LOD (Investigation)', 'Investigating Officer', ISNULL(@oldIOName, ''), @newIOName)

	SELECT 1
	
COMMIT TRANSACTION
GO

