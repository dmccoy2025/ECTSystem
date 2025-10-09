CREATE TABLE [dbo].[core_WorkStatus_Rules] (
    [wsr_id]   INT            IDENTITY (1, 1) NOT NULL,
    [wso_id]   INT            NOT NULL,
    [ruleId]   TINYINT        NOT NULL,
    [ruleData] VARCHAR (1000) NOT NULL,
    [checkAll] BIT            NULL
);
GO

ALTER TABLE [dbo].[core_WorkStatus_Rules]
    ADD CONSTRAINT [FK_core_WorkStatus_Rules_core_WorkStatus_Options] FOREIGN KEY ([wso_id]) REFERENCES [dbo].[core_WorkStatus_Options] ([wso_id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Rules]
    ADD CONSTRAINT [FK_core_WorkStatus_Rules_core_lkupRules] FOREIGN KEY ([ruleId]) REFERENCES [dbo].[core_lkupRules] ([Id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Rules]
    ADD CONSTRAINT [PK_core_WorkStatus_Rules] PRIMARY KEY CLUSTERED ([wsr_id] ASC) WITH (FILLFACTOR = 80);
GO

