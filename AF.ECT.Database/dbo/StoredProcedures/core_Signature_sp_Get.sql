
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/26/2017
-- Description:	Get Signature for case 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_Signature_sp_Get]
	@refid INT,
	@workflow INT,
	@ptype INT
AS
BEGIN
	
	SELECT TOP 1 *
	FROM core_SignOnly_Signatures
	Where refId = @refid
		AND workflow = @workflow
		AND ptype = @ptype
	ORDER BY sig_date DESC
END
GO

