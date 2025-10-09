
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Updates an existing PH Section record in the PH_Section table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_Update]
	@id INT,
	@name NVARCHAR(100),
	@parentId INT,
	@fieldColumns INT,
	@isTopLevel BIT,
	@displayOrder INT,
	@hasPageBreak BIT
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
	
	IF (@fieldColumns = 0)
	BEGIN
		RETURN
	END
	
	IF (@hasPageBreak IS NULL)
	BEGIN
		SET @hasPageBreak = 0
	END
	
	IF (@parentId = 0)
	BEGIN
		SET @parentId = NULL
	END
	
	DECLARE @count INT = 0
	
	-- Make sure the PH Section being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	PH_Section s
	WHERE	s.Id = @id
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	-- Make sure a PH Section with this name does not already exist...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	PH_Section s
	WHERE	s.Name = @name
			AND s.Id <> @id
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	DECLARE @oldDisplayOrder INT = 1
	
	SELECT	@oldDisplayOrder = DisplayOrder
	FROM	PH_Section s
	WHERE	s.Id = @id
	
	BEGIN TRANSACTION

		UPDATE	PH_Section
		SET		Name = @name,
				ParentId = @parentId,
				FieldColumns = @fieldColumns,
				IsTopLevel = @isTopLevel,
				DisplayOrder = @displayOrder,
				PageBreak = @hasPageBreak
		WHERE	Id = @id

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		DECLARE @error INT = 0
		
		-- Update the display order for the other Sections...
		EXEC @error = PH_Section_sp_UpdateDisplayOrders @id, @parentId, @oldDisplayOrder, @displayOrder, @isTopLevel, 0
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

