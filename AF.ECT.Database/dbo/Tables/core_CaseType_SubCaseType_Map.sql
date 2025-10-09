CREATE TABLE [dbo].[core_CaseType_SubCaseType_Map] (
    [CaseTypeId]    INT NOT NULL,
    [SubCaseTypeId] INT NOT NULL
);
GO

ALTER TABLE [dbo].[core_CaseType_SubCaseType_Map]
    ADD CONSTRAINT [FK_CaseType_SubCaseType_Map_core_SubCaseType] FOREIGN KEY ([SubCaseTypeId]) REFERENCES [dbo].[core_SubCaseType] ([Id]);
GO

ALTER TABLE [dbo].[core_CaseType_SubCaseType_Map]
    ADD CONSTRAINT [FK_CaseType_SubCaseType_Map_core_CaseType] FOREIGN KEY ([CaseTypeId]) REFERENCES [dbo].[core_CaseType] ([Id]);
GO

