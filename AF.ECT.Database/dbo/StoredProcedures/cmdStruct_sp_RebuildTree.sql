

CREATE PROCEDURE [dbo].[cmdStruct_sp_RebuildTree]
	@viewType tinyint

AS

DECLARE  @cscId int, @parent int, @parentPas char(4)
--DECLARE @viewType tinyint
--SET @viewType = 7 --(PHA_ADMIN)

DECLARE pasCur CURSOR FOR
SELECT csc_id FROM Command_Struct_Chain 
WHERE view_type = @viewType
--AND csc_id = 3367436; --limit the test for now

OPEN pasCur
FETCH next FROM pasCur INTO @cscId

WHILE @@fetch_status = 0
BEGIN

	DELETE FROM #PasTree;

	SET @parentPas = (
		SELECT pas_code FROM Command_Struct c
		JOIN Command_Struct_Chain p ON p.CS_ID = c.CS_ID AND p.view_type = @viewType
		WHERE p.CSC_ID = @cscId
	);

	SET @parent = (
		SELECT a.cs_id FROM command_struct_chain a
		JOIN command_struct c ON c.cs_id = a.cs_id
		WHERE a.csc_id = @cscId
	);

	IF (@parentPas IS NOT NULL)
	BEGIN 
		WITH pasCTE AS (
			SELECT csc_id, csc_id_parent, cs_id, 0 AS depth
			FROM Command_Struct_Chain WHERE CSc_Id = @cscId

		UNION ALL

		SELECT p.csc_id, p.csc_id_parent, p.cs_id, pcte.depth + 1
		FROM Command_Struct_Chain p 
		INNER JOIN pasCTE pcte ON pcte.csc_id = p.CSC_ID_PARENT
		WHERE pcte.depth < 20 --limit the depth to prevent infinite recursion
		AND p.view_type = @viewType
		)
		INSERT INTO #PasTree
		SELECT c.cs_id, c.PAS_CODE
		FROM pasCTE cte
		JOIN Command_Struct c ON c.CS_ID = cte.cs_id;

		INSERT INTO #command_tree (view_type, parent_pas, parent_id, child_pas, child_id)
		SELECT distinct @viewType, @parentPas, @parent, pas_code, cs_id
		FROM #pastree WHERE cs_id IN (
			SELECT DISTINCt cs_id FROM #PasTree)
		AND pas_code IS NOT NULL;
	END 
	

FETCH next FROM pasCur INTO @cscId
END

CLOSE pasCur
DEALLOCATE pasCur
GO

