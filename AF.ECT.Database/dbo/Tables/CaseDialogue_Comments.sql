CREATE TABLE [dbo].[CaseDialogue_Comments] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [lodid]        INT           NOT NULL,
    [comments]     VARCHAR (MAX) NOT NULL,
    [created_by]   INT           NOT NULL,
    [created_date] DATETIME      NOT NULL,
    [deleted]      BIT           NOT NULL,
    [ModuleID]     INT           NOT NULL,
    [CommentType]  INT           NOT NULL,
    [Role]         VARCHAR (200) NULL
);
GO

ALTER TABLE [dbo].[CaseDialogue_Comments]
    ADD CONSTRAINT [PK_CaseDialogue_Comments] PRIMARY KEY CLUSTERED ([id] ASC);
GO

