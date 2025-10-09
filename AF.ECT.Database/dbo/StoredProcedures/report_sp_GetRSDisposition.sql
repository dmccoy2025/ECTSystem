
-- ============================================================================
-- Author: Ken Barnett
-- Create date: 2/8/2016
-- Description: Returns a set of Recruiting Services (RS) workflow cases based
-- off of a set of specified criteria. Meant to be used in the RS
-- Disposition Report. 
--
-- Based off of the LOD Disposition report stored procedure
-- (report_sp_GetDisposition). 
-- ============================================================================
-- Modified By: Eric Kelley
-- Modified Date: 04/26/2021
-- Description: Change status datatype from tinyInt to Int  to accommodate 
-- status code 324
-- ============================================================================

CREATE PROC [dbo].[report_sp_GetRSDisposition]
(
@cs_id INT,
@ssn VARCHAR(10),
@view INT,
@beginDate DATETIME,
@endDate DATETIME,
@isComplete TINYINT,
@includeSubordinate BIT
)
AS
BEGIN
SET NOCOUNT ON;
IF (@ssn = '')
SET @ssn = NULL;
IF (@beginDate IS NULL OR @ssn IS NOT NULL) -- if ssn is provided, then ignore date
SELECT @beginDate = MIN(created_date) FROM Form348_SC
IF(@endDate IS NULL OR @ssn IS NOT NULL) -- if ssn is provided, then ignore date
SET @endDate = GETDATE()

DECLARE @temp Table
(
refId INT,
caseId VARCHAR(50),
lastName NVARCHAR(100),
ssn VARCHAR(4),
memberUnitId INT,
unit VARCHAR(100),
createdDate DATETIME,
status INT, --this is where the error was
isFinal BIT,
completedByUnit INT,
birthMonth VARCHAR(40),
birthMonthNum INT 
)
 
INSERT INTO @temp
SELECT sc.SC_Id, 
sc.case_id,
sc.member_name,
Right(sc.member_ssn,4) AS ssn,
sc.member_unit_id,
sc.member_unit,
sc.created_date,
sc.status  ,
ws.IsFinal,
sc.completed_by_unit,
datename(mm, sc.member_DOB),
datepart(mm, sc.member_DOB)     
FROM Form348_SC sc
JOIN core_lkupModule m ON sc.Module_Id = m.moduleId
LEFT JOIN vw_workstatus ws ON ws.ws_id = sc.status
WHERE m.moduleName = 'Recruiting Services'
AND
(
sc.completed_by_unit IN ( SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@view ) OR
sc.member_unit_id IN( SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@view )
)
AND
(
CASE 
WHEN @isComplete = 1 THEN 1 
WHEN @isComplete = 0 THEN 0 
ELSE ws.IsFinal
END = ws.IsFinal 
)
AND ( DATEDIFF(dd, 0, sc.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
AND sc.member_ssn = IsNull(@ssn,sc.member_ssn) 
IF (@includeSubordinate = 0)
SELECT  * 
FROM  @temp 
WHERE completedByUnit = @cs_id OR memberUnitId = @cs_id
ORDER BY caseId
ELSE
SELECT *
FROM @Temp
ORDER BY caseId
END

--[dbo].[report_sp_GetRSDisposition]
GO

