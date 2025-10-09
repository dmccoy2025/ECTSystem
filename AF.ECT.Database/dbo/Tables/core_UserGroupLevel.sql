CREATE TABLE [dbo].[core_UserGroupLevel] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_UserGroupLevel]
    ADD CONSTRAINT [PK_core_UserGroupLevel] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

