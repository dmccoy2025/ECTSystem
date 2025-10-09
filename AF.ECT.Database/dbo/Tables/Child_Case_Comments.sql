CREATE TABLE [dbo].[Child_Case_Comments] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [lodid]           INT           NOT NULL,
    [comments]        VARCHAR (MAX) NOT NULL,
    [created_by]      INT           NOT NULL,
    [created_date]    DATETIME      NOT NULL,
    [deleted]         BIT           NOT NULL,
    [ModuleID]        INT           NOT NULL,
    [CommentType]     INT           NOT NULL,
    [ParentCommentID] INT           NOT NULL,
    [Role]            VARCHAR (200) NULL,
    FOREIGN KEY ([ParentCommentID]) REFERENCES [dbo].[CaseDialogue_Comments] ([id])
);
GO

ALTER TABLE [dbo].[Child_Case_Comments]
    ADD CONSTRAINT [PK_Child_Case_Comments] PRIMARY KEY CLUSTERED ([id] ASC);
GO

