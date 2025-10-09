CREATE TABLE [dbo].[core_WorkflowPermissions] (
    [workflowId] TINYINT  NULL,
    [permId]     SMALLINT NULL
);
GO

ALTER TABLE [dbo].[core_WorkflowPermissions]
    ADD CONSTRAINT [FK_core_WorkflowPermissions_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_WorkflowPermissions]
    ADD CONSTRAINT [FK_core_WorkflowPermissions_core_Permissions] FOREIGN KEY ([permId]) REFERENCES [dbo].[core_Permissions] ([permId]);
GO

