CREATE TABLE [dbo].[core_lkupCountries] (
    [country_id]    INT           IDENTITY (1, 1) NOT NULL,
    [country]       NVARCHAR (50) NOT NULL,
    [abbr]          NVARCHAR (50) NULL,
    [created_date]  DATETIME      NOT NULL,
    [created_by]    NVARCHAR (50) NOT NULL,
    [modified_date] DATETIME      NOT NULL,
    [modified_by]   NVARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupCountries]
    ADD CONSTRAINT [DF_core_lkupCountries_created_date] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_lkupCountries]
    ADD CONSTRAINT [DF_core_lkupCountries_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[core_lkupCountries]
    ADD CONSTRAINT [DF_core_lkupCountries_modified_by] DEFAULT (user_name()) FOR [modified_by];
GO

ALTER TABLE [dbo].[core_lkupCountries]
    ADD CONSTRAINT [DF_core_lkupCountries_created_by] DEFAULT (user_name()) FOR [created_by];
GO

ALTER TABLE [dbo].[core_lkupCountries]
    ADD CONSTRAINT [PK_core_lkupCountries] PRIMARY KEY CLUSTERED ([country] ASC) WITH (FILLFACTOR = 90);
GO

