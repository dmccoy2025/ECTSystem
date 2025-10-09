
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/8/2016
-- Description:	Updates an existing Cancel Reasons record in the 
--				core_lkupCancelReason table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_UpdateCancelReasons]
	@CancelReasonsId INT,
	@newCancelReasonsDescription NVARCHAR(100),
	@newCancelReasonsDisplayOrder INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@CancelReasonsId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@newCancelReasonsDescription, '') = '')
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@newCancelReasonsDisplayOrder, 0) = 0)
	BEGIN
		RETURN
	END
	
	-- Check if a cancel reasons record with the specified description already exists....
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_lkupCancelReason d
	WHERE	d.Description = @newCancelReasonsDescription AND d.Id != @CancelReasonsId
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	-- Update record...
	UPDATE	core_lkupCancelReason
	SET		Description = @newCancelReasonsDescription, 
			DisplayOrder = @newCancelReasonsDisplayOrder 
	WHERE	Id = @CancelReasonsId
END
GO

