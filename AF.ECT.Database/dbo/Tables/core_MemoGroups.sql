CREATE TABLE [dbo].[core_MemoGroups] (
    [id]         INT     IDENTITY (1, 1) NOT NULL,
    [templateId] TINYINT NOT NULL,
    [groupId]    TINYINT NOT NULL,
    [canCreate]  BIT     NOT NULL,
    [canEdit]    BIT     NOT NULL,
    [canDelete]  BIT     NOT NULL,
    [canView]    BIT     NOT NULL
);
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [FK_core_MemoGroups_core_MemoTemplates] FOREIGN KEY ([templateId]) REFERENCES [dbo].[core_MemoTemplates] ([templateId]);
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [FK_core_MemoGroups_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [DF_core_MemoGroups_canDelete] DEFAULT ((0)) FOR [canDelete];
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [DF_core_MemoGroups_canView] DEFAULT ((0)) FOR [canView];
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [DF_core_MemoGroups_canEdit] DEFAULT ((0)) FOR [canEdit];
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [DF_core_MemoGroups_canCreate] DEFAULT ((0)) FOR [canCreate];
GO

ALTER TABLE [dbo].[core_MemoGroups]
    ADD CONSTRAINT [PK_core_MemoGroups] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

