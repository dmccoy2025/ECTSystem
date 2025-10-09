
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/19/2016
-- Description:	Deletes a Application Warmup Process log by its Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[ApplicationWarmupProcess_sp_DeleteLogById]
	@logId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@logId, 0) = 0)
	BEGIN
		RETURN
	END
	
	DELETE	FROM	ApplicationWarmupProcessLog
			WHERE	Id = @logId
END
GO

