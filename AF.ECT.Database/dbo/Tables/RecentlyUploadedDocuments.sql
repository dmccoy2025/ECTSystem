CREATE TABLE [dbo].[RecentlyUploadedDocuments] (
    [RefId]      INT    NOT NULL,
    [DocGroupId] BIGINT NOT NULL,
    [DocId]      BIGINT NULL,
    [DocTypeId]  INT    NULL
);
GO

ALTER TABLE [dbo].[RecentlyUploadedDocuments]
    ADD CONSTRAINT [PK_RecentlyUploadedDocuments] PRIMARY KEY CLUSTERED ([RefId] ASC, [DocGroupId] ASC) WITH (FILLFACTOR = 90);
GO

