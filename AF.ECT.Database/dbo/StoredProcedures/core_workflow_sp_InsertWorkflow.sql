-- =============================================
-- Author:		Andy Cooper
-- Create date: 9 May 2008
-- Description:	Inserts a new Workflow
-- =============================================
CREATE PROCEDURE [dbo].[core_workflow_sp_InsertWorkflow] 
	@title varchar(50), 
	@module tinyint,
	@compo char(1),
	@formal bit,
	@initialStatus tinyint
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO core_Workflow
	(title, moduleId, compo, formal, initialStatus)
	VALUES
	(@title, @module, @compo, @formal, @initialStatus)

	SELECT SCOPE_IDENTITY()
END
GO

