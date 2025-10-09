

--EXEC cmdStruct_sp_RebuildAllTrees

CREATE PROCEDURE [dbo].[cmdStruct_sp_RebuildAllTrees_Single]
	@cs_id int
AS

set nocount on;

if (OBJECT_ID('tempdb..#PasTree') is null)
begin
	CREATE TABLE #PasTree
	(
		cs_id int,
		pas_code char(4)
	);
end

if (OBJECT_ID('tempdb..#command_tree') is not null)
begin
	drop table #command_tree;
end

--create our temp table to match our command_struct_tree table
select top 1 * into #command_tree
from Command_Struct_Tree;

delete from #command_tree;


--rebuild the chains
print ('Rebuilding 1...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 1;
print ('Rebuilding 2...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 2;
print ('Rebuilding 3...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 3;
print ('Rebuilding 4...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 4;
print ('Rebuilding 5...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 5;
print ('Rebuilding 6...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 6;
print ('Rebuilding 7...');
EXEC cmdStruct_sp_RebuildTree_Single @cs_id, 7;

--8 (PHA_REPORT_OLD) is no longer used, no need to rebuild it
--print ('Rebuilding 8...');
--EXEC cmdStruct_sp_RebuildTree_Single 8;

print ('Rebuild to tmp completed');


--BEGIN TRY

--	print ('Moving to tree table...');
	
--	BEGIN TRANSACTION
--	DELETE FROM Command_Struct_Tree where child_id = @cs_id;
	
--	INSERT INTO Command_Struct_Tree
--	SELECT * FROM #command_tree;
	
--	COMMIT TRANSACTION
--END TRY
--BEGIN CATCH

--	ROLLBACK TRANSACTION
--END CATCH


DROP TABLE #PasTree;
DROP TABLE #command_tree;

print ('Command Struct Tree rebuild complete.');
GO

