-- =============================================
-- Author:		Nick McQuillen
-- Create date: 4/10/2008
-- Description:	Select the errorlog
-- =============================================
CREATE PROCEDURE [dbo].[core_log_sp_GetErrors]
	
	
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		a.logId, a.errorTime, a.userName, a.page, a.message
	FROM
		core_LogError a
	ORDER BY
		a.errorTime desc

--	SELECT 
--		a.logId, a.errorTime, a.userId, 
--			CASE userId WHEN 0 Then 'Unknown' 
--			  ELSE
--			(SELECT lastName FROM vw_users b WHERE b.userId = a.userId) END AS userName,
-- a.page , a.message
--	FROM
--		core_LogError a
--	ORDER BY
--		a.errorTime desc

END
GO

