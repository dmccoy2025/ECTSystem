CREATE TABLE [dbo].[core_Users] (
    [userID]               INT           IDENTITY (1, 1) NOT NULL,
    [EDIPIN]               VARCHAR (100) NULL,
    [username]             VARCHAR (100) NOT NULL,
    [LastName]             VARCHAR (100) NOT NULL,
    [FirstName]            VARCHAR (100) NOT NULL,
    [MiddleName]           VARCHAR (100) NULL,
    [Title]                VARCHAR (50)  NULL,
    [SSN]                  CHAR (9)      NULL,
    [DateOfBirth]          DATETIME      NULL,
    [Phone]                VARCHAR (40)  NULL,
    [DSN]                  VARCHAR (40)  NULL,
    [Email]                VARCHAR (200) NULL,
    [workCompo]            CHAR (1)      NOT NULL,
    [accessStatus]         TINYINT       NOT NULL,
    [receiveEmail]         BIT           NOT NULL,
    [expirationDate]       DATETIME      NOT NULL,
    [comment]              VARCHAR (400) NULL,
    [lastAccessDate]       DATETIME      NULL,
    [rank_code]            INT           NOT NULL,
    [work_street]          VARCHAR (200) NULL,
    [work_city]            VARCHAR (100) NULL,
    [work_state]           VARCHAR (50)  NULL,
    [work_zip]             VARCHAR (100) NULL,
    [work_country]         VARCHAR (50)  NULL,
    [created_date]         DATETIME      NOT NULL,
    [modified_date]        DATETIME      NULL,
    [modified_by]          INT           NULL,
    [cs_id]                INT           NOT NULL,
    [ada_cs_id]            INT           NULL,
    [currentRole]          INT           NULL,
    [DisabledDate]         DATETIME      NULL,
    [Email2]               VARCHAR (200) NULL,
    [Email3]               VARCHAR (200) NULL,
    [reportView]           TINYINT       NULL,
    [disabled_by]          TINYINT       NULL,
    [unitView]             TINYINT       DEFAULT ((1)) NOT NULL,
    [receiveReminderEmail] BIT           DEFAULT ((1)) NOT NULL
);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_core_Users1] FOREIGN KEY ([modified_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_core_UserRoles] FOREIGN KEY ([currentRole]) REFERENCES [dbo].[core_UserRoles] ([userRoleID]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_core_lkupCompo] FOREIGN KEY ([workCompo]) REFERENCES [dbo].[core_lkupCompo] ([compo]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_core_lkupGrade] FOREIGN KEY ([rank_code]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_core_lkupAccessStatus] FOREIGN KEY ([accessStatus]) REFERENCES [dbo].[core_lkupAccessStatus] ([statusId]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_Command_Struct1] FOREIGN KEY ([ada_cs_id]) REFERENCES [dbo].[Command_Struct] ([CS_ID]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK__core_User__repor__7DCE75F9] FOREIGN KEY ([reportView]) REFERENCES [dbo].[core_lkupChainType] ([id]);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [FK_core_Users_Command_Struct] FOREIGN KEY ([cs_id]) REFERENCES [dbo].[Command_Struct] ([CS_ID]);
GO

CREATE NONCLUSTERED INDEX [IX_EDIPIN]
    ON [dbo].[core_Users]([EDIPIN] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IX_SSN]
    ON [dbo].[core_Users]([SSN] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([userID] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_Users]
    ADD CONSTRAINT [DF_core_Users_lastAccessDate] DEFAULT (getdate()) FOR [lastAccessDate];
GO

