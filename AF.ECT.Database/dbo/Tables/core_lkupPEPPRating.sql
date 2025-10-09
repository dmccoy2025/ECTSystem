CREATE TABLE [dbo].[core_lkupPEPPRating] (
    [ratingId]   INT          IDENTITY (1, 1) NOT NULL,
    [ratingName] VARCHAR (50) NOT NULL,
    [active]     BIT          NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupPEPPRating]
    ADD CONSTRAINT [PK_core_PEPPRating] PRIMARY KEY CLUSTERED ([ratingId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_lkupPEPPRating]
    ADD CONSTRAINT [DF_core_lkupPEPPRating_Active] DEFAULT ((1)) FOR [active];
GO

