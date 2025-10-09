CREATE TABLE [dbo].[rpt_user_query] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [user_id]       INT           NOT NULL,
    [title]         VARCHAR (100) NOT NULL,
    [created_date]  DATETIME      NOT NULL,
    [modified_date] DATETIME      NOT NULL,
    [transient]     BIT           NOT NULL,
    [output_fields] VARCHAR (MAX) NULL,
    [sort_fields]   VARCHAR (MAX) NULL,
    [shared]        BIT           DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[rpt_user_query]
    ADD CONSTRAINT [FK_rpt_user_query_core_Users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[rpt_user_query]
    ADD CONSTRAINT [DF_Table_1_is_default] DEFAULT ((0)) FOR [title];
GO

ALTER TABLE [dbo].[rpt_user_query]
    ADD CONSTRAINT [DF_rpt_user_query_transient] DEFAULT ((0)) FOR [transient];
GO

ALTER TABLE [dbo].[rpt_user_query]
    ADD CONSTRAINT [DF_rpt_user_queries_created_date] DEFAULT (getdate()) FOR [created_date];
GO

CREATE NONCLUSTERED INDEX [IX_USER_QUERY_USER_ID]
    ON [dbo].[rpt_user_query]([user_id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[rpt_user_query]
    ADD CONSTRAINT [PK_rpt_user_query] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

