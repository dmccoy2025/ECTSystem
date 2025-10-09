
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/6/2017
-- Description:	Delete a signature for the desired case 
--				at the desired workstatus
-- ============================================================================
CREATE PROCEDURE [dbo].[core_SignatureMetaData_sp_Delete]
	@refId INT,
	@workflowId INT,
	@workStatus INT
AS
BEGIN
	
	DELETE FROM core_SignatureMetaData
	WHERE refId = @refId
		AND workflowId = @workflowId
		AND workStatus = @workStatus

END
GO

