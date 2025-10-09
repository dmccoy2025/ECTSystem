CREATE PROCEDURE [dbo].[core_email_sp_GetAll]
	@compo char(1)
AS

	SET NOCOUNT ON;

SELECT     TemplateID, left(Subject, 20) as Subject, left(Title,20) As Title, left(Body,50) AS Body, DataProc, active
FROM         dbo.core_EmailTemplates
WHERE		compo=	@compo
GO

