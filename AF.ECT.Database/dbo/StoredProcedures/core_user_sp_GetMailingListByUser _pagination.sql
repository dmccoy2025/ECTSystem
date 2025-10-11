--UC:4
--SA:3
--NA:13
--NL:59
--NP:7
--EXEC  core_user_sp_GetMailingListByGroup 1,'4,2'


CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByUser _pagination]
	@userId int,
	@PageNumber INT = 1,
	@PageSize INT = 10

AS

WITH AllEmails(Email, Email2, Email3) AS
(
	SELECT a.Email ,a.Email2,a.Email3
	FROM vw_users a
	WHERE
	   a.receiveEmail = 1
	 and
		a.accessStatus = 3
	AND
		a.userID = @userId
)
SELECT  Email FROM AllEmails WHERE Email is not NULL and LEN(email) > 0
	UNION
SELECT	 Email2 FROM AllEmails WHERE Email2 is not NULL and LEN(email2) > 0
	UNION
SELECT	 Email3 FROM AllEmails WHERE Email3 is not NULL and LEN(email3) > 0
ORDER BY Email
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO