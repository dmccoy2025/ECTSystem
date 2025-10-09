CREATE TABLE [dbo].[pkgLog] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [pkgName]          NVARCHAR (50) NULL,
    [starttime]        DATETIME      NULL,
    [endtime]          DATETIME      NULL,
    [nRowRawInserted]  INT           NULL,
    [nRowInserted]     INT           NULL,
    [nRowUpdated]      INT           NULL,
    [nDeletedMembers]  INT           NULL,
    [nModifiedRecords] INT           NULL,
    [sourceFileName]   VARCHAR (200) NULL
);
GO

ALTER TABLE [dbo].[pkgLog]
    ADD CONSTRAINT [PK_pkgLog] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

