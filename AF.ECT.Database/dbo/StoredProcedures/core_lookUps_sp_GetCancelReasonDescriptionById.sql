-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 10/21/2015
-- Description:	Selects the description of the cancel reason whose Id matches
--				the passed in value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetCancelReasonDescriptionById]
	@reasonId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	cr.Description
	FROM	core_lkupCancelReason cr
	WHERE	cr.Id = @reasonId
END
GO

