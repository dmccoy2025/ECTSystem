CREATE TABLE [dbo].[core_lkupAvailabilityCode] (
    [availabilityCode] INT          IDENTITY (1, 1) NOT NULL,
    [description]      VARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupAvailabilityCode]
    ADD CONSTRAINT [PK_core_lkupAvailabilityCode] PRIMARY KEY CLUSTERED ([availabilityCode] ASC);
GO

