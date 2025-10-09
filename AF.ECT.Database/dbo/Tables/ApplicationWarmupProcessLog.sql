CREATE TABLE [dbo].[ApplicationWarmupProcessLog] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ProcessId]     INT            NOT NULL,
    [ExecutionDate] DATETIME       NOT NULL,
    [Message]       NVARCHAR (MAX) NULL
);
GO

ALTER TABLE [dbo].[ApplicationWarmupProcessLog]
    ADD CONSTRAINT [FK_ApplicationWarmupProcessLog_ApplicationWarmupProcess] FOREIGN KEY ([ProcessId]) REFERENCES [dbo].[ApplicationWarmupProcess] ([Id]);
GO

ALTER TABLE [dbo].[ApplicationWarmupProcessLog]
    ADD CONSTRAINT [PK_ApplicationWarmupProcessLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

