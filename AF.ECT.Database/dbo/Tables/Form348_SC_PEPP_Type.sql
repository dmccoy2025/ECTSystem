CREATE TABLE [dbo].[Form348_SC_PEPP_Type] (
    [RefId]  INT NOT NULL,
    [TypeId] INT NOT NULL
);
GO

ALTER TABLE [dbo].[Form348_SC_PEPP_Type]
    ADD CONSTRAINT [FK_Form348_SC_PEPP_Type_Form348_SC] FOREIGN KEY ([RefId]) REFERENCES [dbo].[Form348_SC] ([SC_Id]);
GO

ALTER TABLE [dbo].[Form348_SC_PEPP_Type]
    ADD CONSTRAINT [FK_Form348_SC_PEPP_Type_core_lkupPEPPType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[core_lkupPEPPType] ([typeId]);
GO

