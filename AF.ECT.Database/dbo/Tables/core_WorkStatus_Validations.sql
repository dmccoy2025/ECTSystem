CREATE TABLE [dbo].[core_WorkStatus_Validations] (
    [wsv_id]         INT           IDENTITY (1, 1) NOT NULL,
    [ws_id]          INT           NOT NULL,
    [validationType] SMALLINT      NOT NULL,
    [data]           VARCHAR (100) NOT NULL,
    [active]         BIT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_WorkStatus_Validations]
    ADD CONSTRAINT [DF_core_WorkStatus_Validations_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_WorkStatus_Validations]
    ADD CONSTRAINT [PK_core_WorkStatus_Validations] PRIMARY KEY CLUSTERED ([wsv_id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[core_WorkStatus_Validations]
    ADD CONSTRAINT [FK_core_WorkStatus_Validations_core_WorkStatus] FOREIGN KEY ([ws_id]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

