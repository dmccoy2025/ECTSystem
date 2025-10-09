
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/7/2017
-- Description:	Returns the MILPDS change history of a specific Service Member.
-- ============================================================================
CREATE PROCEDURE [dbo].[member_sp_GetMILPDSChangeHistoryBySSN]
	@memberSSN VARCHAR(11)
AS
BEGIN
	IF (ISNULL(@memberSSN, '') = '')
		RETURN

	SELECT	ch.SSAN, 
			ch.FIRST_NAME, 
			ch.LAST_NAME, 
			ch.MIDDLE_NAMES, 
			ch.SUFFIX, 
			ch.SEX_SVC_MBR, 
			ch.DOB, ch.PAS, 
			ch.PAS_GAINING, 
			ch.PAS_NUMBER, 
			ch.ATTACH_PAS, 
			ch.COMM_DUTY_PHONE,
			ch.HOME_PHONE, 
			ch.OFFICE_SYMBOL,
			ch.GR_CURR, 
			ch.DY_POSN_NR,
			ch.ChangeType, 
			ch.Date,
			ch.LOCAL_ADDR_STREET,
			ch.LOCAL_ADDR_CITY,
			ch.LOCAL_ADDR_STATE,
			ch.ZIP,
			ch.ADRS_MAIL_DOM_CITY,
			ch.ADRS_MAIL_DOM_STATE,
			ch.ADRS_MAIL_ZIP
	FROM	MemberData_ChangeHistory ch
	WHERE	ch.SSAN = @memberSSN
	ORDER	BY ch.Date DESC
END
GO

