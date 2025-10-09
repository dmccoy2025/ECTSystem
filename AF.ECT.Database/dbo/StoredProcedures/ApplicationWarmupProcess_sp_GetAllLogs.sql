
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/19/2016
-- Description:	Returns all of the Application Warmup Process Log records.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/19/2016
-- Description:		Added selection of the Message field.
-- ============================================================================
CREATE PROCEDURE [dbo].[ApplicationWarmupProcess_sp_GetAllLogs]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	l.Id, p.Name, l.ExecutionDate, l.Message
	FROM	ApplicationWarmupProcessLog l
			JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
	ORDER	BY l.ExecutionDate DESC
END
GO

