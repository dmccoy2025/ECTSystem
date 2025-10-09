CREATE PROCEDURE [dbo].[core_email_sp_InsertEmailTemplate]
(
	@compo char(1),
	@subject varchar(50),
	@body varchar(500),
	@title varchar(50),
	@dataProc varchar(50),
	@status bit
	
)

AS
	SET NOCOUNT ON

INSERT INTO dbo.core_EmailTemplates
              (compo,Subject, Body, Title, DataProc, Active)
VALUES     (	@compo ,@subject, @body, @title, @dataProc,@status)
GO

