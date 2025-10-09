CREATE TABLE [dbo].[core_lkupPEPPCaseType] (
    [caseTypeId]   INT          IDENTITY (1, 1) NOT NULL,
    [caseTypeName] VARCHAR (50) NOT NULL,
    [active]       BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupPEPPCaseType]
    ADD CONSTRAINT [PK_core_PEPPCaseType] PRIMARY KEY CLUSTERED ([caseTypeId] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[core_lkupPEPPCaseType]
    ADD CONSTRAINT [DF_core_lkupPEPPCaseType_Active] DEFAULT ((1)) FOR [active];
GO

