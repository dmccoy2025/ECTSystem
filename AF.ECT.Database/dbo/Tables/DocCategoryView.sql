CREATE TABLE [dbo].[DocCategoryView] (
    [DocCatViewId] INT IDENTITY (1, 1) NOT NULL,
    [DocViewId]    INT NOT NULL,
    [DocCatId]     INT NOT NULL,
    [ViewOrder]    INT NULL,
    [IsRedacted]   BIT DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[DocCategoryView]
    ADD CONSTRAINT [PK_DocCategoryView] PRIMARY KEY CLUSTERED ([DocCatViewId] ASC);
GO

ALTER TABLE [dbo].[DocCategoryView]
    ADD CONSTRAINT [FK_DocCategoryView_DocumentView] FOREIGN KEY ([DocViewId]) REFERENCES [dbo].[DocumentView] ([viewId]);
GO

ALTER TABLE [dbo].[DocCategoryView]
    ADD CONSTRAINT [FK_DocCategoryView_DocumentCategory2] FOREIGN KEY ([DocCatId]) REFERENCES [dbo].[DocumentCategory2] ([DocCatId]);
GO

