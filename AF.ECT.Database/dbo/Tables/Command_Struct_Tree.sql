CREATE TABLE [dbo].[Command_Struct_Tree] (
    [view_type]  TINYINT  NOT NULL,
    [parent_pas] CHAR (4) NOT NULL,
    [child_pas]  CHAR (4) NOT NULL,
    [parent_id]  INT      NULL,
    [child_id]   INT      NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_CMD_TREE_PARENT_ID]
    ON [dbo].[Command_Struct_Tree]([view_type] ASC, [parent_id] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_CMD_TREE_CHILD_PARENT_VIEW]
    ON [dbo].[Command_Struct_Tree]([child_id] ASC, [parent_id] ASC, [view_type] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_CMD_TREE_CHILD]
    ON [dbo].[Command_Struct_Tree]([child_pas] ASC, [view_type] ASC);
GO

CREATE NONCLUSTERED INDEX [IDX_COMMAND_STRUCT_TREE_PINCCV]
    ON [dbo].[Command_Struct_Tree]([parent_id] ASC)
    INCLUDE([child_id], [view_type]) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_CMD_TREE_PARENT]
    ON [dbo].[Command_Struct_Tree]([parent_pas] ASC, [view_type] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_CMD_TREE_CHILD_ID]
    ON [dbo].[Command_Struct_Tree]([view_type] ASC, [child_id] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IDX_Command_Struct_Tree_ViewParent]
    ON [dbo].[Command_Struct_Tree]([view_type] ASC, [parent_id] ASC) WITH (FILLFACTOR = 80);
GO

