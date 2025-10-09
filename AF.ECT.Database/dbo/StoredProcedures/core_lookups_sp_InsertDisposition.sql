
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/18/2016
-- Description:	Inserts a new Disposition record into the core_lkupDisposition
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_InsertDisposition]
	@newDispositionName NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@newDispositionName, '') = '')
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_lkupDisposition d
	WHERE	d.Name = @newDispositionName
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	INSERT	INTO core_lkupDisposition ([Name])
			VALUES (@newDispositionName)
END
GO

