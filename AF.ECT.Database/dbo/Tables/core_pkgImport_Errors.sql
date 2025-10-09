CREATE TABLE [dbo].[core_pkgImport_Errors] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [storedProcName] VARCHAR (200)  NULL,
    [keyValue]       VARCHAR (100)  NULL,
    [pkgLog_Id]      INT            NULL,
    [time]           DATETIME       NULL,
    [errorMessage]   VARCHAR (4000) NULL
);
GO

