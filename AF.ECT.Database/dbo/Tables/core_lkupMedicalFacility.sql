CREATE TABLE [dbo].[core_lkupMedicalFacility] (
    [facility_id]          INT           IDENTITY (1, 1) NOT NULL,
    [facility_description] VARCHAR (100) NOT NULL,
    [sort_order]           INT           NOT NULL,
    [facility_type]        VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_lkupMedicalFacility]
    ADD CONSTRAINT [PK_core_lkupMedicalFacility] PRIMARY KEY CLUSTERED ([facility_id] ASC);
GO

