CREATE TABLE [dbo].[Case_Comments] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [lodid]        INT           NOT NULL,
    [comments]     VARCHAR (MAX) NOT NULL,
    [created_by]   INT           NOT NULL,
    [created_date] DATETIME      NOT NULL,
    [deleted]      BIT           NOT NULL,
    [ModuleID]     INT           NOT NULL,
    [CommentType]  INT           NOT NULL
);
GO

ALTER TABLE [dbo].[Case_Comments]
    ADD CONSTRAINT [PK_Case_Comments] PRIMARY KEY CLUSTERED ([id] ASC);
GO

