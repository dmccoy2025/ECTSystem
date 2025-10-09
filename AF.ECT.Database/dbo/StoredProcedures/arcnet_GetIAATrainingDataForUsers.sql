
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 5/8/2017
-- Description:	Returns information regarding a user's IAA training date.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2017
-- Description:		- Fixed a bug preventing users with a NULL middle name from
--					from being selected. 
-- ============================================================================
CREATE PROCEDURE [dbo].[arcnet_GetIAATrainingDataForUsers] 
	@ediPIN VARCHAR(100),
	@lastName VARCHAR(100),
	@firstName VARCHAR(100),
	@middleNames VARCHAR(100),
	@beginDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	IF (@ediPIN IS NOT NULL AND dbo.TRIM(@ediPIN) = '')
		SET @ediPIN = NULL

	IF (@lastName IS NOT NULL AND dbo.TRIM(@lastName) = '')
		SET @lastName = NULL

	IF (@firstName IS NOT NULL AND dbo.TRIM(@firstName) = '')
		SET @firstName = NULL

	IF (@middleNames IS NOT NULL AND dbo.TRIM(@middleNames) = '')
		SET @middleNames = NULL

	IF (ISNULL(@beginDate, '') = '')
		SET @beginDate = NULL

	IF (ISNULL(@endDate, '') = '')
		SET @endDate = NULL


	DECLARE @SelectedUsers TABLE
	(
		UserId INT,
		EDIPIN VARCHAR(100),
		SSN CHAR(9),
		Username VARCHAR(100),
		LastName VARCHAR(100),
		FirstName VARCHAR(100),
		MiddleNames VARCHAR(100),
		ExpirationDate DATETIME
	)

	DECLARE @Results TABLE
	(
		UserId INT,
		EDIPIN VARCHAR(100),
		SSN CHAR(9),
		Username VARCHAR(100),
		Name VARCHAR(500),
		ECT_IAAExpirationDate VARCHAR(100),
		ARCNET_IAACompletionDate VARCHAR(100),
		ARCNET_IAAExpirationDate VARCHAR(100)
	)

	INSERT	INTO	@SelectedUsers ([UserId], [EDIPIN], [SSN], [Username], [LastName], [FirstName], [MiddleNames], [ExpirationDate])
			SELECT	u.userID, u.EDIPIN, u.SSN, u.username, u.LastName, u.FirstName, u.MiddleName, u.expirationDate
			FROM	core_Users u
			WHERE	u.EDIPIN = ISNULL(@ediPIN, u.EDIPIN) 
					AND u.LastName LIKE '%' + ISNULL(@lastName, u.LastName) + '%'
					AND u.FirstName LIKE '%' + ISNULL(@firstName, u.FirstName) + '%'
					AND ISNULL(u.MiddleName, '') LIKE '%' + ISNULL(@middleNames, ISNULL(u.MiddleName, '')) + '%'
					AND u.expirationDate BETWEEN ISNULL(@beginDate, u.expirationDate) AND ISNULL(@endDate, u.expirationDate)

	INSERT	INTO	@Results ([UserId], [SSN], [EDIPIN], [Username], [Name], [ECT_IAAExpirationDate], [ARCNET_IAACompletionDate], [ARCNET_IAAExpirationDate])
			SELECT	su.userID, 
					su.SSN,
					su.EDIPIN, 
					su.username, 
					su.LastName + ' ' + su.FirstName + ' ' + ISNULL(su.MiddleNames, '') AS FullName, 
					CONVERT(VARCHAR(100), CONVERT(DATE, su.ExpirationDate), 101) AS ECT_IAAExpirationDate, 
					CONVERT(VARCHAR(100), CAST(arc.completionDate AS DATE), 101) AS ARCNET_IAACompletionDate, 
					CONVERT(VARCHAR(100), CAST(arc.dueDate AS DATE), 101) AS ARCNET_IAAExpirationDate
			FROM	@SelectedUsers su
					JOIN ALOD_ARCNET_RAW arc ON su.EDIPIN = arc.edipi

	INSERT	INTO	@Results ([UserId], [SSN], [EDIPIN], [Username], [Name], [ECT_IAAExpirationDate], [ARCNET_IAACompletionDate], [ARCNET_IAAExpirationDate])
			SELECT	su.userID, 
					su.SSN,
					su.EDIPIN, 
					su.username, 
					su.LastName + ' ' + su.FirstName + ' ' + ISNULL(su.MiddleNames, '') AS FullName, 
					CONVERT(VARCHAR(100), CONVERT(DATE, su.ExpirationDate), 101) AS ECT_IAAExpirationDate, 
					CONVERT(VARCHAR(100), CAST(arc.completionDate AS DATE), 101) AS ARCNET_IAACompletionDate, 
					CONVERT(VARCHAR(100), CAST(arc.dueDate AS DATE), 101) AS ARCNET_IAAExpirationDate
			FROM	@SelectedUsers su
					LEFT JOIN ALOD_ARCNET_RAW arc ON (dbo.TRIM(ISNULL(su.SSN, '')) <> '' AND dbo.TRIM(ISNULL(arc.ssn, '')) <> '' AND su.SSN = arc.ssn)
			WHERE	su.UserId NOT IN (SELECT r.UserId FROM @Results r)	


	SELECT	*
	FROM	@Results r
	ORDER	BY r.ECT_IAAExpirationDate
END
GO

