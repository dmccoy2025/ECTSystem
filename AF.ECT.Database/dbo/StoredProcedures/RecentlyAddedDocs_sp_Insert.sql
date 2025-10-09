
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/3/2016
-- Description:	Inserts a new recently added document record.
-- ============================================================================
CREATE PROCEDURE [dbo].[RecentlyAddedDocs_sp_Insert]
	@refId INT,
	@docGroupId BIGINT,
	@docId BIGINT,
	@docTypeId INT
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
	
	IF (ISNULL(@docId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@docTypeId, 0) = 0)
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	-- Make sure this record does not already exist...
	SELECT	@count = COUNT(*)
	FROM	RecentlyUploadedDocuments rud
	WHERE	rud.RefId = @refId
			AND rud.DocGroupId = @docGroupId
			AND rud.DocId = @docId
			AND rud.DocTypeId = @docTypeId
			
	IF (ISNULL(@count, 0) <> 0)
	BEGIN
		RETURN
	END
	
	INSERT	INTO	RecentlyUploadedDocuments ([RefId], [DocGroupId], [DocId], [DocTypeId])
			VALUES	(@refId, @docGroupId, @docId, @docTypeId)
END
GO

