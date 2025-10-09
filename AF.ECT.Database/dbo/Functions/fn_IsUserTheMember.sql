
-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 1/10/2017
-- Description:	Determines if the information passed in for a user and a member
--				represent the same individual.
--				* A zero (0) is returned if the User and Member are NOT the same
--				individual.
--				* A one (1) is returned if the User and Member are the same
--				individual.
-- ============================================================================
CREATE FUNCTION [dbo].[fn_IsUserTheMember]
(
	@userId INT,
	@memberSSN VARCHAR(11)
)
RETURNS BIT
AS
BEGIN
	IF (@userId <= 0 OR NOT EXISTS (SELECT * FROM core_Users u WHERE u.userID = @userId) OR ISNULL(@memberSSN, '') = '' OR dbo.TRIM(@memberSSN) = '' OR NOT EXISTS (SELECT * FROM MemberData m WHERE m.SSAN = @memberSSN))
		RETURN 0

	DECLARE @minimizedUserSSN VARCHAR(9) = NULL,
			@minimizedMemberSSN VARCHAR(9) = REPLACE(dbo.TRIM(@memberSSN), '-', ''),
			@userFirstName VARCHAR(100) = NULL,
			@userLastName VARCHAR(100) = NULL,
			@userMiddleName VARCHAR(100) = NULL,
			@memberFirstName VARCHAR(50) = NULL,
			@memberLastName VARCHAR(50) = NULL,
			@memberMiddleNames VARCHAR(60) = NULL,
			@useMiddleInitial BIT = 0

	SELECT	@minimizedUserSSN = REPLACE(dbo.TRIM(u.SSN), '-', ''),
			@userFirstName = u.FirstName,
			@userLastName = u.LastName,
			@userMiddleName = u.MiddleName
	FROM	core_Users u
	WHERE	u.userID = @userId


	-- If the User has an SSN then use that value as the comparison identifier...
	IF (NOT ISNULL(@minimizedUserSSN, '') = '')
	BEGIN
		IF (@minimizedUserSSN = @minimizedMemberSSN)
			RETURN 1
		ELSE
			RETURN 0
	END


	-- If the User does not have an SSN then use the name as the comparison identifier...
	-- NOTE: This is a temporary implementation given that names are not a unique identifier for people...
	SELECT	@memberFirstName = m.FIRST_NAME,
			@memberLastName = m.LAST_NAME,
			@memberMiddleNames = m.MIDDLE_NAMES
	FROM	MemberData m
	WHERE	m.SSAN = @memberSSN

	IF (ISNULL(@userFirstName, '') = '' OR ISNULL(@userLastName, '') = '' OR ISNULL(@memberFirstName, '') = '' OR ISNULL(@memberLastName, '') = '')
		RETURN 0

	IF (dbo.TRIM(ISNULL(@userMiddleName, '')) <> '' AND dbo.TRIM(ISNULL(@memberMiddleNames, '')) <> '')
		SET @useMiddleInitial = 1 

	IF (@useMiddleInitial = 1)
	BEGIN
		IF (@userFirstName = @memberFirstName AND @userLastName = @memberLastName AND SUBSTRING(@userMiddleName, 1, 1) = SUBSTRING(@memberMiddleNames, 1, 1))
			RETURN 1
	END
	ELSE
	BEGIN
		IF (@userFirstName = @memberFirstName AND @userLastName = @memberLastName)
			RETURN 1
	END
	
	RETURN 0
END
GO

