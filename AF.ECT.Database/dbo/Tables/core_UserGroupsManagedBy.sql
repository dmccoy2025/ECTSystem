CREATE TABLE [dbo].[core_UserGroupsManagedBy] (
    [groupId]   TINYINT NOT NULL,
    [managedBy] TINYINT NOT NULL,
    [notify]    BIT     NULL
);
GO

ALTER TABLE [dbo].[core_UserGroupsManagedBy]
    ADD CONSTRAINT [FK_core_UserGroupsManagers_core_UserGroups1] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_UserGroupsManagedBy]
    ADD CONSTRAINT [FK_core_UserGroupsManagers_core_UserGroups] FOREIGN KEY ([managedBy]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_UserGroupsManagedBy]
    ADD CONSTRAINT [DF_core_UserGroupsManagedBy_notify] DEFAULT ((0)) FOR [notify];
GO

CREATE NONCLUSTERED INDEX [IX_MANAGEDBY]
    ON [dbo].[core_UserGroupsManagedBy]([managedBy] ASC) WITH (FILLFACTOR = 80);
GO

