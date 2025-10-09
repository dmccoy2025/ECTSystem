CREATE TABLE [dbo].[core_lkupPEPPType] (
    [typeId]   INT          IDENTITY (1, 1) NOT NULL,
    [typeName] VARCHAR (50) NOT NULL,
    [active]   BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupPEPPType]
    ADD CONSTRAINT [PK_core_PEPPType] PRIMARY KEY CLUSTERED ([typeId] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[core_lkupPEPPType]
    ADD CONSTRAINT [DF_core_lkupPEPPType_Active] DEFAULT ((1)) FOR [active];
GO

