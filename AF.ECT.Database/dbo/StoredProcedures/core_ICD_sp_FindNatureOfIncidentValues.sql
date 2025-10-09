-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/8/2015
-- Description:	Selects all of the Nature of Incident values associated with
--				the specified ICD code Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_FindNatureOfIncidentValues]
	 @codeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	n.Id, n.Value, n.Text
	FROM	core_ICD_NatureOfIncident x
			INNER JOIN core_lkupNatureOfIncident n ON x.NOI_Id = n.Id
	WHERE	ICD_Id = @codeId
END
GO

