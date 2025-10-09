
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 5/8/2017
-- Description:	Return all of the Process as options
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_Process]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	id AS [id], title AS [description]
	FROM	dbo.core_lkupProcess process
END
GO

