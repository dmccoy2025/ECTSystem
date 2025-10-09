-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[core_user_sp_Belongs_To_Attach]
	-- Add the parameters for the stored procedure here
	@userId int,
	@attachUnit int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    
			SELECT 
				CONVERT(BIT,COUNT(*))
			FROM 
				Command_Struct_Tree 
			WHERE parent_id IN (
				SELECT isnull(ada_cs_id, cs_id) FROM core_users WHERE userID = @userid
			) 
			AND view_type IN  (
				SELECT viewtype FROM vw_users WHERE userID = @userid
			)
			AND child_id = (@attachUnit)
END
GO

