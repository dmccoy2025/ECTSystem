
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/17/2016
-- Description:	Updates a record in the Form348_SARC_PostProcessing table which 
--				matches the sarcId parameter. If no such record exists then a
--				new record is inserted into the table. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified date:	6/6/2017
-- Description:		Add email data point to post completion process
-- ============================================================================

CREATE PROCEDURE [dbo].[core_sarc_sp_UpdatePostCompletion]
	@sarcId					INT,
	@memberNotified			BIT,
	@memberNotoficationDate	DATETIME, 
	@helpExtensionNumber	NVARCHAR(50) NULL,
	@appealStreet			NVARCHAR(200) NULL,
	@appealCity				NVARCHAR(100) NULL,
	@appealState			NVARCHAR(50) NULL,
	@appealZip				NVARCHAR(100) NULL,
	@appealCountry			NVARCHAR(50) NULL,
	@email					VARCHAR(200) NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--------------------
	-- VALIDATE INPUT --
	--------------------
	IF (ISNULL(@sarcId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	-----------------------------
	-- INSERT OR UPDATE RECORD --
	-----------------------------
	SET XACT_ABORT ON
	
	BEGIN TRANSACTION
	
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	Form348_SARC_PostProcessing fpp
	WHERE	fpp.sarc_id = @sarcId
	
	-- If no records are found then insert a new one...
	IF (ISNULL(@count, 0) = 0)
	BEGIN
		INSERT	INTO Form348_SARC_PostProcessing ([sarc_id], [memberNotified], [memberNotificationDate], [helpExtensionNumber], [appealStreet], [appealCity], [appealState], [appealZip], [appealCountry], [email])
				VALUES (@sarcId, @memberNotified, @memberNotoficationDate, @helpExtensionNumber, @appealStreet, @appealCity, @appealState, @appealZip, @appealCountry, @email)
	END
	
	-- If a single record was found then update it...
	IF (ISNULL(@count, 0) = 1)
	BEGIN
		UPDATE	Form348_SARC_PostProcessing
		SET		memberNotified = @memberNotified,
				memberNotificationDate = @memberNotoficationDate,
				helpExtensionNumber = @helpExtensionNumber,
				appealStreet = @appealStreet,
				appealCity = @appealCity,
				appealState = @appealState,
				appealZip = @appealZip,
				appealCountry = @appealCountry,
				email = @email
		WHERE	sarc_id = @sarcId
	END
	
	-- If multiple records were found then a problem has occured so return false...
	IF (ISNULL(@count, 0) > 1)
	BEGIN
		RETURN 0
	END
	
	IF @@ERROR <> 0
	BEGIN
		ROLLBACK TRANSACTION
		RETURN 0
	END
	
	COMMIT TRANSACTION
	
	RETURN 1
END
GO

