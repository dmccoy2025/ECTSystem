CREATE TABLE [dbo].[form348_approval_authorities] (
    [id]             TINYINT       IDENTITY (1, 1) NOT NULL,
    [name]           VARCHAR (100) NOT NULL,
    [sig_block]      VARCHAR (100) NOT NULL,
    [title]          VARCHAR (100) NOT NULL,
    [effective_date] DATETIME      NOT NULL,
    [created_by]     INT           NOT NULL,
    [created_date]   DATETIME      NOT NULL,
    [modified_by]    INT           NOT NULL,
    [modified_date]  DATETIME      NOT NULL
);
GO

ALTER TABLE [dbo].[form348_approval_authorities]
    ADD CONSTRAINT [DF_form348_approval_authorities_effective_date] DEFAULT (getutcdate()) FOR [effective_date];
GO

