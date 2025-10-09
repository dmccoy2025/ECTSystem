CREATE TABLE [dbo].[PALDocumentLookup] (
    [PalDocID] INT           NOT NULL,
    [URL]      VARCHAR (500) NOT NULL,
    [LastName] VARCHAR (50)  NOT NULL,
    [Last4SSN] CHAR (4)      NOT NULL,
    [DocYear]  INT           NOT NULL,
    [DocMonth] INT           NOT NULL
);
GO

ALTER TABLE [dbo].[PALDocumentLookup]
    ADD CONSTRAINT [PK_PALDocumentLookup] PRIMARY KEY CLUSTERED ([PalDocID] ASC) WITH (FILLFACTOR = 90);
GO

