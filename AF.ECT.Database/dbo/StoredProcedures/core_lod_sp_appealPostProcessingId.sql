
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 8/2/2016
-- Description:	This will return the appropriate post completion for Appeal cases,
--				given the appeal_id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_appealPostProcessingId]
	@appeal_id int
AS


SELECT distinct appeal_id,
	initial_lod_id,
	appeal_street,
	appeal_city,
	appeal_state,
	appeal_zip,
	appeal_country,
	member_notification_date
FROM [dbo].[Form348_PostProcessing_Appeal] PP
WHERE PP.appeal_id = @appeal_id
GO

