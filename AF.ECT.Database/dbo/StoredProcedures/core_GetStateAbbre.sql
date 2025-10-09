CREATE PROCEDURE [dbo].[core_GetStateAbbre]
	
	@states VARCHAR(200)

AS

	SET NOCOUNT ON;

IF (@states = '0')
	BEGIN
		SELECT     state + ','
		FROM         dbo.vw_lkup_States
		ORDER BY state
	END
ELSE
	BEGIN
		SELECT     state + ','
		FROM         dbo.vw_lkup_States
		WHERE     (region_num IN (select value from dbo.split(@states, ',')))
		ORDER BY state
	END
GO

