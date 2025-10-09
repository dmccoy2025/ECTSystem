CREATE TABLE [dbo].[ReminderEmailSettings] (
    [id]           INT     IDENTITY (1, 1) NOT NULL,
    [workflowId]   TINYINT NOT NULL,
    [wsId]         INT     NOT NULL,
    [compo]        BIGINT  NOT NULL,
    [groupId]      TINYINT NOT NULL,
    [templateId]   INT     NOT NULL,
    [intervalTime] INT     NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([templateId]) REFERENCES [dbo].[core_EmailTemplates] ([TemplateID])
);
GO

ALTER TABLE [dbo].[ReminderEmailSettings]
    ADD CONSTRAINT [FK__ReminderEm__wsId__41D98783] FOREIGN KEY ([wsId]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[ReminderEmailSettings]
    ADD CONSTRAINT [FK__ReminderE__workf__72F1C02A] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[ReminderEmailSettings]
    ADD CONSTRAINT [FK__ReminderE__group__42CDABBC] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

