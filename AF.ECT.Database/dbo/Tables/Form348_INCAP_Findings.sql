CREATE TABLE [dbo].[Form348_INCAP_Findings] (
    [sc_Id]                 INT          IDENTITY (1, 1) NOT NULL,
    [init_StartDate]        DATETIME     NULL,
    [init_EndDate]          DATETIME     NULL,
    [init_ExtOrComplete]    BIT          NULL,
    [init_AppealOrComplete] BIT          NULL,
    [med_ReportType]        NVARCHAR (7) NULL,
    [med_AbilityToPreform]  BIT          NULL,
    [iC_Recommendation]     BIT          NULL,
    [wing_Ja_Concur]        BIT          NULL,
    [fin_IncomeLost]        BIT          NULL,
    [wcc_InitApproval]      BIT          NULL,
    [init_LateSubmission]   BIT          NULL,
    [fin_SelfEmployed]      BIT          NULL
);
GO

ALTER TABLE [dbo].[Form348_INCAP_Findings]
    ADD CONSTRAINT [PK_Form348_INCAP] PRIMARY KEY CLUSTERED ([sc_Id] ASC);
GO

