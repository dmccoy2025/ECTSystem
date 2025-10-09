CREATE TABLE [dbo].[core_WorkStatus_Actions] (
    [wsa_id]     INT     IDENTITY (1, 1) NOT NULL,
    [wso_id]     INT     NOT NULL,
    [actionType] TINYINT NOT NULL,
    [target]     INT     NOT NULL,
    [data]       INT     NULL
);
GO

ALTER TABLE [dbo].[core_WorkStatus_Actions]
    ADD CONSTRAINT [FK_core_WorkStatus_Actions_core_lkupWorkflowAction] FOREIGN KEY ([actionType]) REFERENCES [dbo].[core_lkupWorkflowAction] ([type]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Actions]
    ADD CONSTRAINT [FK_core_WorkStatus_Actions_core_WorkStatus_Options] FOREIGN KEY ([wso_id]) REFERENCES [dbo].[core_WorkStatus_Options] ([wso_id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Actions]
    ADD CONSTRAINT [PK_core_WorkStatus_Actions] PRIMARY KEY CLUSTERED ([wsa_id] ASC) WITH (FILLFACTOR = 80);
GO

