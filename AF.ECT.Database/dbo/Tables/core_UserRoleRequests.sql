CREATE TABLE [dbo].[core_UserRoleRequests] (
    [id]                 INT            IDENTITY (1, 1) NOT NULL,
    [status]             TINYINT        NOT NULL,
    [userId]             INT            NOT NULL,
    [requested_group_id] TINYINT        NOT NULL,
    [existing_group_id]  TINYINT        NULL,
    [new_role]           BIT            NOT NULL,
    [requestor_comment]  VARCHAR (1000) NULL,
    [requested_date]     DATETIME       NOT NULL,
    [completed_by]       INT            NULL,
    [completed_date]     DATETIME       NULL,
    [completed_comment]  VARCHAR (500)  NULL
);
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [FK_core_UserRoleRequests_core_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [FK_core_UserRoleRequests_core_UserGroups1] FOREIGN KEY ([existing_group_id]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [FK_core_UserRoleRequests_core_UserGroups] FOREIGN KEY ([requested_group_id]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [FK_core_UserRoleRequests_core_lkupAccessStatus] FOREIGN KEY ([status]) REFERENCES [dbo].[core_lkupAccessStatus] ([statusId]);
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [FK_core_UserRoleRequests_core_Users1] FOREIGN KEY ([completed_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [DF_core_UserRoleRequests_requested_date] DEFAULT (getdate()) FOR [requested_date];
GO

ALTER TABLE [dbo].[core_UserRoleRequests]
    ADD CONSTRAINT [DF_core_UserRoleRequests_new_role] DEFAULT ((0)) FOR [new_role];
GO

