
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/14/2017
-- Description:	Returns the date the ARCNET Import procedure was last run.
-- ============================================================================
CREATE PROCEDURE [dbo].[arcnet_GetLastExecutionDate] 
AS
BEGIN
	SELECT	TOP 1 ISNULL(pl.endtime, pl.starttime) AS LastExecutionDate
	FROM	pkgLog pl
	WHERE	pl.pkgName = 'ARCNETIMPORT'
	ORDER	BY starttime DESC
END
GO

