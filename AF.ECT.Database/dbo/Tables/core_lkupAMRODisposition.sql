CREATE TABLE [dbo].[core_lkupAMRODisposition] (
    [disposition_Id] INT            IDENTITY (1, 1) NOT NULL,
    [description]    NVARCHAR (200) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupAMRODisposition]
    ADD CONSTRAINT [PK_core_lkupAMRODisposition] PRIMARY KEY CLUSTERED ([disposition_Id] ASC);
GO

