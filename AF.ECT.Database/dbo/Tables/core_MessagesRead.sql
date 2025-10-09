CREATE TABLE [dbo].[core_MessagesRead] (
    [MessageID] SMALLINT NOT NULL,
    [userID]    INT      NOT NULL,
    [DateRead]  DATETIME NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IDX_core_MessagesRead_1]
    ON [dbo].[core_MessagesRead]([userID] ASC) WITH (FILLFACTOR = 90);
GO

