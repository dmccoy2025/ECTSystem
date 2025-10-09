CREATE TABLE [dbo].[core_Workflow_MemberComponent] (
    [WorkflowId]  TINYINT NOT NULL,
    [ComponentId] INT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_MemberComponent]
    ADD CONSTRAINT [FK_core_Workflow_MemberComponent_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_Workflow_MemberComponent]
    ADD CONSTRAINT [FK_core_Workflow_MemberComponent_core_lkupComponent] FOREIGN KEY ([ComponentId]) REFERENCES [dbo].[core_lkupComponent] ([component_id]);
GO

