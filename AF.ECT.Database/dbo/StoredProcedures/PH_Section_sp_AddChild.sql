
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/29/2016
-- Description:	Sets the parentId of the specified child PH_Section record.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_AddChild]
	@parentId INT,
	@childId INT,
	@displayOrder INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@parentId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@childId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@displayOrder IS NULL)
	BEGIN
		SET @displayOrder = 1
	END
	
	BEGIN TRANSACTION
	
		UPDATE	PH_Section
		SET		ParentId = @parentId,
				DisplayOrder = @displayOrder
		WHERE	Id = @childId

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		DECLARE @error INT = 0
		
		-- Update the display order for the other Sections...
		EXEC @error = PH_Section_sp_UpdateDisplayOrders @childId, @parentId, @displayOrder, @displayOrder, 0, 1
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

