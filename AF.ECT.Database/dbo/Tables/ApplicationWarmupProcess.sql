CREATE TABLE [dbo].[ApplicationWarmupProcess] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (100) NOT NULL,
    [Active] BIT            DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[ApplicationWarmupProcess]
    ADD CONSTRAINT [PK_ApplicationWarmupProcess] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

