CREATE TABLE [dbo].[Form348_INCAP_Ext] (
    [ext_Id]                INT           IDENTITY (1, 1) NOT NULL,
    [sc_Id]                 INT           NOT NULL,
    [ext_Number]            SMALLINT      NULL,
    [ext_StartDate]         DATETIME      NULL,
    [ext_EndDate]           DATETIME      NULL,
    [med_ExtRecommendation] BIT           NULL,
    [iC_ExtRecommendation]  BIT           NULL,
    [wJA_ConcurWithIC]      BIT           NULL,
    [fin_IncomeLost]        BIT           NULL,
    [wcc_ExtApproval]       BIT           NULL,
    [opr_ExtApproval]       BIT           NULL,
    [ocr_ExtApproval]       BIT           NULL,
    [dos_ExtApproval]       BIT           NULL,
    [ccr_ExtApproval]       BIT           NULL,
    [vcr_ExtApproval]       BIT           NULL,
    [dop_ExtApproval]       BIT           NULL,
    [cafr_ExtApproval]      BIT           NULL,
    [med_AMROStartDate]     DATETIME      NULL,
    [med_AMROEndDate]       DATETIME      NULL,
    [med_AMRODisposition]   VARCHAR (200) NULL,
    [med_NextAMROStartDate] DATETIME      NULL,
    [med_NextAMROEndDate]   DATETIME      NULL,
    [med_IRILOStatus]       VARCHAR (200) NULL
);
GO

ALTER TABLE [dbo].[Form348_INCAP_Ext]
    ADD CONSTRAINT [FK_Form348_INCAP_Ext_Form348_INCAP] FOREIGN KEY ([sc_Id]) REFERENCES [dbo].[Form348_INCAP_Findings] ([sc_Id]);
GO

ALTER TABLE [dbo].[Form348_INCAP_Ext]
    ADD CONSTRAINT [PK_Form348_INCAP_Ext] PRIMARY KEY CLUSTERED ([ext_Id] ASC);
GO

