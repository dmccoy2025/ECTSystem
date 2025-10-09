



-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/18/2019
-- Description:	Save or update a child comment
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ChildCaseComments_UpdateComment]
	@id INT,
	@comment varchar(max),
	@createdDate DATETIME
AS
BEGIN
	
	UPDATE Child_Case_Comments
	SET comments = @comment, created_date = @createdDate
	FROM Child_Case_Comments ch 
	WHERE 
	ch.id = @id

END
GO

