CREATE TABLE [dbo].[core_MemoContents2] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [memoId]       INT           NOT NULL,
    [body]         VARCHAR (MAX) NOT NULL,
    [sigBlock]     VARCHAR (200) NULL,
    [suspenseDate] VARCHAR (20)  NULL,
    [memoDate]     VARCHAR (20)  NULL,
    [created_date] DATETIME      NOT NULL,
    [created_by]   INT           NOT NULL,
    [attachments]  VARCHAR (400) NULL
);
GO

ALTER TABLE [dbo].[core_MemoContents2]
    ADD CONSTRAINT [FK_core_MemoContents_core_Users2] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_MemoContents2]
    ADD CONSTRAINT [FK_core_MemoContents_core_Memos2] FOREIGN KEY ([memoId]) REFERENCES [dbo].[core_Memos2] ([memoId]);
GO

ALTER TABLE [dbo].[core_MemoContents2]
    ADD CONSTRAINT [DF_core_MemoContents_creationDate2] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_MemoContents2]
    ADD CONSTRAINT [PK_core_MemoContents2] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

