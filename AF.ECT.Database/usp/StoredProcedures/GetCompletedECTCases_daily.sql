CREATE Procedure [usp].[GetCompletedECTCases_daily]
As
DECLARE 
@date DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@endDate DATETIME = CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP)) + '09:00',
@startDate DATETIME;
SET @startDate =
	CASE DATEPART(weekday, @date)
		WHEN 2 THEN CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-3)) + '09:00'
		ELSE CONVERT(DATETIME, CONVERT(DATE, CURRENT_TIMESTAMP-1)) + '09:00'
		END
----To show/verify the @startDate and @endDate and adds to the report---- 
SELECT @startDate 'Start Date',
		@endDate 'End Date'
SELECT  f.case_id
		,f.[workflow]
      
      ,f.member_unit_id
      ,f.member_ssn
      ,sc.[description]
      ,usr.[name]
      ,wst.[startDate]
      ,wst.[endDate]
	  
	  ,scl.Position
	  ,scl.Location
	  FROM
  dbo.core_WorkStatus_Tracking AS wst INNER JOIN
                         dbo.core_WorkStatus AS ws ON wst.ws_id = ws.ws_id INNER JOIN
                         dbo.core_StatusCodes AS sc ON ws.statusId = sc.statusId INNER JOIN
                         dbo.core_Users AS usr ON wst.completedBy = usr.userID INNER JOIN
                         dbo.Form348 f ON f.lodId = wst.refId AND wst.workflowId IN (1, 27) LEFT OUTER JOIN
                         dbo.Form348_SARC AS fs ON fs.sarc_id = wst.refId AND wst.workflowId IN (28) LEFT OUTER JOIN
                         dbo.Form348_RR AS fr ON fr.request_id = wst.refId AND wst.workflowId IN (5) LEFT OUTER JOIN
                         dbo.Form348_SC AS fsc ON fsc.SC_Id = wst.refId AND wst.workflowId IN (23, 15, 12, 6, 21, 7, 24, 18, 30, 19, 25, 13, 22, 16, 11, 20, 14, 8) LEFT OUTER JOIN
                         dbo.Form348_AP AS fap ON fap.appeal_id = wst.refId AND wst.workflowId IN (26) LEFT OUTER JOIN
                         dbo.Form348_AP_SARC AS fas ON fas.appeal_sarc_id = wst.refId AND wst.workflowId IN (29)
						 LEFT JOIN dbo.[Special_Cases_&_LOD_Cases] scl ON sc.description = scl.Status
  where f.case_id is not null 
  --AND CONTAINS(description, @test) 
  --AND description LIKE 'Medical%'
  --AND description LIKE '%'
  order by usr.name, f.case_id, scl.Position, sc.description

  SELECT usr.name,
  sc.description,	
  COUNT(scl.Location)
	  FROM
  dbo.core_WorkStatus_Tracking AS wst INNER JOIN
                         dbo.core_WorkStatus AS ws ON wst.ws_id = ws.ws_id INNER JOIN
                         dbo.core_StatusCodes AS sc ON ws.statusId = sc.statusId INNER JOIN
                         dbo.core_Users AS u ON wst.completedBy = u.userID INNER JOIN
                         dbo.Form348 f ON f.lodId = wst.refId AND wst.workflowId IN (1, 27) LEFT OUTER JOIN
                         dbo.Form348_SARC AS fs ON fs.sarc_id = wst.refId AND wst.workflowId IN (28) LEFT OUTER JOIN
                         dbo.Form348_RR AS fr ON fr.request_id = wst.refId AND wst.workflowId IN (5) LEFT OUTER JOIN
                         dbo.Form348_SC AS fsc ON fsc.SC_Id = wst.refId AND wst.workflowId IN (23, 15, 12, 6, 21, 7, 24, 18, 30, 19, 25, 13, 22, 16, 11, 20, 14, 8) LEFT OUTER JOIN
                         dbo.Form348_AP AS fap ON fap.appeal_id = wst.refId AND wst.workflowId IN (26) LEFT OUTER JOIN
                         dbo.Form348_AP_SARC AS fas ON fas.appeal_sarc_id = wst.refId AND wst.workflowId IN (29)
						 
						 LEFT JOIN dbo.[Special_Cases_&_LOD_Cases] scl ON sc.description = scl.Status
  -------------------------change date------------------------------------
  where f.case_id is not null 
  --AND wst.startDate between @startDate and @endDate 
  and sc.description = 'Complete' 
  group by usr.name, sc.description
  order by usr.name
GO

