-- =============================================
-- Author:		<Juan Leon-Cruz>
-- Create date: <03/18/2012>
-- Description:	<Get the Mailing Distributon List for the Lessons Learned Email, It will look for all the users who acted on the LOD (Tracking Tab)>
-- =============================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListForLessonsLearned]
	@refId int 
	, @moduleId int
AS
BEGIN
	WITH AllEmails(Email, Email2, Email3) AS
	( 
		select Users.Email, Users.Email2, Users.Email3 from core_Users Users
			where Users.userID IN (select DISTINCT(CWT.completedBy) from [core_WorkStatus_Tracking] CWT
				where CWT.refId = @refId and CWT.module = @moduleId)
	)
	SELECT   Email FROM AllEmails WHERE Email is not NULL and LEN(Email) > 0 
	UNION
	SELECT	 Email2 FROM AllEmails WHERE Email2 is not NULL and LEN(Email2) > 0 
	UNION
	SELECT	 Email3 FROM AllEmails WHERE Email3 is not NULL and LEN(Email3) > 0		
END
GO

