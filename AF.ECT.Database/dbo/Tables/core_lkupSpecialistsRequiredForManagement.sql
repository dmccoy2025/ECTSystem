CREATE TABLE [dbo].[core_lkupSpecialistsRequiredForManagement] (
    [Id]            TINYINT        NOT NULL,
    [AmountPerYear] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupSpecialistsRequiredForManagement]
    ADD CONSTRAINT [PK_core_lkupSpecialistsRequiredForManagement] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

