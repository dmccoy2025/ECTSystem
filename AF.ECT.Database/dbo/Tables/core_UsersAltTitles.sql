CREATE TABLE [dbo].[core_UsersAltTitles] (
    [UserId]  INT            NOT NULL,
    [GroupId] INT            NOT NULL,
    [Title]   NVARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_UsersAltTitles]
    ADD CONSTRAINT [PK_core_UsersAltTitles] PRIMARY KEY CLUSTERED ([UserId] ASC, [GroupId] ASC) WITH (FILLFACTOR = 80);
GO

