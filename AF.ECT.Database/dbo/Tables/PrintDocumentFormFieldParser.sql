CREATE TABLE [dbo].[PrintDocumentFormFieldParser] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[PrintDocumentFormFieldParser]
    ADD CONSTRAINT [PK_PrintDocumentFormFieldParser] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

