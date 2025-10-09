
CREATE PROCEDURE [dbo].[core_workstatus_sp_UpdateOrder]
	@ws_id int,
	@sortOrder tinyint

AS

UPDATE core_workStatus
SET sortOrder = @sortOrder
WHERE ws_id = @ws_id
GO

