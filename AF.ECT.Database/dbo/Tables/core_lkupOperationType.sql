CREATE TABLE [dbo].[core_lkupOperationType] (
    [OperationTypeId] NVARCHAR (2) NOT NULL,
    [description]     VARCHAR (20) NOT NULL,
    [active]          BIT          NOT NULL,
    [created_date]    DATETIME     NOT NULL,
    [modified_date]   DATETIME     NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupOperationType]
    ADD CONSTRAINT [DF_core_lkupOperationType_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[core_lkupOperationType]
    ADD CONSTRAINT [DF_core_lkupOperationType_created_date] DEFAULT (getdate()) FOR [created_date];
GO

ALTER TABLE [dbo].[core_lkupOperationType]
    ADD CONSTRAINT [DF_core_lkupOperationType_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_lkupOperationType]
    ADD CONSTRAINT [PK_core_lkupOperationType] PRIMARY KEY CLUSTERED ([OperationTypeId] ASC) WITH (FILLFACTOR = 80);
GO

