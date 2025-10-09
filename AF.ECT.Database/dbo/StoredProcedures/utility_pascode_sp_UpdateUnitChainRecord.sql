
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/7/2015
-- Description:	Based off of the core_pascode_sp_UpdateReporting stored 
--				procedure
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_pascode_sp_UpdateUnitChainRecord]
	@userId INT,		 
	@chainType NVARCHAR(20),
	@csId INT,
	@parentCSCId INT
AS

SET NOCOUNT ON;


--------------------
-- VALIDATE INPUT --
--------------------
IF (ISNULL(@userId, 0) = 0)
BEGIN
	PRINT 'userId cannot be NULL or 0...aborting stored procedure!'
	RETURN
END

IF (ISNULL(@csId, -1) = -1)
BEGIN
	PRINT 'csId cannot be NULL or -1...aborting stored procedure!'
	RETURN
END


IF (ISNULL(@parentCSCId, 0) = 0)
BEGIN
	PRINT 'userId cannot be NULL or 0...aborting stored procedure!'
	RETURN
END

IF (ISNULL(@chainType, '') = '')
BEGIN
	PRINT 'userId cannot be NULL or empty...aborting stored procedure!'
	RETURN
END



DECLARE @userName AS NVARCHAR(100)
DECLARE @count INT = 0
DECLARE @chainId INT

SET @userName = (SELECT userName FROM core_Users WHERE userID = @userId);


-- Find chain type Id...
SELECT	@chainId = ct.id
FROM	core_lkupChainType ct
WHERE	ct.name = @chainType

IF (ISNULL(@chainId, 0) = 0)
BEGIN
	PRINT 'Could not find chain type record for ' + @chainType + ' chain type name...abording stored procedure!'
	RETURN
END


-- Check if a chain record exists for the specified unit with the specified report view (chain type)
SELECT	@count = COUNT(*)
FROM	Command_Struct_Chain csc
WHERE	csc.CS_ID = @csId 
		AND csc.CHAIN_TYPE = @chainType

SET XACT_ABORT ON
BEGIN TRANSACTION


-- Update the chain record if it already exists...
IF (@count > 0)
BEGIN
	UPDATE	Command_Struct_Chain
	SET		CSC_ID_PARENT = @parentCSCId,
			MODIFIED_BY = @userName,
			MODIFIED_DATE = GETUTCDATE(),
			view_type = @chainId,
			UserModified = 1
	WHERE	CS_ID = @csId
			AND CHAIN_TYPE = @chainType
END
-- Insert chain record if it does not exist...
ELSE
BEGIN
	INSERT	INTO	Command_Struct_Chain ([CS_ID], [CHAIN_TYPE], [CSC_ID_PARENT], [MODIFIED_BY], [MODIFIED_DATE], [CREATED_BY], [CREATED_DATE], [view_type], [UserModified])
			VALUES	(@csId, @chainType, @parentCSCId, @userName, GETUTCDATE(), @userName, GETUTCDATE(), @chainId, 1)
END


COMMIT TRANSACTION
GO

