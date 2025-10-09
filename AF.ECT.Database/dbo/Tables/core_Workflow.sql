CREATE TABLE [dbo].[core_Workflow] (
    [workflowId]    TINYINT      IDENTITY (1, 1) NOT NULL,
    [moduleId]      TINYINT      NOT NULL,
    [compo]         CHAR (1)     NOT NULL,
    [title]         VARCHAR (50) NOT NULL,
    [formal]        BIT          NOT NULL,
    [active]        BIT          NOT NULL,
    [initialStatus] INT          NULL
);
GO

ALTER TABLE [dbo].[core_Workflow]
    ADD CONSTRAINT [DF_core_Workflow_formal] DEFAULT ((0)) FOR [formal];
GO

ALTER TABLE [dbo].[core_Workflow]
    ADD CONSTRAINT [DF_core_Workflow_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_Workflow]
    ADD CONSTRAINT [FK_core_Workflow_core_lkupModule] FOREIGN KEY ([moduleId]) REFERENCES [dbo].[core_lkupModule] ([moduleId]);
GO

ALTER TABLE [dbo].[core_Workflow]
    ADD CONSTRAINT [FK_core_Workflow_core_WorkStatus] FOREIGN KEY ([initialStatus]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_Workflow]
    ADD CONSTRAINT [PK_core_Workflow] PRIMARY KEY CLUSTERED ([workflowId] ASC) WITH (FILLFACTOR = 80);
GO

