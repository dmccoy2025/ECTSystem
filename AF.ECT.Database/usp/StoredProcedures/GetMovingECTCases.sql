/****** Script for SelectTopNRows command from SSMS  ******/
CREATE Procedure [usp].[GetMovingECTCases]
As
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
                         dbo.core_Users AS usr ON wst.completedBy = usr.userID LEFT OUTER JOIN
                         dbo.Form348 f ON f.lodId = wst.refId AND wst.workflowId IN (1, 27) LEFT OUTER JOIN
                         dbo.Form348_SARC ON dbo.Form348_SARC.sarc_id = wst.refId AND wst.workflowId IN (28) LEFT OUTER JOIN
                         dbo.Form348_RR ON dbo.Form348_RR.request_id = wst.refId AND wst.workflowId IN (5) LEFT OUTER JOIN
                         dbo.Form348_SC ON dbo.Form348_SC.SC_Id = wst.refId AND wst.workflowId IN (23, 15, 12, 6, 21, 7, 24, 18, 30, 19, 25, 13, 22, 16, 11, 20, 14, 8) LEFT OUTER JOIN
                         dbo.Form348_AP ON dbo.Form348_AP.appeal_id = wst.refId AND wst.workflowId IN (26) LEFT OUTER JOIN
                         dbo.Form348_AP_SARC ON dbo.Form348_AP_SARC.appeal_sarc_id = wst.refId AND wst.workflowId IN (29)
						 LEFT JOIN dbo.[Special_Cases_&_LOD_Cases] scl ON sc.description = scl.Status
  where f.case_id is not null 
  --AND CONTAINS(description, @test) 
  --AND description LIKE 'Medical%'
  --AND description LIKE '%'
  order by usr.name, f.case_id, scl.Position, sc.description

  SELECT usr.name,
  scl.Position,
  COUNT(scl.Location)
	  FROM
  dbo.core_WorkStatus_Tracking AS wst INNER JOIN
                         dbo.core_WorkStatus AS ws ON wst.ws_id = ws.ws_id INNER JOIN
                         dbo.core_StatusCodes AS sc ON ws.statusId = sc.statusId INNER JOIN
                         dbo.core_Users AS usr ON wst.completedBy = usr.userID LEFT OUTER JOIN
                         dbo.Form348 f ON f.lodId = wst.refId AND wst.workflowId IN (1, 27) LEFT OUTER JOIN
                         dbo.Form348_SARC AS fs ON fs.sarc_id = wst.refId AND wst.workflowId IN (28) LEFT OUTER JOIN
                         dbo.Form348_RR AS fr ON fr.request_id = wst.refId AND wst.workflowId IN (5) LEFT OUTER JOIN
                         dbo.Form348_SC AS fsc ON fsc.SC_Id = wst.refId AND wst.workflowId IN (23, 15, 12, 6, 21, 7, 24, 18, 30, 19, 25, 13, 22, 16, 11, 20, 14, 8) LEFT OUTER JOIN
                         dbo.Form348_AP AS fap ON fap.appeal_id = wst.refId AND wst.workflowId IN (26) LEFT OUTER JOIN
                         dbo.Form348_AP_SARC AS fas ON fas.appeal_sarc_id = wst.refId AND wst.workflowId IN (29)
						 
						 LEFT JOIN dbo.[Special_Cases_&_LOD_Cases] scl ON sc.description = scl.Status
  -------------------------change date------------------------------------
  where f.case_id is not null AND wst.startDate between '2019-08-19' and '2019-08-20' 
  group by usr.name, scl.Position
  order by usr.name

  --[usp].[GetMovingECTCases]
GO

