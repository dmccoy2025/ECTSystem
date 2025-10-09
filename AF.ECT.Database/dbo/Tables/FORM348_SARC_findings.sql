CREATE TABLE [dbo].[FORM348_SARC_findings] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [SARC_ID]       INT            NOT NULL,
    [ptype]         SMALLINT       NULL,
    [ssn]           VARCHAR (9)    NULL,
    [name]          VARCHAR (50)   NULL,
    [grade]         VARCHAR (50)   NULL,
    [compo]         VARCHAR (56)   NULL,
    [finding]       INT            NULL,
    [remarks]       VARCHAR (MAX)  NULL,
    [created_by]    INT            NULL,
    [created_date]  DATETIME       NULL,
    [modified_by]   INT            NULL,
    [modified_date] DATETIME       NULL,
    [pascode]       VARCHAR (8)    NULL,
    [rank]          VARCHAR (50)   NULL,
    [findings_text] VARCHAR (2400) NULL,
    [concur]        VARCHAR (1)    NULL
);
GO

ALTER TABLE [dbo].[FORM348_SARC_findings]
    ADD CONSTRAINT [PK_FORM348_SARC_findings] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[FORM348_SARC_findings]
    ADD CONSTRAINT [FK_FORM348_SARC_findings_core_Users1] FOREIGN KEY ([modified_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[FORM348_SARC_findings]
    ADD CONSTRAINT [FK_FORM348_SARC_findings_core_Users] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[FORM348_SARC_findings]
    ADD CONSTRAINT [FK_FORM348_SARC_findings_Form348_SARC] FOREIGN KEY ([SARC_ID]) REFERENCES [dbo].[Form348_SARC] ([sarc_id]);
GO

ALTER TABLE [dbo].[FORM348_SARC_findings]
    ADD CONSTRAINT [FK_FORM348_SARC_findings_core_lkupPersonnelTypes] FOREIGN KEY ([ptype]) REFERENCES [dbo].[core_lkupPersonnelTypes] ([Id]);
GO

