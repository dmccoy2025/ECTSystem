
        CREATE PROCEDURE [dbo].[DeleteExpiredSessions]
        AS
            DECLARE @now datetime
            SET @now = GETUTCDATE()

            DELETE [ALOD].dbo.ASPStateTempSessions
            WHERE Expires < @now

            RETURN 0
GO

