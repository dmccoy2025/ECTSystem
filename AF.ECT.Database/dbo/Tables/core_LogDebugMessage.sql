CREATE TABLE [dbo].[core_LogDebugMessage] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    [Message]     NVARCHAR (MAX) NOT NULL
);
GO

ALTER TABLE [dbo].[core_LogDebugMessage]
    ADD CONSTRAINT [PK_core_LogDebugMessage] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

