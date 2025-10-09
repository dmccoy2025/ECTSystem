CREATE TABLE [dbo].[rpt_ph_user_query_output_fields] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [PHQueryId]   INT NOT NULL,
    [SectionId]   INT NOT NULL,
    [FieldId]     INT NOT NULL,
    [FieldTypeId] INT NOT NULL
);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_output_fields]
    ADD CONSTRAINT [FK_rpt_ph_user_query_output_fields_rpt_ph_user_query] FOREIGN KEY ([PHQueryId]) REFERENCES [dbo].[rpt_ph_user_query] ([Id]);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_output_fields]
    ADD CONSTRAINT [FK_rpt_ph_user_query_output_fields_PH_Section] FOREIGN KEY ([SectionId]) REFERENCES [dbo].[PH_Section] ([Id]);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_output_fields]
    ADD CONSTRAINT [FK_rpt_ph_user_query_output_fields_PH_FieldType] FOREIGN KEY ([FieldTypeId]) REFERENCES [dbo].[PH_FieldType] ([Id]);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_output_fields]
    ADD CONSTRAINT [FK_rpt_ph_user_query_output_fields_PH_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[PH_Field] ([Id]);
GO

ALTER TABLE [dbo].[rpt_ph_user_query_output_fields]
    ADD CONSTRAINT [PK_rpt_ph_user_query_output_fields] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

