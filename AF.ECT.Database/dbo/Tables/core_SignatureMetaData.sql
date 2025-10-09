CREATE TABLE [dbo].[core_SignatureMetaData] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [refId]       INT           NOT NULL,
    [workflowId]  TINYINT       NOT NULL,
    [workStatus]  INT           NOT NULL,
    [userGroup]   TINYINT       NOT NULL,
    [userId]      INT           NOT NULL,
    [date]        DATETIME      NOT NULL,
    [nameAndRank] VARCHAR (100) NOT NULL,
    [title]       VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_SignatureMetaData]
    ADD CONSTRAINT [FK_core_SignatureMetaData_core_UserGroups] FOREIGN KEY ([userGroup]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_SignatureMetaData]
    ADD CONSTRAINT [FK_core_SignatureMetaData_core_WorkStatus] FOREIGN KEY ([workStatus]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_SignatureMetaData]
    ADD CONSTRAINT [FK_core_SignatureMetaData_core_Workflow] FOREIGN KEY ([workflowId]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[core_SignatureMetaData]
    ADD CONSTRAINT [FK_core_SignatureMetaData_core_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

