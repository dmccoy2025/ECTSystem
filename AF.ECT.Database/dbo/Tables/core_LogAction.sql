CREATE TABLE [dbo].[core_LogAction] (
    [logId]       INT            IDENTITY (1, 1) NOT NULL,
    [moduleId]    TINYINT        NOT NULL,
    [actionId]    TINYINT        NOT NULL,
    [actionDate]  DATETIME       NOT NULL,
    [userId]      INT            NOT NULL,
    [referenceId] INT            NULL,
    [notes]       VARCHAR (1000) NULL,
    [status]      INT            NULL,
    [newStatus]   INT            NULL,
    [address]     VARCHAR (20)   NULL
);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'referenceId might be LODId, IncapId, AppealId, Budget, etc.  Not null and not FK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'core_LogAction', @level2type = N'COLUMN', @level2name = N'referenceId';
GO

ALTER TABLE [dbo].[core_LogAction]
    ADD CONSTRAINT [FK_core_ActionLog_core_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_LogAction]
    ADD CONSTRAINT [FK_core_LoggingActionModuleId_core_lkupModuleModuleId] FOREIGN KEY ([moduleId]) REFERENCES [dbo].[core_lkupModule] ([moduleId]);
GO

ALTER TABLE [dbo].[core_LogAction]
    ADD CONSTRAINT [FK_core_ActionLog_core_lkupAction] FOREIGN KEY ([actionId]) REFERENCES [dbo].[core_lkupAction] ([actionId]);
GO

ALTER TABLE [dbo].[core_LogAction]
    ADD CONSTRAINT [PK_core_loggingAction] PRIMARY KEY CLUSTERED ([logId] ASC) WITH (FILLFACTOR = 90);
GO

