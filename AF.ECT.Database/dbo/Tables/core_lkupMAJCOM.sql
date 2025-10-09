CREATE TABLE [dbo].[core_lkupMAJCOM] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [MAJCOM_NAME] VARCHAR (50) NOT NULL,
    [Active]      BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupMAJCOM]
    ADD CONSTRAINT [DF_core_lkupMAJCOM_Active] DEFAULT ((1)) FOR [Active];
GO

