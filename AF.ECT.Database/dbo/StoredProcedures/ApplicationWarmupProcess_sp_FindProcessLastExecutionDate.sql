
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/30/2016
-- Description:	Returns the last time an application warmup process was 
--				executed.
-- ============================================================================
CREATE PROCEDURE [dbo].[ApplicationWarmupProcess_sp_FindProcessLastExecutionDate]
	@processName NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@processName, '') = '')
	BEGIN
		RETURN
	END
	
	DECLARE @processId INT = 0
	DECLARE @result DATETIME = NULL
	
	SELECT	@processId = awp.Id
	FROM	ApplicationWarmupProcess awp
	WHERE	awp.Name = @processName
	
	IF (ISNULL(@processId, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	TOP 1 @result = awpl.ExecutionDate
	FROM	ApplicationWarmupProcessLog awpl
	WHERE	awpl.ProcessId = @processId
	ORDER	BY awpl.ExecutionDate DESC
	
	IF (@result IS NULL)
	BEGIN
		RETURN
	END
	
	SELECT @result AS [ExecutionDate]
END
GO

