CREATE TABLE [dbo].[core_lkupAccessScope] (
    [scopeId]     TINYINT      IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupAccessScope]
    ADD CONSTRAINT [PK_lkupAccessScope] PRIMARY KEY CLUSTERED ([scopeId] ASC) WITH (FILLFACTOR = 90);
GO

