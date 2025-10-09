CREATE TABLE [dbo].[DocumentView] (
    [viewId]      INT           IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[DocumentView]
    ADD CONSTRAINT [PK_DocumentView] PRIMARY KEY CLUSTERED ([viewId] ASC) WITH (FILLFACTOR = 80);
GO

