
        CREATE PROCEDURE [dbo].[TempResetTimeout]
            @id     tSessionId
        AS
            UPDATE [ALOD].dbo.ASPStateTempSessions
            SET Expires = DATEADD(n, Timeout, GETUTCDATE())
            WHERE SessionId = @id
            RETURN 0
GO

