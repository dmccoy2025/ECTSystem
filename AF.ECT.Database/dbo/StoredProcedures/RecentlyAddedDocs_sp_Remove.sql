
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/3/2016
-- Description:	Removes all recently added document records associated with the
--				specified reference Id and document group Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[RecentlyAddedDocs_sp_Remove]
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
	
	DELETE	FROM	RecentlyUploadedDocuments
			WHERE	RefId = @refId
					AND DocGroupId = @docGroupId
END
GO

