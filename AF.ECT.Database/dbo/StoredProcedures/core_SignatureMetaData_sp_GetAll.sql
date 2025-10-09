-- =============================================
-- Author:		Evan Morrison
-- Create date: 7/6/2017
-- Description:	Get all signatures for a case
-- =============================================
CREATE PROCEDURE [dbo].[core_SignatureMetaData_sp_GetAll]
(
	@refId int,
	@workflowId int
)

AS

SELECT *
FROM core_SignatureMetaData
where refid = @refId
	AND workflowId = @workflowId
GO

