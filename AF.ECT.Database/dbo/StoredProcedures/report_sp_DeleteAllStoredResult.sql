
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/20/2016
-- Description:	Returns the stored results associated with the userId and 
--				report title parameters.
-- ============================================================================

CREATE PROCEDURE [dbo].[report_sp_DeleteAllStoredResult]
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE	FROM	Report_StoredResult
END
GO

