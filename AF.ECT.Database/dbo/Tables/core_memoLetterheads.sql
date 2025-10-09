CREATE TABLE [dbo].[core_memoLetterheads] (
    [id]                   TINYINT       IDENTITY (1, 1) NOT NULL,
    [title]                VARCHAR (50)  NOT NULL,
    [version]              TINYINT       NOT NULL,
    [compo]                CHAR (1)      NOT NULL,
    [date_created]         DATETIME      NOT NULL,
    [logo_image_left]      VARCHAR (100) NOT NULL,
    [header_title]         VARCHAR (100) NOT NULL,
    [header_title_size]    INT           NOT NULL,
    [header_subtitle]      VARCHAR (100) NOT NULL,
    [header_subtitle_size] INT           NOT NULL,
    [font]                 VARCHAR (50)  NOT NULL,
    [font_size]            INT           NOT NULL,
    [header_font_color]    VARCHAR (50)  DEFAULT ('Black') NOT NULL,
    [logo_image_right]     VARCHAR (100) DEFAULT ('') NOT NULL,
    [footer_image_center]  VARCHAR (100) DEFAULT ('') NOT NULL,
    [effective_date]       DATETIME      DEFAULT (getutcdate()) NOT NULL
);
GO

ALTER TABLE [dbo].[core_memoLetterheads]
    ADD CONSTRAINT [PK_core_memoLetterheads] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_memoLetterheads]
    ADD CONSTRAINT [FK_core_memoLetterheads_core_lkupCompo] FOREIGN KEY ([compo]) REFERENCES [dbo].[core_lkupCompo] ([compo]);
GO

