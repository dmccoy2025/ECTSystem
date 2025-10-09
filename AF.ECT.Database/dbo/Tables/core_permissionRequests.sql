CREATE TABLE [dbo].[core_permissionRequests] (
    [reqId]   INT      IDENTITY (1, 1) NOT NULL,
    [userId]  INT      NOT NULL,
    [permId]  SMALLINT NOT NULL,
    [dateReq] DATETIME NOT NULL
);
GO

ALTER TABLE [dbo].[core_permissionRequests]
    ADD CONSTRAINT [DF_core_permissionRequests_dateReq] DEFAULT (getdate()) FOR [dateReq];
GO

ALTER TABLE [dbo].[core_permissionRequests]
    ADD CONSTRAINT [FK_core_permissionRequests_core_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_permissionRequests]
    ADD CONSTRAINT [FK_core_permissionRequests_core_Permissions] FOREIGN KEY ([permId]) REFERENCES [dbo].[core_Permissions] ([permId]);
GO

