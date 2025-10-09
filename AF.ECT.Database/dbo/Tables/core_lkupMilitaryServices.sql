CREATE TABLE [dbo].[core_lkupMilitaryServices] (
    [serviceID] INT            IDENTITY (1, 1) NOT NULL,
    [service]   NVARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_lkupMilitaryServices]
    ADD CONSTRAINT [PK_core_lkupMilitaryServices] PRIMARY KEY CLUSTERED ([serviceID] ASC);
GO

