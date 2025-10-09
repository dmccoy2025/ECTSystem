
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/20/2016
-- Description:	Returns all of the records in the core_CertificationStamp table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	cs.Id, cs.Name, cs.Body, cs.IsQualified
	FROM	core_CertificationStamp cs
END
GO

