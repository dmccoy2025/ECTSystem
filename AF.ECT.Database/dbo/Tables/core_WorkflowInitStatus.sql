CREATE TABLE [dbo].[core_WorkflowInitStatus] (
    [workflowId] TINYINT NOT NULL,
    [groupId]    TINYINT NOT NULL,
    [statusId]   INT     NOT NULL
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkflowGroup]
    ON [dbo].[core_WorkflowInitStatus]([workflowId] ASC, [groupId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_WorkflowInitStatus]
    ADD CONSTRAINT [FK_core_WorkflowInitStatus_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_WorkflowInitStatus]
    ADD CONSTRAINT [FK_core_WorkflowInitStatus_core_WorkStatus] FOREIGN KEY ([statusId]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_WorkflowInitStatus]
    ADD CONSTRAINT [FK_core_WorkflowInitStatus_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

