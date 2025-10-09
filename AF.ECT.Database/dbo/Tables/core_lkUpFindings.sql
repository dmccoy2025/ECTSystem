CREATE TABLE [dbo].[core_lkUpFindings] (
    [Id]          TINYINT       IDENTITY (1, 1) NOT NULL,
    [findingType] VARCHAR (50)  NULL,
    [Description] VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_lkUpFindings]
    ADD CONSTRAINT [PK_core_lkUpFindings] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

