

-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/22/2019
-- Description:	Get Case Dialogue Comment by Id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseDialogueComments_GetById]
	@id INT
AS
BEGIN
	
	SELECT * FROM CaseDialogue_Comments Where id = @id
END
GO

