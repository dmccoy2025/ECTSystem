CREATE TABLE [dbo].[Form348_PSCD_Findings] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [PSCD_ID]           INT            NOT NULL,
    [Name]              VARCHAR (50)   NULL,
    [PType]             SMALLINT       NULL,
    [Remarks]           VARCHAR (MAX)  NULL,
    [Finding]           TINYINT        NULL,
    [FindingText]       VARCHAR (2400) NULL,
    [CreatedBy]         INT            NULL,
    [CreatedDate]       DATETIME       NULL,
    [ModifiedBy]        INT            NULL,
    [ModifiedDate]      DATETIME       NULL,
    [AdditionalRemarks] VARCHAR (255)  NULL,
    [ReferToDES]        BIT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([PSCD_ID]) REFERENCES [dbo].[Form348_SC] ([SC_Id])
);
GO

