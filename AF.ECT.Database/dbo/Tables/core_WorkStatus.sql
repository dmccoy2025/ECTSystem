CREATE TABLE [dbo].[core_WorkStatus] (
    [ws_id]         INT          IDENTITY (1, 1) NOT NULL,
    [workflowId]    TINYINT      NOT NULL,
    [statusId]      INT          NOT NULL,
    [sortOrder]     TINYINT      NOT NULL,
    [isBoardStatus] BIT          NOT NULL,
    [displayText]   VARCHAR (50) NULL,
    [isHolding]     BIT          NOT NULL,
    [compo]         VARCHAR (1)  NULL,
    [isConsult]     BIT          NULL
);
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [DF_core_WorkStatus_sortOrder] DEFAULT ((0)) FOR [sortOrder];
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [DF__core_Work__displ__1DBC229E] DEFAULT (NULL) FOR [displayText];
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [DF__core_Work__isBoa__61A73897] DEFAULT ((0)) FOR [isBoardStatus];
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [DF__core_Work__isHol__1C92F43B] DEFAULT ((0)) FOR [isHolding];
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [FK_core_WorkStatus_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [FK_core_WorkStatus_core_StatusCodes] FOREIGN KEY ([statusId]) REFERENCES [dbo].[core_StatusCodes] ([statusId]);
GO

ALTER TABLE [dbo].[core_WorkStatus]
    ADD CONSTRAINT [PK_core_WorkStatus] PRIMARY KEY CLUSTERED ([ws_id] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_core_WorkStatus_PostCompletion]
    ON [dbo].[core_WorkStatus]([statusId] ASC) WITH (FILLFACTOR = 80);
GO

