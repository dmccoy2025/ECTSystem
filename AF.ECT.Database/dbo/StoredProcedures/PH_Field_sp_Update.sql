
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Updates an existing PH Field record in the PH_Field table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Field_sp_Update]
	@id INT,
	@name NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@id, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@name, '') = '')
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	-- Make sure the PH Field being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	PH_Field f
	WHERE	f.Id = @id
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	-- Make sure a PH Field with this name does not already exist...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	PH_Field f
	WHERE	f.Name = @name
			AND f.Id <> @id
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	BEGIN TRANSACTION
	
		UPDATE	PH_Field
		SET		Name = @name
		WHERE	Id = @id

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

