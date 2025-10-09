CREATE TABLE [dbo].[rpt_ph_user_query_param] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [PHClauseId]   INT            NOT NULL,
    [WhereType]    NVARCHAR (3)   NOT NULL,
    [OperatorType] NVARCHAR (50)  NOT NULL,
    [StartValue]   NVARCHAR (200) NOT NULL,
    [StartDisplay] NVARCHAR (200) NOT NULL,
    [EndValue]     NVARCHAR (200) NULL,
    [EndDisplay]   NVARCHAR (200) NULL,
    [ExecuteOrder] INT            DEFAULT ((1)) NOT NULL
);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_param]
    ADD CONSTRAINT [PK_rpt_ph_user_query_param] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_param]
    ADD CONSTRAINT [CK_rpt_ph_user_query_param_OperatorType] CHECK ([OperatorType]='between' OR [OperatorType]='greater' OR [OperatorType]='less' OR [OperatorType]='equal' OR [OperatorType]='like' OR [OperatorType]='notequal');
GO

ALTER TABLE [dbo].[rpt_ph_user_query_param]
    ADD CONSTRAINT [CK_rpt_ph_user_query_param_WhereType] CHECK ([WhereType]='OR' OR [WhereType]='AND');
GO

ALTER TABLE [dbo].[rpt_ph_user_query_param]
    ADD CONSTRAINT [FK_rpt_ph_user_query_param_rpt_ph_user_query_clause] FOREIGN KEY ([PHClauseId]) REFERENCES [dbo].[rpt_ph_user_query_clause] ([Id]) ON DELETE CASCADE;
GO

