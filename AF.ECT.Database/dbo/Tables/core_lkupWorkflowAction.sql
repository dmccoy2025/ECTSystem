CREATE TABLE [dbo].[core_lkupWorkflowAction] (
    [type] TINYINT      IDENTITY (1, 1) NOT NULL,
    [text] VARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupWorkflowAction]
    ADD CONSTRAINT [PK_core_lkupWorkflowAction] PRIMARY KEY CLUSTERED ([type] ASC) WITH (FILLFACTOR = 80);
GO

