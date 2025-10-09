
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/7/2017
-- Description:	Returns the System Admin change history of a specific Service 
--				Member.
-- ============================================================================
CREATE PROCEDURE [dbo].[member_sp_GetSystemAdminChangeHistoryBySSN]
	@memberSSN VARCHAR(11)
AS
BEGIN
	IF (ISNULL(@memberSSN, '') = '')
		RETURN

	SELECT	la.logId AS LogId, 
			la.actionDate AS ChangeDate, 
			CASE 
				WHEN la.notes LIKE '%Added%' THEN 'Added'
				WHEN la.notes LIKE '%Changed%' THEN 'Modified'
				ELSE 'UNKNOWN'
			END AS ChangeType,
			u.userID AS ModifiedByUserId, 
			u.username AS ModifiedByUsername, 
			lcs.field AS ModifiedField, 
			lcs.old AS OldValue, 
			lcs.new AS NewValue
	FROM	core_LogAction la
			JOIN core_LogChangeSet lcs ON la.logId = lcs.logId
			JOIN core_Users u ON la.userId = u.userID
	WHERE	la.actionId = 34
			AND 
			(
				la.notes LIKE ('Changed member data for%SSAN '+ @memberSSN)
				OR
				la.notes LIKE ('Added member data for%SSAN '+ @memberSSN)
			)
	ORDER	BY la.actionDate DESC
END
GO

