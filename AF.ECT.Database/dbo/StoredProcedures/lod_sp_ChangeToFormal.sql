

CREATE PROCEDURE [dbo].[lod_sp_ChangeToFormal]
	@refId int
	,@statusIn   tinyInt
	,@userId int

AS

SET NOCOUNT ON

UPDATE Form348
SET formal_inv = 1, modified_by=@userId, modified_date=getdate()
	WHERE lodid = @refId AND status = @statusIn

SELECT @@ROWCOUNT
GO

