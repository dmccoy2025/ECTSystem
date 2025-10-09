CREATE TABLE [dbo].[core_Workflow_ReturnReason] (
    [WorkflowId] TINYINT NOT NULL,
    [return_id]  TINYINT NOT NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_ReturnReason]
    ADD CONSTRAINT [FK_core_Workflow_ReturnReason_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_Workflow_ReturnReason]
    ADD CONSTRAINT [FK_core_Workflow_ReturnReason_core_lkupRWOAReasons] FOREIGN KEY ([return_id]) REFERENCES [dbo].[core_lkupRWOAReasons] ([ID]);
GO

