
--EXEC lod_sp_GetLastStatus 2,2
CREATE PROCEDURE [dbo].[lod_sp_GetLastStatus]
	@refId int,
	@module tinyint 
	

AS
 
SET NOCOUNT ON


DECLARE @trackId int, @currWorkStatus int 
	--get current status
	SELECT TOP 1 @trackId = wst_id, @currWorkStatus = ws_id 
	FROM core_workstatus_tracking 
	WHERE refId = @refId AND module = @module and enddate is not null
	ORDER BY wst_id DESC 
 	select @currWorkStatus   

 
	--select statusId from core_WorkStatus  where ws_id=@currWorkStatus
GO

