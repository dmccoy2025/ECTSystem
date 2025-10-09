CREATE TABLE [dbo].[core_WorkflowLocks] (
    [id]       INT      IDENTITY (1, 1) NOT NULL,
    [refId]    INT      NOT NULL,
    [module]   TINYINT  NOT NULL,
    [userId]   INT      NOT NULL,
    [lockTime] DATETIME NOT NULL
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_CORE_WORKFLOWLOCKS_REFID_MODULE]
    ON [dbo].[core_WorkflowLocks]([refId] ASC, [module] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_WorkflowLocks]
    ADD CONSTRAINT [DF__core_Work__lockT__37A611D3] DEFAULT (getdate()) FOR [lockTime];
GO

ALTER TABLE [dbo].[core_WorkflowLocks]
    ADD CONSTRAINT [FK__core_Work__userI__36B1ED9A] FOREIGN KEY ([userId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_WorkflowLocks]
    ADD CONSTRAINT [FK__core_Work__modul__35BDC961] FOREIGN KEY ([module]) REFERENCES [dbo].[core_lkupModule] ([moduleId]);
GO

ALTER TABLE [dbo].[core_WorkflowLocks]
    ADD CONSTRAINT [PK__core_Wor__3213E83F398E5A45] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

