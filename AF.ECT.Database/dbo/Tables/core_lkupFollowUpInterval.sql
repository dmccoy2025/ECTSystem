CREATE TABLE [dbo].[core_lkupFollowUpInterval] (
    [Id]       TINYINT        NOT NULL,
    [Interval] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupFollowUpInterval]
    ADD CONSTRAINT [PK_core_lkupFollowUpInterval] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

