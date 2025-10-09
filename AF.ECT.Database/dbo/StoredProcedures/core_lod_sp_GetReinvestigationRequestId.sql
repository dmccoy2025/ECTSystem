
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Added an ORDER BY clause to the type 1 SELECT statement. 
--					This was done because multilple RR cases can now be
--					associated with a single LOD case. This stored procedure
--					now makes the assumption that the caller wants the MOST
--					RECENT RR case with an initialLodId equal to @lodId. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetReinvestigationRequestId]
	@lodId INT,
	@idType INT
AS

DECLARE @request_id INT = 0

IF @idType = 1
BEGIN
	SET @request_id = (
			SELECT	TOP 1 request_id 
			FROM	Form348_RR
			WHERE	InitialLodId = @lodId
			ORDER	BY Case_Id DESC
		)
END
ELSE
BEGIN
	SELECT	@request_id = request_id 
	FROM	Form348_RR
	WHERE	ReinvestigationLodId = @lodId
END

SELECT ISNULL(@request_id, 0)
GO

