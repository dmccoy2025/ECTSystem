-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 7/2/2014
-- Description:	Returns the ID of the unit that matches the unit name and unit PAS code that are passed into the stored procedure.
-- =============================================
CREATE PROCEDURE [dbo].[cmdStruct_sp_GetUnitId] 
(
	  @unitName As varchar(100) 
	, @pasCode As varchar(10)
)

AS
BEGIN

	SET NOCOUNT ON;

	-- Select the unit id using the unit name
	SELECT	CS_ID
	FROM	Command_Struct 
	WHERE	LONG_NAME = @unitName
			AND PAS_CODE = @pasCode

END
GO

