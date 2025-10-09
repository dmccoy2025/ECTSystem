CREATE TABLE [dbo].[core_lkupMemberInfluence] (
    [influence_id]          INT           IDENTITY (1, 1) NOT NULL,
    [influence_description] VARCHAR (100) NOT NULL,
    [sort_order]            INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupMemberInfluence]
    ADD CONSTRAINT [PK_core_lkupMemberInfluence] PRIMARY KEY CLUSTERED ([influence_id] ASC);
GO

