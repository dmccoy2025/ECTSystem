
-- =============================================
-- Author:		<Kamal Singh>
-- Create date: <6/20/14>
-- Description:	<This stored procedure is to update appAuthUserId coulmn in Form348 table. Originally, this happens in the AssignIo, which is only called
--                when a lod case is in the formal state, this will update the value when appointing authority signs, which means it will
--				  update regardless if it the case is informal or formal.>
-- =============================================
CREATE PROCEDURE [dbo].[form348_sp_UpdateAAUserID]
		@refid int,
		@aaUserid AS int
	
	
	
AS


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

			UPDATE Form348
			SET   appAuthUserId = @aaUserId  
				 ,modified_by=@aaUserId
				,modified_date=getUTCDate()
				WHERE  lodId = @refId	
				
	
END
GO

