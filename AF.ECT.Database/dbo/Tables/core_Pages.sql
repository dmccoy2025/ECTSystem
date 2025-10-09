CREATE TABLE [dbo].[core_Pages] (
    [pageId] SMALLINT     NOT NULL,
    [title]  VARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_Pages]
    ADD CONSTRAINT [PK_core_Pages] PRIMARY KEY CLUSTERED ([pageId] ASC) WITH (FILLFACTOR = 80);
GO

