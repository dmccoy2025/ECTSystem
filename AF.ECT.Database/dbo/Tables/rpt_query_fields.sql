CREATE TABLE [dbo].[rpt_query_fields] (
    [id]         INT          IDENTITY (1, 1) NOT NULL,
    [field_name] VARCHAR (50) NULL,
    [query_type] VARCHAR (20) NULL,
    [sort_order] INT          DEFAULT ((1)) NULL,
    [table_name] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80)
);
GO

CREATE NONCLUSTERED INDEX [IX_RPT_QUERY_FIELDS_QUERY_TYPE]
    ON [dbo].[rpt_query_fields]([query_type] ASC) WITH (FILLFACTOR = 80);
GO

