
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Return all of the Member Components
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_Component]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	component_id AS [id], component_description AS [description], sort_order AS [sort_order]
	FROM	dbo.core_lkupComponent com
	ORDER	BY com.sort_order ASC
END
GO

