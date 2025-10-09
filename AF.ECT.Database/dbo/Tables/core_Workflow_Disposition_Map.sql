CREATE TABLE [dbo].[core_Workflow_Disposition_Map] (
    [WorkflowId]    TINYINT NOT NULL,
    [DispositionId] INT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_Disposition_Map]
    ADD CONSTRAINT [FK_Workflow_Disposition_Map_core_lkupDisposition] FOREIGN KEY ([DispositionId]) REFERENCES [dbo].[core_lkupDisposition] ([Id]);
GO

ALTER TABLE [dbo].[core_Workflow_Disposition_Map]
    ADD CONSTRAINT [FK_Workflow_Disposition_Map_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

