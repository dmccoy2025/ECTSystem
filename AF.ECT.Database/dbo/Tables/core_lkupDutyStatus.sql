CREATE TABLE [dbo].[core_lkupDutyStatus] (
    [dutyId]          TINYINT      IDENTITY (1, 1) NOT NULL,
    [dutyType]        VARCHAR (50) NULL,
    [dutyDescription] VARCHAR (50) NOT NULL,
    [sortOrder]       INT          NULL
);
GO

ALTER TABLE [dbo].[core_lkupDutyStatus]
    ADD CONSTRAINT [PK_lkupDutyStatus] PRIMARY KEY CLUSTERED ([dutyId] ASC) WITH (FILLFACTOR = 80);
GO

