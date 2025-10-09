CREATE TABLE [dbo].[core_lkupTimeZone] (
    [ZoneId]        NVARCHAR (3)  NOT NULL,
    [description]   VARCHAR (50)  NOT NULL,
    [active]        BIT           NOT NULL,
    [created_date]  DATETIME      NOT NULL,
    [created_by]    NVARCHAR (50) NOT NULL,
    [modified_date] DATETIME      NOT NULL,
    [modified_by]   NVARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupTimeZone]
    ADD CONSTRAINT [DF_core_lkupTimeZone_created_by] DEFAULT (user_name()) FOR [created_by];
GO

ALTER TABLE [dbo].[core_lkupTimeZone]
    ADD CONSTRAINT [DF_core_lkupTimeZone_created_date] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_lkupTimeZone]
    ADD CONSTRAINT [DF_core_lkupTimeZone_modified_by] DEFAULT (user_name()) FOR [modified_by];
GO

ALTER TABLE [dbo].[core_lkupTimeZone]
    ADD CONSTRAINT [DF_core_lkupTimeZone_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_lkupTimeZone]
    ADD CONSTRAINT [DF_core_lkupTimeZone_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[core_lkupTimeZone]
    ADD CONSTRAINT [PK_core_lkupTimeZone] PRIMARY KEY CLUSTERED ([ZoneId] ASC) WITH (FILLFACTOR = 80);
GO

