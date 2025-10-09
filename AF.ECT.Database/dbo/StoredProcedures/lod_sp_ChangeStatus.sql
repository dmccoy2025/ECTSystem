

CREATE PROCEDURE [dbo].[lod_sp_ChangeStatus]
	@refId int
	,@statusIn tinyint
	,@statusOut tinyint
	,@userId int

AS

SET NOCOUNT ON

UPDATE Form348
SET status = @statusOut, modified_by=@userId, modified_date=getdate()
--	,completed_by=case when a.isfinal=1 then @userId else null end
--	,completed_date=case when a.isfinal=1 then getdate() else null end
-- FROM (select isfinal from core_statusCodes where statusId=@statusOut) a 
WHERE lodid = @refId AND status = @statusIn

SELECT @@ROWCOUNT
GO

