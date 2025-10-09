
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Return all of the information sources
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_InfoSources]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	info.source_description AS [Description], info.source_id As [id], info.sort_order As [sort_order]
	FROM	dbo.core_lkupInfoSource info
	ORDER	BY info.sort_order ASC
END
GO

