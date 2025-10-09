--execute arcnet_import
CREATE PROCEDURE [dbo].[arcnet_import]
   @logId  as int
 As 
 
 
DECLARE @insertedMemberRows as int 
DECLARE @rawRowsInserted as int
DECLARE @modifiedRows as int 
DECLARE @parentModified as bit

UPDATE core
SET core.expirationDate = CASE WHEN ISDATE(imp.dueDate) = 1 THEN CAST(imp.dueDate AS DATE) END,
	core.modified_date = CURRENT_TIMESTAMP,
	core.modified_by = NULL
FROM [dbo].[core_users] core
INNER JOIN [dbo].ALOD_ARCNET_RAW  imp ON (core.ediPin = imp.edipi OR core.SSN = imp.ssn) AND 
		DATEDIFF("dd", core.expirationDate, 
			(CASE	WHEN ISDATE(imp.dueDate) = 1
					THEN CAST(imp.dueDate AS DATE)
					ELSE core.expirationDate
			END)
			) > 0

SET @modifiedRows = @@ROWCOUNT

UPDATE pkgLog 
SET  nRowRawInserted=0,
	 nRowInserted=0,
	 nModifiedRecords =@modifiedRows,	
	 nRowUpdated = @modifiedRows, 
	 nDeletedMembers = 0
WHERE id=@logId
GO

