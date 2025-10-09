CREATE TABLE [dbo].[FORM348_findings] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [LODID]         INT            NOT NULL,
    [ptype]         SMALLINT       NOT NULL,
    [ssn]           VARCHAR (9)    NULL,
    [name]          VARCHAR (50)   NULL,
    [grade]         VARCHAR (50)   NULL,
    [compo]         VARCHAR (6)    NULL,
    [rank]          VARCHAR (50)   NULL,
    [pascode]       VARCHAR (8)    NULL,
    [finding]       TINYINT        NULL,
    [decision_yn]   VARCHAR (1)    NULL,
    [explanation]   VARCHAR (MAX)  NULL,
    [modified_by]   INT            NOT NULL,
    [modified_date] DATETIME       NOT NULL,
    [created_by]    INT            NOT NULL,
    [created_date]  DATETIME       NOT NULL,
    [FindingsText]  VARCHAR (2400) NULL,
    [refer_des]     BIT            DEFAULT (NULL) NULL
);
GO

ALTER TABLE [dbo].[FORM348_findings]
    ADD CONSTRAINT [FK_FORM348_findings_core_Users] FOREIGN KEY ([modified_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[FORM348_findings]
    ADD CONSTRAINT [FK_FORM348_findings_core_lkupPersonnelTypes] FOREIGN KEY ([ptype]) REFERENCES [dbo].[core_lkupPersonnelTypes] ([Id]);
GO

ALTER TABLE [dbo].[FORM348_findings]
    ADD CONSTRAINT [FK_FORM348_findings_Form348] FOREIGN KEY ([LODID]) REFERENCES [dbo].[Form348] ([lodId]);
GO

ALTER TABLE [dbo].[FORM348_findings]
    ADD CONSTRAINT [FK_FORM348_findings_core_Users1] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[FORM348_findings]
    ADD CONSTRAINT [PK_FORM348_findings_1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[FORM348_findings]
    ADD CONSTRAINT [IX_FORM348_findings_LODID_PTYPE] UNIQUE NONCLUSTERED ([LODID] ASC, [ptype] ASC) WITH (FILLFACTOR = 90);
GO

