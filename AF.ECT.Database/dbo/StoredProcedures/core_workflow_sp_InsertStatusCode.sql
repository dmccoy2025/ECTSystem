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
CREATE PROCEDURE [dbo].[core_workflow_sp_InsertStatusCode] 
	@description AS VARCHAR(50),
	@isFinal AS BIT,
	@isApproved AS BIT,
	@canAppeal AS BIT,
	@groupId AS TINYINT,
	@moduleId AS TINYINT,
	@compo AS CHAR(1),
	@isDisposition AS BIT,
	@isFormal AS BIT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT	INTO	core_StatusCodes (description, isFinal, isApproved, canAppeal, groupId, moduleId, compo, isDisposition, isFormal)
			VALUES	(@description, @isFinal, @isApproved, @canAppeal, @groupId, @moduleId, @compo, @isDisposition, @isFormal)

	SELECT SCOPE_IDENTITY()
END
GO

