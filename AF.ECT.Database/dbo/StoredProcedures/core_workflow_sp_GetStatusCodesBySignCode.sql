

CREATE PROCEDURE [dbo].[core_workflow_sp_GetStatusCodesBySignCode]
	@groupId smallint,
	@module tinyint

AS


SELECT statusId, description FROM core_StatusCodes WHERE groupId = @groupId AND moduleId = @module
UNION
SELECT status, description
FROM core_StatusCodeSigners a JOIN core_StatusCodes b ON b.statusId = a.status
WHERE a.groupId = @groupId AND b.moduleId = @module
GO

