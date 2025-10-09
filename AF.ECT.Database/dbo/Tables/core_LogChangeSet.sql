CREATE TABLE [dbo].[core_LogChangeSet] (
    [logId]   INT            NOT NULL,
    [section] VARCHAR (50)   NOT NULL,
    [field]   VARCHAR (50)   NOT NULL,
    [old]     VARCHAR (1000) NOT NULL,
    [new]     VARCHAR (1000) NOT NULL
);
GO

ALTER TABLE [dbo].[core_LogChangeSet]
    ADD CONSTRAINT [FK_core_LogChangeSets_core_LogAction] FOREIGN KEY ([logId]) REFERENCES [dbo].[core_LogAction] ([logId]);
GO

