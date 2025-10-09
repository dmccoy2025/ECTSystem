CREATE TABLE [dbo].[Form348_RR] (
    [request_id]                     INT             IDENTITY (1, 1) NOT NULL,
    [ReinvestigationLodId]           INT             NULL,
    [InitialLodId]                   INT             NOT NULL,
    [Case_Id]                        VARCHAR (50)    NOT NULL,
    [CreatedBy]                      INT             NOT NULL,
    [CreatedDate]                    DATE            NOT NULL,
    [workflow]                       INT             NULL,
    [status]                         INT             NULL,
    [rwoa_reason]                    TINYINT         NULL,
    [rwoa_date]                      DATETIME        NULL,
    [rwoa_explanation]               VARCHAR (MAX)   NULL,
    [sig_date_mpf]                   DATETIME        NULL,
    [sig_name_mpf]                   VARCHAR (100)   NULL,
    [sig_title_mpf]                  NVARCHAR (100)  NULL,
    [sig_date_wing_ja]               DATETIME        NULL,
    [sig_name_wing_ja]               VARCHAR (100)   NULL,
    [sig_title_wing_ja]              NVARCHAR (100)  NULL,
    [sig_date_wing_cc]               DATETIME        NULL,
    [sig_name_wing_cc]               VARCHAR (100)   NULL,
    [sig_title_wing_cc]              NVARCHAR (100)  NULL,
    [sig_date_board_admin]           DATETIME        NULL,
    [sig_name_board_admin]           VARCHAR (100)   NULL,
    [sig_title_board_admin]          NVARCHAR (100)  NULL,
    [sig_date_board_medical]         DATETIME        NULL,
    [sig_name_board_medical]         VARCHAR (100)   NULL,
    [sig_title_board_medical]        NVARCHAR (100)  NULL,
    [sig_date_board_legal]           DATETIME        NULL,
    [sig_name_board_legal]           VARCHAR (100)   NULL,
    [sig_title_board_legal]          NVARCHAR (100)  NULL,
    [sig_date_approval]              DATETIME        NULL,
    [sig_name_approval]              VARCHAR (100)   NULL,
    [sig_title_approval]             VARCHAR (100)   NULL,
    [sig_date_board_tech_final]      DATETIME        NULL,
    [sig_name_board_tech_final]      VARCHAR (100)   NULL,
    [sig_title_board_tech_final]     NVARCHAR (100)  NULL,
    [wing_cc_approved]               TINYINT         NULL,
    [board_medical_approved]         TINYINT         NULL,
    [board_legal_approved]           TINYINT         NULL,
    [aa_final_approved]              TINYINT         NULL,
    [return_comment]                 NVARCHAR (2000) NULL,
    [member_ssn]                     NVARCHAR (9)    NULL,
    [wing_ja_approved]               TINYINT         NULL,
    [board_tech_approval1]           TINYINT         NULL,
    [board_tech_approval2]           TINYINT         NULL,
    [wing_ja_approval_comment]       NVARCHAR (2000) NULL,
    [wing_cc_approval_comment]       NVARCHAR (2000) NULL,
    [board_tech_approval1_comment]   NVARCHAR (1000) NULL,
    [board_medical_approval_comment] NVARCHAR (2000) NULL,
    [board_legal_approval_comment]   NVARCHAR (2000) NULL,
    [aa_final_approval_comment]      NVARCHAR (2000) NULL,
    [Modified_By]                    INT             NULL,
    [Modified_Date]                  DATETIME        NULL,
    [doc_group_id]                   INT             NULL,
    [member_name]                    NVARCHAR (100)  NULL,
    [Cancel_Reason]                  INT             NULL,
    [Cancel_Explanation]             NVARCHAR (1000) NULL,
    [Cancel_Date]                    DATETIME        NULL,
    [return_to_group]                INT             NULL,
    [return_by_group]                INT             NULL,
    [sig_name_lodPM]                 VARCHAR (100)   DEFAULT (NULL) NULL,
    [sig_date_lodPM]                 DATETIME        DEFAULT (NULL) NULL,
    [sig_title_lodPM]                VARCHAR (100)   DEFAULT (NULL) NULL,
    [sig_name_board_a1]              VARCHAR (100)   DEFAULT (NULL) NULL,
    [sig_date_board_a1]              DATETIME        DEFAULT (NULL) NULL,
    [sig_title_board_a1]             NVARCHAR (100)  DEFAULT (NULL) NULL,
    [board_a1_approved]              TINYINT         DEFAULT (NULL) NULL,
    [board_a1_approval_comment]      NVARCHAR (2000) DEFAULT (NULL) NULL,
    [member_unit]                    NVARCHAR (100)  DEFAULT ('') NOT NULL,
    [member_unit_id]                 INT             DEFAULT ((0)) NOT NULL,
    [member_compo]                   CHAR (1)        DEFAULT ('') NOT NULL,
    [member_grade]                   INT             DEFAULT ((99)) NOT NULL,
    [is_non_dbsign_case]             BIT             DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[Form348_RR]
    ADD CONSTRAINT [PK_Form348_Reinvestigation_Requests] PRIMARY KEY CLUSTERED ([request_id] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Form348_Reinvestigation]
    ON [dbo].[Form348_RR]([InitialLodId] ASC);
GO

ALTER TABLE [dbo].[Form348_RR]
    ADD CONSTRAINT [FK_Form348_RR_core_lkupGrade] FOREIGN KEY ([member_grade]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

