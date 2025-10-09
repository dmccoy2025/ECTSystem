CREATE TABLE [dbo].[core_Memos2] (
    [memoId]       INT      IDENTITY (1, 1) NOT NULL,
    [refId]        INT      NOT NULL,
    [moduleId]     INT      NOT NULL,
    [templateId]   TINYINT  NOT NULL,
    [deleted]      BIT      NOT NULL,
    [letterHead]   TINYINT  NOT NULL,
    [created_by]   INT      NOT NULL,
    [created_date] DATETIME NOT NULL
);
GO

ALTER TABLE [dbo].[core_Memos2]
    ADD CONSTRAINT [DF_core_Memos_creationDate2] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_Memos2]
    ADD CONSTRAINT [DF_core_Memos_deleted2] DEFAULT ((0)) FOR [deleted];
GO

ALTER TABLE [dbo].[core_Memos2]
    ADD CONSTRAINT [FK_core_Memos_core_memoLetterheads2] FOREIGN KEY ([letterHead]) REFERENCES [dbo].[core_memoLetterheads] ([id]);
GO

ALTER TABLE [dbo].[core_Memos2]
    ADD CONSTRAINT [FK_core_Memos_core_MemoTemplates2] FOREIGN KEY ([templateId]) REFERENCES [dbo].[core_MemoTemplates] ([templateId]);
GO

ALTER TABLE [dbo].[core_Memos2]
    ADD CONSTRAINT [FK_core_Memos_core_Users2] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_Memos2]
    ADD CONSTRAINT [PK_core_Memos2] PRIMARY KEY CLUSTERED ([memoId] ASC) WITH (FILLFACTOR = 80);
GO

