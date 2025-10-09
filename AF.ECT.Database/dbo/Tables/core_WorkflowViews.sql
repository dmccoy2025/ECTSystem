CREATE TABLE [dbo].[core_WorkflowViews] (
    [workflowId] TINYINT  NOT NULL,
    [pageId]     SMALLINT NOT NULL
);
GO

ALTER TABLE [dbo].[core_WorkflowViews]
    ADD CONSTRAINT [FK_core_WorkflowViews_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_WorkflowViews]
    ADD CONSTRAINT [FK_core_WorkflowViews_core_Pages] FOREIGN KEY ([pageId]) REFERENCES [dbo].[core_Pages] ([pageId]);
GO

