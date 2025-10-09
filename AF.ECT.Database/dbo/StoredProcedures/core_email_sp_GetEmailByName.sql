CREATE PROCEDURE [dbo].[core_email_sp_GetEmailByName]
(
	@title varchar(50)

)

AS
	SET NOCOUNT ON


SELECT TOP 1 TemplateID, Subject, Body, Title, DataProc, active
FROM         dbo.core_EmailTemplates
WHERE     (Title like @title)

--core_email_sp_getemailbyname 'Feedback Update'
GO

