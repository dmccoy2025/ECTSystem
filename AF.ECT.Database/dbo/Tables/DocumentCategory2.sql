CREATE TABLE [dbo].[DocumentCategory2] (
    [DocCatId]            INT           NOT NULL,
    [CategoryDescription] VARCHAR (150) NOT NULL
);
GO

ALTER TABLE [dbo].[DocumentCategory2]
    ADD CONSTRAINT [CK_CatDesc_NoCommas] CHECK (NOT [CategoryDescription] like '%,%');
GO

ALTER TABLE [dbo].[DocumentCategory2]
    ADD CONSTRAINT [PK_DocumentCategory2] PRIMARY KEY CLUSTERED ([DocCatId] ASC);
GO

