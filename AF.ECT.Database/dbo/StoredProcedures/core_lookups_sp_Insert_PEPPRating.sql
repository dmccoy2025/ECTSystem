-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 6/18/2014
-- Description:	Inserts a new record into or updates an existing record in core_lkupPEPPRating
-- =============================================
CREATE PROCEDURE [dbo].[core_lookups_sp_Insert_PEPPRating] 
	@id INT = NULL,
	@ratingName VARCHAR(50) = '',
	@active bit = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@id IS NOT NULL) AND (@id <> 0)
		BEGIN
			UPDATE	core_lkupPEPPRating
			SET		ratingName = @ratingName, active = @active
			WHERE	ratingId = @id
		
		END
	 	
	ELSE
		DECLARE @name VARCHAR(50)
			
		SELECT	@name = ratingName 
		FROM	core_lkupPEPPRating
		WHERE	ratingName = @ratingName	
		
		IF @name is null
			BEGIN
				INSERT INTO core_lkupPEPPRating(ratingName)
				VALUES(@ratingName)
			END

END
GO

