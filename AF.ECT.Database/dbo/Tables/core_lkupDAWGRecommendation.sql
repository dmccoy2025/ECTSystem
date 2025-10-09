CREATE TABLE [dbo].[core_lkupDAWGRecommendation] (
    [Id]             TINYINT        NOT NULL,
    [Recommendation] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupDAWGRecommendation]
    ADD CONSTRAINT [PK_core_lkupDAWGRecommendation] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

