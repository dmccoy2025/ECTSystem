CREATE TABLE [dbo].[core_lkupCompo] (
    [compo]        CHAR (1)     NOT NULL,
    [compo_descr]  VARCHAR (50) NOT NULL,
    [abbreviation] VARCHAR (50) NULL
);
GO

ALTER TABLE [dbo].[core_lkupCompo]
    ADD CONSTRAINT [PK_core_lkupCompo] PRIMARY KEY CLUSTERED ([compo] ASC) WITH (FILLFACTOR = 80);
GO

