CREATE TABLE [dbo].[core_AssociatedCases] (
    [workflowId]          TINYINT       NOT NULL,
    [refId]               INT           NOT NULL,
    [associated_workflow] TINYINT       NOT NULL,
    [associated_refId]    INT           NOT NULL,
    [associated_CaseId]   VARCHAR (100) NULL
);
GO

