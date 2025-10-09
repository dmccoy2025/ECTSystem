CREATE TABLE [dbo].[core_StatusCodeSigners] (
    [status]  INT     NOT NULL,
    [groupId] TINYINT NOT NULL
);
GO

ALTER TABLE [dbo].[core_StatusCodeSigners]
    ADD CONSTRAINT [FK_core_StatusCodeSigners_core_WorkStatus] FOREIGN KEY ([status]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_StatusCodeSigners]
    ADD CONSTRAINT [FK_core_StatusCodeSigners_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

CREATE NONCLUSTERED INDEX [IX_STATUSCODESIGNERS_GROUPID]
    ON [dbo].[core_StatusCodeSigners]([groupId] ASC) WITH (FILLFACTOR = 80);
GO

