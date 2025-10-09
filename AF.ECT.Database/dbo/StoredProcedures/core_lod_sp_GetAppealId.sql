
-- ============================================================================
-- Author:			Evan Morrison
-- Create Date:		7/21/2016
-- Description:		Added an ORDER BY clause to the type 1 SELECT statement. 
--					This was done because multilple RR cases can now be
--					associated with a single LOD case. This stored procedure
--					now makes the assumption that the caller wants the MOST
--					RECENT RR case with an initialLodId equal to @lodId. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetAppealId]
	@lodId INT,
	@idType INT
AS

DECLARE @appeal_id INT = 0

IF @idType = 1
BEGIN
	SET @appeal_id = (
			SELECT	TOP 1 appeal_id 
			FROM	[dbo].[Form348_AP]
			WHERE	initial_lod_id = @lodId
			ORDER	BY case_id DESC
		)
END

SELECT ISNULL(@appeal_id, 0)
GO

