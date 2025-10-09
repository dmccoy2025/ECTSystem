CREATE TABLE [dbo].[Form261] (
    [lodId]                   INT            NOT NULL,
    [reportDate]              DATETIME       NULL,
    [investigationOf]         TINYINT        NULL,
    [status]                  TINYINT        NULL,
    [inactiveDutyTraining]    VARCHAR (100)  NULL,
    [durationStartDate]       DATETIME       NULL,
    [durationFinishDate]      DATETIME       NULL,
    [IoUserId]                INT            NULL,
    [otherPersonnels]         XML            NULL,
    [final_approval_findings] TINYINT        NULL,
    [findingsDate]            DATETIME       NULL,
    [place]                   VARCHAR (200)  NULL,
    [howSustained]            VARCHAR (500)  NULL,
    [medicalDiagnosis]        VARCHAR (500)  NULL,
    [presentForDuty]          BIT            NULL,
    [absentWithAuthority]     BIT            NULL,
    [intentionalMisconduct]   BIT            NULL,
    [mentallySound]           BIT            NULL,
    [remarks]                 NVARCHAR (MAX) NULL,
    [modified_by]             INT            NULL,
    [modified_date]           DATETIME       NULL,
    [sig_date_io]             DATETIME       NULL,
    [sig_info_io]             XML            NULL,
    [sig_date_appointing]     DATETIME       NULL,
    [sig_info_appointing]     XML            NULL
);
GO

ALTER TABLE [dbo].[Form261]
    ADD CONSTRAINT [PK_Form261] PRIMARY KEY CLUSTERED ([lodId] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[Form261]
    ADD CONSTRAINT [FK_Form261_final_approval_findings_core_lkUpFindings] FOREIGN KEY ([final_approval_findings]) REFERENCES [dbo].[core_lkUpFindings] ([Id]);
GO

