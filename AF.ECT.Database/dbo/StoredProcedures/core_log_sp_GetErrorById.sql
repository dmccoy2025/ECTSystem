-- =============================================
-- Author:		Nick McQuillen
-- Create date: 4/10/2008
-- Description:	Select the errorlog
-- Note: Temp, will revise with sorting and paging
-- =============================================
CREATE PROCEDURE [dbo].[core_log_sp_GetErrorById]

@ID as int

AS 

BEGIN

	SET NOCOUNT ON;
	SELECT 
		appVersion, browser, message, stackTrace, caller
	FROM
		core_LogError
	WHERE 
		logId=@ID 
END
GO

