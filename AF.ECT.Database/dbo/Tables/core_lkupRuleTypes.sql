CREATE TABLE [dbo].[core_lkupRuleTypes] (
    [Id]          TINYINT      IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupRuleTypes]
    ADD CONSTRAINT [PK_core_lkupRuleTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

