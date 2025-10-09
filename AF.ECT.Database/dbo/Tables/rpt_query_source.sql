CREATE TABLE [dbo].[rpt_query_source] (
    [id]                 INT          IDENTITY (1, 1) NOT NULL,
    [display_name]       VARCHAR (50) NOT NULL,
    [field_name]         VARCHAR (50) NOT NULL,
    [table_name]         VARCHAR (50) NOT NULL,
    [data_type]          CHAR (1)     NOT NULL,
    [lookup_source]      VARCHAR (50) NULL,
    [lookup_value]       VARCHAR (50) NULL,
    [lookup_text]        VARCHAR (50) NULL,
    [lookup_sort]        VARCHAR (50) NULL,
    [lookup_where]       VARCHAR (50) NULL,
    [lookup_where_value] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);
GO

ALTER TABLE [dbo].[rpt_query_source]
    ADD CONSTRAINT [CK_rpt_query_select_fields] CHECK ([data_type]='N' OR [data_type]='C' OR [data_type]='B' OR [data_type]='D' OR [data_type]='T');
GO

