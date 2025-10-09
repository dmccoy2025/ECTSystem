CREATE TABLE [dbo].[PrintDocument] (
    [docid]             SMALLINT     NOT NULL,
    [doc_name]          VARCHAR (50) NOT NULL,
    [filename]          VARCHAR (50) NOT NULL,
    [filetype]          VARCHAR (20) NOT NULL,
    [compo]             SMALLINT     NULL,
    [sp_getdata]        VARCHAR (50) NULL,
    [FormFieldParserId] INT          DEFAULT (NULL) NULL
);
GO

ALTER TABLE [dbo].[PrintDocument]
    ADD CONSTRAINT [FK_PrintDocument_PrintDocumentFormFieldParser] FOREIGN KEY ([FormFieldParserId]) REFERENCES [dbo].[PrintDocumentFormFieldParser] ([Id]);
GO

