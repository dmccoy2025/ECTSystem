CREATE TABLE [dbo].[core_WorkflowPerms] (
    [workflowId] TINYINT NOT NULL,
    [groupId]    TINYINT NOT NULL,
    [canView]    BIT     NOT NULL,
    [canCreate]  BIT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_WorkflowPerms]
    ADD CONSTRAINT [FK_core_WorkflowPerms_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_WorkflowPerms]
    ADD CONSTRAINT [FK_core_WorkflowPerms_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_WorkflowPerms]
    ADD CONSTRAINT [DF_core_WorkflowPerms_canView] DEFAULT ((0)) FOR [canView];
GO

ALTER TABLE [dbo].[core_WorkflowPerms]
    ADD CONSTRAINT [DF_core_WorkflowPerms_canEdit] DEFAULT ((0)) FOR [canCreate];
GO

CREATE NONCLUSTERED INDEX [IX_WorkflowId]
    ON [dbo].[core_WorkflowPerms]([workflowId] ASC) WITH (FILLFACTOR = 80);
GO

