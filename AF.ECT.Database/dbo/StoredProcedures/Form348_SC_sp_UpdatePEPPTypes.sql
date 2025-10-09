
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/12/2016
-- Description:	Updates the PEPP Types associated with the specified special
--				case reference Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_SC_sp_UpdatePEPPTypes]
	 @refId INT,
	 @peppTypeList tblIntegerList READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (@refId IS NULL)
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	@peppTypeList
	
	-- Delete all existing records associated with the case reference Id...
	DELETE	FROM	Form348_SC_PEPP_Type
			WHERE	RefId = @refId
	
	-- Insert all types associated with the case reference Id...
	INSERT	INTO	Form348_SC_PEPP_Type ([RefId], [TypeId])
			SELECT	@refId, t.n
			FROM	@peppTypeList t
END
GO

