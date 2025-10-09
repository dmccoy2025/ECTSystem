
CREATE proc [dbo].[tmp_report_RowaCount]
(
	@beginDate datetime,
	@endDate datetime

)
AS
	set nocount on;

if (@beginDate is null)
   select @beginDate = min(created_date) from form348
if(@endDate is null)
	SET @endDate = getDate()

SELECT     
	ID, Description,
      (
		SELECT COUNT(*) AS TotalCount
        FROM Rwoa  AS b
        WHERE (b.reason_sent_back  = a.ID)  
		AND 
		/* This code is to get the records falling on the begin  and end dates also to be included.DATEDIFF(dd, 0, a.created_date ) will get the date only protion */
		 DATEDIFF(dd, 0, b.date_sent ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) 
		)
	   AS TotalCount  
	  
FROM         
	core_lkupRWOAReasons AS a
GO

