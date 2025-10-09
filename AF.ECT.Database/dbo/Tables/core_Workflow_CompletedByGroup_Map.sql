CREATE TABLE [dbo].[core_Workflow_CompletedByGroup_Map] (
    [WorkflowId]    TINYINT NOT NULL,
    [CompletedById] INT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_CompletedByGroup_Map]
    ADD CONSTRAINT [FK_Workflow_CompletedByGroup_Map_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_Workflow_CompletedByGroup_Map]
    ADD CONSTRAINT [FK_Workflow_CompletedByGroup_Map_core_CompletedByGroup] FOREIGN KEY ([CompletedById]) REFERENCES [dbo].[core_CompletedByGroup] ([Id]);
GO

