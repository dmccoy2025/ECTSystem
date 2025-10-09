CREATE TABLE [dbo].[core_Witness] (
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [lodID]         INT           NOT NULL,
    [witnessType]   TINYINT       NOT NULL,
    [otherType]     TINYINT       NOT NULL,
    [name]          VARCHAR (50)  NOT NULL,
    [address]       VARCHAR (200) NULL,
    [modified_by]   INT           NULL,
    [modified_date] DATETIME      NULL
);
GO

ALTER TABLE [dbo].[core_Witness]
    ADD CONSTRAINT [PK_core_Witness] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_Witness]
    ADD CONSTRAINT [DF_core_Witness_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

