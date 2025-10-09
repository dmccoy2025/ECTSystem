CREATE TABLE [dbo].[core_MessagesGroups] (
    [messageID] SMALLINT NOT NULL,
    [groupID]   TINYINT  NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IDX_core_MessagesGroups_1]
    ON [dbo].[core_MessagesGroups]([groupID] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_MessagesGroups]
    ADD CONSTRAINT [FK_core_MessagesGroups_core_UserGroups] FOREIGN KEY ([groupID]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

