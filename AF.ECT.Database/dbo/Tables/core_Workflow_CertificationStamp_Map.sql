CREATE TABLE [dbo].[core_Workflow_CertificationStamp_Map] (
    [WorkflowId]  TINYINT NOT NULL,
    [CertStampId] INT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_Workflow_CertificationStamp_Map]
    ADD CONSTRAINT [FK_Workflow_CertificationStamp_Map_core_CertificationStamp] FOREIGN KEY ([CertStampId]) REFERENCES [dbo].[core_CertificationStamp] ([Id]);
GO

ALTER TABLE [dbo].[core_Workflow_CertificationStamp_Map]
    ADD CONSTRAINT [FK_Workflow_CertificationStamp_Map_core_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

