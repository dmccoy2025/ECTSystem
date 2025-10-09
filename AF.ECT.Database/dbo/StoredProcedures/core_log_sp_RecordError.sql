-- =============================================
-- Author:		Andy Cooper
-- Create date: 26 March 2008
-- Description:	Records an entry in the error log
-- =============================================
CREATE PROCEDURE [dbo].[core_log_sp_RecordError] 
	-- Add the parameters for the stored procedure here
	@userName varchar(50), 
	@page varchar(200),
	@version varchar(10),
	@browser varchar(50),
	@message  varchar(max),
	@stacktrace  varchar(max),
	@caller varchar(100),
	@address varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.core_LogError
		(userName, page, appVersion, browser, message, stackTrace, caller, address)
	VALUES
		(@userName, @page, @version, @browser, @message, @stacktrace, @caller, @address)

END
GO

