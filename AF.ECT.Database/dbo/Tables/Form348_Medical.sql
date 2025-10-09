CREATE TABLE [dbo].[Form348_Medical] (
    [lodid]                        INT             NOT NULL,
    [member_status]                VARCHAR (50)    NULL,
    [event_nature_type]            VARCHAR (100)   NULL,
    [event_nature_details]         VARCHAR (4000)  NULL,
    [medical_facility]             NVARCHAR (500)  NULL,
    [medical_facility_type]        VARCHAR (100)   NULL,
    [treatment_date]               DATETIME        NULL,
    [death_involved_yn]            VARCHAR (50)    NULL,
    [mva_involved_yn]              VARCHAR (50)    NULL,
    [icd9Id]                       INT             NULL,
    [epts_yn]                      TINYINT         NULL,
    [physician_approval_comments]  VARCHAR (MAX)   NULL,
    [modified_by]                  INT             NOT NULL,
    [modified_date]                DATETIME        NOT NULL,
    [physician_cancel_reason]      TINYINT         NULL,
    [physician_cancel_explanation] VARCHAR (4000)  NULL,
    [diagnosis_text]               NVARCHAR (1000) NULL,
    [icd_7th_Char]                 VARCHAR (7)     DEFAULT (NULL) NULL,
    [member_from]                  INT             NULL,
    [member_component]             INT             NULL,
    [member_category]              INT             NULL,
    [influence]                    INT             NULL,
    [member_responsible]           VARCHAR (10)    NULL,
    [psych_eval]                   VARCHAR (10)    NULL,
    [psych_date]                   DATETIME        NULL,
    [relevant_condition]           VARCHAR (500)   NULL,
    [other_test]                   VARCHAR (10)    NULL,
    [other_test_date]              DATETIME        NULL,
    [deployed_location]            VARCHAR (10)    NULL,
    [condition_epts]               BIT             NULL,
    [service_aggravated]           BIT             NULL,
    [mobility_standards]           VARCHAR (10)    NULL,
    [member_condition]             VARCHAR (10)    NULL,
    [alcohol_test_done]            VARCHAR (10)    NULL,
    [drug_test_done]               VARCHAR (10)    NULL,
    [board_finalization]           VARCHAR (10)    NULL,
    [workflow]                     INT             DEFAULT (NULL) NULL
);
GO

ALTER TABLE [dbo].[Form348_Medical]
    ADD CONSTRAINT [DF_Form348_Medical_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[Form348_Medical]
    ADD CONSTRAINT [FK_Form348_Medical_Form348_Medical] FOREIGN KEY ([lodid]) REFERENCES [dbo].[Form348_Medical] ([lodid]);
GO

ALTER TABLE [dbo].[Form348_Medical]
    ADD CONSTRAINT [FK_Form348_Medical_core_lkupICD9] FOREIGN KEY ([icd9Id]) REFERENCES [dbo].[core_lkupICD9] ([ICD9_ID]);
GO

ALTER TABLE [dbo].[Form348_Medical]
    ADD CONSTRAINT [PK_Form348_Medical_1] PRIMARY KEY CLUSTERED ([lodid] ASC) WITH (FILLFACTOR = 90);
GO

