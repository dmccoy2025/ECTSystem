CREATE TABLE [dbo].[Form348_SARC] (
    [sarc_id]                        INT            IDENTITY (1, 1) NOT NULL,
    [case_id]                        VARCHAR (50)   NOT NULL,
    [status]                         INT            NOT NULL,
    [workflow]                       TINYINT        NULL,
    [member_name]                    NVARCHAR (100) NOT NULL,
    [member_ssn]                     VARCHAR (9)    NOT NULL,
    [member_grade]                   INT            NOT NULL,
    [member_unit]                    NVARCHAR (100) NOT NULL,
    [member_unit_id]                 INT            NOT NULL,
    [member_DOB]                     DATETIME       NULL,
    [member_compo]                   CHAR (1)       NOT NULL,
    [created_by]                     INT            NOT NULL,
    [created_date]                   DATETIME       NOT NULL,
    [doc_group_id]                   INT            NULL,
    [incident_date]                  DATETIME       NULL,
    [duty_status]                    INT            NULL,
    [duration_start_date]            DATETIME       NULL,
    [duration_end_date]              DATETIME       NULL,
    [ICD_E968_8]                     BIT            NULL,
    [ICD_E969_9]                     BIT            NULL,
    [ICD_other]                      BIT            NULL,
    [in_duty_status]                 BIT            NULL,
    [sig_date_rsl_wing_sarc]         DATETIME       NULL,
    [sig_name_rsl_wing_sarc]         VARCHAR (100)  NULL,
    [sig_title_rsl_wing_sarc]        VARCHAR (50)   NULL,
    [sig_date_sarc_a1]               DATETIME       NULL,
    [sig_name_sarc_a1]               VARCHAR (100)  NULL,
    [sig_title_sarc_a1]              VARCHAR (50)   NULL,
    [sig_date_board_medical]         DATETIME       NULL,
    [sig_name_board_medical]         VARCHAR (100)  NULL,
    [sig_title_board_medical]        VARCHAR (50)   NULL,
    [sig_date_board_ja]              DATETIME       NULL,
    [sig_name_board_ja]              VARCHAR (100)  NULL,
    [sig_title_board_ja]             VARCHAR (50)   NULL,
    [sig_date_board_admin]           DATETIME       NULL,
    [sig_name_board_admin]           VARCHAR (100)  NULL,
    [sig_title_board_admin]          VARCHAR (50)   NULL,
    [sig_date_approving]             DATETIME       NULL,
    [sig_name_approving]             VARCHAR (100)  NULL,
    [sig_title_approving]            VARCHAR (50)   NULL,
    [rwoa_reason]                    TINYINT        NULL,
    [rwoa_explanation]               VARCHAR (MAX)  NULL,
    [rwoa_date]                      DATETIME       NULL,
    [modified_by]                    INT            NULL,
    [modified_date]                  DATETIME       NULL,
    [cancel_reason]                  INT            NULL,
    [cancel_explanation]             VARCHAR (MAX)  NULL,
    [cancel_date]                    DATETIME       NULL,
    [def_sex_assault_db_case_num]    VARCHAR (100)  NULL,
    [return_to_group]                INT            NULL,
    [return_by_group]                INT            NULL,
    [consultation_from_usergroup_id] TINYINT        NULL,
    [return_comment]                 VARCHAR (2000) NULL,
    [is_post_processing_complete]    BIT            DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [FK_Form348_SARC_Form348_SARC_core_WorkStatus] FOREIGN KEY ([status]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [FK_Form348_SARC_core_Users] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [FK_Form348_SARC_core_UserGroups] FOREIGN KEY ([consultation_from_usergroup_id]) REFERENCES [dbo].[core_UserGroups] ([groupId]);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [FK_Form348_SARC_core_Workflow] FOREIGN KEY ([workflow]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [FK_Form348_SARC_core_lkupGrade] FOREIGN KEY ([member_grade]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [FK_Form348_SARC_core_lkupRWOAReasons] FOREIGN KEY ([rwoa_reason]) REFERENCES [dbo].[core_lkupRWOAReasons] ([ID]);
GO

ALTER TABLE [dbo].[Form348_SARC]
    ADD CONSTRAINT [PK_Form348_SARC] PRIMARY KEY CLUSTERED ([sarc_id] ASC) WITH (FILLFACTOR = 90);
GO

