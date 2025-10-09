CREATE TABLE [dbo].[Form348_SC_Reassessment] (
    [OriginalRefId]     INT NOT NULL,
    [ReassessmentRefId] INT NOT NULL
);
GO

ALTER TABLE [dbo].[Form348_SC_Reassessment]
    ADD CONSTRAINT [FK_Form348_SC_Original_SC] FOREIGN KEY ([OriginalRefId]) REFERENCES [dbo].[Form348_SC] ([SC_Id]);
GO

ALTER TABLE [dbo].[Form348_SC_Reassessment]
    ADD CONSTRAINT [FK_Form348_SC_Reassessment_SC] FOREIGN KEY ([ReassessmentRefId]) REFERENCES [dbo].[Form348_SC] ([SC_Id]);
GO

