-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReminderUpdateInactiveSettings] 
	@pinterval int,
	@pnotificationInterval int,
	@ptemplateId int,
	@pactive bit
AS
BEGIN

	SET NOCOUNT ON;
	
	BEGIN TRY
		
		UPDATE alod.dbo.ReminderInactiveSettings
		SET interval = @pinterval,
			notification_interval = @pnotificationInterval,
			templateId =@ptemplateId,
			active = @pactive
		WHERE i_id = 1
		
		Return 1
	
	END TRY
	BEGIN CATCH
		
		Return 0 
		
	END CATCH

END
GO

