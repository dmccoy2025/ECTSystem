-- =============================================
-- Author:		Andy Cooper
-- Create date: 27 March 2008q
-- Description:	Update a permission
-- =============================================
CREATE PROCEDURE [dbo].[core_permissions_sp_Update] 
	-- Add the parameters for the stored procedure here
	@permId int, 
	@name varchar(50),
	@description varchar(100),
	@exclude bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE 
		dbo.core_Permissions
	SET
		permName = @name
		,permDesc = @description
		,exclude = @exclude
	WHERE
		permId = @permId
END
GO

