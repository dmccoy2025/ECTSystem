CREATE TABLE [dbo].[core_Workflow_CaseType_Map] (
    [WorkflowId] TINYINT NOT NULL,
    [CaseTypeId] INT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_CaseType_Map]
    ADD CONSTRAINT [FK_Workflow_CaseType_Map_core_lkupCaseType] FOREIGN KEY ([CaseTypeId]) REFERENCES [dbo].[core_CaseType] ([Id]);
GO

ALTER TABLE [dbo].[core_Workflow_CaseType_Map]
    ADD CONSTRAINT [FK_Workflow_CaseType_Map_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

