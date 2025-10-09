CREATE TABLE [dbo].[GradeAbbreviation] (
    [AbbreviationTypeId] INT           NOT NULL,
    [GradeCode]          INT           NOT NULL,
    [Abbreviation]       NVARCHAR (25) NOT NULL
);
GO

ALTER TABLE [dbo].[GradeAbbreviation]
    ADD CONSTRAINT [PK_GradeAbbreviation] PRIMARY KEY CLUSTERED ([AbbreviationTypeId] ASC, [GradeCode] ASC);
GO

ALTER TABLE [dbo].[GradeAbbreviation]
    ADD CONSTRAINT [FK_GradeAbbreviation_core_lkupGrade] FOREIGN KEY ([GradeCode]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

ALTER TABLE [dbo].[GradeAbbreviation]
    ADD CONSTRAINT [FK_GradeAbbreviation_core_lkupGradeAbbreviationType] FOREIGN KEY ([AbbreviationTypeId]) REFERENCES [dbo].[core_lkupGradeAbbreviationType] ([Id]);
GO

