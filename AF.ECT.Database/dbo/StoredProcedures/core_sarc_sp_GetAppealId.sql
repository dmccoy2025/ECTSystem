
-- ============================================================================
-- Author:			Evan Morrison
-- Create Date:		1/23/2017
-- Description:		Get the SARC appeal id for the given refId and workflow
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetAppealId]
	@refId INT,
	@workflow INT
AS

DECLARE @appeal_id INT = 0

SET @appeal_id = (
		SELECT	TOP 1 appeal_sarc_id 
		FROM	[dbo].[Form348_AP_SARC]
		WHERE	initial_id = @refId AND initial_workflow = @workflow
		ORDER	BY case_id DESC
	)

SELECT ISNULL(@appeal_id, 0)
GO

