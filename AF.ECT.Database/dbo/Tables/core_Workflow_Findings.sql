CREATE TABLE [dbo].[core_Workflow_Findings] (
    [WorkflowId] TINYINT NOT NULL,
    [FindingId]  TINYINT NOT NULL,
    [sort_order] INT     NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_Findings]
    ADD CONSTRAINT [FK_core_Workflow_Findings_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_Workflow_Findings]
    ADD CONSTRAINT [FK_core_Workflow_Findings_core_lkupFindings] FOREIGN KEY ([FindingId]) REFERENCES [dbo].[core_lkUpFindings] ([Id]);
GO

