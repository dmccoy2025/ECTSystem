-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 1/12/2017
-- Description:	Attempts to find the ID of a member's user account.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMembersUserId]
	@memberSSN VARCHAR(11)
AS
BEGIN
	IF (dbo.TRIM(@memberSSN) = '' OR NOT EXISTS (SELECT * FROM MemberData m WHERE m.SSAN = @memberSSN))
		RETURN 0

	DECLARE @userId INT = 0,
			@memberFirstName VARCHAR(50) = NULL,
			@memberLastName VARCHAR(50) = NULL,
			@memberMiddleNames VARCHAR(60) = NULL,
			@useMiddleInitial BIT = 0


	-- First attempt to find a user via their SSN...
	-- NOTE: As of 1/12/2017 not all user accounts have their SSN associated with their account. 
	SELECT	@userId = u.userID
	FROM	core_Users u
	WHERE	ISNULL(u.SSN, '') <> ''
			AND u.SSN = @memberSSN


	IF (@userId > 0)
	BEGIN
		RETURN @userId
	END
	ELSE
	BEGIN
		-- If a user could not be found via SSN then attempt to find a user via the member's name...
		-- NOTE: This is a temporary implementation given that names are not a unique identifier for people...
		SELECT	@memberFirstName = m.FIRST_NAME,
				@memberLastName = m.LAST_NAME,
				@memberMiddleNames = m.MIDDLE_NAMES
		FROM	MemberData m
		WHERE	m.SSAN = @memberSSN

		IF (ISNULL(@memberFirstName, '') = '' OR ISNULL(@memberLastName, '') = '')
			RETURN @userId

		IF (ISNULL(@memberMiddleNames, '') <> '')
		BEGIN
			-- Attempt to find a user with member's first, last, and middle names
			SELECT	@userId = u.userID
			FROM	core_Users u
			WHERE	ISNULL(u.MiddleName, '') <> ''
					AND u.LastName = @memberLastName
					AND u.FirstName = @memberFirstName
					AND u.MiddleName = @memberMiddleNames

			IF (@userId > 0)
			BEGIN
				RETURN @userId
			END

			-- Attempt to find a user with member's first, last, and middle initial names
			SELECT	@userId = u.userID
			FROM	core_Users u
			WHERE	ISNULL(u.MiddleName, '') <> ''
					AND u.LastName = @memberLastName
					AND u.FirstName = @memberFirstName
					AND SUBSTRING(u.MiddleName, 1, 1) = SUBSTRING(@memberMiddleNames, 1, 1)

			IF (@userId > 0)
			BEGIN
				RETURN @userId
			END
		END

		-- Attempt to find a user with member's first and last names
		SELECT	@userId = u.userID
		FROM	core_Users u
		WHERE	u.LastName = @memberLastName
				AND u.FirstName = @memberFirstName
	
		RETURN @userId
	END
END
GO

