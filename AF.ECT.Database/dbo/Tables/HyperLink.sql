CREATE TABLE [dbo].[HyperLink] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [TypeId]      INT            NOT NULL,
    [DisplayText] NVARCHAR (200) NOT NULL,
    [Value]       NVARCHAR (500) NOT NULL
);
GO

ALTER TABLE [dbo].[HyperLink]
    ADD CONSTRAINT [PK_BannerLink] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[HyperLink]
    ADD CONSTRAINT [FK_HyperLink_HyperLinkType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[HyperLinkType] ([Id]);
GO

