
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/30/2016
-- Description:	Inserts a new log for the specified application warmup process.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/19/2016
-- Description:		Added the @message parameter.
-- ============================================================================
CREATE PROCEDURE [dbo].[ApplicationWarmupProcess_sp_InsertLog]
	@processName NVARCHAR(100),
	@executionDate DATETIME,
	@message NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@processName, '') = '')
	BEGIN
		RETURN
	END
	
	IF (@executionDate IS NULL)
	BEGIN
		RETURN
	END
	
	DECLARE @processId INT = 0
	
	-- Find the process ID...
	SELECT	@processId = awp.Id
	FROM	ApplicationWarmupProcess awp
	WHERE	awp.Name = @processName
	
	IF (ISNULL(@processId, 0) = 0)
	BEGIN
		RETURN
	END
	
	BEGIN TRANSACTION
	
		INSERT	INTO	ApplicationWarmupProcessLog ([ProcessId], [ExecutionDate], [Message])
				VALUES (@processId, @executionDate, @message)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

