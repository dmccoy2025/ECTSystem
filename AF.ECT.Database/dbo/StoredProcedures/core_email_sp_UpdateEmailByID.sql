CREATE PROCEDURE [dbo].[core_email_sp_UpdateEmailByID]
(
	@templateID int,
	@subject varchar(50),
	@body varchar(500),
	@title varchar(50),
	@dataProc varchar(50),
	@status bit
	
)

AS
	SET NOCOUNT ON


UPDATE    dbo.core_EmailTemplates
SET           Subject = @subject, 
			  Body = @body,
              Title = @title,
			  DataProc = @dataProc, 
			  active = @status, 
			  Sys_Date = GETDATE()
WHERE     (TemplateID = @templateID)
GO

