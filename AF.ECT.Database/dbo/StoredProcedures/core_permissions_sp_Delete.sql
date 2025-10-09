-- =============================================
-- Author:		Andy Cooper
-- Create date: 27 March 2008
-- Description:	Delete a permission
-- =============================================
CREATE PROCEDURE [dbo].[core_permissions_sp_Delete] 
	-- Add the parameters for the stored procedure here
	@permId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET XACT_ABORT ON 

	BEGIN TRANSACTION

	--first we have to delete from the groups that have this permission
	DELETE FROM dbo.core_GroupPermissions
	WHERE permId = @permId

	--now delete from the permissions table
	DELETE FROM core_Permissions
	WHERE permId = @permId

	COMMIT TRANSACTION

END
GO

