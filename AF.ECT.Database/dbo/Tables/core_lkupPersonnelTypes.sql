CREATE TABLE [dbo].[core_lkupPersonnelTypes] (
    [Id]          SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (50)  NOT NULL,
    [Formal]      BIT           NOT NULL,
    [RoleName]    VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_lkupPersonnelTypes]
    ADD CONSTRAINT [PK_core_lkupPersonnelTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

