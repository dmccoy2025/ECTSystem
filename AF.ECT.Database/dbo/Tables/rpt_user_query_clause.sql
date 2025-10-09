CREATE TABLE [dbo].[rpt_user_query_clause] (
    [id]         INT         IDENTITY (1, 1) NOT NULL,
    [query_id]   INT         NOT NULL,
    [where_type] VARCHAR (3) NOT NULL
);
GO

ALTER TABLE [dbo].[rpt_user_query_clause]
    ADD CONSTRAINT [FK_rpt_user_query_clause_rpt_user_query] FOREIGN KEY ([query_id]) REFERENCES [dbo].[rpt_user_query] ([id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[rpt_user_query_clause]
    ADD CONSTRAINT [PK_rpt_user_query_clause] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[rpt_user_query_clause]
    ADD CONSTRAINT [CK_rpt_user_query_clause] CHECK ([where_type]='OR' OR [where_type]='AND');
GO

CREATE NONCLUSTERED INDEX [IX_USER_QUERY_CLAUSE_QUERY_ID]
    ON [dbo].[rpt_user_query_clause]([query_id] ASC) WITH (FILLFACTOR = 80);
GO

