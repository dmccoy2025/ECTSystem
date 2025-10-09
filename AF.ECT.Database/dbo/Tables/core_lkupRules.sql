CREATE TABLE [dbo].[core_lkupRules] (
    [Id]       TINYINT      IDENTITY (1, 1) NOT NULL,
    [workFlow] TINYINT      NULL,
    [ruleType] TINYINT      NOT NULL,
    [name]     VARCHAR (50) NOT NULL,
    [prompt]   VARCHAR (50) NULL,
    [active]   BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupRules]
    ADD CONSTRAINT [FK_core_lkupRules_core_lkupRuleTypes] FOREIGN KEY ([ruleType]) REFERENCES [dbo].[core_lkupRuleTypes] ([Id]);
GO

ALTER TABLE [dbo].[core_lkupRules]
    ADD CONSTRAINT [FK_core_lkupRules_core_Workflow] FOREIGN KEY ([workFlow]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_lkupRules]
    ADD CONSTRAINT [PK_core_lkupRules] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

