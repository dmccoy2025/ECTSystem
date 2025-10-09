CREATE TABLE [dbo].[core_lkupMissedWorkDays] (
    [Id]           TINYINT        NOT NULL,
    [DayIntervals] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupMissedWorkDays]
    ADD CONSTRAINT [PK_core_lkupMissedWorkDays] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

