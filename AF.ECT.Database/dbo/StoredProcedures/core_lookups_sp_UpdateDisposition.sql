
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/20/2016
-- Description:	Updates an existing Disposition record in the 
--				core_lkupDisposition table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_UpdateDisposition]
	@dispositionId INT,
	@newDispositionName NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@dispositionId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@newDispositionName, '') = '')
	BEGIN
		RETURN
	END
	
	-- Check if a disposition record with the specified name already exists....
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_lkupDisposition d
	WHERE	d.Name = @newDispositionName
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	-- Update record...
	UPDATE	core_lkupDisposition
	SET		Name = @newDispositionName 
	WHERE	Id = @dispositionId
END
GO

