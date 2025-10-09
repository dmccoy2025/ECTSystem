CREATE TABLE [dbo].[rpt_user_query_param] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [clause_id]     INT           NOT NULL,
    [source_id]     INT           NOT NULL,
    [operator_type] VARCHAR (50)  NOT NULL,
    [where_type]    VARCHAR (3)   NOT NULL,
    [start_value]   VARCHAR (200) NOT NULL,
    [start_display] VARCHAR (200) NOT NULL,
    [end_value]     VARCHAR (200) NULL,
    [end_display]   VARCHAR (200) NULL,
    [execute_order] INT           NULL
);
GO

ALTER TABLE [dbo].[rpt_user_query_param]
    ADD CONSTRAINT [CK_rpt_user_query_param] CHECK ([where_type]='OR' OR [where_type]='AND');
GO

ALTER TABLE [dbo].[rpt_user_query_param]
    ADD CONSTRAINT [CK_rpt_user_query_param_1] CHECK ([operator_type]='between' OR [operator_type]='greater' OR [operator_type]='less' OR [operator_type]='equal' OR [operator_type]='like' OR [operator_type]='notequal');
GO

ALTER TABLE [dbo].[rpt_user_query_param]
    ADD CONSTRAINT [FK_rpt_user_query_param_rpt_user_query_clause] FOREIGN KEY ([clause_id]) REFERENCES [dbo].[rpt_user_query_clause] ([id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[rpt_user_query_param]
    ADD CONSTRAINT [FK_rpt_user_query_param_rpt_query_source] FOREIGN KEY ([source_id]) REFERENCES [dbo].[rpt_query_source] ([id]);
GO

ALTER TABLE [dbo].[rpt_user_query_param]
    ADD CONSTRAINT [PK_rpt_user_query_param] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

