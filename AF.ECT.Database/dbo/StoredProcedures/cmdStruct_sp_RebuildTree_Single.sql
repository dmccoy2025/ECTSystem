

CREATE PROCEDURE [dbo].[cmdStruct_sp_RebuildTree_Single]
	@cs_id int,
	@viewType tinyint

AS

set nocount on;

DECLARE  @cscId int, @childPas char(4), @maxDepth as int;

set @maxDepth = 10;

DELETE FROM #PasTree;

set @cscId = (
	select csc_id from Command_Struct_Chain where CS_ID = @cs_id and view_type = @viewType);

SET @childPas = (
	SELECT pas_code FROM Command_Struct c where CS_ID = @cs_id);
	
IF (@cscId IS NOT NULL)
BEGIN 
	WITH pasCTE AS (
		SELECT csc_id, csc_id_parent, cs_id, 0 AS depth
		FROM Command_Struct_Chain WHERE CSc_Id = @cscId

	UNION ALL

	SELECT p.csc_id, p.csc_id_parent, p.cs_id, pcte.depth + 1
	FROM Command_Struct_Chain p 
	INNER JOIN pasCTE pcte ON p.csc_id = pcte.CSC_ID_PARENT
	WHERE pcte.depth < @maxDepth --limit the depth to prevent infinite recursion
	AND p.view_type = @viewType
	)
	INSERT INTO #PasTree
	SELECT c.cs_id, c.PAS_CODE
	FROM pasCTE cte
	JOIN Command_Struct c ON c.CS_ID = cte.cs_id;

	INSERT INTO #command_tree (view_type, child_pas,child_id , parent_pas , parent_id)
	SELECT distinct @viewType, @childPas, @cs_id, pas_code, cs_id
	FROM #pastree WHERE cs_id IN (
		SELECT DISTINCT cs_id FROM #PasTree)
	and pas_code IS NOT NULL;
END 
	

BEGIN TRY

	BEGIN TRANSACTION
	
	DELETE FROM Command_Struct_Tree
	where view_type = @viewType
	and child_id = @cs_id;
	
	INSERT INTO Command_Struct_Tree
	SELECT * FROM #command_tree;
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH

	ROLLBACK TRANSACTION
END CATCH
GO

