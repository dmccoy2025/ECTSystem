
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/30/2017
-- Description:	- Returns the Numbered Air Force (NAF) units that the PH workflow
--				is associated with.
-- ============================================================================
CREATE PROCEDURE [dbo].[cmdStruct_sp_GetNumberedAirForcesForPH]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	cs.CS_ID AS [Value], (cs.LONG_NAME + ' (' + cs.PAS_CODE + ')') AS [Name]
	FROM	Command_Struct cs
	WHERE	cs.PAS_CODE IN (
				'FT3K',
				'FT3M',
				'FT29'
			)
	ORDER	BY cs.LONG_NAME
END
GO

