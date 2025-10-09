CREATE TABLE [dbo].[core_UserGroups] (
    [groupId]       TINYINT        IDENTITY (1, 1) NOT NULL,
    [name]          NVARCHAR (100) NOT NULL,
    [abbr]          NVARCHAR (10)  NOT NULL,
    [compo]         CHAR (1)       NOT NULL,
    [accessScope]   TINYINT        NOT NULL,
    [active]        BIT            NOT NULL,
    [partialMatch]  BIT            NOT NULL,
    [showInfo]      BIT            NOT NULL,
    [sortOrder]     TINYINT        NOT NULL,
    [hipaaRequired] BIT            NOT NULL,
    [canRegister]   BIT            NOT NULL,
    [reportView]    TINYINT        NOT NULL,
    [groupLevel]    INT            NOT NULL
);
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [DF_core_UserGroups_canRegister] DEFAULT ((1)) FOR [canRegister];
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [DF_Roles_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [DF_core_UserGroups_showInfo] DEFAULT ((1)) FOR [showInfo];
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [DF__core_User__group__38060486] DEFAULT ((1)) FOR [groupLevel];
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [DF_core_UserGroups_partialMatch] DEFAULT ((0)) FOR [partialMatch];
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [DF_core_UserGroups_hipaaRequired] DEFAULT ((1)) FOR [hipaaRequired];
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([groupId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [FK_core_UserGroups_core_lkupCompo] FOREIGN KEY ([compo]) REFERENCES [dbo].[core_lkupCompo] ([compo]);
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [FK_core_UserGroups_core_UserGroupLevel] FOREIGN KEY ([groupLevel]) REFERENCES [dbo].[core_UserGroupLevel] ([Id]);
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [FK_core_UserGroups_core_lkupChainType] FOREIGN KEY ([reportView]) REFERENCES [dbo].[core_lkupChainType] ([id]);
GO

ALTER TABLE [dbo].[core_UserGroups]
    ADD CONSTRAINT [FK_core_UserGroups_core_lkupAccessScope] FOREIGN KEY ([accessScope]) REFERENCES [dbo].[core_lkupAccessScope] ([scopeId]);
GO

