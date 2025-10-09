CREATE TABLE [dbo].[core_UserPermissions] (
    [userId] INT      NOT NULL,
    [permId] SMALLINT NOT NULL,
    [status] CHAR (1) NOT NULL
);
GO

ALTER TABLE [dbo].[core_UserPermissions]
    ADD CONSTRAINT [DF_core_UserPermissions_status] DEFAULT ('G') FOR [status];
GO

ALTER TABLE [dbo].[core_UserPermissions]
    ADD CONSTRAINT [FK_core_UserPermissions_core_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_UserPermissions]
    ADD CONSTRAINT [FK_core_UserPermissions_core_Permissions] FOREIGN KEY ([permId]) REFERENCES [dbo].[core_Permissions] ([permId]);
GO

ALTER TABLE [dbo].[core_UserPermissions]
    ADD CONSTRAINT [CK_core_UserPermissions] CHECK ([status]='R' OR [status]='G');
GO

