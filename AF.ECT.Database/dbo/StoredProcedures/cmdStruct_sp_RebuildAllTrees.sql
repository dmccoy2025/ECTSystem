
--EXEC cmdStruct_sp_RebuildAllTrees

CREATE PROCEDURE [dbo].[cmdStruct_sp_RebuildAllTrees]
--
AS

set nocount on;

if (OBJECT_ID('tempdb..#command_tree') is not null)
begin
	drop table #command_tree;
end

--create our temp table to match our command_struct_tree table
select top 1 * into #command_tree
from Command_Struct_Tree;

delete from #command_tree;

CREATE TABLE #PasTree
(
	cs_id int,
	pas_code char(4)
);

--rebuild the chains
print ('Rebuilding 1...');
EXEC cmdStruct_sp_RebuildTree 1;
print ('Rebuilding 2...');
EXEC cmdStruct_sp_RebuildTree 2;
print ('Rebuilding 3...');
EXEC cmdStruct_sp_RebuildTree 3;
print ('Rebuilding 4...');
EXEC cmdStruct_sp_RebuildTree 4;
print ('Rebuilding 5...');
EXEC cmdStruct_sp_RebuildTree 5;
print ('Rebuilding 6...');
EXEC cmdStruct_sp_RebuildTree 6;
print ('Rebuilding 7...');
EXEC cmdStruct_sp_RebuildTree 7;

--8 (PHA_REPORT_OLD) is no longer used, no need to rebuild it
--print ('Rebuilding 8...');
--EXEC cmdStruct_sp_RebuildTree 8;

print ('Rebuild to tmp completed');
--now recreate our indexes

--drop our indexes from the tree table to speed inserts
DROP INDEX Command_Struct_Tree.IX_CMD_TREE_PARENT;
DROP INDEX Command_Struct_Tree.IX_CMD_TREE_CHILD;

BEGIN TRY

	print ('Moving to tree table...');
	
	BEGIN TRANSACTION
	DELETE FROM Command_Struct_Tree;
	
	INSERT INTO Command_Struct_Tree
	SELECT * FROM #command_tree;
	
	COMMIT TRANSACTION
END TRY
BEGIN CATCH

	ROLLBACK TRANSACTION
END CATCH

print ('Rebuilding indexes...');

--recreate the indexes
CREATE NONCLUSTERED INDEX [IX_CMD_TREE_PARENT] ON [dbo].[Command_Struct_Tree] 
(
	[parent_pas] ASC,
	[view_type] ASC
);

CREATE NONCLUSTERED INDEX [IX_CMD_TREE_CHILD] ON [dbo].[Command_Struct_Tree] 
(
	[child_pas] ASC,
	[view_type] ASC
);


DROP TABLE #PasTree;
DROP TABLE #command_tree;

print ('Command Struct Tree rebuild complete.');
GO

