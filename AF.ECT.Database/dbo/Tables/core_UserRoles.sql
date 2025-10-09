CREATE TABLE [dbo].[core_UserRoles] (
    [userRoleID] INT     IDENTITY (1, 1) NOT NULL,
    [groupId]    TINYINT NOT NULL,
    [userID]     INT     NOT NULL,
    [status]     TINYINT NOT NULL,
    [active]     BIT     NOT NULL
);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'userRoleID identifies UserRoles', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'core_UserRoles', @level2type = N'COLUMN', @level2name = N'userRoleID';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'roleID from table Roles', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'core_UserRoles', @level2type = N'COLUMN', @level2name = N'groupId';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UserId from table Users', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'core_UserRoles', @level2type = N'COLUMN', @level2name = N'userID';
GO

ALTER TABLE [dbo].[core_UserRoles]
    ADD CONSTRAINT [DF_core_UserRoles_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_UserRoles]
    ADD CONSTRAINT [FK_UserRolesUserID_To_UsersUserID] FOREIGN KEY ([userID]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_UserRoles]
    ADD CONSTRAINT [FK_core_UserRoles_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_UserRoles]
    ADD CONSTRAINT [FK_core_UserRoles_core_lkupAccessStatus] FOREIGN KEY ([status]) REFERENCES [dbo].[core_lkupAccessStatus] ([statusId]);
GO

ALTER TABLE [dbo].[core_UserRoles]
    ADD CONSTRAINT [PK_UserRoles_1] PRIMARY KEY CLUSTERED ([userRoleID] ASC) WITH (FILLFACTOR = 80);
GO

