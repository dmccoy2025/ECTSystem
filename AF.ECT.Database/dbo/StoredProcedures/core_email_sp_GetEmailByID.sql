CREATE PROCEDURE [dbo].[core_email_sp_GetEmailByID]
(
	@templateID int

)

AS
	SET NOCOUNT ON


SELECT     TemplateID, Subject, Body, Title, DataProc, active
FROM         dbo.core_EmailTemplates
WHERE     (TemplateID = @templateID)
GO

