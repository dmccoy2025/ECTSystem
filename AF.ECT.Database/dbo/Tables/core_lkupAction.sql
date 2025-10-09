CREATE TABLE [dbo].[core_lkupAction] (
    [actionId]   TINYINT      NOT NULL,
    [actionName] VARCHAR (50) NOT NULL,
    [hide]       BIT          NOT NULL,
    [logChanges] BIT          NULL
);
GO

ALTER TABLE [dbo].[core_lkupAction]
    ADD CONSTRAINT [DF_core_lkupAction_logChanges] DEFAULT ((0)) FOR [logChanges];
GO

ALTER TABLE [dbo].[core_lkupAction]
    ADD CONSTRAINT [DF_core_lkupAction_hide] DEFAULT ((0)) FOR [hide];
GO

ALTER TABLE [dbo].[core_lkupAction]
    ADD CONSTRAINT [PK_core_lkupAction] PRIMARY KEY CLUSTERED ([actionId] ASC) WITH (FILLFACTOR = 90);
GO

