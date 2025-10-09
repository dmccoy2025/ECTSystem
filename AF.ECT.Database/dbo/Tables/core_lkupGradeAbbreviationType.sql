CREATE TABLE [dbo].[core_lkupGradeAbbreviationType] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupGradeAbbreviationType]
    ADD CONSTRAINT [PK_core_lkupGradeAbbreviationType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

