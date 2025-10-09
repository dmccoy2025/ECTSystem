-- =============================================
-- Author:		Evan Morrison
-- Create date: 7/6/2017
-- Description:	Get most recent signature for 
--				the user group of a case
-- =============================================
CREATE PROCEDURE [dbo].[core_SignatureMetaData_sp_GetByUserGroup]
(
	@refId INT,
	@workflowId INT,
	@groupId INT
)

AS

SELECT Top 1 *
FROM core_SignatureMetaData
where refid = @refId
	AND workflowId = @workflowId
	AND userGroup = @groupId
ORDER BY date DESC
GO

