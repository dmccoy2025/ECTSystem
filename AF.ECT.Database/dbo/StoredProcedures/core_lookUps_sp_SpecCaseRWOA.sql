
-- ==========================================================
-- Author:			Evan Morrison
-- Create date:		8/26/2016
-- Description:		Check if a special case has a RWOA given
--					the workflow id and refid
-- ==========================================================

CREATE PROCEDURE [dbo].[core_lookUps_sp_SpecCaseRWOA]

	@workflow INT,
	@refID	INT
	
AS
BEGIN
	IF EXISTS (SELECT 1 FROM dbo.Rwoa r WHERE r.workflow = @workflow AND r.refId = @refID )
		BEGIN
			SELECT 1
		END
		
	SELECT 0
END
GO

