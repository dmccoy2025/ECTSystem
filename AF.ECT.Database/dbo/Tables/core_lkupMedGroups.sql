CREATE TABLE [dbo].[core_lkupMedGroups] (
    [Id]  INT            IDENTITY (1, 1) NOT NULL,
    [MTF] NVARCHAR (255) NULL
);
GO

ALTER TABLE [dbo].[core_lkupMedGroups]
    ADD CONSTRAINT [PK_core_lkupMedGroups] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

