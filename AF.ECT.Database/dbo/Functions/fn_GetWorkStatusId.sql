-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 6/2/2014
-- Description:	Returns the work status id of a specified status code 
--				description. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
CREATE FUNCTION [dbo].[fn_GetWorkStatusId] 
(
	@workflowId INT,
	@statusCodeDesc VARCHAR(100)
)

RETURNS INT

AS

BEGIN
	-- Declare the return variable here
	DECLARE @Result INT
	
	SELECT	@Result = ws.ws_id
	FROM	dbo.core_WorkStatus AS ws INNER JOIN
			dbo.core_StatusCodes AS sc ON sc.statusId = ws.statusId INNER JOIN
			core_Workflow wf ON ws.workflowId = wf.workflowId
	WHERE	wf.workflowId = @workflowId AND
			sc.description = @statusCodeDesc

	-- Return the result of the function
	RETURN @Result

END
GO

