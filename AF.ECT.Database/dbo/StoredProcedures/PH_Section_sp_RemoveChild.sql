
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/29/2016
-- Description:	Removes the parentId from the specified child PH_Section record. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_RemoveChild]
	@parentId INT,
	@childId INT
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
	
	DECLARE @displayOrder INT = 1
	DECLARE @isTopLevel BIT = 0
	
	SELECT	@displayOrder = DisplayOrder, @isTopLevel = s.IsTopLevel
	FROM	PH_Section s
	WHERE	s.Id = @childId
	
	BEGIN TRANSACTION
	
		UPDATE	PH_Section
		SET		ParentId = NULL,
				DisplayOrder = 1
		WHERE	Id = @childId
				AND ParentId = @parentId

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		DECLARE @error INT = 0
		
		-- Update the display order for the other Sections...
		EXEC @error = PH_Section_sp_UpdateDisplayOrders @childId, @parentId, @displayOrder, @displayOrder, @isTopLevel, 2
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

