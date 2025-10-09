CREATE TABLE [dbo].[rpt_ph_user_query] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserQueryId] INT            NOT NULL,
    [ReportType]  INT            NOT NULL,
    [SortField]   NVARCHAR (MAX) NULL
);
GO

ALTER TABLE [dbo].[rpt_ph_user_query]
    ADD CONSTRAINT [PK_rpt_ph_user_query] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[rpt_ph_user_query]
    ADD CONSTRAINT [FK_rpt_ph_user_query_rpt_user_query] FOREIGN KEY ([UserQueryId]) REFERENCES [dbo].[rpt_user_query] ([id]);
GO

