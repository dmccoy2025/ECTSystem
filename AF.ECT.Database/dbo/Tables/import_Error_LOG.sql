CREATE TABLE [dbo].[import_Error_LOG] (
    [UpdatingData] VARCHAR (2000) NULL,
    [StoredProc]   VARCHAR (2000) NULL,
    [Message]      VARCHAR (2000) NULL,
    [RCPHALODID]   INT            NULL,
    [Time]         DATETIME       NULL,
    [ErrorMessage] VARCHAR (4000) NULL
);
GO

