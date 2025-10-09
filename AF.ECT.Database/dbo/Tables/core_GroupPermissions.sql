CREATE TABLE [dbo].[core_GroupPermissions] (
    [groupId] TINYINT  NOT NULL,
    [permId]  SMALLINT NOT NULL
);
GO

ALTER TABLE [dbo].[core_GroupPermissions]
    ADD CONSTRAINT [fK_RolePermissionsPermID_To_PermissionsPermID] FOREIGN KEY ([permId]) REFERENCES [dbo].[core_Permissions] ([permId]);
GO

ALTER TABLE [dbo].[core_GroupPermissions]
    ADD CONSTRAINT [FK_core_GroupPermissions_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

