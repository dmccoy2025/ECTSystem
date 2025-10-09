
--execute report_pkgLog '12/09/2009' ,'12/31/2009'
CREATE PROCEDURE [dbo].[report_pkgLog]
 
 
(
	@beginDate datetime,
	@endDate datetime

)
AS
	set nocount on;
	
if (@beginDate is null)
   select @beginDate = min(starttime) from pkgLog
if(@endDate is null)
	SET @endDate = getDate()
	
	  
	  
		SELECT   starttime ,endtime,  pkgName			    
				,CASE   
					WHEN  nRowRawInserted  IS null THEN 0 
					ELSE   nRowRawInserted 
					End AS nRowRawInserted 
				,CASE   
					WHEN  nRowInserted IS null THEN 0 
					ELSE   nRowInserted
					End AS nRowInserted
			    ,CASE   
					WHEN  nRowUpdated IS null THEN 0 
					ELSE   nRowUpdated
					End AS nRowUpdated 
				,CASE   
					WHEN  nModifiedRecords IS null THEN 0 
					ELSE   nModifiedRecords
					End AS nModifiedRecords  
				 ,CASE   
					WHEN  nDeletedMembers IS null THEN 0 
					ELSE   nDeletedMembers
					End AS nDeletedMembers
		 FROM 
        pkgLog
 
        WHERE  
		/* This code is to get the records falling on the begin  and end dates also to be included.DATEDIFF(dd, 0, a.created_date ) will get the date only protion */
		 DATEDIFF(dd, 0, startTime ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate )
GO

