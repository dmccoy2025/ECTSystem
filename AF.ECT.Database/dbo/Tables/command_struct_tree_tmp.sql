CREATE TABLE [dbo].[command_struct_tree_tmp] (
    [view_type]  TINYINT  NOT NULL,
    [parent_pas] CHAR (4) NOT NULL,
    [child_pas]  CHAR (4) NOT NULL,
    [parent_id]  INT      NULL,
    [child_id]   INT      NULL
);
GO

