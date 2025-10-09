CREATE TABLE [dbo].[core_lkupYearsSatisfactoryService] (
    [Id]            TINYINT        NOT NULL,
    [RangeCategory] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupYearsSatisfactoryService]
    ADD CONSTRAINT [PK_core_lkupYearsSatisfactoryService] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

