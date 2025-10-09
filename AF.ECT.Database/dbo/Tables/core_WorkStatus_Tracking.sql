CREATE TABLE [dbo].[core_WorkStatus_Tracking] (
    [wst_id]      INT           IDENTITY (1, 1) NOT NULL,
    [ws_id]       INT           NOT NULL,
    [refId]       INT           NOT NULL,
    [module]      TINYINT       NOT NULL,
    [startDate]   DATETIME      NOT NULL,
    [endDate]     DATETIME      NULL,
    [completedBy] INT           NULL,
    [workflowId]  TINYINT       NULL,
    [rank]        INT           NULL,
    [name]        VARCHAR (100) NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_coreWorkStatusTracking_PostCompletion]
    ON [dbo].[core_WorkStatus_Tracking]([ws_id] ASC, [refId] ASC)
    INCLUDE([startDate]) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IDX_WorkStatus_Tracking_endDate]
    ON [dbo].[core_WorkStatus_Tracking]([ws_id] ASC, [endDate] ASC)
    INCLUDE([refId], [startDate]);
GO

CREATE NONCLUSTERED INDEX [IX_TRACKING_REFID_MODULE]
    ON [dbo].[core_WorkStatus_Tracking]([refId] ASC, [module] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_core_WorkStatus_Tracking]
    ON [dbo].[core_WorkStatus_Tracking]([wst_id] ASC, [refId] ASC)
    INCLUDE([ws_id], [startDate]) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [FK_core_WorkStatus_Tracking_core_WorkStatus] FOREIGN KEY ([ws_id]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [FK_core_workstatus_tracking_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [FK_core_WorkStatus_Tracking_core_Users] FOREIGN KEY ([completedBy]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [FK_core_WorkStatus_Tracking_core_lkupModule] FOREIGN KEY ([module]) REFERENCES [dbo].[core_lkupModule] ([moduleId]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [FK_core_workstatus_tracking_core_lkupGrade] FOREIGN KEY ([rank]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [DF_core_workStatus_tracking_startDate] DEFAULT (getdate()) FOR [startDate];
GO

ALTER TABLE [dbo].[core_WorkStatus_Tracking]
    ADD CONSTRAINT [PK_core_WorkStatus_Tracking] PRIMARY KEY CLUSTERED ([wst_id] ASC) WITH (FILLFACTOR = 80);
GO

