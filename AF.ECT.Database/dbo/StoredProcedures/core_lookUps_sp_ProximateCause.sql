
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Return all of the Proximate Cause
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_ProximateCause]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	pc.cause_description As [description], pc.cause_id As[id], pc.sort_order AS [sort_order]
	FROM	dbo.core_lkupProximateCause pc
	ORDER	BY pc.sort_order ASC
END
GO

