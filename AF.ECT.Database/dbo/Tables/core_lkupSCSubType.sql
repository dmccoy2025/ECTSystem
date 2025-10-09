CREATE TABLE [dbo].[core_lkupSCSubType] (
    [subTypeId]            INT           IDENTITY (1, 1) NOT NULL,
    [subTypeTitle]         NVARCHAR (50) NOT NULL,
    [associatedWorkflowId] INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupSCSubType]
    ADD CONSTRAINT [PK_core_lkupSCSubType] PRIMARY KEY CLUSTERED ([subTypeId] ASC) WITH (FILLFACTOR = 80);
GO

