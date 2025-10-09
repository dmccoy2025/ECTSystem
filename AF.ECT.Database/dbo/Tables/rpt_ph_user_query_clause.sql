CREATE TABLE [dbo].[rpt_ph_user_query_clause] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [PHQueryId] INT          NOT NULL,
    [WhereType] NVARCHAR (3) NOT NULL
);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_clause]
    ADD CONSTRAINT [FK_rpt_ph_user_query_clause_rpt_ph_user_query] FOREIGN KEY ([PHQueryId]) REFERENCES [dbo].[rpt_ph_user_query] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[rpt_ph_user_query_clause]
    ADD CONSTRAINT [CK_rpt_ph_user_query_clause_WhereType] CHECK ([WhereType]='OR' OR [WhereType]='AND');
GO

ALTER TABLE [dbo].[rpt_ph_user_query_clause]
    ADD CONSTRAINT [PK_rpt_ph_user_query_clause] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

