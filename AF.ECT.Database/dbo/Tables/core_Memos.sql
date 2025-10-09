CREATE TABLE [dbo].[core_Memos] (
    [memoId]       INT      IDENTITY (1, 1) NOT NULL,
    [refId]        INT      NOT NULL,
    [templateId]   TINYINT  NOT NULL,
    [deleted]      BIT      NOT NULL,
    [letterHead]   TINYINT  NOT NULL,
    [created_by]   INT      NOT NULL,
    [created_date] DATETIME NOT NULL
);
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [FK_core_Memos_core_Users] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [FK_core_Memos_core_Memos] FOREIGN KEY ([memoId]) REFERENCES [dbo].[core_Memos] ([memoId]);
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [FK_core_Memos_core_MemoTemplates] FOREIGN KEY ([templateId]) REFERENCES [dbo].[core_MemoTemplates] ([templateId]);
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [FK_core_Memos_core_memoLetterheads] FOREIGN KEY ([letterHead]) REFERENCES [dbo].[core_memoLetterheads] ([id]);
GO

CREATE NONCLUSTERED INDEX [IX_core_Memos_PostCompletion]
    ON [dbo].[core_Memos]([templateId] ASC)
    INCLUDE([refId], [deleted]) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_core_Memos_PostCompleteion]
    ON [dbo].[core_Memos]([templateId] ASC)
    INCLUDE([deleted], [refId]);
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [DF_core_Memos_deleted] DEFAULT ((0)) FOR [deleted];
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [DF_core_Memos_creationDate] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_Memos]
    ADD CONSTRAINT [PK_core_Memos] PRIMARY KEY CLUSTERED ([memoId] ASC) WITH (FILLFACTOR = 80);
GO

