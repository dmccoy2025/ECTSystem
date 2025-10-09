CREATE TABLE [dbo].[core_UsersOnline] (
    [userId]        INT           NOT NULL,
    [groupId]       INT           NOT NULL,
    [loginTime]     SMALLDATETIME NOT NULL,
    [lastAccess]    SMALLDATETIME NOT NULL,
    [sessionId]     NVARCHAR (50) NULL,
    [remoteAddress] NVARCHAR (50) NULL
);
GO

ALTER TABLE [dbo].[core_UsersOnline]
    ADD CONSTRAINT [DF_core_UsersOnline_loginTime] DEFAULT (getdate()) FOR [loginTime];
GO

ALTER TABLE [dbo].[core_UsersOnline]
    ADD CONSTRAINT [DF_core_UsersOnline_lastAccess] DEFAULT (getdate()) FOR [lastAccess];
GO

