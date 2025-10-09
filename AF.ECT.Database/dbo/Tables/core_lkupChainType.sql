CREATE TABLE [dbo].[core_lkupChainType] (
    [id]            TINYINT       IDENTITY (1, 1) NOT NULL,
    [name]          VARCHAR (20)  NOT NULL,
    [active]        BIT           NOT NULL,
    [created_date]  DATETIME      NOT NULL,
    [created_by]    NVARCHAR (50) NOT NULL,
    [modified_date] DATETIME      NOT NULL,
    [modified_by]   NVARCHAR (50) NOT NULL,
    [description]   VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[core_lkupChainType]
    ADD CONSTRAINT [DF_core_lkupChainType_modified_by] DEFAULT (user_name()) FOR [modified_by];
GO

ALTER TABLE [dbo].[core_lkupChainType]
    ADD CONSTRAINT [DF_core_lkupChainType_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[core_lkupChainType]
    ADD CONSTRAINT [DF_core_lkupChainType_created_date] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_lkupChainType]
    ADD CONSTRAINT [DF_core_lkupChainType_created_by] DEFAULT (user_name()) FOR [created_by];
GO

ALTER TABLE [dbo].[core_lkupChainType]
    ADD CONSTRAINT [DF_core_lkupChainType_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_lkupChainType]
    ADD CONSTRAINT [PK_core_lkupChainType] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90);
GO

