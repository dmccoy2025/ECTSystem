-- ============================================================================
-- Author:		Tim Jacobs
-- Create date: 12 Feb 2014
-- Description:	Returns a table with status id and string value
--				These values should match LodEnums
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/18/2015
-- Work Item:		TFS Task 386
-- Description:		Changed MPFInitiateRequest to InitiateRequest. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Added entries for two new LOD work statuses and one new RR
--					work status:  
--						BoardPersonnelReview
--						FormalBoardPersonnelReview
--						BoardA1RequestReview
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to no longer use the LodStatus enums values, but
--					instead to just use the WorkStatus values. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/26/2017
-- Description:		StatusId is changed from a tinyint to int
-- ============================================================================
CREATE FUNCTION [dbo].[fn_GetStatusEnums] ()

RETURNS @enumValues TABLE (statusId int Primary Key, value varchar(256))

AS
BEGIN
	INSERT	INTO	@enumValues ([statusId], [value])
			SELECT	ws.ws_id, sc.description
			FROM	core_WorkStatus ws
					JOIN core_StatusCodes sc ON ws.statusId = sc.statusId

RETURN
END
GO

