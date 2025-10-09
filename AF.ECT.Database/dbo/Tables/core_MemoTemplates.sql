CREATE TABLE [dbo].[core_MemoTemplates] (
    [templateId]      TINYINT       IDENTITY (1, 1) NOT NULL,
    [title]           VARCHAR (100) NOT NULL,
    [body]            VARCHAR (MAX) NOT NULL,
    [attachments]     VARCHAR (400) NULL,
    [active]          BIT           NOT NULL,
    [sigBlock]        VARCHAR (200) NULL,
    [addDate]         BIT           NOT NULL,
    [addSuspenseDate] BIT           NOT NULL,
    [dataSource]      VARCHAR (50)  NOT NULL,
    [compo]           CHAR (1)      NOT NULL,
    [addSignature]    BIT           NOT NULL,
    [phi]             BIT           NOT NULL,
    [module]          TINYINT       NOT NULL,
    [created_by]      INT           NOT NULL,
    [created_date]    DATETIME      NOT NULL,
    [modified_by]     INT           NOT NULL,
    [modified_date]   DATETIME      NOT NULL,
    [font_size]       INT           NULL
);
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [PK_core_MemoTemplates] PRIMARY KEY CLUSTERED ([templateId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [FK_core_MemoTemplates_core_Users1] FOREIGN KEY ([modified_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [FK_core_MemoTemplates_core_MemoTemplates] FOREIGN KEY ([templateId]) REFERENCES [dbo].[core_MemoTemplates] ([templateId]);
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [FK_core_MemoTemplates_core_Users] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [FK_core_MemoTemplates_core_lkupModule] FOREIGN KEY ([module]) REFERENCES [dbo].[core_lkupModule] ([moduleId]);
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [DF_core_MemoTemplates_addDate] DEFAULT ((1)) FOR [addDate];
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [DF_core_MemoTemplates_addSig] DEFAULT ((0)) FOR [addSignature];
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [DF_core_MemoTemplates_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [DF_core_MemoTemplates_addSuspenseDate] DEFAULT ((0)) FOR [addSuspenseDate];
GO

ALTER TABLE [dbo].[core_MemoTemplates]
    ADD CONSTRAINT [DF_core_MemoTemplates_phi] DEFAULT ((0)) FOR [phi];
GO

