
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/6/2017
-- Description:	Get Case Comment by Id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseComments_GetById]
	@id INT
AS
BEGIN
	
	SELECT * FROM Case_Comments Where id = @id
END
GO

