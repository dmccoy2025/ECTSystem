CREATE TABLE [dbo].[core_Permissions] (
    [permId]   SMALLINT      IDENTITY (1, 1) NOT NULL,
    [permName] VARCHAR (50)  NOT NULL,
    [permDesc] VARCHAR (100) NOT NULL,
    [exclude]  BIT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_Permissions]
    ADD CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([permId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_Permissions]
    ADD CONSTRAINT [DF_core_Permissions_exclude] DEFAULT ((0)) FOR [exclude];
GO

