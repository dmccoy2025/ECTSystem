CREATE TABLE [dbo].[core_lkupPWCategories] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [Para_Info] VARCHAR (20) NULL
);
GO

ALTER TABLE [dbo].[core_lkupPWCategories]
    ADD CONSTRAINT [PK_core_lkupPWCategories] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

