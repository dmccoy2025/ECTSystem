



CREATE PROCEDURE [dbo].[core_workflow_sp_GetStatusCodeScope]
	@statusID tinyint
AS


RETURN   SELECT b.accessScope FROM core_StatusCodes a 
		 LEFT JOIN
		 core_UserGroups b ON a.groupId = b.groupId
		 WHERE 	a.statusId= @statusID
GO

