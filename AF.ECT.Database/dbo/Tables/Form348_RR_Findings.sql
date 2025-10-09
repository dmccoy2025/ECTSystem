CREATE TABLE [dbo].[Form348_RR_Findings] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [request_id]        INT           NOT NULL,
    [ptype]             SMALLINT      NOT NULL,
    [last_name]         VARCHAR (100) NULL,
    [first_name]        VARCHAR (100) NULL,
    [middle_name]       VARCHAR (100) NULL,
    [grade]             VARCHAR (50)  NULL,
    [rank]              VARCHAR (50)  NULL,
    [compo]             VARCHAR (6)   NULL,
    [pascode]           VARCHAR (8)   NULL,
    [finding]           TINYINT       NULL,
    [explanation]       VARCHAR (MAX) NULL,
    [is_legacy_finding] BIT           DEFAULT ((0)) NOT NULL,
    [created_by]        INT           NOT NULL,
    [created_date]      DATETIME      NOT NULL,
    [modified_by]       INT           NOT NULL,
    [modified_date]     DATETIME      NOT NULL,
    [concur]            VARCHAR (1)   NULL
);
GO

ALTER TABLE [dbo].[Form348_RR_Findings]
    ADD CONSTRAINT [FK_Form348_RR_Findings_Form348_RR] FOREIGN KEY ([request_id]) REFERENCES [dbo].[Form348_RR] ([request_id]);
GO

ALTER TABLE [dbo].[Form348_RR_Findings]
    ADD CONSTRAINT [FK_Form348_RR_Findings_core_Users_1] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[Form348_RR_Findings]
    ADD CONSTRAINT [FK_Form348_RR_Findings_core_lkupPersonnelTypes] FOREIGN KEY ([ptype]) REFERENCES [dbo].[core_lkupPersonnelTypes] ([Id]);
GO

ALTER TABLE [dbo].[Form348_RR_Findings]
    ADD CONSTRAINT [FK_Form348_RR_Findings_core_Users_2] FOREIGN KEY ([modified_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[Form348_RR_Findings]
    ADD CONSTRAINT [PK_Form348_RR_Findings] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[Form348_RR_Findings]
    ADD CONSTRAINT [CK_Form348_RR_Findings_REQUESTID_PTYPE] UNIQUE NONCLUSTERED ([request_id] ASC, [ptype] ASC) WITH (FILLFACTOR = 80);
GO

