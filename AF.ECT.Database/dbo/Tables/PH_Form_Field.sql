CREATE TABLE [dbo].[PH_Form_Field] (
    [SectionId]             INT            NOT NULL,
    [FieldId]               INT            NOT NULL,
    [FieldTypeId]           INT            NOT NULL,
    [FieldDisplayOrder]     INT            DEFAULT ((1)) NOT NULL,
    [FieldTypeDisplayOrder] INT            DEFAULT ((1)) NOT NULL,
    [ToolTip]               NVARCHAR (100) DEFAULT (NULL) NULL
);
GO

ALTER TABLE [dbo].[PH_Form_Field]
    ADD CONSTRAINT [FK_PH_Form_Field_PH_FieldType] FOREIGN KEY ([FieldTypeId]) REFERENCES [dbo].[PH_FieldType] ([Id]);
GO

ALTER TABLE [dbo].[PH_Form_Field]
    ADD CONSTRAINT [FK_PH_Form_Field_PH_Section] FOREIGN KEY ([SectionId]) REFERENCES [dbo].[PH_Section] ([Id]);
GO

ALTER TABLE [dbo].[PH_Form_Field]
    ADD CONSTRAINT [FK_PH_Form_Field_PH_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[PH_Field] ([Id]);
GO

ALTER TABLE [dbo].[PH_Form_Field]
    ADD CONSTRAINT [PK_PH_Form_Field] PRIMARY KEY CLUSTERED ([SectionId] ASC, [FieldId] ASC, [FieldTypeId] ASC);
GO

