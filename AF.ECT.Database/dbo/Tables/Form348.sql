CREATE TABLE [dbo].[Form348] (
    [lodId]                          INT            IDENTITY (1, 1) NOT NULL,
    [case_id]                        VARCHAR (50)   NOT NULL,
    [status]                         INT            NOT NULL,
    [workflow]                       TINYINT        NOT NULL,
    [member_name]                    NVARCHAR (100) NOT NULL,
    [member_ssn]                     NVARCHAR (9)   NOT NULL,
    [member_grade]                   INT            NOT NULL,
    [member_unit]                    NVARCHAR (100) NOT NULL,
    [member_unit_id]                 INT            NOT NULL,
    [member_DOB]                     DATETIME       NULL,
    [member_compo]                   CHAR (1)       NOT NULL,
    [created_by]                     INT            NOT NULL,
    [created_date]                   DATETIME       NOT NULL,
    [modified_by]                    INT            NOT NULL,
    [modified_date]                  DATETIME       NOT NULL,
    [doc_group_id]                   INT            NULL,
    [formal_inv]                     BIT            NOT NULL,
    [med_tech_comments]              VARCHAR (MAX)  NULL,
    [appAuthUserId]                  INT            NULL,
    [deleted]                        BIT            NOT NULL,
    [rwoa_reason]                    TINYINT        NULL,
    [rwoa_explantion]                VARCHAR (MAX)  NULL,
    [rwoa_date]                      DATETIME       NULL,
    [FinalDecision]                  VARCHAR (300)  NULL,
    [Board_For_General_YN]           VARCHAR (1)    NULL,
    [sig_date_unit_commander]        DATETIME       NULL,
    [sig_name_unit_commander]        VARCHAR (100)  NULL,
    [sig_date_med_officer]           DATETIME       NULL,
    [sig_name_med_officer]           VARCHAR (100)  NULL,
    [sig_date_legal]                 DATETIME       NULL,
    [sig_name_legal]                 VARCHAR (100)  NULL,
    [sig_date_appointing]            DATETIME       NULL,
    [sig_name_appointing]            VARCHAR (100)  NULL,
    [sig_date_board_legal]           DATETIME       NULL,
    [sig_name_board_legal]           VARCHAR (100)  NULL,
    [sig_date_board_medical]         DATETIME       NULL,
    [sig_name_board_medical]         VARCHAR (100)  NULL,
    [sig_date_board_admin]           DATETIME       NULL,
    [sig_name_board_admin]           VARCHAR (100)  NULL,
    [sig_date_approval]              DATETIME       NULL,
    [sig_name_approval]              VARCHAR (100)  NULL,
    [sig_title_approval]             VARCHAR (100)  NULL,
    [sig_date_formal_approval]       DATETIME       NULL,
    [sig_name_formal_approval]       VARCHAR (100)  NULL,
    [sig_title_formal_approval]      VARCHAR (100)  NULL,
    [to_unit]                        VARCHAR (100)  NULL,
    [from_unit]                      VARCHAR (100)  NULL,
    [FinalFindings]                  TINYINT        NULL,
    [io_completion_date]             DATETIME       NULL,
    [io_instructions]                VARCHAR (MAX)  NULL,
    [io_poc_info]                    VARCHAR (1000) NULL,
    [sarc]                           BIT            NOT NULL,
    [restricted]                     BIT            NOT NULL,
    [io_ssn]                         CHAR (9)       NULL,
    [io_uid]                         INT            NULL,
    [aa_ptype]                       SMALLINT       NOT NULL,
    [return_comment]                 VARCHAR (2000) NULL,
    [memberNotified]                 BIT            NULL,
    [completed_by_unit]              INT            NULL,
    [sig_title_board_admin]          NVARCHAR (100) NULL,
    [sig_title_board_medical]        NVARCHAR (100) NULL,
    [sig_title_board_legal]          NVARCHAR (100) NULL,
    [sig_title_appointing]           NVARCHAR (100) NULL,
    [sig_title_legal]                NVARCHAR (100) NULL,
    [sig_title_unit_commander]       NVARCHAR (100) NULL,
    [sig_title_med_officer]          NVARCHAR (100) NULL,
    [parent_id]                      INT            NULL,
    [isAttachPAS]                    BIT            NULL,
    [member_attached_unit_id]        INT            NULL,
    [return_to_group]                INT            NULL,
    [return_by_group]                INT            NULL,
    [board_tech_comments]            VARCHAR (MAX)  NULL,
    [sig_name_MPF]                   VARCHAR (100)  NULL,
    [sig_date_MPF]                   DATETIME       NULL,
    [sig_title_MPF]                  NVARCHAR (100) NULL,
    [isFormalCancelRecommended]      BIT            DEFAULT ((0)) NULL,
    [appointing_cancel_reason_id]    TINYINT        DEFAULT (NULL) NULL,
    [appointing_cancel_explanation]  VARCHAR (4000) DEFAULT (NULL) NULL,
    [approving_cancel_reason_id]     TINYINT        DEFAULT (NULL) NULL,
    [approving_cancel_explanation]   VARCHAR (4000) DEFAULT (NULL) NULL,
    [sig_name_lodPM]                 VARCHAR (100)  DEFAULT (NULL) NULL,
    [sig_date_lodPM]                 DATETIME       DEFAULT (NULL) NULL,
    [sig_title_lodPM]                VARCHAR (100)  DEFAULT (NULL) NULL,
    [sig_name_board_a1]              VARCHAR (100)  DEFAULT (NULL) NULL,
    [sig_date_board_a1]              DATETIME       DEFAULT (NULL) NULL,
    [sig_title_board_a1]             VARCHAR (100)  DEFAULT (NULL) NULL,
    [hasCredibleService]             BIT            DEFAULT (NULL) NULL,
    [wasMemberOnOrders]              BIT            DEFAULT (NULL) NULL,
    [approvingAuthorityUserId]       INT            DEFAULT (NULL) NULL,
    [tmn]                            INT            NULL,
    [tms]                            INT            NULL,
    [appointing_unit]                VARCHAR (100)  NULL,
    [sig_date_formal_legal]          DATETIME       NULL,
    [sig_name_formal_legal]          VARCHAR (100)  NULL,
    [sig_title_formal_legal]         NVARCHAR (100) NULL,
    [sig_date_formal_appointing]     DATETIME       NULL,
    [sig_name_formal_appointing]     VARCHAR (100)  NULL,
    [sig_title_formal_appointing]    NVARCHAR (100) NULL,
    [sig_date_formal_board_medical]  DATETIME       NULL,
    [sig_name_formal_board_medical]  VARCHAR (100)  NULL,
    [sig_title_formal_board_medical] NVARCHAR (100) NULL,
    [sig_date_formal_board_legal]    DATETIME       NULL,
    [sig_name_formal_board_legal]    VARCHAR (100)  NULL,
    [sig_title_formal_board_legal]   NVARCHAR (100) NULL,
    [sig_date_formal_board_admin]    DATETIME       NULL,
    [sig_name_formal_board_admin]    VARCHAR (100)  NULL,
    [sig_title_formal_board_admin]   NVARCHAR (100) NULL,
    [sig_date_formal_board_a1]       DATETIME       NULL,
    [sig_name_formal_board_a1]       VARCHAR (100)  NULL,
    [sig_title_formal_board_a1]      NVARCHAR (100) NULL,
    [rwoa_direct_reply]              BIT            NULL,
    [wingcc_nilod_byreasonof]        INT            NULL,
    [is_post_processing_complete]    BIT            DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_ApprovingCancelReason] FOREIGN KEY ([approving_cancel_reason_id]) REFERENCES [dbo].[core_lkupCancelReason] ([Id]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_WorkStatus] FOREIGN KEY ([status]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_Workflow] FOREIGN KEY ([workflow]) REFERENCES [dbo].[core_Workflow] ([workflowId]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_AppointingCancelReason] FOREIGN KEY ([appointing_cancel_reason_id]) REFERENCES [dbo].[core_lkupCancelReason] ([Id]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_Users2] FOREIGN KEY ([appAuthUserId]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_Users1] FOREIGN KEY ([modified_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_lkupRWOAReasons] FOREIGN KEY ([rwoa_reason]) REFERENCES [dbo].[core_lkupRWOAReasons] ([ID]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_Users] FOREIGN KEY ([created_by]) REFERENCES [dbo].[core_Users] ([userID]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_lkupPersonnelTypes] FOREIGN KEY ([aa_ptype]) REFERENCES [dbo].[core_lkupPersonnelTypes] ([Id]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_lkupGrade] FOREIGN KEY ([member_grade]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_core_lkUpFindingByReasonOf] FOREIGN KEY ([wingcc_nilod_byreasonof]) REFERENCES [dbo].[core_lkUpFindingByReasonOf] ([Id]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [FK_Form348_Form348] FOREIGN KEY ([lodId]) REFERENCES [dbo].[Form348] ([lodId]);
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_aa_ptype] DEFAULT ((5)) FOR [aa_ptype];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_deleted] DEFAULT ((0)) FOR [deleted];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_case_id] DEFAULT ('') FOR [case_id];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_restricted] DEFAULT ((0)) FOR [restricted];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_formal_inv] DEFAULT ((0)) FOR [formal_inv];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_sarc] DEFAULT ((0)) FOR [sarc];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [DF_Form348_created_date] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[Form348]
    ADD CONSTRAINT [PK_Form348] PRIMARY KEY CLUSTERED ([lodId] ASC) WITH (FILLFACTOR = 80);
GO

