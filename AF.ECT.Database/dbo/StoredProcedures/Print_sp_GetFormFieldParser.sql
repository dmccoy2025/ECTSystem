
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/14/2016
-- Description:	Inserts a new PH Section record into the PH_Section table.
-- ============================================================================
CREATE PROCEDURE [dbo].[Print_sp_GetFormFieldParser]
	@parserId INT
AS
BEGIN
	SELECT	p.Name
	FROM	PrintDocumentFormFieldParser p
	WHERE	p.Id = @parserId
END
GO

