
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/23/2015
-- Description:	Updates a record in the Form348_PostCompletion table which 
--				matches the lodId parameter. If no such record exists then a
--				new record is inserted into the table. 
-- ============================================================================
-- Edited By:	Evan Morrison
-- Date:		12/29/2016
-- Description:	Add notification date to the Post processing table
-- ============================================================================
-- Edited By:	Evan Morrison
-- Date:		3/22/2017
-- Description:	Add email and flags to the Post processing table
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_UpdateLODPostCompletion]
	@lodId					INT,
	@helpExtensionNumber	NVARCHAR(50),
	@appealStreet			NVARCHAR(200),
	@appealCity				NVARCHAR(100),
	@appealState			NVARCHAR(50),
	@appealZip				NVARCHAR(100),
	@appealCountry			NVARCHAR(50),
	@nokFirstName			NVARCHAR(50),
	@nokLastName			NVARCHAR(50),
	@nokMiddleName			NVARCHAR(50),
	@notificationDate		DATETIME,
	@email					NVARCHAR(200),
	@chkAddress				BIT,
	@chkEmail				BIT,
	@chkPhone				BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--------------------
	-- VALIDATE INPUT --
	--------------------
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
	
	IF (@nokFirstName = '')
	BEGIN
		SET @nokFirstName = NULL
	END
	
	IF (@nokLastName = '')
	BEGIN
		SET @nokLastName = NULL
	END
	
	IF (@nokMiddleName = '')
	BEGIN
		SET @nokMiddleName = NULL
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
	FROM	Form348_PostProcessing fpp
	WHERE	fpp.lodId = @lodId
	
	-- If no records are found then insert a new one...
	IF (ISNULL(@count, 0) = 0)
	BEGIN
		INSERT	INTO Form348_PostProcessing ([lodId], [helpExtensionNumber], [appealStreet], [appealCity], [appealState], [appealZip], [appealCountry], [nokFirstName], [nokLastName], [nokMiddleName], [notification_date], [email], [email_flag], [address_flag], [phone_flag])
				VALUES (@lodId, @helpExtensionNumber, @appealStreet, @appealCity, @appealState, @appealZip, @appealCountry, @nokFirstName, @nokLastName, @nokMiddleName, @notificationDate, @email, @chkEmail, @chkAddress, @chkPhone)
	END
	
	-- If a single record was found then update it...
	IF (ISNULL(@count, 0) = 1)
	BEGIN
		UPDATE	Form348_PostProcessing
		SET		helpExtensionNumber = @helpExtensionNumber,
				appealStreet = @appealStreet,
				appealCity = @appealCity,
				appealState = @appealState,
				appealZip = @appealZip,
				appealCountry = @appealCountry,
				nokFirstName = @nokFirstName,
				nokLastName = @nokLastName,
				nokMiddleName = @nokMiddleName,
				notification_date = @notificationDate,
				email = @email,
				email_flag = @chkEmail,
				address_flag = @chkAddress,
				phone_flag = @chkPhone
		WHERE	lodId = @lodId
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

