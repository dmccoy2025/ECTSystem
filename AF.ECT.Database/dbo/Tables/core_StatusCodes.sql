CREATE TABLE [dbo].[core_StatusCodes] (
    [statusId]      INT          IDENTITY (1, 1) NOT NULL,
    [description]   VARCHAR (50) NOT NULL,
    [moduleId]      TINYINT      NOT NULL,
    [compo]         CHAR (1)     NOT NULL,
    [groupId]       TINYINT      NULL,
    [isFinal]       BIT          NOT NULL,
    [isApproved]    BIT          NOT NULL,
    [canAppeal]     BIT          NOT NULL,
    [filter]        VARCHAR (50) NULL,
    [displayOrder]  TINYINT      NOT NULL,
    [isCancel]      BIT          NULL,
    [isDisposition] BIT          NOT NULL,
    [isFormal]      BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_StatusCodes]
    ADD CONSTRAINT [DF_core_StatusCodes_displayOrder] DEFAULT ((0)) FOR [displayOrder];
GO

ALTER TABLE [dbo].[core_StatusCodes]
    ADD CONSTRAINT [DF__core_Stat__isFor__76D75FA7] DEFAULT ((0)) FOR [isFormal];
GO

ALTER TABLE [dbo].[core_StatusCodes]
    ADD CONSTRAINT [DF__core_Stat__isDis__71889AA5] DEFAULT ((0)) FOR [isDisposition];
GO

ALTER TABLE [dbo].[core_StatusCodes]
    ADD CONSTRAINT [FK_StatusCodes_core_UserGroups] FOREIGN KEY ([groupId]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[core_StatusCodes]
    ADD CONSTRAINT [FK_StatusCodes_core_lkupModule] FOREIGN KEY ([moduleId]) REFERENCES [dbo].[core_lkupModule] ([moduleId]);
GO

ALTER TABLE [dbo].[core_StatusCodes]
    ADD CONSTRAINT [PK_StatusCodes] PRIMARY KEY CLUSTERED ([statusId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_core_StatusCodes_PostCompletion]
    ON [dbo].[core_StatusCodes]([isFinal] ASC)
    INCLUDE([description], [moduleId], [compo], [groupId], [filter]) WITH (FILLFACTOR = 80);
GO

