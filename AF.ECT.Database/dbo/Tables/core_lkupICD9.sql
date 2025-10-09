CREATE TABLE [dbo].[core_lkupICD9] (
    [ICD9_ID]    INT           NOT NULL,
    [value]      VARCHAR (7)   NULL,
    [text]       VARCHAR (250) NULL,
    [parentId]   INT           NULL,
    [sortOrder]  INT           NULL,
    [isDisease]  BIT           DEFAULT ((0)) NOT NULL,
    [Active]     BIT           NOT NULL,
    [SeventhChr] BIT           NULL,
    [DiagOrder]  INT           NULL,
    [ICDVersion] INT           NULL
);
GO

ALTER TABLE [dbo].[core_lkupICD9]
    ADD CONSTRAINT [DF_core_lkupICD9_Active] DEFAULT ((1)) FOR [Active];
GO

ALTER TABLE [dbo].[core_lkupICD9]
    ADD CONSTRAINT [PK_core_lkupICD9] PRIMARY KEY CLUSTERED ([ICD9_ID] ASC) WITH (FILLFACTOR = 80);
GO

