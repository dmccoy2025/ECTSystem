CREATE TABLE [dbo].[core_lkupDbSignTemplates] (
    [t_id]                TINYINT       NOT NULL,
    [title]               VARCHAR (100) NULL,
    [template_table_name] VARCHAR (100) DEFAULT (NULL) NULL,
    [primary_key]         VARCHAR (100) DEFAULT ('') NOT NULL,
    [secondary_key]       VARCHAR (100) DEFAULT (NULL) NULL
);
GO

ALTER TABLE [dbo].[core_lkupDbSignTemplates]
    ADD CONSTRAINT [PK_core_lkupDbSignTemplates] PRIMARY KEY CLUSTERED ([t_id] ASC) WITH (FILLFACTOR = 80);
GO

