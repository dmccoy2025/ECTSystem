
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Inserts a new PH Field record into the PH_Field table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Field_sp_Insert]
	@name NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@name, '') = '')
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	-- Make sure a PH Field with this name does not already exist...
	SELECT	@count = COUNT(*)
	FROM	PH_Field f
	WHERE	f.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	BEGIN TRANSACTION
	
		INSERT	INTO	PH_Field ([Name])
				VALUES (@name)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

