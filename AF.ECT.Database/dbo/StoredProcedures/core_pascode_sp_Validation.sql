-- =============================================
-- Author:		<Begashaw, Theodros>
-- Create date: <01/14/2009>
-- Description:	<Checks if the pascode is valid or not>
-- =============================================
CREATE PROCEDURE [dbo].[core_pascode_sp_Validation]
	(
		@pascode VARCHAR(8),
		@valid bit OUTPUT 
     
  )
AS
BEGIN
	
	SET NOCOUNT ON;

	SET @valid = 0

    	IF EXISTS (SELECT * FROM vw_passcode WHERE PAS = @pascode or unit = @pascode)
				SET @valid = 1
		
END
GO

