
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/20/2016
-- Description:	Saves the stored results associated with the userId and 
--				report title parameters.
-- ============================================================================

CREATE PROCEDURE [dbo].[report_sp_SaveStoredResult]
	@userId INT,
	@reportTitle NVARCHAR(100),
	@resultData NVARCHAR(MAX)
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

	DECLARE @count INT = 0

	-- Check if these results need to be inserted or if an existing record needs to be updated...
	SELECT	@count = COUNT(*)
	FROM	Report_StoredResult sr
	WHERE	sr.UserId = @userId
			AND sr.ReportTitle = @reportTitle

	IF (@count > 0) -- Results already stored for this user and this report...
	BEGIN
		UPDATE	Report_StoredResult
		SET		ResultData = @resultData
		WHERE	UserId = @userId
				AND ReportTitle = @reportTitle
	END
	ELSE
	BEGIN
		INSERT	INTO	Report_StoredResult ([UserId], [ReportTitle], [ResultData])
				VALUES	(@userId, @reportTitle, @resultData)
	END
END
GO

