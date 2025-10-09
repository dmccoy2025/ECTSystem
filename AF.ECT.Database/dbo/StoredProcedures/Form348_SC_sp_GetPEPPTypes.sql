
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/12/2016
-- Description:	Returns the PEPP Types associated with the specified special
--				case reference Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_SC_sp_GetPEPPTypes]
	 @refId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (@refId IS NULL)
	BEGIN
		RETURN
	END
	
	SELECT	map.TypeId AS Id, t.typeName AS Name
	FROM	Form348_SC_PEPP_Type map
			JOIN core_lkupPEPPType t ON map.TypeId = t.typeId
	WHERE	RefId = @refId
END
GO

