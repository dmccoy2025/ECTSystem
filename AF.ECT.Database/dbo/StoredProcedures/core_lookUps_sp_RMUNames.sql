
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Returns all of the records in the core_lkupRMUs table.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified date:	4/7/2016
-- Description:		Modified to now also return the PASCode of the RMU and 
--					collocated fields.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_RMUNames]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	Id As Value, RMU As Name, cs.PAS_CODE As PAS, r.collocated As Collocated
	FROM	core_lkupRMUs r
			LEFT JOIN Command_Struct cs ON r.cs_id = cs.CS_ID
	ORDER	BY RMU
END
GO

