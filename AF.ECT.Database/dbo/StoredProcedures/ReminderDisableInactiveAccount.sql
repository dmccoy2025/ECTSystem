-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReminderDisableInactiveAccount]
	-- Add the parameters for the stored procedure here
	@puserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
	
		UPDATE alod.dbo.core_Users
		SET accessStatus = 4
		Where userID = @puserId
		
		Return 1
	
	END TRY
	BEGIN CATCH
		Return 0 
	END CATCH
END
GO

