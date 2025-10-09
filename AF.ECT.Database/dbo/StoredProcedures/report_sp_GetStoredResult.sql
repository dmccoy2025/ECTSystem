
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/20/2016
-- Description:	Returns the stored results associated with the userId and 
--				report title parameters.
-- ============================================================================

CREATE PROCEDURE [dbo].[report_sp_GetStoredResult]
	@userId INT,
	@reportTitle NVARCHAR(100)
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		RETURN
	END

	IF (ISNULL(@reportTitle, '') = '')
	BEGIN
		RETURN
	END

	SELECT	sr.ResultData
	FROM	Report_StoredResult sr
	WHERE	sr.UserId = @userId
			AND sr.ReportTitle = @reportTitle
END
GO

