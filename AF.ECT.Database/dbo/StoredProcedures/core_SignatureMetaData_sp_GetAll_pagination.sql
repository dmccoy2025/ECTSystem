-- =============================================
-- Author:		Evan Morrison
-- Create date: 7/6/2017
-- Description:	Get all signatures for a case with pagination
-- =============================================
CREATE PROCEDURE [dbo].[core_SignatureMetaData_sp_GetAll_pagination]
(
	@refId int,
	@workflowId int,
	@PageNumber INT = 1,
	@PageSize INT = 10
)

AS

SELECT *
FROM core_SignatureMetaData
where refid = @refId
	AND workflowId = @workflowId
ORDER BY id
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO