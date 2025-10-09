-- ============================================================================
-- Author:		Andy Cooper
-- Create date: 8 May 2008
-- Description:	Inserts a new status code
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/7/2015	
-- Description:		Added the @isDisposition parameter.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/15/2016	
-- Description:		Added the @isFormal parameter.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_UpdateStatusCode] 
	@statusId AS TINYINT,
	@description AS VARCHAR(50),
	@isFinal AS BIT,
	@isApproved AS BIT,
	@canAppeal AS BIT,
	@groupId AS TINYINT,
	@moduleId AS TINYINT,
	@displayOrder AS TINYINT,
	@isDisposition AS BIT,
	@isFormal AS BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		core_StatusCodes
	SET 
		description = @description,
		isFinal = @isFinal, 
		isApproved = @isApproved,
		canAppeal = @canAppeal, 
		groupId = @groupId, 
		moduleId = @moduleId,
		displayOrder = @displayOrder,
		isDisposition = @isDisposition
	WHERE
		statusId = @statusId

END
GO

