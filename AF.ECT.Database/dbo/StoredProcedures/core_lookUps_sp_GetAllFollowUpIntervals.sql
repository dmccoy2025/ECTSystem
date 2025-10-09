
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the core_lkupFollowUpInterval
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllFollowUpIntervals]
AS
BEGIN
	SELECT	l.Id, l.Interval
	FROM	core_lkupFollowUpInterval l
END
GO

