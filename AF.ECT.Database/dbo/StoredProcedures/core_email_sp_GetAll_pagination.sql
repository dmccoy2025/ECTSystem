CREATE PROCEDURE [dbo].[core_email_sp_GetAll_pagination]
	@compo char(1),
	@PageNumber INT = 1,
	@PageSize INT = 10
AS

	SET NOCOUNT ON;

SELECT     TemplateID, left(Subject, 20) as Subject, left(Title,20) As Title, left(Body,50) AS Body, DataProc, active
FROM         dbo.core_EmailTemplates
WHERE		compo=	@compo
ORDER BY TemplateID
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO