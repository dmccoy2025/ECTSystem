CREATE TABLE [dbo].[core_lkupUnitLevelType] (
    [LevelID]       NVARCHAR (10) NOT NULL,
    [description]   VARCHAR (25)  NOT NULL,
    [active]        BIT           NOT NULL,
    [created_date]  DATETIME      NOT NULL,
    [created_by]    NVARCHAR (50) NOT NULL,
    [modified_date] DATETIME      NOT NULL,
    [modified_by]   NVARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupUnitLevelType]
    ADD CONSTRAINT [DF_core_lkupUnitLevelType_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_lkupUnitLevelType]
    ADD CONSTRAINT [DF_core_lkupUnitLevelType_created_by] DEFAULT (user_name()) FOR [created_by];
GO

ALTER TABLE [dbo].[core_lkupUnitLevelType]
    ADD CONSTRAINT [DF_core_lkupUnitLevelType_modified_by] DEFAULT (user_name()) FOR [modified_by];
GO

ALTER TABLE [dbo].[core_lkupUnitLevelType]
    ADD CONSTRAINT [DF_core_lkupUnitLevelType_created_date] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_lkupUnitLevelType]
    ADD CONSTRAINT [DF_core_lkupUnitLevelType_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[core_lkupUnitLevelType]
    ADD CONSTRAINT [PK_core_lkupUnitLevelType] PRIMARY KEY CLUSTERED ([LevelID] ASC) WITH (FILLFACTOR = 80);
GO

