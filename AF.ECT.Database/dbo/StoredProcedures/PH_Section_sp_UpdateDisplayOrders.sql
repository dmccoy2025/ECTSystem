
-- ============================================================================
-- Author:		Ken Barnett
-- Create date:	4/15/2016
-- Description:	Updates the PH Section display order values. 
--				@filter: 0 = edit; 1 = insert; 2 = delete
--				DO = Display Order
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_UpdateDisplayOrders]
	@sectionId INT,
	@parentId INT,
	@oldOrder INT,
	@newOrder INT,
	@isTopLevel BIT,
	@filter TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- VALIDATE INPUT PARAMETERS --
	IF (@sectionId IS NULL)
	BEGIN
		RETURN 1
	END
	
	IF (@filter IS NULL)
	BEGIN
		RETURN 1
	END
	
	IF (@isTopLevel IS NULL)
	BEGIN
		RETURN 1
	END
	
	IF (@oldOrder IS NULL)
	BEGIN
		SET @oldOrder = 0
	END
	
	IF (@newOrder IS NULL)
	BEGIN
		SET @newOrder = 0
	END
	
	BEGIN TRANSACTION
	
		-- An existing PH Section record was edited....
		IF (@filter = 0)
		BEGIN
			IF (@isTopLevel = 1)
			BEGIN
				-- If the DO increased, then decrement the DO of records who have an DO between the old and new DOs (inclusive)...
				IF (@newOrder > @oldOrder)
				BEGIN
					UPDATE	PH_Section
					SET		DisplayOrder = DisplayOrder - 1
					WHERE	IsTopLevel = 1
							AND Id <> @sectionId
							AND DisplayOrder <= @newOrder
							AND DisplayOrder >= @oldOrder
				END
				
				-- If the DO decreased, then increment the DO of records who have an DO between the old and new DOs (inclusive)...
				IF (@newOrder < @oldOrder)
				BEGIN
					UPDATE	PH_Section
					SET		DisplayOrder = DisplayOrder + 1
					WHERE	IsTopLevel = 1
							AND Id <> @sectionId
							AND DisplayOrder >= @newOrder
							AND DisplayOrder <= @oldOrder
				END
			END
			ELSE
			BEGIN
				-- If the DO increased, then decrement the DO of records who have an DO between the old and new DOs (inclusive)...
				IF (@newOrder > @oldOrder)
				BEGIN
					UPDATE	PH_Section
					SET		DisplayOrder = DisplayOrder - 1
					WHERE	ParentId = @parentId
							AND Id <> @sectionId
							AND DisplayOrder <= @newOrder
							AND DisplayOrder >= @oldOrder
				END
				
				-- If the DO decreased, then increment the DO of records who have an DO between the old and new DOs (inclusive)...
				IF (@newOrder < @oldOrder)
				BEGIN
					UPDATE	PH_Section
					SET		DisplayOrder = DisplayOrder + 1
					WHERE	ParentId = @parentId
							AND Id <> @sectionId
							AND DisplayOrder >= @newOrder
							AND DisplayOrder <= @oldOrder
				END
			END
		END
		
		-- A new Form Field record was inserted...
		ELSE IF (@filter = 1)
		BEGIN
			IF (@isTopLevel = 1)
			BEGIN
				-- Increase the DO by one for records which had a DO greater than the DO of the record which was inserted...
				UPDATE	PH_Section
				SET		DisplayOrder = DisplayOrder + 1
				WHERE	IsTopLevel = 1
						AND Id <> @sectionId
						AND DisplayOrder >= @newOrder
			END
			ELSE
			BEGIN
				-- Increase the DO by one for records which had a DO greater than the DO of the record which was inserted...
				UPDATE	PH_Section
				SET		DisplayOrder = DisplayOrder + 1
				WHERE	ParentId = @parentId
						AND Id <> @sectionId
						AND DisplayOrder >= @newOrder
			END
		END
		
		-- An existing PH Section is no longer mapped to a parent PH Section...
		ELSE IF (@filter = 2)
		BEGIN
			IF (@isTopLevel = 1)
			BEGIN
				-- Decrease the DO by one for records which had a DO greater than the DO of the record which was deleted...
				UPDATE	PH_Section
				SET		DisplayOrder = DisplayOrder - 1
				WHERE	IsTopLevel = 1
						AND DisplayOrder > @oldOrder
			END
			ELSE
			BEGIN
				-- Decrease the DO by one for records which had a DO greater than the DO of the record which was deleted...
				UPDATE	PH_Section
				SET		DisplayOrder = DisplayOrder - 1
				WHERE	ParentId = @parentId
						AND DisplayOrder > @oldOrder
			END
		END
		
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 1
		END
		
	COMMIT TRANSACTION
	
	RETURN 0
END
GO

