CREATE TABLE [dbo].[SuicideMethod] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (100) NOT NULL,
    [Active] BIT            DEFAULT ((1)) NOT NULL
);
GO

ALTER TABLE [dbo].[SuicideMethod]
    ADD CONSTRAINT [PK_SuicideMethod] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

