
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		8/2/2016
-- Description:		Updates a record in the Form348_PostCompletion_Appeal 
--					table which matches the appealId parameter. If no such record 
--					exists then a new record is inserted into the table. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	6/5/2017
-- Description:		Added help extension number and email to post processing data
-- ============================================================================


CREATE PROCEDURE [dbo].[core_lod_sp_UpdateAppealPostCompletion]
	@appealId				INT,
	@lodId					INT,
	@appealStreet			NVARCHAR(200),
	@appealCity				NVARCHAR(100),
	@appealState			NVARCHAR(50),
	@appealZip				NVARCHAR(100),
	@appealCountry			NVARCHAR(50),
	@memberNotificationDate	Date,
	@helpExtensionNumber	NVARCHAR(50),
	@email					NVARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--------------------
	-- VALIDATE INPUT --
	--------------------
	IF (ISNULL(@appealId, 0) = 0)
	BEGIN
		RETURN 0
	END

	IF (ISNULL(@lodId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	IF (@helpExtensionNumber = '')
	BEGIN
		SET @helpExtensionNumber = NULL
	END
	
	IF (@appealStreet = '')
	BEGIN
		SET @appealStreet = NULL
	END
	
	IF (@appealCity = '')
	BEGIN
		SET @appealCity = NULL
	END
	
	IF (@appealState = '')
	BEGIN
		SET @appealState = NULL
	END
	
	IF (@appealZip = '')
	BEGIN
		SET @appealZip = NULL
	END
	
	IF (@appealCountry = '')
	BEGIN
		SET @appealCountry = NULL
	END
	
	IF (@email = '')
	BEGIN
		SET @email = NULL
	END
	
	-----------------------------
	-- INSERT OR UPDATE RECORD --
	-----------------------------
	SET XACT_ABORT ON
	
	BEGIN TRANSACTION
	
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	Form348_PostProcessing_Appeal App
	WHERE	App.appeal_id = @appealId
	
	-- If no records are found then insert a new one...
	IF (ISNULL(@count, 0) = 0)
	BEGIN
		INSERT	INTO Form348_PostProcessing_Appeal ([appeal_id], [initial_lod_id], [appeal_street], [appeal_city], [appeal_state], [appeal_zip], [appeal_country], [member_notification_date], [helpExtensionNumber], [email] )
						VALUES (@appealId, @lodId, @appealStreet, @appealCity, @appealState, @appealZip, @appealCountry, @memberNotificationDate, @helpExtensionNumber, @email)
	END
	
	-- If a single record was found then update it...
	IF (ISNULL(@count, 0) = 1)
	BEGIN
		UPDATE	Form348_PostProcessing_Appeal
		SET		appeal_street = @appealStreet,
				appeal_city = @appealCity,
				appeal_state = @appealState,
				appeal_zip = @appealZip,
				appeal_country = @appealCountry,
				member_notification_date = @memberNotificationDate,
				helpExtensionNumber = @helpExtensionNumber,
				email = @email
		WHERE	appeal_id = @appealId
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

