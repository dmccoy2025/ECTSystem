CREATE TABLE [dbo].[Form348_INCAP_Appeal] (
    [ap_Id]               INT IDENTITY (1, 1) NOT NULL,
    [sc_Id]               INT NOT NULL,
    [ext_Id]              INT NULL,
    [wcc_AppealApproval]  BIT NULL,
    [opr_AppealApproval]  BIT NULL,
    [ocr_AppealApproval]  BIT NULL,
    [dos_AppealApproval]  BIT NULL,
    [ccr_AppealApproval]  BIT NULL,
    [vcr_AppealApproval]  BIT NULL,
    [dop_AppealApproval]  BIT NULL,
    [cafr_AppealApproval] BIT NULL
);
GO

ALTER TABLE [dbo].[Form348_INCAP_Appeal]
    ADD CONSTRAINT [PK_Form348_INCAP_Appeal] PRIMARY KEY CLUSTERED ([ap_Id] ASC);
GO

ALTER TABLE [dbo].[Form348_INCAP_Appeal]
    ADD CONSTRAINT [FK_Form348_INCAP_Appeal_Form348_INCAP] FOREIGN KEY ([sc_Id]) REFERENCES [dbo].[Form348_INCAP_Findings] ([sc_Id]);
GO

