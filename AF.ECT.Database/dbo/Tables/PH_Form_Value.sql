CREATE TABLE [dbo].[PH_Form_Value] (
    [RefId]       INT             NOT NULL,
    [SectionId]   INT             NOT NULL,
    [FieldId]     INT             NOT NULL,
    [FieldTypeId] INT             NOT NULL,
    [Value]       NVARCHAR (1000) NOT NULL
);
GO

ALTER TABLE [dbo].[PH_Form_Value]
    ADD CONSTRAINT [FK_PH_Form_Value_PH_Section] FOREIGN KEY ([SectionId]) REFERENCES [dbo].[PH_Section] ([Id]);
GO

ALTER TABLE [dbo].[PH_Form_Value]
    ADD CONSTRAINT [FK_PH_Form_Value_PH_FieldType] FOREIGN KEY ([FieldTypeId]) REFERENCES [dbo].[PH_FieldType] ([Id]);
GO

ALTER TABLE [dbo].[PH_Form_Value]
    ADD CONSTRAINT [FK_PH_Form_Value_PH_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[PH_Field] ([Id]);
GO

ALTER TABLE [dbo].[PH_Form_Value]
    ADD CONSTRAINT [FK_PH_Form_Value_Form348_SC] FOREIGN KEY ([RefId]) REFERENCES [dbo].[Form348_SC] ([SC_Id]);
GO

ALTER TABLE [dbo].[PH_Form_Value]
    ADD CONSTRAINT [PK_PH_Form_Value] PRIMARY KEY CLUSTERED ([RefId] ASC, [SectionId] ASC, [FieldId] ASC, [FieldTypeId] ASC);
GO

