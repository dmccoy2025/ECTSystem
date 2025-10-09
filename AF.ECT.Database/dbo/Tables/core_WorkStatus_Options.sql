CREATE TABLE [dbo].[core_WorkStatus_Options] (
    [wso_id]      INT           IDENTITY (1, 1) NOT NULL,
    [ws_id]       INT           NOT NULL,
    [displayText] VARCHAR (100) NOT NULL,
    [active]      BIT           NOT NULL,
    [sortOrder]   TINYINT       NOT NULL,
    [template]    TINYINT       NOT NULL,
    [ws_id_out]   INT           NOT NULL,
    [Compo]       INT           NULL
);
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [FK_core_WorkStatus_Options_core_WorkStatus_Options] FOREIGN KEY ([wso_id]) REFERENCES [dbo].[core_WorkStatus_Options] ([wso_id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [FK_core_WorkStatus_Options_core_WorkStatus] FOREIGN KEY ([ws_id]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [FK_core_WorkStatus_Options_core_WorkStatus1] FOREIGN KEY ([ws_id_out]) REFERENCES [dbo].[core_WorkStatus] ([ws_id]);
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [DF_core_WorkStatus_Options_template] DEFAULT ((1)) FOR [template];
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [DF_core_WorkStatus_Options_active] DEFAULT ((1)) FOR [active];
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [DF_core_WorkStatus_Options_sortOrder] DEFAULT ((0)) FOR [sortOrder];
GO

ALTER TABLE [dbo].[core_WorkStatus_Options]
    ADD CONSTRAINT [PK_core_WorkStatus_Options] PRIMARY KEY CLUSTERED ([wso_id] ASC) WITH (FILLFACTOR = 90);
GO

