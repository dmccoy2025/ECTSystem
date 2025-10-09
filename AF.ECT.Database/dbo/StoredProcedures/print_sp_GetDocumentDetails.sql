
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/14/2016	
-- Description:		Selects the FormFieldParserId field. 
-- ============================================================================
CREATE PROCEDURE [dbo].[print_sp_GetDocumentDetails]
(
	@docId int
)
AS
BEGIN
	SELECT	docid, doc_name, filename, filetype, compo, sp_getdata, FormFieldParserId
	FROM	PrintDocument 
	WHERE	docid = @docId
END
GO

