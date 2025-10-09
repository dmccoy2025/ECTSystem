CREATE TABLE [dbo].[core_lkupPEPPDisposition] (
    [dispositionId]   INT          IDENTITY (1, 1) NOT NULL,
    [dispositionName] VARCHAR (50) NOT NULL,
    [active]          BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupPEPPDisposition]
    ADD CONSTRAINT [DF_core_lkupPEPPDisposition_Active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_lkupPEPPDisposition]
    ADD CONSTRAINT [PK_core_PEPPDisposition] PRIMARY KEY CLUSTERED ([dispositionId] ASC) WITH (FILLFACTOR = 80);
GO

