
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/3/2016
-- Description:	Gets all of the recently added document records for the
--				specified reference Id and document group Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[RecentlyAddedDocs_sp_GetByIds]
	@refId INT,
	@docGroupId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@refId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@docGroupId, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	rud.DocId, rud.DocTypeId
	FROM	RecentlyUploadedDocuments rud
	WHERE	rud.RefId = @refId
			AND rud.DocGroupId = @docGroupId
END
GO

