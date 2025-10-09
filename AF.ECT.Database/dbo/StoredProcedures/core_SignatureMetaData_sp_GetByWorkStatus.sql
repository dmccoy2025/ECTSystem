-- =============================================
-- Author:		Evan Morrison
-- Create date: 7/6/2017
-- Description:	Get most recent signature for 
--				the workStatus of a case
-- =============================================
CREATE PROCEDURE [dbo].[core_SignatureMetaData_sp_GetByWorkStatus]
(
	@refId int,
	@workflowId int,
	@workStatus INT
)

AS

SELECT Top 1 *
FROM core_SignatureMetaData
where refid = @refId
	AND workflowId = @workflowId
	AND workStatus = @workStatus
ORDER BY date DESC
GO

