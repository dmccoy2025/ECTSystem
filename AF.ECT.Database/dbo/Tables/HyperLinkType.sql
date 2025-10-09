CREATE TABLE [dbo].[HyperLinkType] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[HyperLinkType]
    ADD CONSTRAINT [PK_BannerLinkType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

