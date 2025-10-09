-- =============================================
-- Author:		<Kamal Singh>
-- Create date: <6/29/2015>
-- Description:	<Gets the Inactive setting information,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReminderGetInactiveSettings] 
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT *
	FROM alod.dbo.ReminderInactiveSettings

END
GO

