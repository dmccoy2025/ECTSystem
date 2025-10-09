
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/5/2017
-- Description:	Inserts a new record into the core_LogDebugMessage table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Debug_sp_InsertLog]
	@message NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@message, '') = '')
	BEGIN
		RETURN
	END
	
	INSERT	INTO	core_LogDebugMessage ([CreatedDate], [Message])
			VALUES	(GETDATE(), @message)
END
GO

