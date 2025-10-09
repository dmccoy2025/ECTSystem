CREATE TABLE [dbo].[core_PageAccess] (
    [mapId]      INT     IDENTITY (1, 1) NOT NULL,
    [statusId]   INT     NOT NULL,
    [groupId]    TINYINT NOT NULL,
    [workflowId] TINYINT NOT NULL,
    [access]     TINYINT NOT NULL,
    [pageId]     INT     NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_PAGE_ACCESS_WORKFLOWID]
    ON [dbo].[core_PageAccess]([statusId] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_PAGE_ACCESS_GROUPID]
    ON [dbo].[core_PageAccess]([groupId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_PageAccess]
    ADD CONSTRAINT [DF_core_PageAccess_edit] DEFAULT ((0)) FOR [access];
GO

ALTER TABLE [dbo].[core_PageAccess]
    ADD CONSTRAINT [FK_core_PageAccess_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_PageAccess]
    ADD CONSTRAINT [PK_core_PageAccess] PRIMARY KEY CLUSTERED ([mapId] ASC) WITH (FILLFACTOR = 80);
GO

