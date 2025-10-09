CREATE TABLE [dbo].[Form348_AP_SARC] (
    [appeal_sarc_id]                 INT             IDENTITY (1, 1) NOT NULL,
    [initial_id]                     INT             NOT NULL,
    [initial_workflow]               INT             NOT NULL,
    [case_id]                        VARCHAR (50)    NOT NULL,
    [created_by]                     INT             NOT NULL,
    [created_date]                   DATETIME        NOT NULL,
    [modified_by]                    INT             NULL,
    [modified_date]                  DATETIME        NULL,
    [status]                         INT             NOT NULL,
    [workflow]                       TINYINT         NULL,
    [cancel_reason]                  INT             NULL,
    [cancel_explanation]             VARCHAR (MAX)   NULL,
    [cancel_date]                    DATETIME        NULL,
    [rwoa_reason]                    TINYINT         NULL,
    [rwoa_explanation]               VARCHAR (MAX)   NULL,
    [rwoa_date]                      DATETIME        NULL,
    [rwoa_reply]                     VARCHAR (MAX)   NULL,
    [return_to_group]                INT             NULL,
    [return_by_group]                INT             NULL,
    [member_name]                    NVARCHAR (100)  NOT NULL,
    [member_ssn]                     VARCHAR (9)     NOT NULL,
    [sig_date_wing_sarc]             DATETIME        NULL,
    [sig_name_wing_sarc]             NVARCHAR (100)  NULL,
    [sig_title_wing_sarc]            NVARCHAR (100)  NULL,
    [sig_date_sarc_admin]            DATETIME        NULL,
    [sig_name_sarc_admin]            NVARCHAR (100)  NULL,
    [sig_title_sarc_admin]           NVARCHAR (100)  NULL,
    [sig_date_appellate_auth]        DATETIME        NULL,
    [sig_name_appellate_auth]        NVARCHAR (100)  NULL,
    [sig_title_appellate_auth]       NVARCHAR (100)  NULL,
    [sig_date_board_medical]         DATETIME        NULL,
    [sig_name_board_medical]         NVARCHAR (100)  NULL,
    [sig_title_board_medical]        NVARCHAR (100)  NULL,
    [sig_date_board_legal]           DATETIME        NULL,
    [sig_name_board_legal]           NVARCHAR (100)  NULL,
    [sig_title_board_legal]          NVARCHAR (100)  NULL,
    [sig_date_board_admin]           DATETIME        NULL,
    [sig_name_board_admin]           NVARCHAR (100)  NULL,
    [sig_title_board_admin]          NVARCHAR (100)  NULL,
    [return_comment]                 NVARCHAR (2000) NULL,
    [doc_group_id]                   INT             NULL,
    [member_notified]                BIT             NULL,
    [consultation_from_usergroup_id] TINYINT         NULL,
    [is_post_processing_complete]    BIT             DEFAULT ((0)) NOT NULL,
    [member_unit]                    NVARCHAR (100)  DEFAULT ('') NOT NULL,
    [member_unit_id]                 INT             DEFAULT ((0)) NOT NULL,
    [member_compo]                   CHAR (1)        DEFAULT ('') NOT NULL,
    [member_grade]                   INT             DEFAULT ((99)) NOT NULL,
    [is_non_dbsign_case]             BIT             DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[Form348_AP_SARC]
    ADD CONSTRAINT [FK_Form348_AP_SARC_core_lkupGrade] FOREIGN KEY ([member_grade]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

ALTER TABLE [dbo].[Form348_AP_SARC]
    ADD CONSTRAINT [PK_Form348_AP_SARC] PRIMARY KEY CLUSTERED ([appeal_sarc_id] ASC);
GO

