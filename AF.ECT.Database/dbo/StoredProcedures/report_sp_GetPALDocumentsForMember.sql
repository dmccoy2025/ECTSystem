-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- Exec [dbo].[report_sp_GetPALDocumentsForMember] 'Bun', '2875'

CREATE PROCEDURE [dbo].[report_sp_GetPALDocumentsForMember]
	-- Add the parameters for the stored procedure here
	@LastName varchar(50) 
	, @Last4SSN char(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

Set @LastName = @LastName + '%'

PRINT @Last4SSN
PRINT Upper(@LastName)

    -- Insert statements for procedure here
	SELECT 
		Upper(LastName) As LastName
		, Last4SSN
		, URL
		, Case DocYear
			When 2001 Then 'ARCHIVE'
			Else Convert(varchar, DocYear)
			End As DocYear
		, Case DocMonth
			When 13 Then '--'
			Else Convert(varchar, DocMonth)
			End As DocMonth
			, PalDocID
	 From PALDocumentLookup
	Where Last4SSN = @Last4SSN
		And Upper(LastName) Like Upper(@LastName)
	Order By DocYear Desc, Convert(int, DocMonth) Desc
END
GO

