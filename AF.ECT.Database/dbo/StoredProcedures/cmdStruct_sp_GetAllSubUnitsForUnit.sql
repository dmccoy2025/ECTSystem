
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/22/2017
-- Description:	Returns all sub units for the specified unit by
--				searching the reporting view unit hierarchy as defined by the
--				Command_Struct_Chain table.
-- ============================================================================
CREATE PROCEDURE [dbo].[cmdStruct_sp_GetAllSubUnitsForUnit]
	@unitId INT,
	@rptView INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE	@maxDepth INT = 50,
			@parentCSC_ID AS INT

	DECLARE @ChildUnits TABLE
	(
		csId INT,
		Name NVARCHAR(100),
		PAS NVARCHAR(4),
		parentCSId INT,
		LevelDown INT
	)

	SET @parentCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@unitId AND CHAIN_TYPE='PHA_NMREPORT')

	;WITH  childUnits (CS_ID, CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, [Level], rn) 
	AS
	( 
		-- Anchor member   
		SELECT	CS_ID ,CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, 0, convert(varchar(max),right(row_number() over (order by CSC_ID),10)) rn  
		FROM	COMMAND_STRUCT_CHAIN 
		WHERE	CSC_ID = @parentCSC_ID AND CHAIN_TYPE = 'PHA_NMREPORT' AND CS_ID IS NOT NULL
				
		UNION ALL 
				
		-- Recursive member
		SELECT	t1.CS_ID ,t1.CSC_ID, t1.CSC_ID_PARENT, t1.CHAIN_TYPE, childUnits.[Level] + 1,
				rn + '/' + convert(varchar(max),right(row_number() over (order by childUnits.CSC_ID),10))
		FROM	COMMAND_STRUCT_CHAIN t1  
				INNER JOIN childUnits ON t1.CSC_ID_PARENT = childUnits.CSC_ID AND t1.CHAIN_TYPE = childUnits.CHAIN_TYPE AND t1.cs_id IS NOT NULL
		WHERE	'/' + cast(t1.cs_id as varchar(20)) + '/' NOT LIKE '%/' + CAST(childUnits.cs_id AS VARCHAR(MAX)) + '/%'
				AND childUnits.[Level] + 1 < @maxDepth
	)
	INSERT	INTO	@ChildUnits ([csId], [Name], [PAS], [parentCSId], [LevelDown])
			SELECT	cu.CS_ID, cs.LONG_NAME, cs.PAS_CODE, pCSC.CS_ID, cu.Level
			FROM	childUnits cu
					JOIN Command_Struct cs ON cu.CS_ID = cs.CS_ID
					JOIN Command_Struct_Chain pCSC ON cu.CSC_ID_PARENT = pCSC.CSC_ID
			--WHERE	cu.Level <> 0

	SELECT	cu.csId AS CS_ID, cu.Name AS LONG_NAME, cu.PAS AS PAS_CODE, cu.parentCSId AS Parent_CS_ID, cu.LevelDown AS LevelDown
	FROM	@ChildUnits cu 
	WHERE	cu.LevelDown <> 0
	ORDER	BY cu.Name
END
GO

